import { useState, useEffect } from 'react';
import ModelStarship from '../models/ModelStarship';
import { FetchData } from '../utils/StarWarsInfoClient';
import { DataTable, type DataTableFilterMeta } from 'primereact/datatable';
import SwapiColumn from './SwapiColumn';
import { useTableFilters } from '../utils/DataTableColumn';
import {useCachedFilterCallbacks} from "../utils/DataTableFilterState";

function StarshipsTable() {
    const [starships, setStarships] = useState<any[]>([]);
    const cssHeightToPageBottom = "calc(100vh - 100px)";
    const filterCallbacks = useCachedFilterCallbacks();
    
    useEffect(() => {
        FetchData('starships', ['created', 'edited'], setStarships);
    }, []);
    
    // Use the custom useTableFilters helper function to simplify all the complex
    // wiring that the PrimeReact DataTable needs for advanced filters.
    const { filters, setFilters, /*globalFilterFields*/ } = useTableFilters(ModelStarship);

    return (
        <DataTable
            value={starships}
            paginator={false}
            rows={10}
            rowHover={true}
            scrollable={true}
            scrollHeight={cssHeightToPageBottom}
            dataKey="starship_id"
            sortMode="single"
            sortField="name"
            filters={filters}
            onFilter={(e) => setFilters(e.filters as DataTableFilterMeta)}
            // globalFilterFields={globalFilterFields}
            removableSort
        >
            {
                Object.entries(ModelStarship)
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

export default StarshipsTable;