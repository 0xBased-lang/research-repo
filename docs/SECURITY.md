# Security Model & Guidelines

## Overview

This document outlines the security model for the Claude Code framework and best practices for secure usage.

---

## Threat Model

### What We Protect Against

1. **Accidental secret exposure** - Prevent committing .env files
2. **Path traversal attacks** - Prevent access to files outside project
3. **Malicious file modification** - Block sensitive system files
4. **Code injection** - Validate inputs in hooks
5. **Self-modification** - Prevent hooks from modifying themselves

### What We Don't Protect Against

1. **Malicious users with write access** - If attacker has repo write access, they can modify anything
2. **Compromised dependencies** - Supply chain attacks require dependency scanning
3. **Social engineering** - Users deliberately bypassing safety features

---

## Security Features

### 1. PreToolUse Hook (Security Layer)

**Prevents:**
- ✅ Modifying .env files (except .env.example)
- ✅ Path traversal (`../../../etc/passwd`)
- ✅ Accessing sensitive files (credentials, private keys)
- ✅ Modifying system paths (/etc/, /root/)
- ✅ Self-modifying hooks

**Example blocked operations:**
```bash
# These will be blocked:
Write to ".env"
Write to "../../.env"
Write to "secrets.json"
Write to "private.key"
Write to ".claude/hooks/PreToolUse.sh"
```

### 2. Permission System

**settings.json allows granular control:**

```json
{
  "permissions": {
    "defaultMode": "ask",  // Prompt for each tool use
    "allow": {
      "Read": ["*"],       // Allow all reads
      "Grep": ["*"]        // Allow all searches
    },
    "deny": {
      "Write": ["*.env", ".env.*"],  // Block .env writes
      "Edit": ["*.env", ".env.*"],   // Block .env edits
      "Bash": ["rm", "del"]          // Block dangerous commands
    }
  }
}
```

### 3. Agent Tool Restrictions

**Agents have limited tool access:**

```yaml
# Read-only agents (architect, security)
tools:
  allow:
    - Read
    - Grep
    - Glob
  deny:
    - Write
    - Edit
    - Bash

# Full-access agents (backend, frontend, database)
tools:
  allow:
    - Read
    - Write
    - Edit
    - Bash
  # Still blocked by PreToolUse hook for sensitive files
```

---

## Security Best Practices

### For Repository Maintainers

1. **Review all PRs that modify .claude/ directory**
   - Agents, skills, hooks can execute code
   - Malicious agents/hooks can compromise system

2. **Never commit secrets**
   - Use .env files (gitignored)
   - Use .env.example templates
   - Use secrets management (Vault, AWS Secrets Manager)

3. **Validate hook changes**
   - Hooks run with shell access
   - Test hooks in isolated environment first
   - Use `set -euo pipefail` for safety

4. **Sign git commits** (optional but recommended)
   ```bash
   git config --global commit.gpgsign true
   ```

5. **Enable branch protection**
   - Require PR reviews for .claude/
   - Require status checks to pass
   - Restrict who can push to main

### For Users

1. **Don't load untrusted agents/skills**
   - Only use agents from trusted sources
   - Review agent prompts before invoking
   - Skills provide context that AI uses - verify they're accurate

2. **Validate .env files**
   - Never commit .env
   - Use strong, unique secrets
   - Rotate secrets regularly

3. **Review hook execution**
   - Hooks have file system access
   - Check hook output in logs
   - Report suspicious behavior

4. **Use principle of least privilege**
   - Grant only necessary permissions
   - Use read-only agents when possible
   - Avoid `"allow": ["*"]` unless needed

5. **Keep dependencies updated**
   ```bash
   npm audit
   npm audit fix
   ```

---

## Security Checklist

### Initial Setup

- [ ] Review all files in .claude/ directory
- [ ] Verify .gitignore includes .env
- [ ] Copy .env.example to .env
- [ ] Fill .env with your secrets (don't commit!)
- [ ] Test PreToolUse hook blocks .env writes
- [ ] Review agent tool permissions
- [ ] Set up MCP servers with API keys in .env

### Regular Maintenance

- [ ] Review PRs that touch .claude/
- [ ] Audit npm/pip dependencies monthly
- [ ] Rotate API keys quarterly
- [ ] Review hook logs for anomalies
- [ ] Update Claude Code regularly

### Before Production Deployment

- [ ] Run security audit: `/security-audit full`
- [ ] Verify all secrets are in environment (not hardcoded)
- [ ] Test error handling (what if API is down?)
- [ ] Set up monitoring and alerting
- [ ] Document incident response plan
- [ ] Enable HTTPS everywhere
- [ ] Configure rate limiting
- [ ] Set up WAF (Web Application Firewall) if applicable

---

## Common Security Issues

### Issue: "My .env file was committed!"

**Prevention:**
1. `.env` is in `.gitignore`
2. PreToolUse hook blocks .env modifications
3. GitHub has secret scanning (alerts if secrets detected)

**Remediation:**
1. Immediately rotate all secrets in the committed .env
2. Remove from git history:
   ```bash
   git filter-branch --force --index-filter \
     'git rm --cached --ignore-unmatch .env' \
     --prune-empty --tag-name-filter cat -- --all
   ```
3. Force push (if safe to do): `git push --force`
4. Or use git-filter-repo / BFG Repo-Cleaner

### Issue: "Hook is blocking legitimate operations"

**Solution:**
1. Temporarily disable hook:
   ```bash
   chmod -x .claude/hooks/PreToolUse.sh
   ```
2. Perform operation
3. Re-enable hook:
   ```bash
   chmod +x .claude/hooks/PreToolUse.sh
   ```
4. Report false positive so we can improve hook logic

### Issue: "Agent is trying to use denied tools"

**Expected behavior:** Claude Code blocks the operation and shows error

**If bypassed:** Report as bug - this shouldn't happen

---

## Reporting Security Issues

**DO NOT** open public GitHub issues for security vulnerabilities.

Instead:
1. Email security concerns to repository maintainer
2. Use GitHub Security Advisories (if available)
3. Allow 90 days for fix before public disclosure

**Include:**
- Description of vulnerability
- Steps to reproduce
- Impact assessment
- Suggested fix (if any)

---

## Security Updates

We will issue security updates for:
- Critical vulnerabilities in hooks
- Privilege escalation bugs
- Secret exposure risks
- Cross-platform security issues

**Stay updated:**
- Watch repository for security advisories
- Subscribe to release notifications
- Review CHANGELOG.md

---

## Compliance

### OWASP Top 10 Coverage

See `/security-audit` command and `security-checklist.md` skill for details on:
1. Broken Access Control → ✅ Covered
2. Cryptographic Failures → ✅ Covered
3. Injection → ✅ Covered
4. Insecure Design → ✅ Covered
5. Security Misconfiguration → ✅ Covered
6. Vulnerable Components → ✅ Covered (npm audit)
7. Authentication Failures → ✅ Covered
8. Data Integrity Failures → ✅ Covered
9. Logging Failures → ✅ Covered
10. SSRF → ✅ Covered

### Data Privacy

- Framework doesn't collect telemetry
- Hooks run locally, don't send data externally
- MCP servers may send data to their services (review each)
- Skills are static text, no data collection

---

## Conclusion

Security is a shared responsibility:
- **Framework** provides secure defaults
- **Maintainers** review changes carefully
- **Users** follow best practices

**When in doubt, ask!** Better to check than to introduce a vulnerability.
