import { Menu } from 'primereact/menu';

function SwapiMenu() {
    const keycloakUrl = import.meta.env.VITE_KEYCLOAK_URL;
    const redirectUrl = encodeURIComponent(window.location.origin);
    
    let items = [
        { 
            label: 'My Account', 
            icon: 'pi pi-user', 
            url: `${keycloakUrl}/realms/starwarsinfo/account/` 
        },
        { 
            label: 'Logout', 
            icon: 'pi pi-sign-out',
            url: `${keycloakUrl}/realms/starwarsinfo/protocol/openid-connect/logout?redirect_uri=${redirectUrl}`
        }
    ];

    return (
        <Menu model={items} className="ml-auto"/>
    )
}

export default SwapiMenu;