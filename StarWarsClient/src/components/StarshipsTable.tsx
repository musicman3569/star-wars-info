import { useState, useEffect } from 'react';
import { FetchData } from '../utils/KendoUtils';
import { 
    Grid, 
    GridColumn as Column
} from '@progress/kendo-react-grid';

function StarshipsTable() {
    const defaultWidth = "200px";
    const [starships, setStarships] = useState<any[]>([]);

    useEffect(() => {
        FetchData('starships', ['created', 'edited'], setStarships);
    }, [])
    
    return <>
        <h1>Starships</h1>
        <Grid
          data={starships}
          dataItemKey="starship_id"
          autoProcessData={true}
          sortable={true}
          filterable={true}
          pageable={true}
          editable={{ mode: 'incell' }}
        >
            <Column field="name" title="Name" editor="text" width={defaultWidth}/>
            <Column field="model" title="Model" editor="text" width={defaultWidth}/>
            <Column field="manufacturer" title="Manufacturer" editor="text" width={defaultWidth}/>
            <Column field="cost_in_credits" title="Cost In Credits" editor="numeric" width={defaultWidth} format="{0:n}"/>
            <Column field="length" title="Length" editor="numeric" width={defaultWidth} format="{0:n}" />
            <Column field="max_atmosphering_speed" title="Max Atmosphering Speed" editor="numeric" width={defaultWidth} format="{0:n} km"/>
            <Column field="crew" title="Crew" editor="numeric" width={defaultWidth} format="{0:n}"/>
            <Column field="passengers" title="Passengers" editor="numeric" width={defaultWidth} format="{0:n}"/>
            <Column field="cargo_capacity" title="Cargo Capacity" editor="numeric" width={defaultWidth} format="{0:n}"/>
            <Column field="consumables" title="Consumables" editor="text" width={defaultWidth}/>
            <Column field="hyperdrive_rating" title="Hyperdrive Rating" editor="numeric" width={defaultWidth} format="{0:n1}" />
            <Column field="MGLT" title="MGLT" editor="numeric" width={defaultWidth} format="{0:n}"/>
            <Column field="starship_class" title="Starship Class" editor="text" width={defaultWidth}/>
            <Column field="created" title="Created" editor="date" width={defaultWidth} format="{0:yyyy-MM-dd hh:mm:ss}" />
            <Column field="edited" title="Edited" editor="date" width={defaultWidth} format="{0:yyyy-MM-dd hh:mm:ss}" />
        </Grid>
    </>;
}

export default StarshipsTable;