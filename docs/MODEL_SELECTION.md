# Model Selection & Flexibility Guide

## Understanding Model Configuration in Agents

When you see this in an agent file:

```yaml
---
name: architect-agent
model: opus
---
```

**Important:** The `model` field is a **recommendation**, NOT a requirement!

---

## Can I Use Different Models?

**YES!** All agents work with any Claude model. The `model` field suggests the optimal model for that agent's tasks, but you can:

1. ✅ Use Sonnet for everything if Opus is unavailable
2. ✅ Use Haiku for simple tasks to save costs
3. ✅ Override model on a per-invocation basis
4. ✅ Change the default in agent configuration

---

## Model Recommendations Explained

### Why Certain Agents Suggest Opus

**Architect Agent** → `model: opus`
- **Why:** Complex reasoning for system design
- **Fallback:** Sonnet works fine for most architectures
- **When Opus matters:** Very complex distributed systems, novel architecture patterns

**Security Agent** → `model: opus`
- **Why:** Deep analysis for vulnerability detection
- **Fallback:** Sonnet can handle most security audits
- **When Opus matters:** Novel attack vectors, complex cryptographic issues

### Why Certain Agents Suggest Sonnet

**Backend/Frontend/Database Agents** → `model: sonnet`
- **Why:** Excellent code generation, good reasoning, cost-effective
- **Fallback:** Haiku for simple boilerplate
- **When Sonnet matters:** Complex business logic, intricate state management

**DevOps Agent** → `model: sonnet`
- **Why:** Good at configuration, pipelines, infrastructure code
- **Fallback:** Haiku for simple Docker configs
- **When Sonnet matters:** Complex Kubernetes manifests, multi-stage pipelines

---

## Model Comparison

| Feature | Haiku | Sonnet | Opus |
|---------|-------|--------|------|
| **Speed** | ⚡⚡⚡ Fastest | ⚡⚡ Fast | ⚡ Slower |
| **Cost** | 💰 Cheapest | 💰💰 Moderate | 💰💰💰 Most expensive |
| **Code Quality** | ✅ Good | ✅✅ Excellent | ✅✅✅ Outstanding |
| **Reasoning** | 🧠 Basic | 🧠🧠 Strong | 🧠🧠🧠 Superior |
| **Complex Tasks** | ⚠️ Limited | ✅ Capable | ✅✅ Excels |
| **Best For** | Boilerplate, simple tasks | Most development work | Architecture, complex logic |

---

## How to Override Model Selection

### Method 1: Temporary Override (In Session)

Just specify when invoking an agent:

```
"Use the architect agent with Sonnet model to design this"
"Have the security agent use Sonnet to audit this code"
```

Claude Code will respect your preference.

### Method 2: Edit Agent Configuration

Change the model in the agent file:

```yaml
---
name: architect-agent
description: System architect
model: sonnet  # Changed from opus
---
```

### Method 3: Remove Model Specification

Let Claude Code use the default:

```yaml
---
name: architect-agent
description: System architect
# No model field = uses session default
---
```

### Method 4: Global Default

Set your preferred model at the start of each session:

```
"For this session, use Sonnet for all agents"
```

---

## Cost Optimization Strategies

### Strategy 1: Smart Model Selection by Task Complexity

```
Simple Task (Boilerplate) → Haiku
├─ Generate basic CRUD endpoint
├─ Create simple component
└─ Basic Dockerfile

Medium Task (Most Work) → Sonnet
├─ Complex business logic
├─ API integration
├─ State management
└─ Database optimization

Complex Task (Rare) → Opus
├─ Novel architecture design
├─ Complex security analysis
├─ Distributed systems
└─ Performance optimization
```

### Strategy 2: Use Sonnet as Primary, Opus Selectively

**Default to Sonnet for:**
- All development tasks (backend, frontend, database)
- Most architecture decisions
- Security audits of standard patterns
- DevOps configurations

**Use Opus only for:**
- Truly complex architecture (microservices, distributed systems)
- Novel security vulnerabilities
- Complex performance optimization
- Critical production issues

### Strategy 3: Start with Lower Model, Escalate if Needed

```
Attempt 1: Try Haiku
    ↓ (if insufficient)
Attempt 2: Use Sonnet
    ↓ (if still insufficient)
Attempt 3: Use Opus
```

---

## Real-World Cost Examples

Assuming typical usage:

### All Opus (Expensive)
```
Architect: Opus ✗ Unnecessary for simple architectures
Backend: Opus ✗ Overkill for CRUD APIs
Frontend: Opus ✗ Excessive for UI components
Database: Opus ✗ Too much for migrations
Security: Opus ✓ Good for critical audits
DevOps: Opus ✗ Wasteful for configs

Estimated: $$$$$
```

### Recommended Mix (Cost-Effective)
```
Architect: Sonnet ✓ Handles 90% of architectures
Backend: Sonnet ✓ Excellent code generation
Frontend: Sonnet ✓ Great for React/TypeScript
Database: Sonnet ✓ Perfect for schemas/migrations
Security: Sonnet ✓ Good for standard audits
DevOps: Sonnet ✓ Ideal for infrastructure code

Estimated: $$ (60-70% cheaper)
```

