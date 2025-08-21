import { PrimeReactProvider } from 'primereact/api';
import 'primereact/resources/themes/soho-dark/theme.css'
import 'primeicons/primeicons.css'
import 'primeflex/primeflex.css'
import './App.css'
// import SwapiDataTable from './components/SwapiDataTable.tsx';
import starshipsIcon from './assets/icon_starships.svg'
import SectionIcon from './components/SectionIcon';
// import ModelStarship from "./models/ModelStarship.ts";
import { ReactKeycloakProvider } from "@react-keycloak/web";
import keycloak from "./utils/KeycloakInit"

function App() {
    return (
        <ReactKeycloakProvider 
            authClient={keycloak}
            initOptions={{
                onLoad: 'login-required',
                pkceMethod: 'S256',
                checkLoginIframe: false,
            }}
        >
            <PrimeReactProvider>
                <h1>
                    <SectionIcon src={starshipsIcon} alt="Starships Icon"/>
                    Starships
                </h1>
                {/*<SwapiDataTable modelSpec={ModelStarship}/>*/}
            </PrimeReactProvider>
        </ReactKeycloakProvider>
    );
}

export default App;
