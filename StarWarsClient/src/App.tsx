import { PrimeReactProvider } from 'primereact/api';
import 'primereact/resources/themes/soho-dark/theme.css'
import 'primeicons/primeicons.css'
import 'primeflex/primeflex.css'
import './App.css'
import SwapiDataTable from './components/SwapiDataTable.tsx';
import starshipsIcon from './assets/icon_starships.svg';
import peopleIcon from './assets/icon_people.svg';
import speciesIcon from './assets/icon_species.svg';
import vehiclesIcon from './assets/icon_vehicles.svg';
import planetsIcon from './assets/icon_planets.svg';
import filmsIcon from './assets/icon_films.svg';
import SectionIcon from './components/SectionIcon';
import ModelStarship from "./models/ModelStarship.ts";
import { ReactKeycloakProvider } from "@react-keycloak/web";
import keycloak from "./utils/KeycloakInit"
import {TabPanel, TabView} from "primereact/tabview";
import {useState} from "react";
import SwapiMenu from "./components/SwapiMenu.tsx";

function App() {
    const [activeTabIndex, setActiveTabIndex] = useState(0);
    
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
                <TabView activeIndex={activeTabIndex} onTabChange={(e) => setActiveTabIndex(e.index)}>
                    <TabPanel className="border-round-top-xl mr-2" header={<>
                        <SectionIcon src={starshipsIcon} alt="Starships Icon"/>
                        <span>Starships</span>
                    </>}>
                        <SwapiDataTable modelSpec={ModelStarship}/>
                    </TabPanel>
                    <TabPanel className="border-round-top-xl mr-2" header={<>
                        <SectionIcon src={peopleIcon} alt="People Icon"/>
                        <span>People</span>
                    </>}>
                        <p>People here</p>
                    </TabPanel>
                    <TabPanel className="border-round-top-xl mr-2" header={<>
                        <SectionIcon src={speciesIcon} alt="Species Icon"/>
                        <span>Species</span>
                    </>}>
                        <p>Species Here</p>
                    </TabPanel>
                    <TabPanel className="border-round-top-xl mr-2" header={<>
                        <SectionIcon src={planetsIcon} alt="Planets Icon"/>
                        <span>Planets</span>
                    </>}>
                        <p>Planets Here</p>
                    </TabPanel>
                    <TabPanel className="border-round-top-xl mr-2" header={<>
                        <SectionIcon src={vehiclesIcon} alt="Vehicles Icon"/>
                        <span>Vehicles</span>
                    </>}>
                        <p>Vehicles Here</p>
                    </TabPanel>
                    <TabPanel className="border-round-top-xl mr-2" header={<>
                        <SectionIcon src={filmsIcon} alt="Films Icon"/>
                        <span>Films</span>
                    </>}>
                        <p>Films Here</p>
                    </TabPanel>
                    <TabPanel className="border-round-top-xl mr-2" headerClassName="tab-rightmost" header={<>
                        <i className="pi pi-bars mr-2"></i>
                    </>}>
                        <div className="flex">
                            <SwapiMenu />
                        </div>
                    </TabPanel>
                </TabView>
            </PrimeReactProvider>
        </ReactKeycloakProvider>
    );
}

export default App;
