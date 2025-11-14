# Skills vs Agents: Understanding the Difference

## TL;DR

| Aspect | Skills | Agents (Subagents) |
|--------|--------|-------------------|
| **Invocation** | ✨ Automatic (context-based) | 🎯 Manual (@agent-name or Task tool) |
| **Purpose** | Provide contextual information | Execute complex workflows |
| **Location** | `.claude/skills/*.md` | `.claude/agents/*.md` |
| **When Used** | When keywords/context match | When explicitly called |
| **Complexity** | Simple context providers | Complex multi-step processes |

---

## Skills: Auto-Invoked Context Providers

### What Are Skills?

Skills are **automatic context providers** that Claude Code loads based on description matching with your conversation.

Think of them as "helpful assistants" that automatically provide relevant information when you discuss certain topics.

### How Skills Work

```
You mention "API endpoint"
           ↓
Claude Code scans skill descriptions
           ↓
Finds api-documentation.md (matches "API")
           ↓
Automatically loads skill content
           ↓
Skill provides REST conventions, status codes, etc.
```

### Skill Structure

```yaml
---
name: skill-name
description: Auto-invokes when discussing [keywords]
---

# Skill Content

Information that will be automatically provided when skill activates.
```

### Example Skills in This Framework

1. **api-documentation.md**
   - **Triggers on:** "API", "endpoint", "REST", "HTTP"
   - **Provides:** REST conventions, status codes, request/response patterns

2. **testing-patterns.md**
   - **Triggers on:** "test", "TDD", "coverage", "jest"
   - **Provides:** Testing best practices, AAA pattern, coverage goals

3. **database-patterns.md**
   - **Triggers on:** "database", "SQL", "migration", "schema"
   - **Provides:** Naming conventions, indexing strategy, safe migrations

4. **security-checklist.md**
   - **Triggers on:** "security", "authentication", "OWASP"
   - **Provides:** OWASP Top 10, security headers, validation patterns

### When to Create a Skill

✅ **Create a skill when:**
- Information is needed repeatedly across conversations
- Context is domain-specific and standard
- You want automatic help without asking
- Information is reference material (patterns, checklists, conventions)

**Examples:**
- Coding standards for your team
- Company-specific API patterns
- Project-specific conventions
- Technology stack reference guides

❌ **Don't create a skill for:**
- One-off tasks
- Complex workflows requiring multiple steps
- Tasks requiring tool execution
- Project-specific business logic

---

## Agents (Subagents): Explicit Workflow Orchestrators

### What Are Agents?

Agents (also called subagents) are **specialized AI assistants** with their own:
- System prompt
- Context window
- Tool permissions
- Model preference

They execute complex, multi-step workflows when explicitly invoked.

### How Agents Work

```
You type: "Use the backend-agent to create an API endpoint"
           ↓
Claude invokes backend-agent
           ↓
Backend-agent has its own context window
           ↓
Follows specialized system prompt
           ↓
Uses allowed tools (Read, Write, Edit, Bash)
           ↓
Returns completed work
```

### Agent Structure

```yaml
---
name: agent-name
description: Specialized in [domain]
model: sonnet|opus|haiku
tools:
  allow:
    - Read
    - Write
    - Edit
  deny:
    - Bash
---

# Agent System Prompt

You are a specialized [domain] agent.

## Your Responsibilities
...

## Your Approach
...
```

### Example Agents in This Framework

1. **architect-agent**
   - **Invoked:** Manually when designing systems
   - **Tools:** Read, Grep, Glob (read-only)
   - **Purpose:** Design architectures, plan systems

2. **backend-agent**
   - **Invoked:** Manually for backend development
   - **Tools:** Read, Write, Edit, Bash (full access)
   - **Purpose:** Build APIs, implement business logic

3. **frontend-agent**
   - **Invoked:** Manually for UI development
   - **Tools:** Read, Write, Edit, Bash
   - **Purpose:** Create React components, state management

4. **database-agent**
   - **Invoked:** Manually for database work
   - **Tools:** Read, Write, Edit, Bash
   - **Purpose:** Create migrations, optimize queries

5. **devops-agent**
   - **Invoked:** Manually for infrastructure
   - **Tools:** Read, Write, Edit, Bash
   - **Purpose:** CI/CD, deployment, monitoring

6. **security-agent**
   - **Invoked:** Manually for security audits
   - **Tools:** Read, Grep, Glob, Bash
   - **Purpose:** Vulnerability scanning, OWASP compliance

### When to Create an Agent

