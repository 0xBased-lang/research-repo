# Integration Guide: Popular Services & Libraries

This guide covers integration patterns for popular databases, services, and libraries commonly used in modern applications.

---

## Database Integrations

### Supabase

**What it is:** Open-source Firebase alternative with PostgreSQL database, authentication, storage, and real-time subscriptions.

**When to use:**
- Need built-in authentication
- Want real-time data sync
- Need file storage
- Prefer managed PostgreSQL
- Building MVP quickly

#### Setup

```bash
npm install @supabase/supabase-js
```

#### Configuration

```typescript
// lib/supabase.ts
import { createClient } from '@supabase/supabase-js';

const supabaseUrl = process.env.SUPABASE_URL!;
const supabaseAnonKey = process.env.SUPABASE_ANON_KEY!;

export const supabase = createClient(supabaseUrl, supabaseAnonKey);

// For server-side with service role key
export const supabaseAdmin = createClient(
  supabaseUrl,
  process.env.SUPABASE_SERVICE_ROLE_KEY!
);
```

#### Environment Variables

```bash
# .env.example
SUPABASE_URL=https://xxxxx.supabase.co
SUPABASE_ANON_KEY=eyJxxx...
SUPABASE_SERVICE_ROLE_KEY=eyJxxx...
```

#### Usage Examples

**Authentication:**
```typescript
// Sign up
const { data, error } = await supabase.auth.signUp({
  email: 'user@example.com',
  password: 'password123',
});

// Sign in
const { data, error } = await supabase.auth.signInWithPassword({
  email: 'user@example.com',
  password: 'password123',
});

// Get current user
const { data: { user } } = await supabase.auth.getUser();
```

**Database Queries:**
```typescript
// Select
const { data, error } = await supabase
  .from('users')
  .select('*')
  .eq('status', 'active');

// Insert
const { data, error } = await supabase
  .from('users')
  .insert({ name: 'John', email: 'john@example.com' });

// Update
const { data, error } = await supabase
  .from('users')
  .update({ status: 'inactive' })
  .eq('id', userId);

// Delete
const { data, error } = await supabase
  .from('users')
  .delete()
  .eq('id', userId);
```

**Real-time Subscriptions:**
```typescript
const channel = supabase
  .channel('users-channel')
  .on('postgres_changes',
    { event: 'INSERT', schema: 'public', table: 'users' },
    (payload) => {
      console.log('New user:', payload.new);
    }
  )
  .subscribe();
```

**File Storage:**
```typescript
// Upload file
const { data, error } = await supabase.storage
  .from('avatars')
  .upload(`${userId}/avatar.png`, file);

// Get public URL
const { data } = supabase.storage
  .from('avatars')
  .getPublicUrl(`${userId}/avatar.png`);
```

---

### Prisma ORM

**What it is:** Next-generation ORM for Node.js & TypeScript with type-safety and migrations.

**When to use:**
- Want type-safe database queries
- Need auto-generated types
- Want migration management
- Building with TypeScript

#### Setup

```bash
npm install prisma @prisma/client
npx prisma init
```

#### Configuration

```prisma
// prisma/schema.prisma
generator client {
  provider = "prisma-client-js"
}

datasource db {
  provider = "postgresql"
  url      = env("DATABASE_URL")
}

model User {
  id        String   @id @default(uuid())
  email     String   @unique
  name      String
  posts     Post[]
  createdAt DateTime @default(now()) @map("created_at")
  updatedAt DateTime @updatedAt @map("updated_at")

  @@map("users")
}

model Post {
  id        String   @id @default(uuid())
  title     String
  content   String?
  published Boolean  @default(false)
  authorId  String   @map("author_id")
  author    User     @relation(fields: [authorId], references: [id], onDelete: Cascade)
  createdAt DateTime @default(now()) @map("created_at")
  updatedAt DateTime @updatedAt @map("updated_at")

  @@index([authorId])
  @@map("posts")
}
```

