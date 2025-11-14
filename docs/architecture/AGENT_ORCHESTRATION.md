# Agent Orchestration Patterns

## Overview

This document describes how to effectively orchestrate multiple specialized agents in Claude Code to build complex systems.

## Orchestration Hierarchy

```
                    ┌─────────────────────┐
                    │   Main Claude       │
                    │   (Orchestrator)    │
                    └──────────┬──────────┘
                               │
            ┌──────────────────┼──────────────────┐
            │                  │                  │
    ┌───────▼──────┐   ┌───────▼──────┐   ┌─────▼──────┐
    │  Architect   │   │   Security   │   │   DevOps   │
    │    Agent     │   │    Agent     │   │   Agent    │
    │  (Planning)  │   │   (Audit)    │   │  (Deploy)  │
    └───────┬──────┘   └──────────────┘   └────────────┘
            │
    ┌───────┼────────────────┐
    │       │                │
┌───▼────┐ ┌▼────────┐ ┌────▼────┐
│Backend │ │Frontend │ │Database │
│ Agent  │ │  Agent  │ │  Agent  │
└────────┘ └─────────┘ └─────────┘
```

## When to Use Each Agent

### Architect Agent

**Use when:**
- Starting a new project
- Designing system architecture
- Making technology stack decisions
- Planning data flow and integrations
- Evaluating scalability requirements
- Conducting architecture reviews

**Don't use when:**
- Implementing code (use dev agents)
- Writing tests
- Fixing bugs (unless architectural)

**Example workflow:**
```
You: "I need to design a multi-tenant SaaS platform with separate databases per tenant"

Architect Agent will:
1. Ask clarifying questions about scale, requirements
2. Research current codebase patterns
3. Design tenant isolation architecture
4. Recommend technology stack
5. Create data flow diagrams
6. Plan implementation phases
7. Identify risks and mitigations
```

### Backend Agent

**Use when:**
- Creating API endpoints
- Implementing business logic
- Integrating third-party services
- Writing server-side code
- Creating database queries
- Building authentication/authorization

**Don't use when:**
- Designing UI components (use frontend agent)
- Writing database migrations (use database agent)
- Deploying applications (use devops agent)

**Example workflow:**
```
You: "Create a POST /api/users endpoint with validation and tests"

Backend Agent will:
1. Create route definition
2. Implement controller
3. Add service layer with business logic
4. Create repository for data access
5. Add input validation (Zod schema)
6. Write unit and integration tests
7. Update API documentation
```

### Frontend Agent

**Use when:**
- Creating React components
- Building user interfaces
- Implementing state management
- Styling with TailwindCSS
- Adding form validation
- Optimizing performance

**Don't use when:**
- Creating API endpoints (use backend agent)
- Writing database queries (use database agent)
- Setting up CI/CD (use devops agent)

**Example workflow:**
```
You: "Create a UserProfile component with avatar, name, and edit button"

Frontend Agent will:
1. Create component file with TypeScript
2. Define props interface
3. Implement component with Tailwind styling
4. Add accessibility (ARIA labels, semantic HTML)
5. Write component tests
6. Create Storybook story
7. Export from index
```

### Database Agent

**Use when:**
- Designing database schemas
- Creating migrations
- Optimizing queries
- Adding indexes
- Planning data models
- Handling data transformations

**Don't use when:**
- Creating API routes (use backend agent)
- Building UI (use frontend agent)
- Deploying infrastructure (use devops agent)

**Example workflow:**
```
You: "Create a migration to add user preferences table"

Database Agent will:
1. Plan migration structure
2. Write up migration (create table, indexes)
3. Write down migration (rollback)
4. Consider data migration needs
5. Update Prisma schema
6. Generate TypeScript types
7. Document migration
```

### DevOps Agent

**Use when:**
- Setting up CI/CD pipelines
- Creating Docker containers
- Deploying applications
- Configuring infrastructure
- Setting up monitoring
- Managing environments

**Don't use when:**
- Writing application code (use dev agents)
- Designing features (use architect agent)
- Security audits (use security agent)

**Example workflow:**
```
You: "Set up CI/CD pipeline for automated testing and deployment"

DevOps Agent will:
1. Create GitHub Actions workflow
2. Configure test jobs (lint, test, build)
3. Set up deployment jobs
4. Create Docker configuration
5. Add health check endpoints
6. Configure monitoring
7. Document deployment process
```

### Security Agent

**Use when:**
- Conducting security audits
- Scanning for vulnerabilities
- Reviewing authentication/authorization
- Checking for secrets in code
- Evaluating OWASP Top 10 compliance
- Planning security improvements

**Don't use when:**
- Implementing features (use dev agents)
- Deploying code (use devops agent)
- Writing tests (use dev agents)

**Example workflow:**
```
You: "Run a comprehensive security audit"

Security Agent will:
1. Scan for OWASP Top 10 vulnerabilities
2. Check for hard-coded secrets
3. Audit dependencies (npm audit)
4. Review authentication implementation
5. Check authorization logic
6. Verify input validation
7. Generate prioritized report
```

## Orchestration Patterns

### Pattern 1: Sequential Execution

Use when tasks depend on each other.

```
Step 1: Architect Agent designs system
        ↓
Step 2: Backend Agent implements API
        ↓
Step 3: Frontend Agent builds UI
        ↓
Step 4: Database Agent optimizes queries
        ↓
Step 5: DevOps Agent deploys
        ↓
Step 6: Security Agent audits
```

