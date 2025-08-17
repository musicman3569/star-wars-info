import {
    type FilterFieldSpec,
    textFilterTemplate,
    numberBetweenFilterTemplate,
    dateBetweenFilterTemplate,
    booleanFilterTemplate,
    dropdownFilterTemplate, multiselectFilterTemplate
} from '../utils/DataTableFilters.tsx';

import {Column, type ColumnFilterElementTemplateOptions} from "primereact/column";
import type { CSSProperties } from "react";
import {formatDateCustom, formatNumber} from "../utils/DataTableColumnBody.ts";

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

interface Props {
    field: string;
    spec: FilterFieldSpec;
}

function SwapiColumn({ field, spec }: Props) {
    const style: CSSProperties = {
        minWidth: spec.width ?? defaultWidth
    };
    
    if (spec.frozen) {
        style.background = frozenBackgroundColor;
    }

    const getFilterElement = () => {
        switch (spec.kind) {
            case 'id':
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
                return textFilterTemplate;
        }
    };
    
    const getBody = (rowData: any) => {
        switch (spec.kind) {
            case 'number':
                return formatNumber(rowData, field, spec.decimalPlaces);
            case 'date':
                return formatDateCustom(rowData, field);
            case 'boolean':
                return rowData[field] ? 'Y' : 'N';
            case 'dropdown':
            case 'multiselect':
                return spec.selectItems?.find(i => i?.value === rowData[field])?.label ?? rowData[field];
            default:
                return rowData[field] ?? '';
        }
    }

    return <Column
        field={field}
        header={formatHeaderText(field)}
        style={style}
        frozen
        sortable
        filter
        showFilterMatchModes={spec.kind !== 'number'}
        filterElement={getFilterElement()}
        body={getBody}
    />;
}

export default SwapiColumn;