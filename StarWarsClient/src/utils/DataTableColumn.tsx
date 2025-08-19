import React, { useMemo, useState } from 'react';
import { FilterMatchMode, FilterOperator } from 'primereact/api';
import { type DataTableFilterMeta } from 'primereact/datatable';
import { Button } from 'primereact/button';
import { IconField } from 'primereact/iconfield';
import { InputIcon } from 'primereact/inputicon';
import { InputText } from 'primereact/inputtext';
import type { SelectItem } from "primereact/selectitem";

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

export interface ColumnSpec {
    /** One of the preset kinds above. */
    kind: ColumnFilterKind;
    dataType?: 'text' | 'numeric' | 'date' | string | undefined;
    width?: string;
    frozen?: boolean;
    decimalPlaces?: number;
    /** Override operator if needed (defaults chosen per kind). */
    operator?: FilterOperator;
    /** For simple kinds use a single matchMode override (optional). */
    matchMode?: FilterMatchMode;
    selectItems?: SelectItem[];
    isDataKey?: boolean;
    isReadOnly?: boolean;
    isHidden?: boolean;
}

export type ModelSpec = Record<string, ColumnSpec>;

export function getModelDataKey(spec: ModelSpec): string {
    for (const [field, def] of Object.entries(spec)) {
        if (def.isDataKey) {
            return field;
        }
    }
    throw new Error('No data key found in model spec');
}

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
 * This function defines how multiple filter constraints are combined for each 
 * filter type (AND/OR).
 *
 * @param kind - The type of filter to determine the operator for
 * @returns The appropriate FilterOperator (AND/OR) for the filter kind
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
 */
function buildDefaultFilters(spec: ModelSpec): DataTableFilterMeta {
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
            meta[field] = {operator: operator, constraints: constraint};
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
    spec: ModelSpec,
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
            <div className="flex items-center gap-2">
                <span className="flex flex-1">
                    <Button label="Add" icon="pi pi-plus" />
                </span>
                <span className="flex items-center gap-0">
                    <IconField iconPosition="left" className="flex-initial">
                        <InputIcon className="pi pi-search"/>
                        <InputText
                            value={globalFilterValue}
                            onChange={onGlobalFilterChange}
                            placeholder={opts.globalPlaceholder ?? 'Keyword Search'}
                        />
                    </IconField>
                    <Button type="button" icon="pi pi-filter-slash" onClick={clearFilters}/>
                </span>
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
