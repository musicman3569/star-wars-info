# Stars Wars Info

Stars Wars Info site is an information resource about Starships from the Star Wars films.
This includes all the details you could ever want to know, including models, size, passenger
capacity, cost, crew size, hyperdrive ratings, and more!  All this is presented in a searchable
and sortable information site, so you can impress your friends with your astute
(and perhaps excessive) trivia knowledge.

![Star Wars Ship Models](StarWarsClient/public/starwars_ships.png)

## System Requirements:
- Docker/Docker Compose
- BASH (for Windows, Git Bash works and is usually installed alongside Git)
- Works on Windows, Linux, and MacOS

## Quick Start
- To get started, go to the project root directory and run `DevTools/initialize-environment.sh`
- When the script completes, the services should be started with the following URLS:
    - https://starwarsinfo.test:8080/ -  The main website written in React.js
    - https://starwarsinfo.test:8081/ - The ASP.NET Core API backend
    - https://starwarsinfo.test:8082/ - The Keycloak SSO service, used for login and administering the SSO service.
- When you first visit the site, you will be prompted to log in. Register an email/password to gain access (email verification is off for easier testing)

## Project Structure

At the root of the solution there are several project folders:
- __/DevTools__: Scripts/tools for developers that simplify environment management.
- __/Keycloak__: Template files to import/bootstrap the Keycloak identity and access management service.
- __/Postgres__: Database initialization files for the Postgres database service.
- __/SSL__: PEM-formatted SSL certificate and key files for use with API, client, & Keycloak services.
- __/StarWarsClient__: The React.js front end client application (via Vite)
- __/StarWarsInfo__: The backend API project, written in .NET as a REST provider
- __/StarWarsInfoTests__: Unit tests for the backend API project

## Developer Resources and Links
- [Star Wars API](https://swapi.info//)
- [Keycloak, Importing Realm on Startup](https://www.keycloak.org/nightly/server/containers#_importing_a_realm_on_startup)
- [Keycloak, Full Env Variables Reference](https://www.keycloak.org/server/all-config)
- [Postgres, Initialization Scripts](https://hub.docker.com/_/postgres#initialization-scripts)