import { PrimeReactProvider } from 'primereact/api';
import 'primereact/resources/themes/soho-dark/theme.css'
import 'primeicons/primeicons.css'
import 'primeflex/primeflex.css'
import './App.css'
import SwapiDataTable from './components/SwapiDataTable.tsx';
import starshipsIcon from './assets/icon_starships.svg'
import SectionIcon from './components/SectionIcon';
import ModelStarship from "./models/ModelStarship.ts";

function App() {
    return (
        <PrimeReactProvider>
            <h1>
                <SectionIcon src={starshipsIcon} alt="Starships Icon" />
                Starships
            </h1>
            <SwapiDataTable modelSpec={ModelStarship} />
        </PrimeReactProvider>
    )
}

export default App
