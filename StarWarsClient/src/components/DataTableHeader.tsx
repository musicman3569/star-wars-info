import {Button} from "primereact/button";
import {IconField} from "primereact/iconfield";
import {InputIcon} from "primereact/inputicon";
import {InputText} from "primereact/inputtext";
import React from "react";


interface DataTableHeaderProps {
    globalFilterValue: string;
    onGlobalFilterChange: React.ChangeEventHandler<HTMLInputElement>;
    clearGlobalFilter:  React.MouseEventHandler<HTMLButtonElement>;
}
export function DataTableHeader({
    globalFilterValue,
    onGlobalFilterChange,
    clearGlobalFilter,
}:DataTableHeaderProps){
    return (
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
                        placeholder={'Keyword Search'}
                    />
                </IconField>
                <Button type="button" icon="pi pi-filter-slash" onClick={clearGlobalFilter}/>
            </span>
        </div>        
    );
}

export default DataTableHeader;