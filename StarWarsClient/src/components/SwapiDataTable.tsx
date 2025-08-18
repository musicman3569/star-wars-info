import { useState, useEffect } from 'react';
import { FetchData } from '../utils/StarWarsInfoClient';
import { DataTable, type DataTableFilterMeta } from 'primereact/datatable';
import SwapiColumn from './SwapiColumn';
import {type ModelSpec, useTableFilters, getModelDataKey} from '../utils/DataTableColumn';
import { useCachedFilterCallbacks } from "../utils/DataTableFilterCache";

function SwapiDataTable({
    modelSpec
}:{
    modelSpec: ModelSpec;
}) {
    const [starships, setStarships] = useState<any[]>([]);
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
        FetchData(modelSpec, modelDataKey, setStarships);
    }, []);

    return (
        <DataTable
            value={starships}
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
        >
            {
                Object.entries(modelSpec)
                    .map(([field, spec]) => 
                        SwapiColumn({
                            field: field, 
                            spec: spec,
                            filterCallbacks: filterCallbacks
                        })
                    )
            }
        </DataTable>
    );
}

export default SwapiDataTable;