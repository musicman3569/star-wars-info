import { PrimeReactProvider } from 'primereact/api';
import 'primereact/resources/themes/soho-dark/theme.css'
import './App.css'
import StarshipsTable from './components/StarshipsTable';

function App() {
    return (
        <PrimeReactProvider>
            <h1>Starships</h1>
            <StarshipsTable />
        </PrimeReactProvider>
    )
}

export default App
