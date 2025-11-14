# Research Repository
## Multi-Project Research Repository with Claude Code Framework

A comprehensive, production-ready framework for managing multiple projects across backend, frontend, and database layers with clean separation of concerns, automated workflows, and specialized AI agents.

---

## 🎯 What is This?

This repository provides a complete Claude Code framework for:

- **Structured project organization** - Clean separation of backend, frontend, and database
- **Specialized AI agents** - Domain-specific assistants for each layer
- **Automated workflows** - Slash commands for common development tasks
- **Quality enforcement** - Hooks for code quality and security
- **Scalable architecture** - Grows from prototype to production

---

## 🚀 Quick Start

### 1. Explore the Framework

```bash
# Review the comprehensive framework guide
cat docs/FRAMEWORK_GUIDE.md

# Check project guidelines
cat CLAUDE.md

# See available agents
ls -la .claude/agents/

# See available commands
ls -la .claude/commands/
```

### 2. Create Your First Project

```bash
# Start Claude Code session
claude

# Use slash command to create new project
/new-project backend my-api

# Or create manually and follow structure
mkdir -p backend/my-api
```

### 3. Start Building

```
# Design architecture
/architect Design a RESTful API for user management

# Create API endpoint
/api users POST

# Create UI component
/component UserList container

# Create database migration
/migrate create_users_table
```

---

## 📁 Repository Structure

```
research-repo/
├── .claude/              # Claude Code configuration
│   ├── agents/          # 6 specialized agents
│   ├── commands/        # 6 slash commands
│   └── hooks/           # 3 automation hooks
├── backend/             # Backend services
├── frontend/            # Frontend applications
├── database/            # Database schemas & migrations
├── docs/                # Documentation
│   ├── FRAMEWORK_GUIDE.md        # Complete framework guide
│   └── architecture/             # Architecture docs
├── scripts/             # Automation scripts
├── .mcp.json           # MCP server configuration
├── CLAUDE.md           # Project guidelines
└── README.md           # This file
```

---

## 🤖 Specialized Agents

This framework includes 6 specialized agents:

| Agent | Purpose | Model* |
|-------|---------|-------|
| **architect-agent** | System design, architecture decisions, technical planning | Opus |
| **backend-agent** | APIs, business logic, server-side development | Sonnet |
| **frontend-agent** | UI components, React, user experience | Sonnet |
| **database-agent** | Schema design, migrations, query optimization | Sonnet |
| **devops-agent** | CI/CD, deployment, infrastructure, monitoring | Sonnet |
| **security-agent** | Security audits, vulnerability scanning, compliance | Opus |

**\*Model Flexibility:** The model field is a **recommendation**, not a requirement! All agents work perfectly with Sonnet. Use Opus only for complex reasoning. Out of Opus credits? No problem - just use Sonnet for everything. See **[MODEL_SELECTION.md](docs/MODEL_SELECTION.md)** for details.

**How to use agents:**
```
Just ask Claude to invoke them in conversation:
"Use the architect agent to design this system"
"Use the backend agent with Sonnet to create this API"
"Have the security agent audit the authentication code"
```

---

## ⚡ Slash Commands

Quick commands for common workflows:

```bash
/new-project <type> <name>    # Create new project
/architect <description>       # Design architecture
/api <resource> <method>      # Generate API endpoint
/component <name> <type>      # Create UI component
/migrate <description>        # Create database migration
/security-audit <scope>       # Run security audit
```

---

## 🎣 Hooks System

Automated quality enforcement:

- **session-start.sh** - Display project context on session start
- **pre-commit.sh** - Run quality checks before git commit (lint, tests, secrets scan)
- **post-tool-use.sh** - Auto-format code after edits

---

## 🔌 MCP Integration

Pre-configured MCP servers (7 total):

- **Filesystem** - Enhanced file operations
- **GitHub** - GitHub API integration
- **PostgreSQL** - Direct database access
- **Context7** - Access docs for React, Next.js, TypeScript, 100+ libraries
- **Tavily** - Web search capabilities
- **Playwright** - Browser automation and testing
- **Sentry** - Error tracking and monitoring

Configure in `.mcp.json` - just add your API keys to `.env`

## 🔗 Popular Integrations

Framework includes guides and agent knowledge for:

- **Supabase** - Database + Auth + Storage
- **Prisma** - Type-safe ORM with migrations
- **Redis** - Caching and rate limiting
- **Stripe** - Payment processing
- **SendGrid/Resend** - Email services
- **AWS S3** - File storage

