import React, {type ReactNode, useMemo, useState} from 'react';
import { FilterMatchMode, FilterOperator } from 'primereact/api';
import { type DataTableFilterMeta } from 'primereact/datatable';
import { Button } from 'primereact/button';
import { IconField } from 'primereact/iconfield';
import { InputIcon } from 'primereact/inputicon';
import { InputText } from 'primereact/inputtext';
import { Dropdown, type DropdownChangeEvent } from 'primereact/dropdown';
import { MultiSelect, type MultiSelectChangeEvent } from 'primereact/multiselect';
import { Calendar } from 'primereact/calendar';
import { InputNumber, type InputNumberChangeEvent } from 'primereact/inputnumber';
import type { SelectItem } from "primereact/selectitem";
import type { ColumnFilterElementTemplateOptions } from "primereact/column";

/**
 * Defines the available filter types for DataTable columns.
 * Each type corresponds to a specific filtering behavior and UI component.
 */
export type ColumnFilterKind =
    | 'id'
    | 'text'
    | 'number'
    | 'date'
    | 'dropdown'
    | 'multiselect'
    | 'boolean';

export interface FilterFieldSpec {
    /** One of the preset kinds above. */
    kind: ColumnFilterKind;
    width?: string; // TODO: handle default width
    frozen?: boolean; // TODO: handle column freezing, and set background to #363749ff
    decimalPlaces?: number; // TODO: add decimal place handling
    /** Override operator if needed (defaults chosen per kind). */
    operator?: FilterOperator;
    /** For simple kinds use a single matchMode override (optional). */
    matchMode?: FilterMatchMode;
    selectItems?: SelectItem[];
}

export type FilterSpec = Record<string, FilterFieldSpec>;

/**
 * Configuration options for the useTableFilters hook.
 * Controls the behavior and appearance of table filtering functionality.
 */
export interface UseTableFiltersOptions {
    /** Fields to search when typing in the global search input. */
    globalFilterFields?: string[];
    /** Placeholder text for the global search input. */
    globalPlaceholder?: string;
    /** Whether to render the global search input. Default: true */
    showGlobal?: boolean;
}


/**
 * Determines the default filter constraint configuration for a given column filter kind.
 * This function sets up the initial state of filters with appropriate match modes and empty values.
 *
 * @param kind - The type of filter to create default constraints for
 * @returns An object containing the default value (null) and appropriate match mode for the filter type
 *
 * Filter kinds and their corresponding match modes:
 * - text: Uses CONTAINS match mode for substring matching
 * - id: Uses EQUALS match mode for exact number matching
 * - number: Uses BETWEEN match mode for exact number matching
 * - date: Uses DATE_AFTER/DATE_BEFORE match mode for date filtering
 * - dropdown: Uses EQUALS match mode for single selection
 * - multiselect: Uses IN match mode for multiple selection
 * - boolean: Uses EQUALS match mode for true/false comparison
 */
function defaultConstraint(kind: ColumnFilterKind) {
    switch (kind) {
        case 'id':
            return { value: null, matchMode: FilterMatchMode.EQUALS };
        case 'text':
            return { value: null, matchMode: FilterMatchMode.CONTAINS };
        case 'number':
            // DataTable uses a single value for BETWEEN with array [min, max] or a small object – we’ll keep null start
            return { value: null, matchMode: FilterMatchMode.BETWEEN };
        case 'date':
            return [
                { value: null, matchMode: FilterMatchMode.DATE_AFTER },  // start
                { value: null, matchMode: FilterMatchMode.DATE_BEFORE }  // end
            ];
        case 'dropdown':
            return { value: null, matchMode: FilterMatchMode.EQUALS };
        case 'multiselect':
            return { value: null, matchMode: FilterMatchMode.IN };
        case 'boolean':
            return { value: null, matchMode: FilterMatchMode.EQUALS };
    }
}


/**
 * Determines the default filter operator for a given column filter kind.
 * This function defines how multiple filter constraints are combined for each filter type.
 *
 * @param kind - The type of filter to determine the operator for
 * @returns The appropriate FilterOperator (AND/OR) for the filter kind
 *
 * Operator assignments:
 * - AND operator: Used for text, dates, numbers, between ranges, and multiselect
 *   to ensure all conditions must be met
 * - OR operator: Used for dropdown and boolean filters where meeting any condition
 *   is sufficient (typically better UX for single-choice selections)
 */
function defaultOperator(kind: ColumnFilterKind): FilterOperator {
    // UX-wise this often feels better for single-choice
    switch (kind) {
        case 'dropdown':
        case 'boolean':
            return FilterOperator.OR; 
    }

    return FilterOperator.AND;
}


