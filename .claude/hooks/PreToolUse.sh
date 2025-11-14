#!/bin/bash
# PreToolUse hook - Runs before Claude executes any tool

TOOL_NAME="$1"
TOOL_ARGS="$2"

# Security checks
if [ "$TOOL_NAME" = "Write" ] || [ "$TOOL_NAME" = "Edit" ]; then
    FILE_PATH=$(echo "$TOOL_ARGS" | grep -oP '(?<="file_path": ")[^"]+' || echo "")

    # Block .env file modifications
    if [[ "$FILE_PATH" =~ ^\.env$ ]] || [[ "$FILE_PATH" =~ ^\.env\. ]]; then
        if [[ ! "$FILE_PATH" =~ \.env\.example$ ]]; then
            echo "❌ BLOCKED: Attempted to modify .env file"
            echo "💡 Only .env.example files should be committed"
            exit 1
        fi
    fi

    # Block sensitive paths
    if [[ "$FILE_PATH" =~ (credentials|secrets|private|\.pem|\.key)$ ]]; then
        echo "❌ BLOCKED: Attempted to modify sensitive file: $FILE_PATH"
        exit 1
    fi
fi

# Allow the operation
exit 0
