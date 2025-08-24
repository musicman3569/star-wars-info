#!/bin/bash
# This is the main setup script for setting up your environment for the first time.
# Once your environment is working, do you not need to use this any more and can instead
# use individial utility scripts from the DevTools.

set -e

# Initialize global script variables.
{
    SCRIPT_DIR="$(dirname "$0")"
}

"$SCRIPT_DIR"/env_run_all_checks.sh
"$SCRIPT_DIR"/keycloak-prep-import.sh
"$SCRIPT_DIR"/hosts-add-entries.sh
"$SCRIPT_DIR"/ssl-generate-certs.sh

cd "$SCRIPT_DIR"/..
dotnet tool restore
