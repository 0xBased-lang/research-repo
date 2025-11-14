#!/bin/bash
# Session start hook - Displays project context

echo "🚀 Claude Code Framework - Research Repository"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo ""

# Display current branch
BRANCH=$(git rev-parse --abbrev-ref HEAD 2>/dev/null || echo "unknown")
echo "📍 Branch: $BRANCH"

# Display git status summary
if git rev-parse --git-dir > /dev/null 2>&1; then
    MODIFIED=$(git status --porcelain | grep -E '^ M' | wc -l)
    STAGED=$(git status --porcelain | grep -E '^M ' | wc -l)
    UNTRACKED=$(git status --porcelain | grep -E '^\?\?' | wc -l)

    if [ $MODIFIED -gt 0 ] || [ $STAGED -gt 0 ] || [ $UNTRACKED -gt 0 ]; then
        echo "📝 Changes: $MODIFIED modified, $STAGED staged, $UNTRACKED untracked"
    else
        echo "✨ Working directory clean"
    fi
fi

echo ""
echo "🏗️  Project Structure:"
echo "   • Backend:  $(find backend -maxdepth 1 -mindepth 1 -type d 2>/dev/null | wc -l) projects"
echo "   • Frontend: $(find frontend -maxdepth 1 -mindepth 1 -type d 2>/dev/null | wc -l) projects"
echo "   • Database: $(find database -maxdepth 1 -mindepth 1 -type d 2>/dev/null | wc -l) projects"

echo ""
echo "🤖 Available Agents:"
echo "   • architect-agent  - System design & architecture"
echo "   • backend-agent    - API & server-side development"
echo "   • frontend-agent   - UI components & React"
echo "   • database-agent   - Schema & migrations"
echo "   • devops-agent     - CI/CD & deployment"
echo "   • security-agent   - Security audits"

echo ""
echo "⚡ Quick Commands:"
echo "   /new-project     - Create new project"
echo "   /architect       - Design architecture"
echo "   /api             - Generate API endpoint"
echo "   /component       - Create UI component"
echo "   /migrate         - Create database migration"
echo "   /security-audit  - Run security audit"

echo ""
echo "📚 Documentation:"
echo "   • CLAUDE.md - Project guidelines & standards"
echo "   • README.md - Repository overview"

echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "Ready to code! 🎉"
echo ""

exit 0