#### Client Setup

```typescript
// lib/prisma.ts
import { PrismaClient } from '@prisma/client';

const globalForPrisma = globalThis as unknown as {
  prisma: PrismaClient | undefined;
};

export const prisma =
  globalForPrisma.prisma ??
  new PrismaClient({
    log: process.env.NODE_ENV === 'development' ? ['query', 'error', 'warn'] : ['error'],
  });

if (process.env.NODE_ENV !== 'production') globalForPrisma.prisma = prisma;
```

#### Usage

```typescript
// Create
const user = await prisma.user.create({
  data: {
    email: 'user@example.com',
    name: 'John Doe',
  },
});

// Read
const users = await prisma.user.findMany({
  where: { email: { contains: '@example.com' } },
  include: { posts: true },
});

// Update
const updated = await prisma.user.update({
  where: { id: userId },
  data: { name: 'Jane Doe' },
});

// Delete
await prisma.user.delete({
  where: { id: userId },
});

// Transactions
await prisma.$transaction([
  prisma.user.create({ data: { email: 'user1@example.com', name: 'User 1' } }),
  prisma.user.create({ data: { email: 'user2@example.com', name: 'User 2' } }),
]);
```

#### Migrations

```bash
# Create migration
npx prisma migrate dev --name add_users_table

# Apply migrations in production
npx prisma migrate deploy

# Generate Prisma Client
npx prisma generate

# Open Prisma Studio (GUI)
npx prisma studio
```

---

### Redis

**What it is:** In-memory data structure store used as cache, message broker, and session store.

**When to use:**
- Need fast caching
- Want to implement rate limiting
- Building real-time features
- Need session storage

#### Setup

```bash
npm install ioredis
```

#### Configuration

```typescript
// lib/redis.ts
import Redis from 'ioredis';

export const redis = new Redis({
  host: process.env.REDIS_HOST || 'localhost',
  port: parseInt(process.env.REDIS_PORT || '6379'),
  password: process.env.REDIS_PASSWORD,
  db: parseInt(process.env.REDIS_DB || '0'),
  retryStrategy: (times) => {
    const delay = Math.min(times * 50, 2000);
    return delay;
  },
});

redis.on('error', (err) => {
  console.error('Redis connection error:', err);
});

redis.on('connect', () => {
  console.log('Redis connected');
});
```

#### Usage Examples

**Caching:**
```typescript
// Set cache with TTL
await redis.setex('user:123', 3600, JSON.stringify(user));

// Get from cache
const cached = await redis.get('user:123');
const user = cached ? JSON.parse(cached) : null;

// Delete from cache
await redis.del('user:123');

// Cache pattern
async function getCachedUser(userId: string) {
  const cacheKey = `user:${userId}`;

  // Try cache first
  const cached = await redis.get(cacheKey);
  if (cached) return JSON.parse(cached);

  // Fetch from database
  const user = await db.user.findUnique({ where: { id: userId } });

  // Store in cache
  await redis.setex(cacheKey, 3600, JSON.stringify(user));

  return user;
}
```

**Rate Limiting:**
```typescript
async function rateLimit(identifier: string, limit: number, windowSeconds: number): Promise<boolean> {
  const key = `rate_limit:${identifier}`;

  const current = await redis.incr(key);

  if (current === 1) {
    await redis.expire(key, windowSeconds);
  }

  return current <= limit;
}

// Usage in middleware
app.use(async (req, res, next) => {
  const allowed = await rateLimit(req.ip, 100, 60); // 100 requests per minute

  if (!allowed) {
    return res.status(429).json({ error: 'Too many requests' });
  }

  next();
});
```

**Pub/Sub:**
```typescript
// Publisher
await redis.publish('notifications', JSON.stringify({
  userId: '123',
  message: 'New notification',
}));

// Subscriber
const subscriber = new Redis();
subscriber.subscribe('notifications');

subscriber.on('message', (channel, message) => {
  const data = JSON.parse(message);
  console.log('Received:', data);
});
```