✅ **Create an agent when:**
- Task requires multiple steps
- Need specialized system prompt
- Requires specific tool permissions
- Complex domain-specific workflows
- Want to isolate context from main agent

**Examples:**
- Code review agent (reads code, analyzes, reports)
- Migration generator (designs, writes, tests migrations)
- Documentation agent (reads code, generates docs)
- Test generator (analyzes code, creates tests)

❌ **Don't create an agent for:**
- Simple reference information (use skills)
- One-time tasks (just ask main agent)
- Tasks main agent handles well already

---

## Comparison by Use Case

### Use Case: API Development

**Skill Approach (api-documentation.md):**
```
You: "I need to create a POST endpoint"
     ↓
Skill auto-provides: REST conventions, status codes, patterns
     ↓
Main agent uses skill info to help you
```
**Result:** Reference information provided

**Agent Approach (backend-agent):**
```
You: "Use backend-agent to create POST /api/users endpoint"
     ↓
Backend agent: Creates route, controller, service, tests
     ↓
Returns: Complete implementation
```
**Result:** Code generated

---

### Use Case: Security

**Skill Approach (security-checklist.md):**
```
You: "How do I secure this endpoint?"
     ↓
Skill auto-provides: OWASP checklist, security headers, validation
     ↓
Main agent suggests improvements based on checklist
```
**Result:** Best practices provided

**Agent Approach (security-agent):**
```
You: "Use security-agent to audit the authentication system"
     ↓
Security agent: Scans code, identifies vulnerabilities, generates report
     ↓
Returns: Detailed security audit with prioritized fixes
```
**Result:** Comprehensive analysis

---

## How They Work Together

Skills and agents **complement each other**:

```
1. You ask about testing
2. testing-patterns skill auto-loads (provides context)
3. Main agent uses skill info in response
4. If needed, you invoke test-generator agent (executes work)
5. Agent creates tests using patterns from skill
```

**Example conversation:**

```
You: "I need to add tests to the user service"

[testing-patterns skill auto-loads]

Main Agent (with skill context):
"I'll help you add tests. Following TDD best practices and 80% coverage goal
from our testing patterns, let me create unit tests for UserService..."

You: "Actually, use the test-generator agent to do this"

[test-generator agent takes over]

Test-Generator Agent:
"I'll create comprehensive tests for UserService following our patterns.
I see the service has 5 methods. Creating tests with AAA structure..."

[Agent creates test files]
```

---

## Directory Structure

```
.claude/
├── agents/              # Subagents (explicit invocation)
│   ├── architect-agent.md
│   ├── backend-agent.md
│   ├── frontend-agent.md
│   ├── database-agent.md
│   ├── devops-agent.md
│   └── security-agent.md
│
├── skills/              # Skills (auto-invocation)
│   ├── api-documentation.md
│   ├── testing-patterns.md
│   ├── database-patterns.md
│   └── security-checklist.md
│
├── commands/            # Slash commands
│   └── ...
│
└── hooks/               # Event hooks
    └── ...
```

---

## Quick Reference

### When to Use Skills

- ✅ You want automatic context on topics
- ✅ Information is standard/reference material
- ✅ No tool execution needed
- ✅ Context helps across many conversations

**How to invoke:** Automatic (mention keywords)

**Example:** "Let's design the API" → api-documentation skill auto-loads

### When to Use Agents

- ✅ Complex multi-step tasks
- ✅ Need specific tool permissions
- ✅ Specialized workflows
- ✅ Want isolated context window

**How to invoke:** Explicitly call agent

**Example:** "Use backend-agent to implement this" → Backend agent executes

---

## Best Practices

### For Skills

1. **Keep them focused** - One domain per skill
2. **Make descriptions specific** - Clear trigger keywords
3. **Provide reference info** - Patterns, conventions, checklists
4. **Update regularly** - Keep current with best practices

### For Agents

1. **Give clear system prompts** - Define responsibilities and approach
2. **Set appropriate tool permissions** - Least privilege principle
3. **Choose right model** - Opus for complex, Sonnet for most work
4. **Test invocation** - Ensure agent behaves as expected

---

## Summary

| When you need... | Use... | How it works |
|------------------|--------|--------------|
| Automatic reference info | **Skill** | Auto-loads on keywords |
| Complex task execution | **Agent** | Explicitly invoke |
| Best practices reminder | **Skill** | Provides context |
| Multi-step implementation | **Agent** | Executes workflow |
| Coding standards | **Skill** | Auto-provides conventions |
| Specialized analysis | **Agent** | Analyzes and reports |

**Both are powerful. Use them together for maximum effectiveness!**