**Example:**
```
You: "Build a complete user authentication system"

Workflow:
1. Architect: Design auth architecture (JWT, refresh tokens, RBAC)
2. Database: Create users, sessions, roles tables
3. Backend: Implement login, register, token refresh endpoints
4. Frontend: Create login/register forms, auth state management
5. Security: Audit for vulnerabilities
6. DevOps: Deploy with environment variables, monitoring
```

### Pattern 2: Parallel Execution

Use when tasks are independent.

```
                ┌→ Backend Agent (API)
Architect  →   ├→ Frontend Agent (UI)
                └→ Database Agent (Schema)
```

**Example:**
```
You: "Build dashboard with stats API and charts"

After architecture design:
- Backend Agent: Builds /api/stats endpoint (parallel)
- Frontend Agent: Creates Chart components (parallel)
- Database Agent: Adds indexes for performance (parallel)

Then integrate all pieces.
```

### Pattern 3: Iterative Refinement

Use when exploring solutions or refining quality.

```
Design → Implement → Review → Refine → Repeat
```

**Example:**
```
You: "Optimize application performance"

Iteration 1:
- Architect: Identify bottlenecks
- Database: Add indexes
- Test performance

Iteration 2:
- Backend: Implement caching
- Test performance

Iteration 3:
- Frontend: Add code splitting
- Test performance

Repeat until performance targets met.
```

### Pattern 4: Review & Improve

Use for quality improvements.

```
Existing Code → Security Agent (Audit)
              → Backend Agent (Fixes)
              → Security Agent (Re-audit)
```

**Example:**
```
You: "Improve security of authentication system"

1. Security Agent: Audit current auth implementation
2. Backend Agent: Fix identified vulnerabilities
3. Security Agent: Re-audit to verify fixes
4. DevOps Agent: Deploy security improvements
```

## Advanced Orchestration

### Multi-Agent Collaboration

Some complex tasks require multiple agents working together:

```
Task: "Build a real-time notification system"

Phase 1 - Design (Architect Agent)
├─ System architecture
├─ Technology selection (WebSockets vs SSE)
├─ Data flow design
└─ Scalability planning

Phase 2 - Implementation (Parallel)
├─ Backend Agent
│  ├─ WebSocket server
│  ├─ Notification service
│  └─ API endpoints
├─ Frontend Agent
│  ├─ Notification component
│  ├─ WebSocket client
│  └─ State management
└─ Database Agent
   ├─ Notifications table
   ├─ User preferences
   └─ Indexes

Phase 3 - Infrastructure (DevOps Agent)
├─ Redis for pub/sub
├─ Load balancer configuration
└─ Monitoring setup

Phase 4 - Security (Security Agent)
├─ Authentication on WebSocket
├─ Rate limiting
└─ Input validation
```

### Error Recovery Patterns

When an agent encounters issues:

```
Agent encounters error
       ↓
Report issue to orchestrator
       ↓
Orchestrator decides:
├─ Retry with different approach
├─ Switch to different agent
├─ Ask user for clarification
└─ Break down into smaller tasks
```

## Best Practices

### 1. Start with Planning

Always use Architect Agent for non-trivial features:

```
❌ Bad:
You: "Create user registration"
→ Directly start coding without planning

✅ Good:
You: "Design and plan user registration system"
→ Architect Agent: Plans architecture
→ Then implementation agents execute
```

### 2. Use Right Agent for the Job

```
❌ Bad:
You ask Backend Agent to design system architecture

✅ Good:
You ask Architect Agent to design, then Backend Agent to implement
```

### 3. Break Down Complex Tasks

```
❌ Bad:
"Build entire e-commerce platform"

✅ Good:
1. "Design e-commerce architecture" (Architect)
2. "Create product catalog API" (Backend)
3. "Create product listing UI" (Frontend)
4. "Optimize product queries" (Database)
5. "Set up deployment" (DevOps)
6. "Security audit" (Security)
```

### 4. Maintain Context Boundaries

Each agent has specialized context. Use `/clear` between major topic changes:

```
You: "Build user authentication" (Backend Agent)
→ Backend Agent builds auth

You: /clear  # Clear context

You: "Design notification system" (Architect Agent)
→ Fresh context for new feature
```

### 5. Document Decisions

After agent work, document outcomes:

```
After Architect Agent designs system:
→ Save to docs/architecture/SYSTEM_NAME.md

After Security Agent audits:
→ Save report to docs/security/AUDIT_YYYY-MM-DD.md
```

## Troubleshooting

### Agent Not Following Instructions

**Problem:** Agent not doing what you expect

**Solution:**
1. Be more specific in instructions
2. Use the correct agent for the task
3. Provide examples of desired output
4. Break task into smaller pieces

### Agent Lacks Context

**Problem:** Agent doesn't understand codebase

**Solution:**
1. Let agent explore codebase first
2. Provide relevant file paths
3. Reference existing patterns
4. Use CLAUDE.md for persistent context

### Agent Makes Wrong Decisions

**Problem:** Agent chooses suboptimal approach

**Solution:**
1. Provide constraints upfront
2. Ask for multiple options
3. Review plan before implementation
4. Use Architect Agent for major decisions

## Conclusion

Effective agent orchestration requires:

1. **Right agent for the job** - Use specialized agents
2. **Clear communication** - Be specific in requests
3. **Proper sequencing** - Plan dependencies
4. **Context management** - Clear between major changes
5. **Documentation** - Capture decisions and outcomes

Master these patterns and your development efficiency will significantly improve.