---

## Payment Processing

### Stripe

**What it is:** Payment processing platform for online and in-person payments.

**When to use:**
- Need to accept payments
- Want subscription billing
- Need payment method storage
- Require PCI compliance

#### Setup

```bash
npm install stripe
```

#### Configuration

```typescript
// lib/stripe.ts
import Stripe from 'stripe';

export const stripe = new Stripe(process.env.STRIPE_SECRET_KEY!, {
  apiVersion: '2023-10-16',
  typescript: true,
});
```

#### Environment Variables

```bash
STRIPE_SECRET_KEY=sk_test_xxx
STRIPE_PUBLISHABLE_KEY=pk_test_xxx
STRIPE_WEBHOOK_SECRET=whsec_xxx
```

#### Usage Examples

**Create Payment Intent:**
```typescript
const paymentIntent = await stripe.paymentIntents.create({
  amount: 2000, // $20.00
  currency: 'usd',
  customer: customerId,
  metadata: {
    orderId: '12345',
  },
});
```

**Create Subscription:**
```typescript
const subscription = await stripe.subscriptions.create({
  customer: customerId,
  items: [{ price: 'price_xxxxx' }],
  payment_behavior: 'default_incomplete',
  payment_settings: { save_default_payment_method: 'on_subscription' },
  expand: ['latest_invoice.payment_intent'],
});
```

**Webhook Handling:**
```typescript
app.post('/webhook', express.raw({ type: 'application/json' }), async (req, res) => {
  const sig = req.headers['stripe-signature']!;

  try {
    const event = stripe.webhooks.constructEvent(
      req.body,
      sig,
      process.env.STRIPE_WEBHOOK_SECRET!
    );

    switch (event.type) {
      case 'payment_intent.succeeded':
        const paymentIntent = event.data.object;
        await handlePaymentSuccess(paymentIntent);
        break;

      case 'customer.subscription.updated':
        const subscription = event.data.object;
        await updateSubscriptionStatus(subscription);
        break;
    }

    res.json({ received: true });
  } catch (err) {
    res.status(400).send(`Webhook Error: ${err.message}`);
  }
});
```

---

## MCP Server Integrations

### Context7 (Documentation MCP)

**What it is:** MCP server that provides access to documentation from various frameworks and libraries.

**Setup:**

```json
// .mcp.json
{
  "mcpServers": {
    "context7": {
      "command": "npx",
      "args": ["-y", "@context7/mcp-server"]
    }
  }
}
```

**Usage:**
```
Claude can now access documentation for:
- React, Next.js, Vue
- Node.js, Express, Fastify
- TypeScript, JavaScript
- And 100+ more libraries
```

### Tavily (Web Search MCP)

**What it is:** MCP server for web search capabilities.

**Setup:**

```json
{
  "mcpServers": {
    "tavily": {
      "command": "npx",
      "args": ["-y", "@tavily/mcp-server"],
      "env": {
        "TAVILY_API_KEY": "${TAVILY_API_KEY}"
      }
    }
  }
}
```

### Playwright (Browser Automation MCP)

**What it is:** MCP server for browser automation and testing.

**Setup:**

```json
{
  "mcpServers": {
    "playwright": {
      "command": "npx",
      "args": ["-y", "@playwright/mcp-server"]
    }
  }
}
```

**Usage:**
- Take screenshots of web pages
- Test UI components
- Scrape web content
- Automate browser tasks

---

## Email Services

### SendGrid

**Setup:**

```bash
npm install @sendgrid/mail
```

**Configuration:**

```typescript
// lib/email.ts
import sgMail from '@sendgrid/mail';

sgMail.setApiKey(process.env.SENDGRID_API_KEY!);

export async function sendEmail(to: string, subject: string, html: string) {
  const msg = {
    to,
    from: process.env.SENDGRID_FROM_EMAIL!,
    subject,
    html,
  };

  await sgMail.send(msg);
}
```

