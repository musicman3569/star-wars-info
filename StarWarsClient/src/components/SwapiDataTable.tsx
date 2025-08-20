import React, {useState, useEffect, useMemo} from 'react';
import {FetchData, UpdateData} from '../utils/StarWarsInfoClient';
import {
    DataTable,
    type DataTableFilterMeta,
} from 'primereact/datatable';
import SwapiColumn from './SwapiColumn';
import {type ModelSpec, getModelDataKey, buildDefaultFilters} from '../utils/DataTableColumn';
import {Column} from "primereact/column";
import {useCachedFilterCallbacks} from "../utils/DataTableFilterCache.ts";
import DataTableHeader from "./DataTableHeader.tsx";
import DataTableEditForm from "./DataTableEditForm.tsx";

function SwapiDataTable({
    modelSpec
}:{
    modelSpec: ModelSpec;
}) {
    const cssHeightToPageBottom = "calc(100vh - 100px - 50px)";
    const modelDataKey = getModelDataKey(modelSpec);

    const defaultFilters = useMemo(() => buildDefaultFilters(modelSpec), [modelSpec]);
    const filterCallbacks = useCachedFilterCallbacks();
    const [tableData, setTableData] = useState<any[]>([]);
    const [filters, setFilters] = useState<DataTableFilterMeta>(defaultFilters);
    const [globalFilterValue, setGlobalFilterValue] = useState('');
    const [editFormVisible, setEditFormVisible] = useState(false);
    
    const clearGlobalFilter = () => {
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
    
    useEffect(() => {
        FetchData(modelSpec, modelDataKey, setTableData);
    }, []);

    const onRowEditComplete = (newRowData:any) => {
        UpdateData(
            modelSpec, 
            modelDataKey,
            newRowData,
            (responseData) => {
                const newTableData = [...tableData];
                const updatedRowIndex = newTableData.findIndex(item => 
                    item[modelDataKey] === responseData[modelDataKey]
                );
                newTableData[updatedRowIndex] = responseData;
                setTableData(newTableData);
            }
        );
    }

    const onClickRowAdd = () => {
        if (!editFormVisible) {
            setEditFormVisible(true);
        }
    }

    return (<>
        <DataTable
            value={tableData}
            header={DataTableHeader({
                globalFilterValue,
                onGlobalFilterChange,
                clearGlobalFilter,
                onClickRowAdd
            })}
            paginator={false}
            rows={10}
            rowHover={true}
            scrollable={true}
            scrollHeight={cssHeightToPageBottom}
            dataKey={modelDataKey}
            sortMode="single"
            sortField="name"
            filters={filters}
            onFilter={(e) => setFilters(e.filters as DataTableFilterMeta)}
            globalFilterFields={[modelDataKey]}
            removableSort
            editMode="row"
            onRowEditComplete={(e) => onRowEditComplete(e.newData)}
        >
            <Column rowEditor header="Edit" frozen />
            {
                Object.entries(modelSpec)
                    .map(([field, spec]) => 
                        SwapiColumn({
                            field: field, 
                            spec: spec,
                            filterCallbacks: filterCallbacks,
                        })
                    )
            }
        </DataTable>
        <DataTableEditForm 
            visible={editFormVisible}
            onHide={() => setEditFormVisible(false)}
            modelSpec={modelSpec}
            onSave={(formData) => onRowEditComplete(formData)}
        />
    </>);
}

export default SwapiDataTable;