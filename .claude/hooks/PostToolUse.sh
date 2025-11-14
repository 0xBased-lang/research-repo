#!/bin/bash
# Post-tool-use hook for quality enforcement

TOOL_NAME="$1"
TOOL_ARGS="$2"
TOOL_RESULT="$3"

# Run linter after file edits
if [ "$TOOL_NAME" = "Edit" ] || [ "$TOOL_NAME" = "Write" ]; then
    FILE_PATH=$(echo "$TOOL_ARGS" | grep -oP '(?<="file_path": ")[^"]+' || echo "")

    if [ -n "$FILE_PATH" ]; then
        echo "🎨 Auto-formatting $FILE_PATH..."

        # TypeScript/JavaScript files
        if [[ "$FILE_PATH" =~ \.(ts|tsx|js|jsx)$ ]]; then
            if command -v npx &> /dev/null && [ -f "package.json" ]; then
                npx prettier --write "$FILE_PATH" 2>/dev/null || true
                npx eslint --fix "$FILE_PATH" 2>/dev/null || true
            fi
        fi

        # Python files
        if [[ "$FILE_PATH" =~ \.py$ ]]; then
            if command -v black &> /dev/null; then
                black "$FILE_PATH" 2>/dev/null || true
            fi
        fi

        echo "✅ Formatting complete"
    fi
fi

# Run tests after significant changes
if [ "$TOOL_NAME" = "Edit" ] || [ "$TOOL_NAME" = "Write" ]; then
    FILE_PATH=$(echo "$TOOL_ARGS" | grep -oP '(?<="file_path": ")[^"]+' || echo "")

    # Only for source files, not config
    if [[ "$FILE_PATH" =~ ^(src/|tests/).* ]]; then
        echo "🧪 Running related tests..."

        if [ -f "package.json" ]; then
            # Run tests for the modified file
            npm run test:file "$FILE_PATH" --passWithNoTests 2>/dev/null || true
        fi
    fi
fi

exit 0