See **[INTEGRATIONS.md](docs/INTEGRATIONS.md)** for complete setup guides

---

## 📚 Documentation

### Essential Reading

1. **[FRAMEWORK_GUIDE.md](docs/FRAMEWORK_GUIDE.md)** - Complete framework documentation (500+ lines)
   - Architecture blueprint
   - Agent hierarchy
   - Workflow patterns
   - Development guidelines
   - Deployment strategies

2. **[INTEGRATIONS.md](docs/INTEGRATIONS.md)** - Popular service integrations
   - Supabase, Prisma, Redis setup
   - Stripe payment processing
   - Email services (SendGrid, Resend)
   - MCP server configurations
   - Environment variables guide

3. **[MODEL_SELECTION.md](docs/MODEL_SELECTION.md)** - Model flexibility & cost optimization
   - When to use Opus vs Sonnet vs Haiku
   - Cost optimization strategies
   - How to override model preferences
   - No agent fails without Opus credits!

4. **[CLAUDE.md](CLAUDE.md)** - Project-specific guidelines
   - Code standards
   - Common patterns
   - Git workflow
   - Testing requirements

5. **[Agent Orchestration](docs/architecture/AGENT_ORCHESTRATION.md)** - How to use agents effectively

---

## 🏗️ Architecture Principles

### 1. Separation of Concerns

- **Backend**: Independent services in separate directories
- **Frontend**: Separate applications with shared component libraries
- **Database**: Version-controlled schemas and migrations

### 2. Clean Code Standards

- TypeScript strict mode (no `any` types)
- 80%+ test coverage
- Comprehensive documentation
- Security-first development

### 3. Development Workflow

```
Plan → Implement → Test → Review → Deploy
  ↓        ↓        ↓       ↓        ↓
Architect Backend  Tests  Security DevOps
Agent    Agent           Agent    Agent
```

---

## 🛠️ Tech Stack

### Backend
- Node.js 20+ with TypeScript
- Express/Fastify for APIs
- Prisma/TypeORM for database
- Jest/Vitest for testing

### Frontend
- React 18+ with TypeScript
- Next.js/Vite for build tools
- TailwindCSS for styling
- React Query for data fetching

### Database
- PostgreSQL (primary)
- Redis (caching)
- Prisma for migrations

### DevOps
- Docker & Docker Compose
- GitHub Actions for CI/CD
- Prometheus + Grafana for monitoring

---

## 🔐 Security

Built-in security features:

- ✅ Secrets scanning (gitleaks)
- ✅ Dependency auditing (npm audit)
- ✅ OWASP Top 10 compliance checks
- ✅ Security headers configuration
- ✅ Input validation (Zod)
- ✅ SQL injection prevention
- ✅ XSS protection

Run security audit:
```
/security-audit full
```

---

## 🧪 Testing

Testing requirements:

- **Unit tests**: 80% minimum coverage
- **Integration tests**: Critical paths
- **E2E tests**: User workflows

Run tests:
```bash
npm test                  # Run all tests
npm run test:coverage     # With coverage
npm run test:watch        # Watch mode
```

---

## 📦 Project Templates

### Backend Project Template

```
backend/project-name/
├── src/
│   ├── routes/          # HTTP routes
│   ├── controllers/     # Request handlers
│   ├── services/        # Business logic
│   ├── repositories/    # Data access
│   ├── models/          # Data models
│   ├── middlewares/     # Middlewares
│   ├── utils/           # Utilities
│   └── types/           # TypeScript types
├── tests/
│   ├── unit/
│   ├── integration/
│   └── e2e/
├── package.json
├── tsconfig.json
└── README.md
```

### Frontend Project Template

```
frontend/project-name/
├── src/
│   ├── components/      # Reusable components
│   ├── features/        # Feature modules
│   ├── hooks/           # Custom React hooks
│   ├── lib/             # Utilities
│   ├── pages/           # Page components
│   ├── api/             # API client
│   └── types/           # TypeScript types
├── public/
├── tests/
├── package.json
└── README.md
```

---

## 🚀 Deployment

### Development
```bash
docker-compose up -d
```

### Staging
```bash
docker-compose -f docker-compose.staging.yml up -d
```

### Production
Automated via GitHub Actions on push to `main`

---

## 📊 Workflow Examples

### Create New Feature

```
1. /architect Design user authentication system
   ↓
2. Review and approve architecture
   ↓
3. /migrate create_users_and_sessions_tables
   ↓
4. /api auth/login POST
   ↓
5. /api auth/register POST
   ↓
6. /component LoginForm container
   ↓
7. /security-audit backend
   ↓
8. Test, review, deploy
```

