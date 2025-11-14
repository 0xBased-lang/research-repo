#!/bin/bash
# PostToolUse hook - Runs after tool execution
# FIXED VERSION - Addresses critical edge cases

set -euo pipefail  # Strict error handling

TOOL_NAME="$1"
TOOL_ARGS="$2"
TOOL_RESULT="${3:-}"

# Configuration
HOOK_TIMEOUT=30
FORMAT_TIMEOUT=30
TEST_TIMEOUT=60

# Function: Extract file_path portably
extract_file_path() {
    local args="$1"

    if command -v jq &> /dev/null; then
        echo "$args" | jq -r '.file_path // empty' 2>/dev/null && return
    fi

    if command -v python3 &> /dev/null; then
        echo "$args" | python3 -c "import sys, json; print(json.loads(sys.stdin.read()).get('file_path', ''))" 2>/dev/null && return
    fi

    echo "$args" | grep -o '"file_path"[[:space:]]*:[[:space:]]*"[^"]*"' | sed 's/.*"file_path"[[:space:]]*:[[:space:]]*"//;s/"$//' 2>/dev/null && return

    echo ""
}

# Only process file edits
if [ "$TOOL_NAME" != "Edit" ] && [ "$TOOL_NAME" != "Write" ]; then
    exit 0
fi

FILE_PATH=$(extract_file_path "$TOOL_ARGS")

if [ -z "$FILE_PATH" ]; then
    exit 0  # Can't determine file, skip
fi

# Normalize path
FILE_PATH=$(echo "$FILE_PATH" | tr '\\' '/')

# Verify file exists before processing
if [ ! -f "$FILE_PATH" ]; then
    echo "⚠️  File not found, skipping post-processing: $FILE_PATH"
    exit 0
fi

# Skip formatting for non-code files
FILE_EXT="${FILE_PATH##*.}"
case "$FILE_EXT" in
    md|txt|json|yaml|yml|toml|ini|conf|lock)
        # Skip formatting for config/doc files
        exit 0
        ;;
esac

echo "🎨 Post-processing: $FILE_PATH"

# Find project root (for package.json, pyproject.toml, etc.)
PROJECT_ROOT=$(git rev-parse --show-toplevel 2>/dev/null || pwd)

# TypeScript/JavaScript formatting
if [[ "$FILE_EXT" =~ ^(ts|tsx|js|jsx|mjs|cjs)$ ]]; then
    if command -v npx &> /dev/null && [ -f "$PROJECT_ROOT/package.json" ]; then
        echo "  📝 Running Prettier..."

        # Run with timeout, capture output
        if timeout $FORMAT_TIMEOUT npx prettier --write "$FILE_PATH" 2>&1 | grep -v "^$"; then
            echo "  ✅ Prettier completed"
        else
            EXIT_CODE=$?
            if [ $EXIT_CODE -eq 124 ]; then
                echo "  ⏱️  Prettier timed out (non-blocking)"
            else
                echo "  ⚠️  Prettier failed (non-blocking)"
            fi
        fi

        echo "  🔍 Running ESLint..."
        if timeout $FORMAT_TIMEOUT npx eslint --fix "$FILE_PATH" 2>&1 | grep -v "^$"; then
            echo "  ✅ ESLint completed"
        else
            EXIT_CODE=$?
            if [ $EXIT_CODE -eq 124 ]; then
                echo "  ⏱️  ESLint timed out (non-blocking)"
            else
                echo "  ⚠️  ESLint failed (non-blocking)"
            fi
        fi
    fi
fi

# Python formatting
if [[ "$FILE_EXT" == "py" ]]; then
    if command -v black &> /dev/null; then
        echo "  📝 Running Black..."

        if timeout $FORMAT_TIMEOUT black "$FILE_PATH" 2>&1 | grep -v "^$"; then
            echo "  ✅ Black completed"
        else
            EXIT_CODE=$?
            if [ $EXIT_CODE -eq 124 ]; then
                echo "  ⏱️  Black timed out (non-blocking)"
            else
                echo "  ⚠️  Black failed (non-blocking)"
            fi
        fi
    fi
fi

# IMPORTANT: Don't run tests synchronously - too slow!
# Tests should run in CI/CD, not on every file change
#
# If you want to run tests, do it in background:
# (cd "$PROJECT_ROOT" && npm run test:file "$FILE_PATH" &> /tmp/test-$$.log &)
#
# Or skip entirely and rely on CI

echo "✅ Post-processing complete"

exit 0
