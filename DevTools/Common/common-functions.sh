#!/bin/bash

{
    # ANSI colors for coloring terminal output
    declare COLOR_RED='\033[0;31m'
    declare COLOR_NONE='\033[0m'
    declare COLOR_GREEN='\033[0;32m'
    declare COLOR_BLUE='\033[0;34m'
    
    # Get paths relative to the script itself
    declare SCRIPT_INVOKED_NAME="${BASH_SOURCE[${#BASH_SOURCE[@]}-1]}"
    declare SCRIPT_INVOKED_PATH="$( dirname "${SCRIPT_INVOKED_NAME}" )"
    declare SCRIPT_PATH="$( cd "${SCRIPT_INVOKED_PATH}"; pwd )"
    
    # Prefix to use for logging output in the log_output function
    # Gets the name of the script that was called, without the file extension.
    declare LOG_OUTPUT_PREFIX="$(basename -- "$0")"
}

# Output nice logging messages that prefix which step the script is on
# in the specified output color (optional).
log_output() {
    local output_message="$1" # Message to output
    local output_color="$2" # Optional ANSI color, empty for default color
    local output_prefix="$3" # Optional prefix, defaults to LOG_OUTPUT_PREFIX
    
    # Use the calling function's name as the prefix if not provided,
    # or fall back to the name of the script if not available
    if [ -z ${output_prefix} ]; then
        output_prefix="${FUNCNAME[1]:-${LOG_OUTPUT_PREFIX}}"
    fi
    
    echo -e "${output_color}[$output_prefix]: ${output_message}${COLOR_NONE}"
}

# Checks if sudo is needed and available, and return the cmd as
# a string if it is so it can be used when adding the host entries
get_sudo_cmd() {
    if [ "$(id -u)" -ne 0 ]; then
        if command -v sudo &> /dev/null; then
            echo "sudo"
        fi
    fi
}

# Function to call if there is an error writing to a permissions restricted file.
# Display OS-specific instructions on resolving it.
output_permissions_error() {
    log_output "[ERROR] You do not have permission to modify $HOSTS_FILE." "${COLOR_RED}"
    case "$OS" in
        Linux)
            log_output "Please run this script as root or with sudo." "${COLOR_RED}"
            ;;
        Darwin)
            log_output "Please run this script as root or with sudo." "${COLOR_RED}"
            ;;
        CYGWIN*|MINGW32*|MSYS*|MINGW*)
            log_output "Please open your BASH program (e.g. Git Bash) as administrator to run this command." "${COLOR_RED}"
            ;;
    esac
    exit 1
}

# Generate a secure random password of the specified length from
# the listed set of characters in the tr command
# If no length is specified, it defaults to 32 characters.
# If no character set is specified, it defaults to alphanumeric characters.
# Usage: generate_password [<length>] [<character_set>]
# Example: generate_password 32 "A-Za-z0-9!@#$%^&"
generate_password() {
    local length="${1:-32}"  # Default length is 32 characters if not specified
    local password_chars="${2:-A-Za-z0-9}"  # Default character set is alphanumeric
    LC_CTYPE=C LC_COLLATE=C tr -dc "$password_chars" < /dev/urandom | head -c $length
    echo
}