### Budget Mode (Maximum Savings)
```
Architect: Sonnet ✓ Use Opus only for complex systems
Backend: Haiku ✓ Simple endpoints, Sonnet for complex
Frontend: Haiku ✓ Simple components, Sonnet for complex
Database: Haiku ✓ Basic migrations, Sonnet for optimization
Security: Sonnet ✓ Use Opus only for critical audits
DevOps: Haiku ✓ Most configs, Sonnet for complex

Estimated: $ (80-85% cheaper)
```

---

## When Each Model Truly Matters

### Haiku is Sufficient For:

✅ **Backend:**
- Simple CRUD endpoints
- Basic validation
- Standard middleware
- Boilerplate code

✅ **Frontend:**
- Simple presentational components
- Basic forms
- Standard layouts
- CSS/styling

✅ **Database:**
- Simple table creation
- Basic indexes
- Standard foreign keys

✅ **DevOps:**
- Basic Dockerfiles
- Simple CI/CD steps
- Standard nginx configs

### Sonnet is Better For:

✅ **Backend:**
- Complex business logic
- Authentication/authorization
- API integrations
- Error handling patterns
- Performance optimization

✅ **Frontend:**
- Complex state management
- Custom hooks
- Performance optimization
- Accessibility implementation
- Advanced TypeScript types

✅ **Database:**
- Complex queries
- Query optimization
- Data modeling
- Migration strategies

✅ **DevOps:**
- Multi-stage builds
- Complex CI/CD workflows
- Kubernetes configurations
- Monitoring setup

### Opus is Worth It For:

✅ **Architecture:**
- Microservices design
- Distributed systems
- Event-driven architecture
- Real-time systems
- Novel patterns

✅ **Security:**
- Zero-day vulnerability analysis
- Complex attack vector identification
- Cryptographic implementations
- Novel security patterns

✅ **Complex Problems:**
- Performance bottlenecks
- Scalability challenges
- System-wide refactoring
- Critical bug investigation

---

## Practical Recommendations

### For Most Projects: Use Sonnet Everywhere

**Why:**
- Sonnet handles 95% of development tasks excellently
- Significant cost savings vs. Opus
- Fast enough for good developer experience
- High-quality code generation

**Configuration:**
```yaml
# Update all agents to use Sonnet
model: sonnet
```

### When to Upgrade to Opus

**Indicators you need Opus:**
- Claude with Sonnet suggests a complex approach you don't understand
- Multiple iterations with Sonnet aren't solving the problem
- Designing truly novel architecture (not standard patterns)
- Critical security audit for production launch
- Complex performance optimization needed

**How to upgrade:**
Just ask: "Use Opus model for this task"

### Budget Constraints: Haiku + Sonnet Mix

**Use Haiku for:**
- Initial boilerplate generation
- Simple file creation
- Basic configuration files
- Quick prototypes

**Use Sonnet for:**
- Refining Haiku's output
- Complex logic
- Production code
- Critical features

**Skip Opus entirely** unless absolutely critical.

---

## FAQ

**Q: Will agents fail if I don't have Opus credits?**
A: No! Agents automatically fall back to Sonnet if Opus is unavailable.

**Q: Can I set a global "use Sonnet only" preference?**
A: Yes, either edit all agent files or specify at session start: "Use Sonnet for all agents this session"

**Q: Is the code quality worse with Sonnet vs Opus?**
A: For 95% of development tasks, Sonnet produces equivalent quality. Opus shines in complex reasoning and novel problem-solving.

**Q: Should I ever use Haiku?**
A: Yes! Haiku is great for boilerplate, simple components, and prototypes. It's fast and cheap.

**Q: What if a task is too complex for the chosen model?**
A: Claude will indicate if a task is challenging. You can then retry with a more capable model.

**Q: Can I mix models in a single workflow?**
A: Absolutely! "Use Sonnet to design, then Haiku to generate boilerplate"

---

## Model Selection Decision Tree

```
Is this a novel/complex architecture problem?
    YES → Use Opus
    NO ↓

Is this a critical security audit for production?
    YES → Use Opus
    NO ↓

Is this complex business logic or advanced patterns?
    YES → Use Sonnet
    NO ↓

Is this standard CRUD/boilerplate/simple code?
    YES → Use Haiku (or Sonnet if you prefer quality)
    NO ↓

Default → Use Sonnet (best balance)
```

---

## Summary

✅ **Model field is a suggestion, not a requirement**
✅ **All agents work with all models**
✅ **Sonnet is recommended for 95% of tasks**
✅ **Opus is valuable for complex reasoning**
✅ **Haiku is great for cost savings on simple tasks**
✅ **You can override model anytime**
✅ **No agent will fail due to model unavailability**

**Bottom Line:** Don't worry about Opus credits. Sonnet is excellent for nearly everything. Use Opus only when you truly need superior reasoning for complex problems.

---

## Recommended Default Configuration

Edit all agents to use Sonnet by default:

```bash
# Quick script to update all agents
cd .claude/agents
for file in *.md; do
  sed -i 's/model: opus/model: sonnet/g' "$file"
done
```

Or keep the recommendations and override when needed - your choice!