### Resend (Modern Alternative)

```bash
npm install resend
```

```typescript
import { Resend } from 'resend';

const resend = new Resend(process.env.RESEND_API_KEY);

await resend.emails.send({
  from: 'onboarding@yourdomain.com',
  to: 'user@example.com',
  subject: 'Welcome!',
  html: '<p>Welcome to our platform!</p>',
});
```

---

## Environment Variables Template

Create a comprehensive `.env.example`:

```bash
# Application
NODE_ENV=development
PORT=3000
API_URL=http://localhost:3000

# Database
DATABASE_URL=postgresql://user:password@localhost:5432/dbname
DATABASE_POOL_MIN=2
DATABASE_POOL_MAX=10

# Supabase
SUPABASE_URL=https://xxxxx.supabase.co
SUPABASE_ANON_KEY=eyJxxx...
SUPABASE_SERVICE_ROLE_KEY=eyJxxx...

# Redis
REDIS_HOST=localhost
REDIS_PORT=6379
REDIS_PASSWORD=
REDIS_DB=0

# Authentication
JWT_SECRET=your-secret-key-min-32-chars
JWT_EXPIRES_IN=15m
JWT_REFRESH_SECRET=your-refresh-secret-key
JWT_REFRESH_EXPIRES_IN=7d

# Stripe
STRIPE_SECRET_KEY=sk_test_xxx
STRIPE_PUBLISHABLE_KEY=pk_test_xxx
STRIPE_WEBHOOK_SECRET=whsec_xxx

# Email
SENDGRID_API_KEY=SG.xxx
SENDGRID_FROM_EMAIL=noreply@yourdomain.com
# OR
RESEND_API_KEY=re_xxx

# AWS (if using S3, etc.)
AWS_ACCESS_KEY_ID=xxx
AWS_SECRET_ACCESS_KEY=xxx
AWS_REGION=us-east-1
AWS_S3_BUCKET=your-bucket-name

# Monitoring
SENTRY_DSN=https://xxx@xxx.ingest.sentry.io/xxx
LOG_LEVEL=info

# MCP Services
GITHUB_TOKEN=ghp_xxx
TAVILY_API_KEY=tvly_xxx

# OAuth Providers
GOOGLE_CLIENT_ID=xxx.apps.googleusercontent.com
GOOGLE_CLIENT_SECRET=xxx
GITHUB_CLIENT_ID=xxx
GITHUB_CLIENT_SECRET=xxx
```

---

## Agent Knowledge

The agents in this framework are aware of these integrations:

- **Backend Agent**: Knows Prisma, Supabase, Redis, Stripe, SendGrid
- **Frontend Agent**: Knows Supabase client, React Query patterns
- **Database Agent**: Knows Prisma migrations, Supabase schemas
- **DevOps Agent**: Knows Docker configs for all services
- **Security Agent**: Knows security best practices for each service

Just ask the appropriate agent to implement integration with any of these services!

---

## Quick Reference

### Starting a New Project with Integrations

```bash
# 1. Create project
/new-project backend my-api

# 2. Install dependencies
npm install @prisma/client ioredis @supabase/supabase-js stripe

# 3. Set up Prisma
npx prisma init

# 4. Configure environment variables (copy from above)

# 5. Ask agents to set up integrations
"Backend agent: Set up Prisma with PostgreSQL and create User model"
"Backend agent: Configure Redis for caching"
"Backend agent: Integrate Stripe for payment processing"
```

### Common Integration Patterns

**Pattern 1: Database + Cache**
```
PostgreSQL (Prisma) → Redis (Cache) → API Response
```

**Pattern 2: Auth + Database**
```
Supabase Auth → Supabase Database → API
```

**Pattern 3: Full Stack**
```
React → Next.js → Prisma → PostgreSQL
              ↓
           Redis Cache
```

---

All agents understand these integrations and can help implement them! Just ask.
