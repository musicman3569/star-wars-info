import { useState, useEffect } from 'react';
import { StarshipModelFilterSpec } from '../models/StarshipModel';
import { FetchData } from '../utils/StarWarsInfoClient';
import { DataTable, type DataTableFilterMeta } from 'primereact/datatable';
import { Column } from 'primereact/column';
import {
    useTableFilters,
    textFilterTemplate,
    dateBetweenFilterTemplate,
    numberBetweenFilterTemplate,
} from '../utils/DataTableFilters.tsx';
import {formatDateCustom} from '../utils/DataTableColumnBody';

function StarshipsTable() {
    const defaultWidth = "14rem";
    const [starships, setStarships] = useState<any[]>([]);
    const cssHeightToPageBottom = "calc(100vh - 100px)";
    
    useEffect(() => {
        FetchData('starships', ['created', 'edited'], setStarships);
    }, []);

    // 2) Use hook once for the table
    const { filters, setFilters, globalFilterFields } = useTableFilters(StarshipModelFilterSpec);

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
            globalFilterFields={globalFilterFields}
            removableSort
        >
            <Column field="name" header="Name" style={{minWidth: defaultWidth}} frozen sortable filter filterElement={textFilterTemplate} />
            <Column field="model" header="Model" style={{minWidth: defaultWidth}} sortable filter filterElement={textFilterTemplate}/>
            <Column field="manufacturer" header="Manufacturer" style={{minWidth: defaultWidth}} sortable filter filterElement={textFilterTemplate}/>
            <Column field="cost_in_credits" header="Cost In Credits" style={{minWidth: defaultWidth}} sortable filter filterElement={numberBetweenFilterTemplate} showFilterMatchModes={false}/>
            <Column field="length" header="Length" style={{minWidth: defaultWidth}} sortable filter filterElement={numberBetweenFilterTemplate} showFilterMatchModes={false} />
            <Column field="max_atmosphering_speed" header="Max Atmosphering Speed" 
                    style={{minWidth: "18rem"}} sortable filter filterElement={numberBetweenFilterTemplate} showFilterMatchModes={false} />
            <Column field="crew" header="Crew" style={{minWidth: defaultWidth}} sortable filter filterElement={numberBetweenFilterTemplate} showFilterMatchModes={false} />
            <Column field="passengers" header="Passengers" style={{minWidth: defaultWidth}} sortable filter filterElement={numberBetweenFilterTemplate} showFilterMatchModes={false} />
            <Column field="cargo_capacity" header="Cargo Capacity" style={{minWidth: defaultWidth}} sortable filter filterElement={numberBetweenFilterTemplate} showFilterMatchModes={false} />
            <Column field="consumables" header="Consumables" style={{minWidth: defaultWidth}} sortable filter filterElement={textFilterTemplate} />
            <Column field="hyperdrive_rating" header="Hyperdrive Rating" 
                    style={{minWidth: defaultWidth}} sortable filter filterElement={numberBetweenFilterTemplate} showFilterMatchModes={false} />
            <Column field="MGLT" header="MGLT" style={{minWidth: defaultWidth}} sortable filter filterElement={numberBetweenFilterTemplate} showFilterMatchModes={false} />
            <Column field="starship_class" header="Starship Class" style={{minWidth: defaultWidth}} sortable filter filterElement={textFilterTemplate} />
            <Column field="created" header="Created" dataType="date" style={{minWidth: defaultWidth}} sortable filter
                    filterElement={dateBetweenFilterTemplate}
                    body={(rowData) => formatDateCustom(rowData, 'created')}/>
            <Column field="edited" header="Edited" dataType="date"  style={{minWidth: defaultWidth}} sortable filter filterElement={dateBetweenFilterTemplate}
                    body={(rowData) => formatDateCustom(rowData, 'edited')}/>
        </DataTable>
    );
}

export default StarshipsTable;