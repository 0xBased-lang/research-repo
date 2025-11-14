---
name: security-checklist
description: Provides security best practices and OWASP Top 10 guidance when discussing security, authentication, or vulnerabilities
---

# Security Checklist Skill

## Purpose

Auto-invoked when conversation involves security, authentication, vulnerabilities, or OWASP.

## Context Provided

When discussing security, always check:

### 1. OWASP Top 10 (2021)

**A01: Broken Access Control**
- ✅ Authorization checks on every endpoint
- ✅ Verify user owns resource (no IDOR)
- ✅ Role-based access control (RBAC)
- ✅ Principle of least privilege

**A02: Cryptographic Failures**
- ✅ HTTPS everywhere
- ✅ Strong password hashing (bcrypt, argon2)
- ✅ Secure encryption algorithms
- ✅ No hard-coded secrets

**A03: Injection**
- ✅ Parameterized queries (no string concatenation)
- ✅ ORM usage
- ✅ Input validation
- ✅ Output encoding

**A04: Insecure Design**
- ✅ Threat modeling
- ✅ Security requirements in design
- ✅ Rate limiting
- ✅ Defense in depth

**A05: Security Misconfiguration**
- ✅ Secure defaults
- ✅ No default credentials
- ✅ Error messages don't leak info
- ✅ Security headers configured

**A06: Vulnerable Components**
- ✅ Dependencies up to date
- ✅ npm audit / snyk scanning
- ✅ No unmaintained packages
- ✅ SCA tools in CI/CD

**A07: Authentication Failures**
- ✅ Strong password policies
- ✅ MFA available
- ✅ Rate limiting on login
- ✅ Secure session management

**A08: Data Integrity Failures**
- ✅ Signed/encrypted data
- ✅ Secure CI/CD pipelines
- ✅ Input validation
- ✅ Serialization security

**A09: Logging Failures**
- ✅ Security events logged
- ✅ Audit trail maintained
- ✅ Log injection prevented
- ✅ Monitoring and alerting

**A10: SSRF**
- ✅ URL whitelist
- ✅ No private IP access
- ✅ Input validation on URLs
- ✅ Network segmentation

### 2. Authentication Checklist

**Password Security:**
```typescript
// ✅ Good
import bcrypt from 'bcrypt';
const hash = await bcrypt.hash(password, 12);

// ❌ Bad
const hash = md5(password); // Weak algorithm
```

**JWT Security:**
```typescript
// ✅ Good
const token = jwt.sign(payload, secret, {
  expiresIn: '15m',
  algorithm: 'HS256'
});

// ❌ Bad
const token = jwt.sign(payload, 'weak-secret'); // No expiry
```

### 3. Input Validation

**Always validate:**
```typescript
import { z } from 'zod';

const userSchema = z.object({
  email: z.string().email().max(255),
  password: z.string().min(12).max(128),
  age: z.number().int().min(18).max(120).optional(),
});

// Validate before processing
const validatedData = userSchema.parse(req.body);
```

### 4. Security Headers

**Required headers:**
```typescript
app.use((req, res, next) => {
  res.setHeader('X-Content-Type-Options', 'nosniff');
  res.setHeader('X-Frame-Options', 'DENY');
  res.setHeader('X-XSS-Protection', '1; mode=block');
  res.setHeader('Strict-Transport-Security', 'max-age=31536000; includeSubDomains');
  res.setHeader('Content-Security-Policy', "default-src 'self'");
  res.setHeader('Referrer-Policy', 'strict-origin-when-cross-origin');
  next();
});
```

### 5. Rate Limiting

**Implement on sensitive endpoints:**
```typescript
import rateLimit from 'express-rate-limit';

const loginLimiter = rateLimit({
  windowMs: 15 * 60 * 1000, // 15 minutes
  max: 5, // 5 attempts
  message: 'Too many login attempts',
});

app.post('/api/login', loginLimiter, loginHandler);
```

### 6. SQL Injection Prevention

```typescript
// ❌ Bad - SQL injection vulnerable
db.query(`SELECT * FROM users WHERE email = '${email}'`);

// ✅ Good - Parameterized query
db.query('SELECT * FROM users WHERE email = $1', [email]);

// ✅ Good - ORM
db.user.findUnique({ where: { email } });
```

### 7. XSS Prevention

**Backend:**
```typescript
// Set Content-Security-Policy header
app.use(helmet.contentSecurityPolicy({
  directives: {
    defaultSrc: ["'self'"],
    scriptSrc: ["'self'"],
  },
}));
```

**Frontend:**
```typescript
import DOMPurify from 'dompurify';

// Sanitize user input
const clean = DOMPurify.sanitize(userInput);
```

### 8. Secrets Management

**Never commit:**
- `.env` files
- API keys
- Database credentials
- Private keys/certificates

**Use:**
- Environment variables
- Secrets management (Vault, AWS Secrets Manager)
- `.env.example` template only

### 9. CORS Configuration

```typescript
import cors from 'cors';

app.use(cors({
  origin: process.env.ALLOWED_ORIGINS?.split(','),
  credentials: true,
  methods: ['GET', 'POST', 'PUT', 'DELETE'],
  allowedHeaders: ['Content-Type', 'Authorization'],
}));
```

### 10. Error Handling

```typescript
// ❌ Bad - Leaks information
app.use((err, req, res, next) => {
  res.status(500).json({ error: err.stack });
});

// ✅ Good - Generic error
app.use((err, req, res, next) => {
  logger.error(err); // Log internally
  res.status(500).json({
    error: 'An unexpected error occurred'
  });
});
```

### 11. Security Audit Commands

```bash
# Dependency audit
npm audit
npm audit fix

# Secrets scanning
gitleaks detect --source . --verbose

# Static analysis
eslint --ext .js,.ts .
```

## When This Skill Activates

Automatically provides this context when you mention:
- "security" or "secure"
- "authentication" or "authorization"
- "vulnerability" or "exploit"
- "OWASP"
- "SQL injection" or "XSS"
- "password" or "hash"
- "token" or "JWT"
- "audit" or "scan"
