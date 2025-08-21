import { Column, type ColumnFilterElementTemplateOptions } from "primereact/column";
import { type ColumnSpec } from '../utils/DataTableColumn';
import { type FilterCallback } from "../utils/DataTableFilterCache";
import { type CSSProperties } from "react";
import {formatDateCustom, formatNumber, formatHeaderText, type RowData} from "../utils/DataTableCellFormat";
import {FilterText} from "./FilterElement/FilterText";
import {FilterId} from "./FilterElement/FilterId";
import {FilterNumber} from "./FilterElement/FilterNumber";
import {FilterDate} from "./FilterElement/FilterDate";
import {FilterDropdown} from "./FilterElement/FilterDropdown";
import {FilterBoolean} from "./FilterElement/FilterBoolean";
import {FilterMultiselect} from "./FilterElement/FilterMultiselect"
import DataTableEditor from "./DataTableEditor";

const defaultWidth = '14rem';

interface SwapiColumnProps {
    field: string;
    spec: ColumnSpec;
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

    const getFilterElement = () => {
        switch (spec.kind) {
            case 'id':
                return (opts: ColumnFilterElementTemplateOptions) =>
                    <FilterId field={field} options={opts} filterCallbacks={filterCallbacks}/>;
            case 'number':
                return (opts: ColumnFilterElementTemplateOptions) =>
                    <FilterNumber field={field} options={opts} filterCallbacks={filterCallbacks}/>;
            case 'date':
                return (opts: ColumnFilterElementTemplateOptions) =>
                    <FilterDate field={field} options={opts} filterCallbacks={filterCallbacks}/>;
            case 'boolean':
                return (opts: ColumnFilterElementTemplateOptions) =>
                    <FilterBoolean field={field} options={opts} filterCallbacks={filterCallbacks}/>;
            case 'dropdown':
                return (opts: ColumnFilterElementTemplateOptions) =>
                    <FilterDropdown 
                        field={field} options={opts} filterCallbacks={filterCallbacks}
                        items={spec.selectItems ?? []}/>;
            case 'multiselect':
                return (opts: ColumnFilterElementTemplateOptions) =>
                    <FilterMultiselect
                        field={field} options={opts} filterCallbacks={filterCallbacks}
                        items={spec.selectItems ?? []}/>;
            default:
                return (opts: ColumnFilterElementTemplateOptions) =>
                    <FilterText field={field} options={opts} filterCallbacks={filterCallbacks}/>;
        }
    };

    const getBody = () => {
        switch (spec.kind) {
            case 'number':
                return (rowData: RowData) => formatNumber(rowData, field, spec.decimalPlaces, spec.displaySuffix);
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
    
    const getDataType = () => {
        if (spec.dataType) return spec.dataType;
        if (spec.kind === 'number' || spec.kind === 'id') return 'numeric';
        return spec.kind;
    }

    return <Column
        field={field}
        hidden={spec.isHidden}
        dataType={getDataType()}
        header={formatHeaderText(field)}
        style={style}
        frozen={spec.frozen}
        sortable
        filter
        showFilterMatchModes={spec.kind !== 'number'}
        filterElement={getFilterElement()}
        body={getBody()}
        editor={!spec.isReadOnly ? DataTableEditor({
            field: field,
            columnSpec: spec,
        }) : null}
        onFilterApplyClick={() => {
            filterCallbacks.applyCallbacks(field);
        }}
        onFilterClear={() => {
            filterCallbacks.clearCallbacks(field);
        }}
        
    />;
}

export default SwapiColumn;