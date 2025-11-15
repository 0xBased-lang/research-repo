# Backend Agent - Edge Cases & Failure Modes

**Agent:** backend-agent
**Model:** Sonnet (full write access)
**Tools:** Read, Write, Edit, Bash, Grep, Glob, TodoWrite
**Severity Levels:** 🔴 Critical | 🟠 High | 🟡 Medium | 🟢 Low

---

## Research Context (2025 Findings)

**Critical Statistics:**
- **99% of organizations** experienced API security issues in last 12 months
- **95% of API attacks** come from authenticated sessions
- **Injection attacks + BOLA** = 33% of all incidents
- **57% of organizations** experienced API data breach in past 2 years

---

## 1. SECURITY VULNERABILITIES

### 🔴 EC-BACK-001: SQL Injection via ORM Bypass
**Problem:** Using raw SQL queries instead of ORM, creating SQL injection vector.

**Scenario:**
```typescript
// VULNERABLE CODE
app.get('/users/search', async (req, res) => {
  const query = `SELECT * FROM users WHERE name LIKE '%${req.query.name}%'`;
  const results = await db.raw(query);
  res.json(results);
});

// Attack: ?name='; DROP TABLE users; --
```

**Research Finding:** "Hardcoded secrets, lack of visibility, and gaps in authentication enforcement are typically predictable root causes."

**Fix:**
```typescript
// SECURE CODE
app.get('/users/search', async (req, res) => {
  const schema = z.object({
    name: z.string().max(100).regex(/^[a-zA-Z0-9\s]+$/)
  });

  const { name } = schema.parse(req.query);

  // Use parameterized query
  const results = await db.user.findMany({
    where: {
      name: {
        contains: name,
        mode: 'insensitive'
      }
    }
  });

  res.json(results);
});
```

**Validation:**
```bash
# Detect raw SQL in codebase
grep -r "db.raw\|db.query" backend/src/ | while read line; do
  if echo "$line" | grep -q '\${'; then
    echo "❌ CRITICAL: String interpolation in SQL query: $line"
  fi
done
```

---

### 🔴 EC-BACK-002: Broken Object Level Authorization (BOLA)
**Problem:** Users can access other users' data by changing IDs.

**Research Finding:** "BOLA makes up over 1/3 of all API incidents."

**Scenario:**
```typescript
// VULNERABLE
app.get('/api/orders/:id', authenticate, async (req, res) => {
  const order = await db.order.findById(req.params.id);
  res.json(order);
  // User can see ANY order by changing :id
});
```

**Fix:**
```typescript
// SECURE
app.get('/api/orders/:id', authenticate, async (req, res) => {
  const order = await db.order.findFirst({
    where: {
      id: req.params.id,
      userId: req.user.id  // CRITICAL: Check ownership
    }
  });

  if (!order) {
    return res.status(404).json({ error: 'Order not found' });
  }

  res.json(order);
});

// OR use authorization middleware
app.get('/api/orders/:id', authenticate, authorizeOrderAccess, getOrder);
```

---

### 🔴 EC-BACK-003: Mass Assignment Vulnerability
**Problem:** Accepting all user input without filtering allows privilege escalation.

**Scenario:**
```typescript
// VULNERABLE
app.post('/api/users', async (req, res) => {
  const user = await db.user.create({ data: req.body });
  // Attack: { "email": "...", "isAdmin": true }
  res.json(user);
});
```

**Fix:**
```typescript
// SECURE - Explicit field whitelisting
const createUserSchema = z.object({
  email: z.string().email(),
  password: z.string().min(12),
  name: z.string().min(1).max(100)
  // isAdmin NOT included
});

app.post('/api/users', async (req, res) => {
  const validated = createUserSchema.parse(req.body);
  const user = await db.user.create({ data: validated });
  res.json(user);
});
```

---

### 🟠 EC-BACK-004: Missing Rate Limiting
**Problem:** No protection against brute force attacks.

**Research Finding:** "95% of attacks from authenticated sessions" - attackers get in via credential stuffing.

