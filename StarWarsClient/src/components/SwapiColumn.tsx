import {
    type FilterFieldSpec,
    numberBetweenFilterTemplate,
    dateBetweenFilterTemplate,
    booleanFilterTemplate,
    dropdownFilterTemplate, multiselectFilterTemplate,
    idFilterTemplate
} from '../utils/DataTableFilters.tsx';

import {
    Column,
    type ColumnFilterElementTemplateOptions
} from "primereact/column";
import type {CSSProperties} from "react";
import {formatDateCustom, formatNumber, type RowData} from "../utils/DataTableColumnBody.ts";
import {TextFilter} from "./FilterElement/TextFilter.tsx";
import type {FilterCallback} from "../utils/DataTableFilterState.ts";

const defaultWidth = '14rem';
const frozenBackgroundColor = '#363749ff';

const formatHeaderText = (field: string): string => {
    return field
        .split('_')
        .map(word => word
            .charAt(0)
            .toUpperCase() + word.slice(1)
        )
        .join(' ');
};

interface SwapiColumnProps {
    field: string;
    spec: FilterFieldSpec;
    filterCallbacks: FilterCallback;
}

function SwapiColumn({
    field,
    spec,
    filterCallbacks,
}: SwapiColumnProps) {
    const style: CSSProperties = {
        minWidth: spec.width ?? defaultWidth
    };

    if (spec.frozen) {
        style.background = frozenBackgroundColor;
    }

    const getFilterElement = () => {
        switch (spec.kind) {
            case 'id':
                return idFilterTemplate;
            case 'number':
                return numberBetweenFilterTemplate;
            case 'date':
                return dateBetweenFilterTemplate;
            case 'boolean':
                return booleanFilterTemplate;
            case 'dropdown':
                return (opts: ColumnFilterElementTemplateOptions) =>
                    dropdownFilterTemplate(opts, spec.selectItems ?? []);
            case 'multiselect':
                return (opts: ColumnFilterElementTemplateOptions) =>
                    multiselectFilterTemplate(opts, spec.selectItems ?? []);
            default:
                return (opts: ColumnFilterElementTemplateOptions) =>
                    <TextFilter field={field} options={opts} filterCallbacks={filterCallbacks} />;
        }
    };

    const getBody = () => {
        switch (spec.kind) {
            case 'number':
                return (rowData: RowData) => formatNumber(rowData, field, spec.decimalPlaces);
            case 'date':
                return (rowData: RowData) => formatDateCustom(rowData, field);
            case 'boolean':
                return (rowData: RowData) => rowData[field] ? 'Y' : 'N';
            case 'dropdown':
            case 'multiselect':
                return (rowData: RowData) => spec.selectItems?.find(i => i?.value === rowData[field])?.label ?? rowData[field];
            default:
                return (rowData: RowData) => rowData[field] ?? '';
        }
    }

    return <Column
        field={field}
        header={formatHeaderText(field)}
        style={style}
        frozen={spec.frozen}
        sortable
        filter
        showFilterMatchModes={spec.kind !== 'number'}
        filterElement={getFilterElement()}
        body={getBody()}
        onFilterApplyClick={() => {
            filterCallbacks.applyCallbacks(field);
        }}
        onFilterClear={() => {
            filterCallbacks.clearCallbacks(field);
        }}
    />;
}

export default SwapiColumn;