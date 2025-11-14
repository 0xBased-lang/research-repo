# Production Deployment Checklist

## Pre-Deployment Checklist

Use this before deploying the framework to production.

---

## 🔧 Configuration

### Environment Variables
- [ ] All required env vars documented in .env.example
- [ ] .env file created locally (NOT committed)
- [ ] All secrets use strong, unique values
- [ ] API keys have appropriate scopes/permissions
- [ ] Database credentials are secure
- [ ] JWT secrets are 32+ characters, randomly generated

### File Structure
- [ ] .gitignore includes .env, .env.*, secrets/
- [ ] .gitattributes configured for line endings
- [ ] All hooks are executable (`chmod +x .claude/hooks/*.sh`)
- [ ] settings.json has correct permissions configured
- [ ] No sensitive files in version control

---

## 🛡️ Security

### Hooks
- [ ] PreToolUse hook tested (blocks .env modifications)
- [ ] PreToolUse hook tested (blocks path traversal)
- [ ] PostToolUse hook has timeouts configured
- [ ] Hooks run from correct working directory
- [ ] Hooks don't log sensitive data

### Agents
- [ ] All agents reviewed for appropriate tool permissions
- [ ] Read-only agents can't modify files
- [ ] Agent prompts don't suggest insecure code
- [ ] No hardcoded secrets in agent prompts

### Skills
- [ ] All skills provide accurate, secure guidance
- [ ] Skills don't contradict security best practices
- [ ] No untrusted skills loaded

### MCP Servers
- [ ] All MCP API keys in .env (not hardcoded)
- [ ] MCP server errors handled gracefully
- [ ] Timeout configured for each MCP server
- [ ] Unnecessary MCP servers disabled

---

## 🧪 Testing

### Unit Tests
- [ ] All critical functionality has tests
- [ ] Test coverage ≥ 80%
- [ ] Tests pass: `npm test`
- [ ] No skipped/pending tests in production code

### Integration Tests
- [ ] Database connections tested
- [ ] API integrations tested
- [ ] MCP servers tested (or mocked)
- [ ] Error handling tested

### Security Tests
- [ ] `/security-audit full` run and passed
- [ ] No critical/high vulnerabilities
- [ ] `npm audit` shows no high/critical issues
- [ ] Secrets scanning (gitleaks) shows no leaks

### Cross-Platform Tests (if applicable)
- [ ] Tested on Linux
- [ ] Tested on macOS (or hooks use portable grep)
- [ ] Tested on Windows (WSL/Git Bash)

---

## 📦 Dependencies

### Backend
- [ ] `npm audit` run, vulnerabilities addressed
- [ ] Dependencies updated to latest stable
- [ ] No deprecated dependencies
- [ ] Lock file (package-lock.json) committed

### Frontend
- [ ] Same as backend
- [ ] Bundle size analyzed and optimized
- [ ] No dev dependencies in production build

### Python (if applicable)
- [ ] `pip-audit` or `safety check` run
- [ ] requirements.txt or Pipfile.lock committed

---

## 🚀 Deployment

### Database
- [ ] Migrations tested on staging
- [ ] Backup of production data created
- [ ] Migration rollback plan documented
- [ ] Indexes created for performance

### Application
- [ ] Environment set to `NODE_ENV=production`
- [ ] Debug mode disabled
- [ ] Verbose logging disabled (LOG_LEVEL=info or error)
- [ ] Source maps generated (but not served to clients)
- [ ] Health check endpoint working (`/health`)

### Infrastructure
- [ ] HTTPS configured (not HTTP)
- [ ] SSL certificates valid and auto-renewing
- [ ] Load balancer configured (if applicable)
- [ ] Auto-scaling configured (if applicable)
- [ ] CDN configured for static assets (if applicable)

### CI/CD
- [ ] GitHub Actions (or CI) running successfully
- [ ] Automated tests run on every PR
- [ ] Automated deployment on merge to main
- [ ] Rollback procedure documented
- [ ] Secrets stored in CI/CD secrets (not in code)

---

## 📊 Monitoring

### Logs
- [ ] Centralized logging configured (CloudWatch, Datadog, etc.)
- [ ] Log rotation configured
- [ ] Logs don't contain sensitive data (passwords, tokens)
- [ ] Error logs monitored

### Metrics
- [ ] Application metrics collected (Prometheus, Datadog, etc.)
- [ ] Infrastructure metrics monitored (CPU, memory, disk)
- [ ] Database metrics monitored (connections, query time)
- [ ] API response times tracked

### Alerts
- [ ] Error rate alerts configured
- [ ] Performance degradation alerts
- [ ] Database connection alerts
- [ ] Disk space alerts
- [ ] SSL certificate expiration alerts