/**
 * Builds the initial filter configuration for a DataTable based on the provided specification.
 * This function creates a filter metadata object that defines how each column should be filtered,
 * including the default values, match modes, and operators.
 *
 * @param spec - An object defining the filter types and behaviors for each field in the table
 * @returns A DataTableFilterMeta object containing the complete filter configuration
 *
 * The resulting configuration includes:
 * - A global filter with CONTAINS match mode
 * - Field-specific filters based on their defined kinds:
 *   - Text fields: Use constraints array with AND/OR operators
 *   - Number fields: Use constraints array with specified match modes
 *   - Date fields: Use constraints array with date-specific match modes
 *   - Between ranges: Use single value with BETWEEN match mode
 *   - Dropdowns/MultiSelect: Use single value with appropriate match mode
 */
function buildDefaultFilters(spec: FilterSpec): DataTableFilterMeta {
    const meta: DataTableFilterMeta = {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS },
    };

    for (const [field, def] of Object.entries(spec)) {
        const operator = def.operator ?? defaultOperator(def.kind);
        const base = defaultConstraint(def.kind);
        const constraint = Array.isArray(base) ? 
            base : 
            [{
                value: base.value,
                matchMode: def.matchMode ?? base.matchMode,
            }];

        // kinds using constraint array vs simple value:
        if (
            def.kind === 'id' ||
            def.kind === 'text' ||
            def.kind === 'date'
        ) {
            meta[field] = {operator, constraints: constraint};
        } else if (def.kind === 'number') {
            meta[field] = { value: null, matchMode: FilterMatchMode.BETWEEN };
        } else if (def.kind === 'dropdown' || def.kind === 'multiselect' || def.kind === 'boolean') {
            meta[field] = constraint[0];
        }
    }

    return meta;
}

/**
 * Hook that manages filtering state and UI for PrimeReact DataTable components.
 * Provides filter controls, global search, and filter management functionality.
 *
 * @param spec - Specification object defining filter types for each field
 * @param opts - Optional configuration for filter behavior and appearance
 * @returns Object containing filter state and control methods
 */
export function useTableFilters(
    spec: FilterSpec,
    opts: UseTableFiltersOptions = {}
): {
    filters: DataTableFilterMeta;
    setFilters: React.Dispatch<React.SetStateAction<DataTableFilterMeta>>;
    globalFilterFields: string[] | undefined;
    header: React.ReactNode | null;
    clearFilters: () => void;
    onGlobalFilterChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
} {
    const [globalFilterValue, setGlobalFilterValue] = useState('');
    const defaultFilters = useMemo(() => buildDefaultFilters(spec), [spec]);
    const [filters, setFilters] = useState<DataTableFilterMeta>(defaultFilters);

    const clearFilters = () => {
        setFilters(defaultFilters);
        setGlobalFilterValue('');
    };

    const onGlobalFilterChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setGlobalFilterValue(value);
        setFilters((prev) => {
            const next = { ...prev };
            // @ts-ignore – PrimeReact types don't index cleanly
            next['global'].value = value;
            return next;
        });
    };

    const header = opts.showGlobal === false
        ? null
        : (
            <div className="flex gap-2 items-center">
                <IconField iconPosition="left" className="w-full">
                    <InputIcon className="pi pi-search"/>
                    <InputText
                        value={globalFilterValue}
                        onChange={onGlobalFilterChange}
                        placeholder={opts.globalPlaceholder ?? 'Keyword Search'}
                    />
                    <Button type="button" icon="pi pi-filter-slash" text onClick={clearFilters}/>
                </IconField>
            </div>
        );

    return {
        filters,
        setFilters,
        globalFilterFields: opts.globalFilterFields,
        header,
        clearFilters,
        onGlobalFilterChange,
    };
}

/**
 * Creates a clear button template for PrimeReact DataTable column filters.
 *
 * @param options - Object containing the filter callback function
 * @returns Button component for clearing the filter
 */
export function filterClearTemplate(options: { filterCallback: () => void }) {
    return <Button type="button" icon="pi pi-times" onClick={options.filterCallback} rounded text />;
}

/**
 * Creates an apply button template for PrimeReact DataTable column filters.
 *
 * @param options - Object containing the filter callback function
 * @returns Button component for applying the filter
 */
export function filterApplyTemplate(options: { filterCallback: () => void }) {
    return <Button type="button" icon="pi pi-check" onClick={options.filterCallback} rounded text />;
}

