# Stars Wars Info

The Stars Wars Info project is an information resource about Starships from the Star Wars films.
This includes all the details you could ever want to know, including models, size, passenger
capacity, cost, crew size, hyperdrive ratings, and more!  All this is presented in a searchable
and sortable information site, so you can impress your friends with your astute
(and perhaps excessive) trivia knowledge.

![Star Wars Ship Models](StarWarsClient/public/starwars_ships.png)

## System Requirements:
- Docker/Docker Compose (Docker Desktop for Windows)
- BASH (for Windows, Git Bash works and is usually installed alongside Git)
- Works on Windows, Linux, and MacOS!

## Quick Start
- To get started, go to the project root directory and run in a BASH terminal run `DevTools/initialize-environment.sh`. 
  This only needs to be run once when you first clone the project, and automatically
  handles all the environment setup for you.
- Run `docker compose up --build --detach` to start the services, or open the solution in your favorite IDE/editor 
  and run the Docker Compose build option.  
- When the build completes (Keycloak can take a few minutes the first time), the services should be available at the following URLS:
    - https://host.docker.internal:8080/ -  The main website written in React.js
    - https://host.docker.internal:8081/ - The ASP.NET Core API backend and Swagger docs
    - https://host.docker.internal:8082/ - The Keycloak SSO service, used for login and administering the SSO service.
- When you first visit the site, you will be prompted to log in. Register an email/password to gain access (email verification is off for easier testing).
- On the first page, it will detect the empty database and prompt you to import the initial data.
  Click yes to automatically import the data from the 3rd party SWAPI REST API into the application's database.

## Project Structure Overview

At the root of the solution there are several project folders:
- __/DevTools__: Scripts/tools for developers that simplify environment management.
- __/Keycloak__: Template files to import/bootstrap the Keycloak identity and access management service.
- __/Postgres__: Database initialization files for the Postgres database service.
- __/SSL__: PEM-formatted SSL certificate and key files for use with API, client, & Keycloak services.
- __/StarWarsClient__: The React.js front end client application (via Vite)
- __/StarWarsInfo__: The backend API project, written in .NET as a REST provider
- __/StarWarsInfoTests__: Unit tests for the backend API project

## Single-Sign-On Administration
Star Wars Info uses Keycloak as its SSO service. Keycloak is a powerful and flexible identity and access 
management service made by Red Hat. It is opensource and free to use for commercial and non-commercial 
projects. Both OAuth2 and SAML2 protocols are supported for authentication and authorization, and
it also allows federated login from providers such as Google, Microsoft, and Facebook.

- The Keycloak admin console is available at https://host.docker.internal:8082 and allows
  you to manage Single-Sign-On (SSO) users, roles, permissions, login themes, etc.
- The credentials for the `master` realm admin account are in your .env file from the values:
  - KC_BOOTSTRAP_ADMIN_USERNAME
  - KC_BOOTSTRAP_ADMIN_PASSWORD
- The realm used by the application is called `starwarsinfo` -- the first time you log in you will 
  be in the `master` realm.  Click `Manage realms` > `starwarsinfo` to switch realms.

## Developer Resources and Links
- [Star Wars API](https://swapi.info//)
- [Keycloak, Importing Realm on Startup](https://www.keycloak.org/nightly/server/containers#_importing_a_realm_on_startup)
- [Keycloak, Full Env Variables Reference](https://www.keycloak.org/server/all-config)
- [Postgres, Initialization Scripts](https://hub.docker.com/_/postgres#initialization-scripts)