### Error Tracking
- [ ] Sentry (or similar) configured
- [ ] Errors grouped and prioritized
- [ ] Notifications set up for critical errors
- [ ] Source maps uploaded for stack traces

---

## 🔒 Security (Production)

### Headers
- [ ] Security headers configured (CSP, HSTS, etc.)
- [ ] CORS configured with specific origins (not *)
- [ ] X-Frame-Options: DENY or SAMEORIGIN
- [ ] X-Content-Type-Options: nosniff

### Rate Limiting
- [ ] Rate limiting enabled on API
- [ ] Stricter limits on auth endpoints (login, register)
- [ ] DDoS protection configured (Cloudflare, AWS Shield)

### Authentication
- [ ] Password requirements enforced (min length, complexity)
- [ ] Account lockout after failed attempts
- [ ] JWT tokens have short expiration (15m recommended)
- [ ] Refresh tokens rotate on use
- [ ] Session management secure (httpOnly cookies)

### Authorization
- [ ] All endpoints check permissions
- [ ] Principle of least privilege applied
- [ ] Role-based access control (RBAC) implemented
- [ ] No IDOR vulnerabilities

### Data Protection
- [ ] Sensitive data encrypted at rest
- [ ] Passwords hashed with bcrypt/argon2 (NOT md5/sha1)
- [ ] PII handling compliant with regulations
- [ ] Data retention policy implemented

---

## 📝 Documentation

### Code Documentation
- [ ] README.md up to date
- [ ] API documentation current (OpenAPI/Swagger)
- [ ] Architecture docs reflect reality
- [ ] CHANGELOG.md updated

### Operational Documentation
- [ ] Deployment process documented
- [ ] Rollback procedure documented
- [ ] Incident response plan documented
- [ ] On-call runbook created
- [ ] Emergency contacts listed

### User Documentation
- [ ] User guides current
- [ ] API examples working
- [ ] FAQs address common issues

---

## 🔄 Post-Deployment

### Immediate (First Hour)
- [ ] Monitor logs for errors
- [ ] Check health endpoint
- [ ] Verify key user flows work
- [ ] Monitor error rates
- [ ] Check database connections

### First Day
- [ ] Review metrics for anomalies
- [ ] Check for performance degradation
- [ ] Monitor error tracking dashboard
- [ ] Verify background jobs running
- [ ] Check email/notifications working

### First Week
- [ ] Analyze usage patterns
- [ ] Review user feedback
- [ ] Check for performance bottlenecks
- [ ] Monitor costs (infrastructure)
- [ ] Plan for scaling if needed

---

## 🚨 Rollback Plan

### When to Rollback
- Critical bugs affecting > 25% of users
- Security vulnerability discovered
- Data corruption detected
- Performance degraded > 50%

### How to Rollback

#### Application
```bash
# Git revert
git revert <bad-commit>
git push

# Or deploy previous version
git checkout <previous-tag>
./deploy.sh

# Or CI/CD
# Trigger deployment of previous version
```

#### Database
```bash
# Run down migration
npx prisma migrate resolve --rolled-back <migration-name>

# Or restore from backup
pg_restore -h localhost -U postgres -d mydb backup.sql
```

#### Infrastructure
```bash
# Rollback infrastructure changes
terraform apply -target=... # previous state

# Or use blue-green deployment
# Switch traffic back to previous version
```

---

## ✅ Sign-Off

### Development Team
- [ ] Code reviewed and approved
- [ ] All tests passing
- [ ] Documentation updated
- [ ] Signed by: ________________ Date: ________

### QA Team
- [ ] Smoke tests passed on staging
- [ ] Performance tests passed
- [ ] Security tests passed
- [ ] Signed by: ________________ Date: ________

### Security Team
- [ ] Security audit completed
- [ ] Vulnerabilities addressed
- [ ] Compliance verified
- [ ] Signed by: ________________ Date: ________

### Operations Team
- [ ] Infrastructure ready
- [ ] Monitoring configured
- [ ] Alerts set up
- [ ] Signed by: ________________ Date: ________

---

## 📌 Additional Notes

### Lessons Learned (Post-Deployment)
Document issues encountered:
1.
2.
3.

### Performance Baseline
Record baseline metrics for comparison:
- Response time (p50): ______ ms
- Response time (p95): ______ ms
- Response time (p99): ______ ms
- Error rate: ______ %
- Throughput: ______ req/s

---

**Deployment Date:** ______________
**Deployed By:** ______________
**Version/Tag:** ______________
**Environment:** Production / Staging / Development