/**
 * Creates a footer template for PrimeReact DataTable column filters.
 *
 * @param options - Object containing the field name to display
 * @returns Footer component showing the field name being filtered
 */
export function filterFooterTemplate(options: { field: string }) {
    return <div className="px-2 py-1 text-color-secondary text-sm">Filter by {options.field}</div>;
}

/** Use for BETWEEN; supports [min, max] input with two number boxes stacked. */
/**
 * Creates a between range filter template with two number inputs for PrimeReact DataTable columns.
 *
 * @param options - Filter options object containing current value and callback
 * @returns Two InputNumber components configured for min/max range filtering
 */
export function numberBetweenFilterTemplate(options: any) {
    let [min, max] = Array.isArray(options.value) ? options.value : [null, null];

    return (
        <div className="flex flex-col gap-2 p-column-filter">
            <InputNumber
                value={min}
                onChange={(e: InputNumberChangeEvent) => options.filterCallback([e.value, max || Infinity])}
                placeholder="Min"
            />
            <InputNumber
                value={max}
                onChange={(e: InputNumberChangeEvent) => options.filterCallback([min || -Infinity, e.value])}
                placeholder="Max"
            />
        </div>
    );
}


export function dateBetweenFilterTemplate(options: any) {
    return (
        <Calendar
            value={options.value ?? null}
            onChange={(e) => options.filterCallback(e.value, options.index)}
            dateFormat="yy-mm-dd"
            placeholder={options.index === 0 ? 'Start date' : 'End date'}
            mask="9999-99-99 99:99"
            showIcon
            showTime 
            hourFormat="24"
        />
    );
}

/**
 * Creates a dropdown filter template for PrimeReact DataTable columns.
 * Allows selecting a single value from a predefined list of options.
 *
 * @template T - Type of the option items, must include an optional label and required value property
 * @param {ColumnFilterElementTemplateOptions} options - Filter options object from PrimeReact containing the current value and filter callback
 * @param {SelectItem[]} items - Array of selectable options to display in the dropdown
 * @param {string} [placeholder='Select one'] - Placeholder text to show when no value is selected
 * @returns {JSX.Element} A Dropdown component configured for filtering
 *
 * @example
 * const options: SelectItem[] = [
 *   { label: 'Option 1', value: 1 },
 *   { label: 'Option 2', value: 2 }
 * ];
 * <Column filterElement={(opts) => dropdownFilterTemplate(opts, options)} />
 */
export function dropdownFilterTemplate(
    options: ColumnFilterElementTemplateOptions,
    items: SelectItem[],
    placeholder = 'Select one'
) {
    return (
        <Dropdown
            value={options.value ?? null}
            options={items}
            onChange={(e: DropdownChangeEvent) => options.filterCallback(e.value)}
            placeholder={placeholder}
            className="p-column-filter"
            showClear
        />
    );
}


/**
 * Creates a MultiSelect filter template for PrimeReact DataTable columns.
 * Allows selecting multiple values from a predefined list of options.
 *
 * @template T - Type of the option items, must include an optional label and required value property
 * @param {ColumnFilterElementTemplateOptions} options - Filter options object from PrimeReact containing the current value and filter callback
 * @param {SelectItem[]} items - Array of selectable options to display in the dropdown
 * @param {string} [placeholder='Select...'] - Placeholder text to show when no value is selected
 * @returns {JSX.Element} A MultiSelect component configured for filtering
 *
 * @example
 * const options = [
 *   { label: 'Option 1', value: 1 },
 *   { label: 'Option 2', value: 2 }
 * ];
 * <Column filterElement={(opts) => multiselectFilterTemplate(opts, options)} />
 */
export function multiselectFilterTemplate(
    options: ColumnFilterElementTemplateOptions,
    items: SelectItem[],
    placeholder: string = 'Select...'
): ReactNode {
    return (
        <MultiSelect
            value={options.value ?? []}
            options={items}
            onChange={(e: MultiSelectChangeEvent) => options.filterCallback(e.value)}
            placeholder={placeholder}
            className="p-column-filter"
            display="chip"
            showClear
        />
    );
}

/**
 * Creates a boolean dropdown filter template for PrimeReact DataTable columns.
 *
 * @param options - Filter options object containing current value and callback
 * @returns Dropdown component configured for boolean filtering
 */
export function booleanFilterTemplate(options: any) {
    const values: SelectItem[] = [
        { label: "Y", value: true },
        { label: "N", value: false },
    ];
    return dropdownFilterTemplate(options, values, 'Any');
}

