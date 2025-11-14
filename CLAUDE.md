# Research Repository - Claude Code Framework

## 🎯 Project Overview
Multi-project research repository with clean separation of concerns across backend, frontend, and database components.

## 📁 Repository Structure

```
research-repo/
├── .claude/                    # Claude Code configuration
│   ├── commands/               # Slash commands
│   ├── agents/                 # Specialized agents
│   └── hooks/                  # Event-driven automations
├── backend/                    # Backend services & APIs
├── frontend/                   # Frontend applications
├── database/                   # Database schemas & migrations
├── docs/                       # Documentation
├── scripts/                    # Automation scripts
└── CLAUDE.md                   # This file
```

## 🏗️ Architecture Principles

### 1. Separation of Concerns
- **Backend**: Independent services, each in its own directory
- **Frontend**: Separate apps, shared component library
- **Database**: Version-controlled schemas, migrations, and seeds

### 2. Clean Code Standards
- **TypeScript**: Strict mode enabled, no `any` types
- **Python**: Type hints required, PEP 8 compliant
- **Documentation**: Every module has a README.md
- **Testing**: Minimum 80% coverage required

### 3. Development Workflow
- **Plan**: Always research and plan before coding
- **Implement**: Small, focused commits
- **Test**: Write tests first (TDD)
- **Review**: Self-review before committing
- **Document**: Update docs with code changes

## 🚀 Quick Start Commands

### Development
```bash
# Backend
cd backend/<project-name>
npm install && npm run dev

# Frontend
cd frontend/<project-name>
npm install && npm run dev

# Database
cd database/<project-name>
npm run migrate
```

### Testing
```bash
# Run all tests
npm test

# Run with coverage
npm run test:coverage

# Run specific test
npm test <test-file>
```

### Deployment
```bash
# Build for production
npm run build

# Deploy to staging
npm run deploy:staging

# Deploy to production
npm run deploy:prod
```

## 🤖 Agent Usage Guidelines

### When to Use Agents
- **architect-agent**: System design, architecture decisions
- **backend-agent**: API development, server-side logic
- **frontend-agent**: UI components, client-side features
- **database-agent**: Schema design, query optimization
- **security-agent**: Security audits, vulnerability scanning
- **devops-agent**: CI/CD, deployment, infrastructure

### Agent Workflow
1. Use `/agent-help` to see all available agents
2. Select appropriate agent for task
3. Agent operates with specialized context
4. Review agent's work before merging

## 📋 Slash Commands

- `/new-project` - Create new project with templates
- `/architect` - Design system architecture
- `/api` - Generate API endpoints
- `/component` - Create UI components
- `/migrate` - Create database migration
- `/test` - Generate tests for code
- `/deploy` - Deploy to environment
- `/review` - Code review current changes
- `/docs` - Generate/update documentation

## 🔐 Security Standards

### Never Commit
- `.env` files (use `.env.example` instead)
- API keys or secrets
- Database credentials
- Private certificates

### Always Use
- Environment variables for config
- Encrypted secrets management
- HTTPS for all communications
- Input validation and sanitization
- SQL parameterized queries

## 📝 Documentation Requirements

Every project MUST have:
- `README.md` - Overview, setup, usage
- `API.md` - API documentation (if applicable)
- `ARCHITECTURE.md` - System design
- `CHANGELOG.md` - Version history
- `CONTRIBUTING.md` - Contribution guidelines

## 🧪 Testing Standards

### Test Coverage
- Unit tests: 80% minimum
- Integration tests: Critical paths
- E2E tests: User workflows

### Test Structure
```
tests/
├── unit/           # Unit tests
├── integration/    # Integration tests
└── e2e/           # End-to-end tests
```

## 🎨 Code Style

### TypeScript/JavaScript
- ESLint + Prettier configured
- Functional programming preferred
- Async/await over promises
- Descriptive variable names

### Python
- Black formatter
- Pylint enabled
- Type hints required
- Docstrings for all functions

### Git Commits
- Conventional Commits format
- Format: `type(scope): description`
- Types: feat, fix, docs, style, refactor, test, chore

## 🔄 Git Workflow

### Branching Strategy
- `main` - Production-ready code
- `develop` - Development branch
- `feature/*` - New features
- `bugfix/*` - Bug fixes
- `hotfix/*` - Production hotfixes

### Commit Process
1. Create feature branch
2. Make changes with small commits
3. Write/update tests
4. Update documentation
5. Self-review changes
6. Create pull request
7. Wait for CI/CD to pass
8. Merge after approval

## 🛠️ Common Patterns

### Backend API Pattern
```typescript
// Route → Controller → Service → Repository → Database
router.post('/api/resource', controller.create);
```

### Frontend Component Pattern
```typescript
// Container → Component → Hooks → API
export const FeatureContainer = () => {
  const data = useFeatureData();
  return <FeatureComponent data={data} />;
};
```

### Database Migration Pattern
```sql
-- Up migration
CREATE TABLE ...

-- Down migration
DROP TABLE ...
```

## 🚨 Common Pitfalls

### Avoid
- ❌ Mixing business logic in controllers
- ❌ Direct database queries in routes
- ❌ Storing state in module scope
- ❌ Ignoring error handling
- ❌ Skipping validation
- ❌ Hardcoding configuration

### Instead
- ✅ Keep controllers thin, logic in services
- ✅ Use repository pattern for data access
- ✅ Use proper state management
- ✅ Handle errors with try/catch
- ✅ Validate all inputs
- ✅ Use environment variables

## 📊 Project Status Tracking

Each project directory contains:
- `STATUS.md` - Current status, blockers, next steps
- `TODO.md` - Task backlog
- `DECISIONS.md` - Architecture decision records

## 🔍 Debugging Guidelines

1. **Read error messages carefully**
2. **Check recent changes** - What was modified?
3. **Reproduce consistently** - Can you trigger it reliably?
4. **Isolate the problem** - Narrow down the scope
5. **Fix root cause** - Not just symptoms
6. **Add tests** - Prevent regression

## 💡 Performance Guidelines

- **Backend**: Response time < 200ms
- **Frontend**: First paint < 1s
- **Database**: Query time < 100ms
- **Bundle size**: < 200KB initial load

## 📦 Dependencies

### Backend (Node.js)
- Express/Fastify for APIs
- TypeORM/Prisma for database
- Jest for testing
- Winston for logging

### Frontend (React/Next.js)
- React 18+ with hooks
- TailwindCSS for styling
- React Query for data fetching
- Vitest for testing

### Database
- PostgreSQL primary
- Redis for caching
- Migrations version controlled

## 🤝 Collaboration Guidelines

When working with Claude Code:
1. **Start with context** - Explain what you're trying to achieve
2. **Be specific** - Provide file paths, error messages, examples
3. **Review plans** - Ask Claude to plan before implementing
4. **Iterate** - Give feedback, ask for refinements
5. **Clear context** - Use `/clear` between unrelated tasks

## 📚 Learning Resources

- Architecture patterns in `docs/architecture/`
- Code examples in `docs/examples/`
- Best practices in `docs/best-practices/`
- Troubleshooting in `docs/troubleshooting/`

---

**Remember**: Quality over speed. Well-architected, tested, documented code saves time in the long run.
