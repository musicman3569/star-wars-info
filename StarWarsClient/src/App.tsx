import { PrimeReactProvider } from 'primereact/api';
import 'primereact/resources/themes/soho-dark/theme.css'
import './App.css'
import StarshipsTable from './components/StarshipsTable';
import starshipsIcon from './assets/icon_starships.svg'
import SectionIcon from './components/SectionIcon';

function App() {
    return (
        <PrimeReactProvider>
            <h1>
                <SectionIcon src={starshipsIcon} alt="Starships Icon" />
                Starships
            </h1>
            <StarshipsTable />
        </PrimeReactProvider>
    )
}

export default App
