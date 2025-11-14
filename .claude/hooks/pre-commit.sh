#!/bin/bash
# Pre-commit hook for code quality checks

set -e

echo "🔍 Running pre-commit checks..."

# Check for secrets
if command -v gitleaks &> /dev/null; then
    echo "🔐 Scanning for secrets..."
    if ! gitleaks protect --staged --verbose; then
        echo "❌ Secret detected! Commit blocked."
        exit 1
    fi
    echo "✅ No secrets found"
fi

# Check for .env files
if git diff --cached --name-only | grep -E '^\.env$|^\.env\..*' | grep -v '\.env\.example$'; then
    echo "❌ Attempted to commit .env file! Commit blocked."
    echo "💡 Only .env.example files should be committed."
    exit 1
fi

# Run linter
if [ -f "package.json" ]; then
    echo "🎨 Running linter..."
    if ! npm run lint --if-present; then
        echo "❌ Linting failed! Fix errors before committing."
        exit 1
    fi
    echo "✅ Linting passed"
fi

# Run type check
if [ -f "tsconfig.json" ]; then
    echo "📝 Type checking..."
    if ! npm run type-check --if-present; then
        echo "❌ Type check failed! Fix type errors before committing."
        exit 1
    fi
    echo "✅ Type check passed"
fi

# Run tests
if [ -f "package.json" ]; then
    echo "🧪 Running tests..."
    if ! npm run test:ci --if-present; then
        echo "❌ Tests failed! Fix failing tests before committing."
        exit 1
    fi
    echo "✅ Tests passed"
fi

echo "✨ All pre-commit checks passed!"
exit 0