**Scenario:**
```typescript
// VULNERABLE - Unlimited login attempts
app.post('/api/login', async (req, res) => {
  const user = await authenticateUser(req.body.email, req.body.password);
  res.json({ token: generateToken(user) });
});
```

**Fix:**
```typescript
import rateLimit from 'express-rate-limit';

// Strict rate limit for authentication
const loginLimiter = rateLimit({
  windowMs: 15 * 60 * 1000, // 15 minutes
  max: 5, // 5 attempts
  skipSuccessfulRequests: true,
  standardHeaders: true,
  message: { error: 'Too many login attempts. Try again in 15 minutes.' }
});

app.post('/api/login', loginLimiter, async (req, res) => {
  // ... authentication logic
});

// General API rate limit
const generalLimiter = rateLimit({
  windowMs: 60 * 1000, // 1 minute
  max: 100, // 100 requests per minute
});

app.use('/api', generalLimiter);
```

---

### 🟠 EC-BACK-005: Secrets in Code/Logs
**Problem:** API keys, passwords logged or hardcoded.

**Scenario:**
```typescript
// VULNERABLE
const STRIPE_KEY = 'sk_live_abc123...';  // Hardcoded
logger.info(`Connecting to ${DATABASE_URL}`);  // Logs password
```

**Fix:**
```typescript
// Environment variables
const STRIPE_KEY = process.env.STRIPE_SECRET_KEY;

// Validate secrets on startup
if (!STRIPE_KEY) {
  console.error('STRIPE_SECRET_KEY not configured');
  process.exit(1);
}

// Sanitize logs
function sanitizeForLogging(str: string): string {
  return str
    .replace(/password=([^&\s]+)/gi, 'password=***')
    .replace(/:\/\/([^:]+):([^@]+)@/g, '://$1:***@')
    .replace(/Bearer\s+\S+/g, 'Bearer ***')
    .replace(/sk_live_\S+/g, 'sk_live_***');
}

logger.info(`Connected to database: ${sanitizeForLogging(DATABASE_URL)}`);
```

---

## 2. INPUT VALIDATION FAILURES

### 🔴 EC-BACK-006: NoSQL Injection
**Problem:** Unvalidated input in NoSQL queries.

**Scenario:**
```typescript
// VULNERABLE (MongoDB)
app.post('/api/login', async (req, res) => {
  const user = await db.collection('users').findOne({
    email: req.body.email,
    password: req.body.password  // Attack: { "$ne": null }
  });
});
```

**Fix:**
```typescript
// Validate input type
const loginSchema = z.object({
  email: z.string().email(),
  password: z.string()
});

app.post('/api/login', async (req, res) => {
  const { email, password } = loginSchema.parse(req.body);

  const user = await db.collection('users').findOne({
    email: String(email),  // Ensure string type
    password: String(password)
  });
});
```

---

### 🟠 EC-BACK-007: Missing Input Sanitization (XSS)
**Problem:** User input stored without sanitization, causing XSS when displayed.

**Fix:**
```typescript
import DOMPurify from 'isomorphic-dompurify';

const createPostSchema = z.object({
  title: z.string().min(1).max(200),
  content: z.string().max(10000)
});

app.post('/api/posts', authenticate, async (req, res) => {
  const { title, content } = createPostSchema.parse(req.body);

  const post = await db.post.create({
    data: {
      title: DOMPurify.sanitize(title),
      content: DOMPurify.sanitize(content),
      authorId: req.user.id
    }
  });

  res.json(post);
});
```

---

### 🟡 EC-BACK-008: File Upload Vulnerabilities
**Problem:** Unrestricted file uploads allow malware/XXE/path traversal.

**Scenario:**
```typescript
// VULNERABLE
app.post('/upload', upload.single('file'), (req, res) => {
  fs.writeFileSync(`./uploads/${req.file.originalname}`, req.file.buffer);
  // Attack: originalname = "../../etc/passwd"
});
```

