import { Grid, GridColumn as Column } from '@progress/kendo-react-grid';
import starships from '../data/Starships'

function StarshipsTable() {
    const defaultWidth = "200px";
    
    return <>
        <h1>Starships</h1>
        <Grid
          data={starships}
          autoProcessData={true}
          sortable={true}
          filterable={true}
          pageable={true}
        >
            <Column field="name" title="Name" editor="text" width={defaultWidth}/>
            <Column field="model" title="Model" editor="text" width={defaultWidth}/>
            <Column field="manufacturer" title="Manufacturer" editor="text" width={defaultWidth}/>
            <Column field="cost_in_credits" title="Cost In Credits" editor="text" width={defaultWidth}/>
            <Column field="length" title="Length" editor="text" width={defaultWidth}/>
            <Column field="max_atmosphering_speed" title="Max Atmosphering Speed" editor="text" width={defaultWidth}/>
            <Column field="crew" title="Crew" editor="text" width={defaultWidth}/>
            <Column field="passengers" title="Passengers" editor="text" width={defaultWidth}/>
            <Column field="cargo_capacity" title="Cargo Capacity" editor="text" width={defaultWidth}/>
            <Column field="consumables" title="Consumables" editor="text" width={defaultWidth}/>
            <Column field="hyperdrive_rating" title="Hyperdrive Rating" editor="text" width={defaultWidth}/>
            <Column field="MGLT" title="MGLT" editor="text" width={defaultWidth}/>
            <Column field="starship_class" title="Starship Class" editor="text" width={defaultWidth}/>
            <Column field="created" title="Created" editor="text" width={defaultWidth}/>
            <Column field="edited" title="Edited" editor="text" width={defaultWidth}/>
        </Grid>
    </>;
}

export default StarshipsTable;