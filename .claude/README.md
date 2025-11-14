# Claude Code Configuration Directory

This directory contains all Claude Code configuration for the research-repo framework.

## Directory Structure

```
.claude/
├── agents/              # Subagents (6 total)
│   ├── architect-agent.md
│   ├── backend-agent.md
│   ├── database-agent.md
│   ├── devops-agent.md
│   ├── frontend-agent.md
│   └── security-agent.md
│
├── commands/            # Slash commands (6 total)
│   ├── api.md
│   ├── architect.md
│   ├── component.md
│   ├── migrate.md
│   ├── new-project.md
│   └── security-audit.md
│
├── hooks/               # Event hooks (3 total)
│   ├── PreToolUse.sh
│   ├── PostToolUse.sh
│   └── SessionStart.sh
│
├── skills/              # Auto-invoked skills (4 total)
│   ├── api-documentation.md
│   ├── database-patterns.md
│   ├── security-checklist.md
│   └── testing-patterns.md
│
├── settings.json        # Hook and permission configuration
└── README.md            # This file
```

## Component Descriptions

### Agents (Subagents)

Specialized AI assistants with their own context, tools, and system prompts. Invoked manually.

**Usage:** `"Use the backend-agent to create an API endpoint"`

### Commands (Slash Commands)

Reusable prompt templates for common workflows. Invoked with `/command-name`.

**Usage:** `/api users POST`

### Hooks

Shell scripts that run at specific Claude Code lifecycle events.

- **PreToolUse**: Before any tool execution (security checks)
- **PostToolUse**: After tool execution (auto-format, run tests)
- **SessionStart**: When Claude Code session starts (display context)

### Skills

Auto-invoked context providers that load when conversation matches their description.

**Triggers automatically** when you mention relevant keywords.

### Settings

JSON configuration for hooks and permissions.

## See Also

- [Skills vs Agents Guide](../docs/SKILLS_VS_AGENTS.md) - Detailed comparison
- [Compliance Audit](../docs/COMPLIANCE_AUDIT.md) - Framework validation
- [Framework Guide](../docs/FRAMEWORK_GUIDE.md) - Complete documentation
