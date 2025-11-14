#!/bin/bash
# PreToolUse hook - Runs before Claude executes any tool
# FIXED VERSION - Addresses critical edge cases

set -euo pipefail  # Strict error handling

TOOL_NAME="$1"
TOOL_ARGS="$2"

# Timeout for entire hook (30 seconds)
HOOK_TIMEOUT=30

# Function: Extract file_path portably (works on macOS and Linux)
extract_file_path() {
    local args="$1"

    # Try jq first (most reliable)
    if command -v jq &> /dev/null; then
        echo "$args" | jq -r '.file_path // empty' 2>/dev/null && return
    fi

    # Fallback: Python (very portable)
    if command -v python3 &> /dev/null; then
        echo "$args" | python3 -c "import sys, json; print(json.loads(sys.stdin.read()).get('file_path', ''))" 2>/dev/null && return
    fi

    # Fallback: Basic grep + sed (POSIX compliant)
    echo "$args" | grep -o '"file_path"[[:space:]]*:[[:space:]]*"[^"]*"' | sed 's/.*"file_path"[[:space:]]*:[[:space:]]*"//;s/"$//' 2>/dev/null && return

    # If all fail, return empty
    echo ""
}

# Security checks for Write and Edit operations
if [ "$TOOL_NAME" = "Write" ] || [ "$TOOL_NAME" = "Edit" ]; then
    FILE_PATH=$(extract_file_path "$TOOL_ARGS")

    if [ -z "$FILE_PATH" ]; then
        # Can't extract path - allow but log warning
        echo "⚠️  WARNING: Could not extract file_path from arguments"
        exit 0
    fi

    # Normalize path (handle Windows backslashes, resolve relative paths)
    FILE_PATH=$(echo "$FILE_PATH" | tr '\\' '/')

    # Get basename for checking
    FILE_BASENAME=$(basename "$FILE_PATH" 2>/dev/null || echo "$FILE_PATH")

    # Check 1: Block .env files (except .env.example)
    if [[ "$FILE_BASENAME" == ".env" ]] || [[ "$FILE_BASENAME" =~ ^\.env\. ]]; then
        if [[ "$FILE_BASENAME" != ".env.example" ]]; then
            echo "❌ BLOCKED: Attempted to modify .env file: $FILE_PATH"
            echo "💡 Only .env.example files should be committed"
            echo "💡 .env files contain secrets and should never be version controlled"
            exit 1
        fi
    fi

    # Check 2: Block path traversal attempts
    if [[ "$FILE_PATH" =~ \.\./|/\.\./|^\.\. ]]; then
        echo "❌ BLOCKED: Path traversal detected: $FILE_PATH"
        echo "💡 Paths should not contain ../ sequences"
        exit 1
    fi

    # Check 3: Block sensitive file patterns
    if [[ "$FILE_BASENAME" =~ (credentials|secrets|private)$ ]] || \
       [[ "$FILE_BASENAME" =~ \.(pem|key|p12|pfx)$ ]] || \
       [[ "$FILE_PATH" =~ ^/etc/|^/root/|^~/.ssh/ ]]; then
        echo "❌ BLOCKED: Attempted to modify sensitive file: $FILE_PATH"
        echo "💡 Files matching patterns: credentials, secrets, private, .pem, .key"
        echo "💡 Or system paths: /etc/, /root/, ~/.ssh/"
        exit 1
    fi

    # Check 4: Block modification of hook files (prevent self-modification attacks)
    if [[ "$FILE_PATH" =~ \.claude/hooks/ ]] || [[ "$FILE_PATH" =~ \.claude/settings\.json$ ]]; then
        echo "❌ BLOCKED: Attempted to modify hook/settings: $FILE_PATH"
        echo "💡 Hook files should only be modified manually for security"
        exit 1
    fi

    # Check 5: Warn on large files (> 1MB) to prevent DOS
    if [ -f "$FILE_PATH" ]; then
        FILE_SIZE=$(stat -f%z "$FILE_PATH" 2>/dev/null || stat -c%s "$FILE_PATH" 2>/dev/null || echo "0")
        if [ "$FILE_SIZE" -gt 1048576 ]; then
            echo "⚠️  WARNING: Large file modification ($(( FILE_SIZE / 1024 / 1024 ))MB): $FILE_PATH"
            echo "💡 This may take a while. Consider breaking into smaller changes."
            # Allow but warn
        fi
    fi
fi

# All checks passed
exit 0
