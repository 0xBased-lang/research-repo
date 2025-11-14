# Security Audit

Perform a comprehensive security audit of the codebase using the security-agent.

## Scope

$SCOPE (full/backend/frontend/database/specific-feature)

## Instructions

1. **Invoke Security Agent**
   Use the security-agent for this audit.

2. **OWASP Top 10 Check**
   Scan for:
   - [ ] Broken Access Control
   - [ ] Cryptographic Failures
   - [ ] Injection vulnerabilities
   - [ ] Insecure Design
   - [ ] Security Misconfiguration
   - [ ] Vulnerable and Outdated Components
   - [ ] Identification and Authentication Failures
   - [ ] Software and Data Integrity Failures
   - [ ] Security Logging and Monitoring Failures
   - [ ] Server-Side Request Forgery (SSRF)

3. **Code Analysis**
   Review for:
   - Hard-coded secrets or credentials
   - Insecure cryptographic functions
   - Missing input validation
   - SQL injection vulnerabilities
   - XSS vulnerabilities
   - Insecure deserialization
   - Inadequate error handling
   - Missing authorization checks

4. **Dependency Audit**
   ```bash
   npm audit
   npm audit --json > audit-report.json
   ```
   - Check for known vulnerabilities
   - Review outdated dependencies
   - Identify unmaintained packages

5. **Configuration Review**
   - Environment variables
   - Security headers
   - CORS settings
   - Rate limiting
   - Session management
   - Authentication configuration

6. **Authentication & Authorization**
   - Password policies
   - Token management (JWT)
   - Session security
   - Role-based access control
   - Permission checks

7. **API Security**
   - Input validation on all endpoints
   - Output encoding
   - Rate limiting
   - Authentication requirements
   - Authorization checks
   - CSRF protection

8. **Database Security**
   - SQL injection prevention
   - Parameterized queries
   - Row-level security
   - Connection pooling
   - Credentials management

9. **Secrets Scanning**
   ```bash
   # Use gitleaks
   gitleaks detect --source . --verbose --report-path gitleaks-report.json
   ```

10. **Security Headers Check**
    Verify presence of:
    - Content-Security-Policy
    - X-Content-Type-Options
    - X-Frame-Options
    - Strict-Transport-Security
    - X-XSS-Protection
    - Referrer-Policy
    - Permissions-Policy

11. **Generate Report**
    Create a security audit report with:
    - Executive summary
    - Findings categorized by severity (Critical/High/Medium/Low)
    - Proof of concept (if applicable)
    - Remediation recommendations
    - Priority order for fixes

## Severity Levels

**Critical**: Immediate exploitation possible, high impact
- Hard-coded credentials
- SQL injection in production
- Authentication bypass

**High**: Exploitation likely, significant impact
- Missing authorization checks
- Weak password policies
- Insecure deserialization

**Medium**: Exploitation possible, moderate impact
- Missing security headers
- Verbose error messages
- Weak encryption

**Low**: Limited impact or difficult to exploit
- Information disclosure (non-sensitive)
- Missing rate limits on non-critical endpoints
- Outdated dependencies (no known exploits)

## Deliverables

- Security audit report
- Prioritized list of vulnerabilities
- Remediation recommendations
- Implementation timeline
- Re-audit plan

The security-agent will identify issues without making changes. Implementation should be done by appropriate dev agents after review.
