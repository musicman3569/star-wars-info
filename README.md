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
- Works as-is on Windows, Linux, and MacOS!

## Quick Start
- To get started, go to the project root directory and run in a BASH terminal run `DevTools/initialize-environment.sh`. 
  This only needs to be run once when you first clone the project, and automatically
  handles all the environment setup for you.
- Run `docker compose up --build --detach` to start the services, or open the solution in your favorite IDE/editor 
  and run the Docker Compose build option. That's it!

## Logging In and Using the Application
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
- The Keycloak realm is defined in the `Keycloak/import/realm-export.json` file.  This file is used to
  bootstrap the Keycloak realm on startup.  You can modify this file to customize the realm
  for your own needs by exporting it from the admin console or hand-editing it.
  - See the README in the `Keycloak/import` folder for more information.

## Customizing the Application Further
The streamlined initialization is done by the `DevTools/initialize-environment.sh` script,
  and is driven entirely by the environment variables defined in the `.env` file.
- The minimum amount of variables are used and the rest are generated.
- Passwords are auto-generated based on the .env.template annotations and are
  cryptographically secure, avoiding the need for risky default/shared passwords.
- All the env vars and generated values are then used for configuration of all
  the services, data seeding, etc., ensuring application consistency without complex configuration.
- A custom .env template file can be used prior to running `initialize-environment.sh` to
  customize the environment variables for things like different hostnames/ports.  

Aside from the .env file values, there are a few other files that are seeded into the build:

- Custom PEM-formatted SSL certificates can be placed in the `SSL` directory, and should be named
  to match the hostnames used in the .env file, followed by the extensions `.crt` and `.key`.
- The initial database schema SQL scripts are defined in the `Postgres/docker-entrypoint-initdb.d/` file.
  This file is used to initialize the necessary schemas.  NOTE: It __does not__ include data seeding,
  which is done at runtime by the application itself. 

## Entity Framework Migrations
- The database is managed using a code-first approach in Entity Framework.
- Migrations are automatically applied when the starwarsinfo docker service starts.
- Because the database connection is defined via docker compose from .env variables
  instead of the local machine, a wrapper script is provided to run `dotnet ef` commands This allows the solution to be IDE/platform agnostic. 
  on the docker image. This allows the solution to be more IDE/platform agnostic.
  - Just use `DevTools/ef-cli-wrapper.sh` as a drop-in replacement for `dotnet ef` and
    follow it with the usual arguments, or run it with no arguments to see a convenient list of ef commands..

## Developer Resources and Links
- [Star Wars API](https://swapi.info//)
- [Keycloak, Importing Realm on Startup](https://www.keycloak.org/nightly/server/containers#_importing_a_realm_on_startup)
- [Keycloak, Full Env Variables Reference](https://www.keycloak.org/server/all-config)
- [Postgres, Initialization Scripts](https://hub.docker.com/_/postgres#initialization-scripts)