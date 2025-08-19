import { useState, useEffect } from 'react';
import {FetchData, UpdateData} from '../utils/StarWarsInfoClient';
import {DataTable, type DataTableFilterMeta, type DataTableRowEditCompleteEvent} from 'primereact/datatable';
import SwapiColumn from './SwapiColumn';
import {type ModelSpec, useTableFilters, getModelDataKey} from '../utils/DataTableColumn';
import { useCachedFilterCallbacks } from "../utils/DataTableFilterCache";
import {Column} from "primereact/column";

function SwapiDataTable({
    modelSpec
}:{
    modelSpec: ModelSpec;
}) {
    const [tableData, setTableData] = useState<any[]>([]);
    const cssHeightToPageBottom = "calc(100vh - 100px)";
    const filterCallbacks = useCachedFilterCallbacks();
    const modelDataKey = getModelDataKey(modelSpec); 

    // Use the custom useTableFilters helper function to simplify all the complex
    // wiring that the PrimeReact DataTable needs for advanced filters.
    const { 
        filters: filters, 
        setFilters: setFilters,
    } = useTableFilters(modelSpec);

    useEffect(() => {
        FetchData(modelSpec, modelDataKey, setTableData);
    }, []);

    const onRowEditComplete = (e:DataTableRowEditCompleteEvent) => {
        UpdateData(
            modelSpec, 
            modelDataKey, 
            e.newData,
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

    return (
        <DataTable
            value={tableData}
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
            // globalFilterFields={globalFilterFields}
            removableSort
            editMode="row"
            onRowEditComplete={onRowEditComplete}
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
    );
}

export default SwapiDataTable;