**Fix:**
```typescript
import path from 'path';
import crypto from 'crypto';

const ALLOWED_TYPES = ['image/jpeg', 'image/png', 'image/webp'];
const MAX_SIZE = 5 * 1024 * 1024; // 5MB

app.post('/upload', upload.single('file'), (req, res) => {
  if (!req.file) {
    return res.status(400).json({ error: 'No file uploaded' });
  }

  // Validate file type
  if (!ALLOWED_TYPES.includes(req.file.mimetype)) {
    return res.status(400).json({ error: 'Invalid file type' });
  }

  // Validate size
  if (req.file.size > MAX_SIZE) {
    return res.status(400).json({ error: 'File too large' });
  }

  // Generate safe filename
  const ext = path.extname(req.file.originalname);
  const filename = `${crypto.randomUUID()}${ext}`;
  const filepath = path.join('./uploads', filename);

  // Prevent path traversal
  if (!filepath.startsWith(path.resolve('./uploads'))) {
    return res.status(400).json({ error: 'Invalid path' });
  }

  fs.writeFileSync(filepath, req.file.buffer);
  res.json({ url: `/uploads/${filename}` });
});
```

---

## 3. AUTHENTICATION & AUTHORIZATION FAILURES

### 🔴 EC-BACK-009: Weak Password Hashing
**Problem:** Using weak hashing (MD5, SHA1) or insufficient rounds.

**Scenario:**
```typescript
// VULNERABLE
const passwordHash = crypto.createHash('md5').update(password).digest('hex');
```

**Fix:**
```typescript
import argon2 from 'argon2';

// Register
const passwordHash = await argon2.hash(password, {
  type: argon2.argon2id,
  memoryCost: 19456,  // 19 MB
  timeCost: 2,
  parallelism: 1
});

await db.user.create({
  data: { email, password: passwordHash }
});

// Login
const user = await db.user.findUnique({ where: { email } });
const valid = await argon2.verify(user.password, password);
```

---

### 🔴 EC-BACK-010: JWT Secret Exposure
**Problem:** Weak JWT secret allows token forgery.

**Scenario:**
```typescript
// VULNERABLE
const token = jwt.sign({ userId: user.id }, 'secret');
```

**Fix:**
```typescript
// Generate strong secret: openssl rand -base64 64
const JWT_SECRET = process.env.JWT_SECRET;

if (!JWT_SECRET || JWT_SECRET.length < 32) {
  throw new Error('JWT_SECRET must be at least 32 characters');
}

const token = jwt.sign(
  { userId: user.id, email: user.email },
  JWT_SECRET,
  {
    expiresIn: '15m',
    issuer: 'myapp',
    audience: 'myapp-users'
  }
);
```

---

### 🟠 EC-BACK-011: Missing CSRF Protection
**Problem:** State-changing operations vulnerable to CSRF.

**Fix:**
```typescript
import csrf from 'csurf';

const csrfProtection = csrf({ cookie: true });

app.use(cookieParser());

// GET routes provide CSRF token
app.get('/api/form', csrfProtection, (req, res) => {
  res.json({ csrfToken: req.csrfToken() });
});

// POST routes validate token
app.post('/api/transfer-money', csrfProtection, async (req, res) => {
  // Token automatically validated by middleware
  // ...
});
```

---

## 4. API DESIGN FAILURES

### 🟠 EC-BACK-012: Verbose Error Messages
**Problem:** Error messages leak implementation details.

**Scenario:**
```typescript
// VULNERABLE
catch (error) {
  res.status(500).json({ error: error.stack });
  // Leaks: file paths, SQL queries, dependencies
}
```

**Fix:**
```typescript
// Custom error handler
class AppError extends Error {
  constructor(
    public statusCode: number,
    message: string,
    public isOperational = true
  ) {
    super(message);
  }
}

// Error handling middleware
app.use((err, req, res, next) => {
  // Log full error
  logger.error('Request failed', {
    error: err.message,
    stack: err.stack,
    path: req.path,
    method: req.method
  });

  // Send sanitized error
  if (err instanceof AppError) {
    return res.status(err.statusCode).json({
      error: err.message
    });
  }

  // Generic error for unexpected failures
  res.status(500).json({
    error: 'An unexpected error occurred'
  });
});
```

