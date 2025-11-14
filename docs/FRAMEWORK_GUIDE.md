# Claude Code Framework Guide
## Complete Blueprint for Building Structured, Clean, and Scalable Projects

---

## Table of Contents

1. [Framework Overview](#framework-overview)
2. [Architecture Blueprint](#architecture-blueprint)
3. [Directory Structure](#directory-structure)
4. [Agent Hierarchy System](#agent-hierarchy-system)
5. [Slash Commands](#slash-commands)
6. [Hooks System](#hooks-system)
7. [MCP Integration](#mcp-integration)
8. [Workflow Patterns](#workflow-patterns)
9. [Development Guidelines](#development-guidelines)
10. [Deployment Strategy](#deployment-strategy)

---

## Framework Overview

This Claude Code framework provides a complete, production-ready structure for managing multiple projects across backend, frontend, and database layers with:

- **Clean separation of concerns** - Each project in its own directory
- **Specialized agents** - Domain-specific AI assistants for each layer
- **Automated workflows** - Slash commands for common tasks
- **Quality enforcement** - Hooks for code quality and security
- **Documentation-first** - Built-in documentation standards
- **Scalable architecture** - Grows with your needs

### Core Principles

1. **Plan → Implement → Test → Review** - Always follow the workflow
2. **Documentation is code** - Keep docs updated with changes
3. **Security by design** - Build security in from the start
4. **Automation over repetition** - Automate recurring tasks
5. **Quality gates** - Enforce standards through hooks
6. **Agent specialization** - Use the right agent for the job

---

## Architecture Blueprint

### System Architecture Layers

```
┌─────────────────────────────────────────────────────────┐
│                   Claude Code Layer                      │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌─────────┐ │
│  │  Agents  │  │ Commands │  │  Hooks   │  │   MCP   │ │
│  └──────────┘  └──────────┘  └──────────┘  └─────────┘ │
└─────────────────────────────────────────────────────────┘
                           ↓
┌─────────────────────────────────────────────────────────┐
│                  Application Layer                       │
│  ┌─────────────┐  ┌──────────────┐  ┌────────────────┐ │
│  │   Backend   │  │   Frontend   │  │    Database    │ │
│  │  (Node.js)  │  │   (React)    │  │  (PostgreSQL)  │ │
│  └─────────────┘  └──────────────┘  └────────────────┘ │
└─────────────────────────────────────────────────────────┘
                           ↓
┌─────────────────────────────────────────────────────────┐
│              Infrastructure Layer                        │
│  ┌─────────────┐  ┌──────────────┐  ┌────────────────┐ │
│  │   Docker    │  │    CI/CD     │  │   Monitoring   │ │
│  │  Containers │  │GitHub Actions│  │   Prometheus   │ │
│  └─────────────┘  └──────────────┘  └────────────────┘ │
└─────────────────────────────────────────────────────────┘
```

### Agent Hierarchy

```
                      ┌──────────────────┐
                      │ Architect Agent  │
                      │   (Planning)     │
                      └────────┬─────────┘
                               │
        ┌──────────────────────┼──────────────────────┐
        │                      │                      │
   ┌────▼─────┐         ┌──────▼──────┐       ┌──────▼──────┐
   │ Backend  │         │  Frontend   │       │  Database   │
   │  Agent   │         │   Agent     │       │   Agent     │
   └──────────┘         └─────────────┘       └─────────────┘
        │                      │                      │
        └──────────────────────┼──────────────────────┘
                               │
                        ┌──────▼───────┐
                        │   DevOps     │
                        │    Agent     │
                        └──────┬───────┘
                               │
                        ┌──────▼───────┐
                        │  Security    │
                        │    Agent     │
                        └──────────────┘
```

### Agent Responsibilities

| Agent | Responsibility | Tools | Model |
|-------|---------------|-------|-------|
| **Architect** | Design systems, plan architecture, make technical decisions | Read, Grep, Glob, TodoWrite | Opus |
| **Backend** | Build APIs, services, business logic, integrations | Read, Write, Edit, Bash, Grep, Glob, TodoWrite | Sonnet |
| **Frontend** | Create UI components, state management, user interactions | Read, Write, Edit, Bash, Grep, Glob, TodoWrite | Sonnet |
| **Database** | Design schemas, write migrations, optimize queries | Read, Write, Edit, Bash, Grep, Glob, TodoWrite | Sonnet |
| **DevOps** | CI/CD pipelines, deployment, infrastructure, monitoring | Read, Write, Edit, Bash, Grep, Glob, TodoWrite | Sonnet |
| **Security** | Security audits, vulnerability scanning, compliance | Read, Grep, Glob, TodoWrite, Bash | Opus |

---

## Directory Structure

### Complete Repository Layout

```
research-repo/
│
├── .claude/                        # Claude Code configuration
│   ├── agents/                     # Specialized agents
│   │   ├── architect-agent.md
│   │   ├── backend-agent.md
│   │   ├── frontend-agent.md
│   │   ├── database-agent.md
│   │   ├── devops-agent.md
│   │   └── security-agent.md
│   │
│   ├── commands/                   # Slash commands
│   │   ├── new-project.md
│   │   ├── architect.md
│   │   ├── api.md
│   │   ├── component.md
│   │   ├── migrate.md
│   │   └── security-audit.md
│   │
│   └── hooks/                      # Event hooks
│       ├── pre-commit.sh
│       ├── post-tool-use.sh
│       └── session-start.sh
│
├── backend/                        # Backend services
│   ├── project-1/
│   │   ├── src/
│   │   │   ├── routes/            # HTTP routes
│   │   │   ├── controllers/       # Request handlers
│   │   │   ├── services/          # Business logic
│   │   │   ├── repositories/      # Data access
│   │   │   ├── models/            # Data models
│   │   │   ├── middlewares/       # Express middlewares
│   │   │   ├── utils/             # Utilities
│   │   │   └── types/             # TypeScript types
│   │   ├── tests/
│   │   │   ├── unit/
│   │   │   ├── integration/
│   │   │   └── e2e/
│   │   ├── package.json
│   │   ├── tsconfig.json
│   │   ├── .env.example
│   │   ├── docker-compose.yml
│   │   └── README.md
│   │
│   └── project-2/
│       └── ...
│
├── frontend/                       # Frontend applications
│   ├── project-1/
│   │   ├── src/
│   │   │   ├── components/        # Reusable components
│   │   │   │   ├── ui/           # Base UI components
│   │   │   │   ├── forms/        # Form components
│   │   │   │   └── layout/       # Layout components
│   │   │   ├── features/          # Feature modules
│   │   │   ├── hooks/             # Custom React hooks
│   │   │   ├── lib/               # Utilities
│   │   │   ├── pages/             # Page components
│   │   │   ├── api/               # API client
│   │   │   ├── styles/            # Global styles
│   │   │   └── types/             # TypeScript types
│   │   ├── public/
│   │   ├── tests/
│   │   ├── package.json
│   │   ├── tsconfig.json
│   │   ├── tailwind.config.js
│   │   └── README.md
│   │
│   └── project-2/
│       └── ...
│
├── database/                       # Database schemas
│   ├── project-1/
│   │   ├── migrations/            # Database migrations
│   │   ├── seeds/                 # Seed data
│   │   ├── schema.prisma          # Prisma schema
│   │   ├── .env.example
│   │   └── README.md
│   │
│   └── project-2/
│       └── ...
│
├── docs/                           # Documentation
│   ├── architecture/              # Architecture docs
│   ├── examples/                  # Code examples
│   ├── best-practices/            # Best practices guides
│   ├── troubleshooting/           # Troubleshooting guides
│   └── FRAMEWORK_GUIDE.md         # This guide
│
├── scripts/                        # Automation scripts
│   ├── setup.sh                   # Initial setup
│   ├── backup-db.sh               # Database backups
│   └── deploy.sh                  # Deployment script
│
├── .mcp.json                       # MCP server configuration
├── .gitignore                      # Git ignore rules
├── CLAUDE.md                       # Project guidelines
└── README.md                       # Repository overview
```

---

## Agent Hierarchy System

### How Agents Work Together

#### 1. Planning Phase (Architect Agent)

```markdown
User Request → Architect Agent
              ↓
        Design architecture
        Identify components
        Plan data flow
        Technology recommendations
              ↓
        Implementation roadmap
```

**When to use:**
- Starting a new project
- Adding major features
- Refactoring systems
- Making technology decisions

**Example:**
```
User: "I need to build a multi-tenant SaaS platform"
Architect: Designs the architecture, recommends tech stack,
           plans database isolation, creates implementation phases
```

#### 2. Implementation Phase (Dev Agents)

```markdown
Implementation Plan → Backend/Frontend/Database Agents
                    ↓
              Implement features
              Write tests
              Create documentation
                    ↓
              Working code
```

**Agent Selection:**
- **Backend Agent**: APIs, business logic, integrations
- **Frontend Agent**: UI components, state management
- **Database Agent**: Schemas, migrations, queries

#### 3. Deployment Phase (DevOps Agent)

```markdown
Working Code → DevOps Agent
             ↓
        Create CI/CD pipeline
        Configure infrastructure
        Set up monitoring
             ↓
        Deployed application
```

#### 4. Security Phase (Security Agent)

```markdown
Codebase → Security Agent
         ↓
    Security audit
    Vulnerability scan
    Compliance check
         ↓
    Security report
```

### Agent Invocation Patterns

#### Pattern 1: Sequential (Waterfall)
```
Architect → Backend → Frontend → Database → DevOps → Security
```
Use for: Complete feature development

#### Pattern 2: Parallel (Concurrent)
```
              → Backend Agent
Architect   → Frontend Agent
              → Database Agent
```
Use for: Independent components

#### Pattern 3: Iterative (Agile)
```
Architect → Backend → Test → Review → Refine → Repeat
```
Use for: Complex features with unknowns

---

## Slash Commands

### Available Commands

#### `/new-project` - Create New Project

**Usage:**
```
/new-project backend my-api
/new-project frontend my-app
/new-project database my-db
```

**What it does:**
1. Creates directory structure
2. Initializes package.json
3. Sets up TypeScript
4. Creates .env.example
5. Generates boilerplate
6. Initializes git
7. Updates documentation

#### `/architect` - Design Architecture

**Usage:**
```
/architect Design a real-time chat system
```

**What it does:**
1. Invokes architect-agent
2. Gathers requirements
3. Designs system architecture
4. Creates diagrams
5. Recommends technologies
6. Plans implementation

#### `/api` - Generate API Endpoint

**Usage:**
```
/api users POST
```

**What it does:**
1. Creates route file
2. Generates controller
3. Implements service
4. Creates repository
5. Adds validation
6. Writes tests
7. Updates API docs

#### `/component` - Create UI Component

**Usage:**
```
/component UserProfile presentational
```

**What it does:**
1. Creates component file
2. Defines TypeScript types
3. Implements styling
4. Adds accessibility
5. Writes tests
6. Creates Storybook story
7. Updates exports

#### `/migrate` - Create Database Migration

**Usage:**
```
/migrate add_user_preferences_table
```

**What it does:**
1. Plans migration
2. Creates up/down scripts
3. Handles data migration
4. Tests migration
5. Updates ORM schema
6. Generates types
7. Documents changes

#### `/security-audit` - Run Security Audit

**Usage:**
```
/security-audit full
```

**What it does:**
1. Invokes security-agent
2. Scans for OWASP Top 10
3. Analyzes code
4. Audits dependencies
5. Reviews configuration
6. Scans for secrets
7. Generates report

---

## Hooks System

### Available Hooks

#### Session Start Hook

**Triggered:** When Claude Code session starts

**Purpose:** Display project context and available tools

**Location:** `.claude/hooks/session-start.sh`

**Output:**
- Current git branch
- Git status summary
- Project structure overview
- Available agents list
- Quick commands reference

#### Pre-Commit Hook

**Triggered:** Before git commit

**Purpose:** Enforce code quality standards

**Location:** `.claude/hooks/pre-commit.sh`

**Checks:**
- ✅ Secrets scanning (gitleaks)
- ✅ .env file prevention
- ✅ Linting (ESLint)
- ✅ Type checking (TypeScript)
- ✅ Test execution

**Behavior:** Blocks commit if checks fail

#### Post-Tool-Use Hook

**Triggered:** After Edit or Write tools

**Purpose:** Auto-format code and run tests

**Location:** `.claude/hooks/post-tool-use.sh`

**Actions:**
- 🎨 Auto-format with Prettier
- 🔧 Auto-fix with ESLint
- 🐍 Format Python with Black
- 🧪 Run related tests

### Creating Custom Hooks

```bash
# Create hook file
touch .claude/hooks/my-hook.sh

# Make executable
chmod +x .claude/hooks/my-hook.sh

# Hook structure
#!/bin/bash

# Your hook logic here
echo "Hook triggered!"

exit 0  # or exit 1 to block action
```

---

## MCP Integration

### Configured MCP Servers

#### 1. Filesystem Server

**Purpose:** Enhanced file system operations

**Usage:** Automatically available for file operations

#### 2. GitHub Server

**Purpose:** GitHub API integration

**Requirements:** `GITHUB_TOKEN` environment variable

**Features:**
- Create issues
- Manage pull requests
- Review code
- Check workflows

#### 3. PostgreSQL Server

**Purpose:** Direct database access

**Requirements:** `DATABASE_URL` environment variable

**Features:**
- Query database
- Inspect schema
- Analyze queries
- Monitor performance

### Adding More MCP Servers

Edit `.mcp.json`:

```json
{
  "mcpServers": {
    "new-server": {
      "command": "npx",
      "args": ["-y", "@modelcontextprotocol/server-name"],
      "env": {
        "API_KEY": "${YOUR_API_KEY}"
      }
    }
  }
}
```

### Popular MCP Servers

- **Puppeteer**: Browser automation
- **Sentry**: Error tracking
- **Slack**: Team notifications
- **AWS**: Cloud services
- **Docker**: Container management

---

## Workflow Patterns

### Pattern 1: New Feature Development

```
1. 📋 Plan with Architect Agent
   ↓
2. 🎨 Design database schema (Database Agent)
   ↓
3. 🔧 Implement API (Backend Agent)
   ↓
4. 💻 Build UI (Frontend Agent)
   ↓
5. 🧪 Write tests (All Agents)
   ↓
6. 🔐 Security audit (Security Agent)
   ↓
7. 🚀 Deploy (DevOps Agent)
```

### Pattern 2: Bug Fix Workflow

```
1. 🐛 Reproduce issue
   ↓
2. 🔍 Identify root cause
   ↓
3. ✍️  Write failing test
   ↓
4. 🔧 Fix the bug
   ↓
5. ✅ Verify test passes
   ↓
6. 📝 Update documentation
   ↓
7. 🔄 Commit and deploy
```

### Pattern 3: Refactoring Workflow

```
1. 📊 Analyze current code (Architect Agent)
   ↓
2. 📋 Create refactoring plan
   ↓
3. ✅ Ensure test coverage ≥ 80%
   ↓
4. 🔧 Refactor in small steps
   ↓
5. 🧪 Run tests after each step
   ↓
6. 📝 Update documentation
   ↓
7. 👁️  Code review
```

### Pattern 4: Security Hardening

```
1. 🔍 Run security audit (/security-audit)
   ↓
2. 📊 Prioritize findings (Critical → Low)
   ↓
3. 🔧 Fix critical issues first
   ↓
4. 🧪 Test security fixes
   ↓
5. 📝 Document changes
   ↓
6. 🔄 Re-audit to verify
   ↓
7. 📈 Monitor in production
```

---

## Development Guidelines

### Code Quality Standards

#### TypeScript/JavaScript

```typescript
// ✅ Good: Type-safe, descriptive, testable
export async function getUserById(userId: string): Promise<User | null> {
  const user = await db.user.findUnique({ where: { id: userId } });

  if (!user) {
    return null;
  }

  return user;
}

// ❌ Bad: No types, unclear, side effects
async function getUser(id) {
  let user = await db.user.findUnique({ where: { id } });
  console.log(user);
  return user;
}
```

#### Python

```python
# ✅ Good: Type hints, docstring, error handling
def calculate_discount(price: float, discount_percent: int) -> float:
    """
    Calculate discounted price.

    Args:
        price: Original price
        discount_percent: Discount percentage (0-100)

    Returns:
        Discounted price

    Raises:
        ValueError: If discount is invalid
    """
    if not 0 <= discount_percent <= 100:
        raise ValueError("Discount must be between 0 and 100")

    return price * (1 - discount_percent / 100)

# ❌ Bad: No types, no docs, no validation
def discount(p, d):
    return p * (1 - d / 100)
```

### Testing Standards

#### Unit Test Example

```typescript
describe('UserService', () => {
  describe('createUser', () => {
    it('should create user with hashed password', async () => {
      // Arrange
      const userData = {
        email: 'test@example.com',
        password: 'password123',
      };
      const mockRepository = {
        create: vi.fn().mockResolvedValue({ id: '1', ...userData }),
      };
      const service = new UserService(mockRepository);

      // Act
      const user = await service.createUser(userData);

      // Assert
      expect(user.password).not.toBe(userData.password);
      expect(await bcrypt.compare(userData.password, user.password)).toBe(true);
    });

    it('should throw error for duplicate email', async () => {
      // Test error case
    });
  });
});
```

### Git Commit Standards

**Format:** Conventional Commits

```
type(scope): description

[optional body]

[optional footer]
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation
- `style`: Formatting
- `refactor`: Code restructuring
- `test`: Tests
- `chore`: Maintenance

**Examples:**

```
feat(auth): add JWT token refresh mechanism

Implements automatic token refresh using refresh tokens.
Tokens expire after 15 minutes and refresh after 7 days.

Closes #123
```

```
fix(api): prevent SQL injection in user search

Use parameterized queries instead of string concatenation
```

---

## Deployment Strategy

### Development Environment

```bash
# Backend
cd backend/project-name
npm install
npm run dev

# Frontend
cd frontend/project-name
npm install
npm run dev

# Database
cd database/project-name
npx prisma migrate dev
npx prisma studio
```

### Staging Environment

```yaml
# docker-compose.staging.yml
version: '3.8'

services:
  backend:
    build: ./backend/project-name
    environment:
      - NODE_ENV=staging
      - DATABASE_URL=postgresql://...
    ports:
      - "3000:3000"

  frontend:
    build: ./frontend/project-name
    environment:
      - VITE_API_URL=https://api.staging.example.com
    ports:
      - "80:80"

  db:
    image: postgres:16
    environment:
      - POSTGRES_DB=mydb
    volumes:
      - postgres_data:/var/lib/postgresql/data
```

### Production Environment

```yaml
# .github/workflows/deploy.yml
name: Deploy to Production

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Build and push Docker image
        run: |
          docker build -t myapp:${{ github.sha }} .
          docker push ghcr.io/${{ github.repository }}:latest

      - name: Deploy to production
        run: |
          # Deployment commands
```

---

## Best Practices Summary

### ✅ Always Do

1. **Plan before coding** - Use architect-agent
2. **Write tests first** - TDD approach
3. **Document as you go** - Keep docs updated
4. **Use type safety** - TypeScript strict mode
5. **Validate inputs** - Never trust user data
6. **Handle errors** - Graceful error handling
7. **Security first** - Run security audits
8. **Code review** - Review before merging
9. **Small commits** - Atomic, focused changes
10. **Automate** - Use hooks and CI/CD

### ❌ Never Do

1. **Commit secrets** - Use .env files
2. **Skip tests** - Maintain coverage
3. **Use `any` type** - Be explicit
4. **Concatenate SQL** - Use parameterized queries
5. **Ignore errors** - Always handle exceptions
6. **Push to main** - Use feature branches
7. **Hard-code config** - Use environment variables
8. **Skip validation** - Validate all inputs
9. **Write huge functions** - Keep them small
10. **Forget documentation** - Document everything

---

## Quick Reference

### Commands Cheatsheet

```bash
# Project Creation
/new-project backend api-service
/new-project frontend web-app

# Development
/architect Design notification system
/api users POST
/component Button presentational
/migrate add_notifications_table

# Quality & Security
/security-audit full
npm run lint
npm run test
npm run type-check

# Git Workflow
git checkout -b feature/add-auth
git add .
git commit -m "feat(auth): add user authentication"
git push -u origin feature/add-auth

# Deployment
docker-compose up -d
npm run deploy:staging
npm run deploy:prod
```

### Agent Quick Reference

| Task | Agent | Command |
|------|-------|---------|
| Design system | Architect | Invoke in chat |
| Build API | Backend | Invoke in chat |
| Create UI | Frontend | Invoke in chat |
| Database work | Database | Invoke in chat |
| Deploy | DevOps | Invoke in chat |
| Security audit | Security | `/security-audit` |

---

## Conclusion

This framework provides everything you need to build clean, structured, and scalable projects with Claude Code. Follow the patterns, use the agents, leverage the automation, and maintain high standards.

**Remember:** Quality over speed. Well-architected code saves time in the long run.

---

**Next Steps:**

1. Review CLAUDE.md for project-specific guidelines
2. Explore the agents in `.claude/agents/`
3. Try the slash commands in `.claude/commands/`
4. Set up your first project with `/new-project`
5. Run `/security-audit` to ensure security

Happy coding! 🚀