### Fix Security Issue

```
1. /security-audit full
   ↓
2. Review findings, prioritize by severity
   ↓
3. Use appropriate agent to fix issues
   ↓
4. Run tests to verify fixes
   ↓
5. /security-audit to re-verify
   ↓
6. Deploy fixes
```

---

## 🎓 Learning Resources

### Framework Documentation
- [Complete Framework Guide](docs/FRAMEWORK_GUIDE.md)
- [Agent Orchestration Patterns](docs/architecture/AGENT_ORCHESTRATION.md)
- [Project Guidelines](CLAUDE.md)

### External Resources
- [Claude Code Best Practices](https://www.anthropic.com/engineering/claude-code-best-practices)
- [Awesome Claude Code](https://github.com/hesreallyhim/awesome-claude-code)
- [Claude Code Documentation](https://docs.anthropic.com/en/docs/claude-code)

---

## 🤝 Contributing

When adding new projects:

1. Follow the directory structure
2. Include comprehensive README
3. Add tests (80%+ coverage)
4. Update documentation
5. Run security audit
6. Create pull request

---

## 📝 Git Workflow

```bash
# Create feature branch
git checkout -b feature/my-feature

# Make changes, commit frequently
git commit -m "feat(scope): description"

# Push and create PR
git push -u origin feature/my-feature
```

**Commit Format:** Conventional Commits
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation
- `refactor`: Code restructuring
- `test`: Tests
- `chore`: Maintenance

---

## 🔧 Configuration

### Environment Variables

A comprehensive `.env.example` is included with 100+ variables for:

- **Databases:** PostgreSQL, Supabase, Redis
- **Authentication:** JWT, OAuth (Google, GitHub)
- **Payments:** Stripe
- **Email:** SendGrid, Resend, SMTP
- **Cloud Storage:** AWS S3, Cloudinary
- **Monitoring:** Sentry, logging
- **MCP Servers:** GitHub, Tavily, Sentry
- **Feature Flags:** Enable/disable features

Copy `.env.example` to `.env` and fill in your values.

**Never commit `.env` files!** Only commit `.env.example`

See **[INTEGRATIONS.md](docs/INTEGRATIONS.md)** for service-specific setup guides.

---

## 💡 Tips & Tricks

### Use the Right Agent

- 🏗️ Planning? → **Architect Agent**
- 🔧 Building API? → **Backend Agent**
- 💻 Creating UI? → **Frontend Agent**
- 🗄️ Database work? → **Database Agent**
- 🚀 Deploying? → **DevOps Agent**
- 🔐 Security? → **Security Agent**

### Leverage Automation

- Use slash commands for common tasks
- Let hooks enforce quality automatically
- Use MCP servers for integrations

### Keep Context Clean

- Use `/clear` between unrelated tasks
- Provide relevant file paths
- Reference existing patterns

---

## ❓ FAQ

**Q: How do I create a new project?**
A: Use `/new-project <type> <name>` or follow the templates

**Q: Which agent should I use?**
A: See [Agent Orchestration Guide](docs/architecture/AGENT_ORCHESTRATION.md)

**Q: What if I run out of Opus credits?**
A: No problem! All agents work perfectly with Sonnet. The model field is just a recommendation. See [MODEL_SELECTION.md](docs/MODEL_SELECTION.md)

**Q: How do I integrate Supabase/Prisma/Redis/Stripe?**
A: See [INTEGRATIONS.md](docs/INTEGRATIONS.md) for complete setup guides with code examples

**Q: How do I run security audits?**
A: Use `/security-audit full`

**Q: Can I customize the framework?**
A: Yes! Modify agents, commands, and hooks as needed

**Q: Which MCP servers are pre-configured?**
A: Filesystem, GitHub, PostgreSQL, Context7, Tavily, Playwright, Sentry. Just add API keys to `.env`

---

## 📄 License

This is a research repository template. Use freely for your projects.

---

## 🌟 What Makes This Framework Special?

✅ **Complete** - Everything you need to start building
✅ **Structured** - Clean organization from day one
✅ **Automated** - Slash commands and hooks save time
✅ **Intelligent** - Specialized agents for each domain
✅ **Secure** - Built-in security best practices
✅ **Scalable** - Grows from prototype to production
✅ **Documented** - Comprehensive guides and examples

---

**Ready to build? Start with:**
```
claude
/new-project backend my-first-api
```

Happy coding! 🚀