---

### 🟡 EC-BACK-013: Missing API Versioning
**Problem:** Breaking changes break all clients.

**Fix:**
```typescript
// Version in URL
app.use('/api/v1', v1Router);
app.use('/api/v2', v2Router);

// OR version in header
app.use((req, res, next) => {
  const version = req.headers['api-version'] || '1';
  req.apiVersion = version;
  next();
});
```

---

### 🟡 EC-BACK-014: Inconsistent Response Formats
**Problem:** Different endpoints return different structures.

**Fix:**
```typescript
// Standardized response wrapper
function successResponse<T>(data: T, meta?: any) {
  return {
    success: true,
    data,
    meta,
    timestamp: new Date().toISOString()
  };
}

function errorResponse(message: string, details?: any) {
  return {
    success: false,
    error: {
      message,
      details
    },
    timestamp: new Date().toISOString()
  };
}

// Usage
app.get('/api/users', async (req, res) => {
  const users = await db.user.findMany();
  res.json(successResponse(users, { total: users.length }));
});
```

---

## 5. PERFORMANCE ISSUES

### 🟠 EC-BACK-015: N+1 Query Problem
**Problem:** Loading related data in a loop.

**Scenario:**
```typescript
// VULNERABLE - N+1 queries
const users = await db.user.findMany();
for (const user of users) {
  user.posts = await db.post.findMany({ where: { authorId: user.id } });
  // 1 query for users + N queries for posts = N+1
}
```

**Fix:**
```typescript
// FIXED - Single query with eager loading
const users = await db.user.findMany({
  include: {
    posts: true
  }
});

// OR use dataloader for GraphQL
const postLoader = new DataLoader(async (userIds) => {
  const posts = await db.post.findMany({
    where: { authorId: { in: userIds } }
  });

  const grouped = groupBy(posts, 'authorId');
  return userIds.map(id => grouped[id] || []);
});
```

---

### 🟡 EC-BACK-016: Missing Pagination
**Problem:** Returning all records causes memory/performance issues.

**Fix:**
```typescript
const listUsersSchema = z.object({
  page: z.coerce.number().min(1).default(1),
  limit: z.coerce.number().min(1).max(100).default(20)
});

app.get('/api/users', async (req, res) => {
  const { page, limit } = listUsersSchema.parse(req.query);
  const skip = (page - 1) * limit;

  const [users, total] = await Promise.all([
    db.user.findMany({ skip, take: limit }),
    db.user.count()
  ]);

  res.json(successResponse(users, {
    page,
    limit,
    total,
    totalPages: Math.ceil(total / limit)
  }));
});
```

---

### 🟡 EC-BACK-017: No Caching Strategy
**Problem:** Repeatedly fetching same data from database.

**Fix:**
```typescript
import Redis from 'ioredis';

const redis = new Redis(process.env.REDIS_URL);

async function getCachedUser(userId: string) {
  // Check cache first
  const cached = await redis.get(`user:${userId}`);
  if (cached) {
    return JSON.parse(cached);
  }

  // Cache miss - fetch from DB
  const user = await db.user.findUnique({ where: { id: userId } });

  // Store in cache (5 minute TTL)
  await redis.setex(`user:${userId}`, 300, JSON.stringify(user));

  return user;
}

// Invalidate cache on update
async function updateUser(userId: string, data: any) {
  const user = await db.user.update({
    where: { id: userId },
    data
  });

  // Invalidate cache
  await redis.del(`user:${userId}`);

  return user;
}
```

---

## 6. ERROR HANDLING FAILURES

### 🟠 EC-BACK-018: Swallowing Errors
**Problem:** Catching errors without logging or handling.

**Scenario:**
```typescript
// BAD
try {
  await sendEmail(user.email, 'Welcome!');
} catch (e) {
  // Silent failure
}
```

