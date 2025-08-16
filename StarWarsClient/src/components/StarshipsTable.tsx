import { useState, useEffect } from 'react';
import { FetchData } from '../utils/StarWarsInfoClient';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';

function StarshipsTable() {
    const defaultWidth = "200px";
    const [starships, setStarships] = useState<any[]>([]);

    useEffect(() => {
        FetchData('starships', [/*'created', 'edited'*/], setStarships);
    }, []);
    
    return (
        <DataTable
            value={starships}
            paginator={false}
            rows={10}
            //rowHover={true}
            scrollable={true}
            scrollHeight="calc(100vh - 100px)"
            dataKey="starship_id"
            sortMode="single"
            sortField="name"
            
        >
            <Column field="name" header="Name" editor="text" style={{minWidth:defaultWidth}}/>
            <Column field="model" header="Model" editor="text" style={{minWidth:defaultWidth}}/>
            <Column field="manufacturer" header="Manufacturer" editor="text" style={{minWidth:defaultWidth}}/>
            <Column field="cost_in_credits" header="Cost In Credits" editor="numeric" style={{minWidth:defaultWidth}} />
            <Column field="length" header="Length" editor="numeric" style={{minWidth:defaultWidth}} />
            <Column field="max_atmosphering_speed" header="Max Atmosphering Speed" editor="numeric" style={{minWidth:defaultWidth}} />
            <Column field="crew" header="Crew" editor="numeric" style={{minWidth:defaultWidth}} />
            <Column field="passengers" header="Passengers" editor="numeric" style={{minWidth:defaultWidth}} />
            <Column field="cargo_capacity" header="Cargo Capacity" editor="numeric" style={{minWidth:defaultWidth}} />
            <Column field="consumables" header="Consumables" editor="text" style={{minWidth:defaultWidth}}/>
            <Column field="hyperdrive_rating" header="Hyperdrive Rating" editor="numeric" style={{minWidth:defaultWidth}} />
            <Column field="MGLT" header="MGLT" editor="numeric" style={{minWidth:defaultWidth}} />
            <Column field="starship_class" header="Starship Class" editor="text" style={{minWidth:defaultWidth}}/>
            <Column field="created" header="Created" editor="date" style={{minWidth:defaultWidth}} />
            <Column field="edited" header="Edited" editor="date" style={{minWidth:defaultWidth}} />
        </DataTable>
    );
}

export default StarshipsTable;