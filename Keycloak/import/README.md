# Keycloak Docker Import Files

- This directory contains client/group/realm import files into a Keycloak instance.
- They are automatically imported when the container starts up if
the KEYCLOAK_ENTRYPOINT_ARG variable in the .env file is set to
"--import-realm".
- This allows automatically seeding the necessary 
OAuth client and realm configurations that the app is configured to use.

## Creating/Updating Import Files

- Get the Keycloak instance running and login into the admin console by
  going to https://{KEYCLOAK_HOSTNAME}:{KEYCLOAK_PORT} (see .env file)
- Use the values from KC_BOOTSTRAP_ADMIN_USERNAME and KC_BOOTSTRAP_ADMIN_PASSWORD to login
- Make the desired changes (add/edit realms, clients, etc.)
- Export the realm from the admin console:
  - Go to Realm Settings -> Action (dropdown) -> Partial Export
  - Enable the options to "Include groups and roles" and "Include clients"
  - Click Export
  - Move the downloaded realm-export.json file to this directory

- Copy the JSON files into this directory

## Sources
- https://www.keycloak.org/server/containers#_importing_a_realm_on_startup
- https://www.keycloak.org/server/importExport#_importing_and_exporting_by_using_the_admin_console