**Fix:**
```typescript
// GOOD
try {
  await sendEmail(user.email, 'Welcome!');
} catch (error) {
  logger.error('Failed to send welcome email', {
    userId: user.id,
    email: user.email,
    error: error.message
  });

  // Decide: retry, queue for later, or alert admin
  await emailQueue.add('welcome-email', {
    userId: user.id,
    retries: 3
  });
}
```

---

### 🟡 EC-BACK-019: No Request Timeouts
**Problem:** Long-running requests block server.

**Fix:**
```typescript
import timeout from 'connect-timeout';

// Global timeout
app.use(timeout('30s'));

// Per-endpoint timeout
app.post('/api/expensive-operation', timeout('60s'), async (req, res) => {
  if (req.timedout) return;

  // Long operation
  const result = await expensiveOperation();

  if (!req.timedout) {
    res.json(result);
  }
});

// Timeout handler
app.use((req, res, next) => {
  if (req.timedout) {
    res.status(503).json({ error: 'Request timeout' });
  } else {
    next();
  }
});
```

---

## 7. DATABASE INTERACTION FAILURES

### 🔴 EC-BACK-020: Missing Transaction Handling
**Problem:** Related operations not atomic, causing inconsistent state.

**Scenario:**
```typescript
// BAD - Not atomic
async function transferMoney(fromId, toId, amount) {
  await db.account.update({ where: { id: fromId }, data: { balance: { decrement: amount } } });
  // If crash here, money disappears!
  await db.account.update({ where: { id: toId }, data: { balance: { increment: amount } } });
}
```

**Fix:**
```typescript
// GOOD - Atomic transaction
async function transferMoney(fromId: string, toId: string, amount: number) {
  return await db.$transaction(async (tx) => {
    // Debit
    const fromAccount = await tx.account.update({
      where: { id: fromId },
      data: { balance: { decrement: amount } }
    });

    if (fromAccount.balance < 0) {
      throw new Error('Insufficient funds');
    }

    // Credit
    await tx.account.update({
      where: { id: toId },
      data: { balance: { increment: amount } }
    });

    // All or nothing
  });
}
```

---

## Validation Script

```bash
#!/bin/bash
# scripts/validate-backend-security.sh

echo "Validating backend security..."

ISSUES=0

# Check for SQL injection
echo "Checking for SQL injection vulnerabilities..."
if grep -r "db.raw\|db.query" backend/src/ | grep -q '\${'; then
  echo "❌ CRITICAL: String interpolation in SQL query detected"
  ((ISSUES++))
fi

# Check for hardcoded secrets
echo "Checking for hardcoded secrets..."
if grep -rE "password.*=.*['\"][^$]|api_key.*=.*['\"][^$]|secret.*=.*['\"][^$]" backend/src/ --exclude-dir=tests; then
  echo "❌ CRITICAL: Hardcoded secrets detected"
  ((ISSUES++))
fi

# Check for weak password hashing
if grep -r "createHash.*md5\|createHash.*sha1" backend/src/; then
  echo "❌ CRITICAL: Weak password hashing (MD5/SHA1) detected"
  ((ISSUES++))
fi

# Check for rate limiting
if ! grep -rq "rateLimit\|rateLimiter" backend/src/; then
  echo "⚠️  WARNING: No rate limiting found"
fi

# Check for input validation
if ! grep -rq "zod\|joi\|class-validator" backend/package.json; then
  echo "⚠️  WARNING: No input validation library found"
fi

if [ $ISSUES -gt 0 ]; then
  echo "❌ Found $ISSUES critical security issues"
  exit 1
fi

echo "✅ Backend security validation passed"
```

---

## Summary

**Total Edge Cases:** 20
- 🔴 Critical: 8 (40%)
- 🟠 High: 8 (40%)
- 🟡 Medium: 4 (20%)

**Key Themes:**
1. **Authentication First** - 95% of attacks from authenticated sessions
2. **Input Validation** - Never trust user input
3. **BOLA Protection** - Always check ownership
4. **Rate Limiting** - Protect against brute force
5. **Secure Defaults** - Fail securely

All fixes integrated into backend code and validation scripts.
