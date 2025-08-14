#!/bin/bash
# This script is a wrapper for the "dotnet ef database" commands.
# See https://learn.microsoft.com/en-us/ef/core/cli/dotnet#common-options
#
# It is needed in order to set the database connection automatically from
# the environment variables defined in the .env file, as well aas
# automatically setting the common options for the project.
# 
# It will pass all the arguments through, so you can use it with the same
# options for each command (just not the common options since they are
# pre-defined). 

set -e

{
    # Get the path to the directory where this script is located so other 
    # scripts can be sourced from this script relative to it's own path.
    declare SCRIPT_PATH="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
}

# Function to display usage instructions
show_usage() {
    echo -e "\nProject wrapper for: dotnet ef database ..."
    echo "Usage: $0 <command> [options]"
    echo "Commands:"
    echo "  update      Updates the database to the latest migration (or a specified one)"
    echo "  drop        Removes the database"
    echo "  script      Generates a SQL script from migrations"
    echo ""
    echo "Example: $0 update [args...]"
    echo "See https://learn.microsoft.com/en-us/ef/core/cli/dotnet#dotnet-ef-database-update"
    echo ""
    exit 1
}

# Check if the required command argument is provided
if [ -z "$1" ] || [[ ! "$1" =~ ^(update|drop|script)$ ]]; then
    show_usage
fi

# Import environment variables
source "$SCRIPT_PATH/../.env"

# Initialize global script variables.
export ConnectionStrings__DefaultConnection="Host=localhost;Port=${LOCALDEV__POSTGRES_PORT:-5432};Database=${POSTGRES_DB};Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD};Search Path=starwarsinfo"

/usr/share/dotnet/dotnet ef database "$1" \
    --project StarWarsInfo/StarWarsInfo.csproj \
    --startup-project StarWarsInfo/StarWarsInfo.csproj \
    --context StarWarsInfo.Data.AppDbContext \
    --configuration Debug \
    "${@:2}"