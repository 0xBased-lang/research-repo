---
name: database-patterns
description: Provides database design patterns and best practices when discussing databases, schemas, or queries
---

# Database Patterns Skill

## Purpose

Auto-invoked when conversation involves databases, schemas, migrations, or SQL queries.

## Context Provided

When discussing databases, remember:

### 1. Naming Conventions

**Tables:**
- Use plural snake_case: `users`, `user_profiles`, `order_items`

**Columns:**
- Use snake_case: `created_at`, `updated_at`, `first_name`
- Boolean columns: `is_active`, `has_verified_email`

**Primary Keys:**
- Usually `id` (UUID or BigInt)

**Foreign Keys:**
- Format: `{table}_id` (e.g., `user_id`, `order_id`)

**Indexes:**
- Format: `idx_{table}_{column}` (e.g., `idx_users_email`)

**Constraints:**
- Format: `{table}_{column}_{type}` (e.g., `users_email_unique`)

### 2. Common Patterns

**Timestamps (Always Include):**
```sql
created_at TIMESTAMP NOT NULL DEFAULT NOW()
updated_at TIMESTAMP NOT NULL DEFAULT NOW()
```

**Soft Deletes:**
```sql
deleted_at TIMESTAMP NULL

-- Query active records
SELECT * FROM users WHERE deleted_at IS NULL;

-- Index for performance
CREATE INDEX idx_users_deleted_at ON users(deleted_at)
WHERE deleted_at IS NULL;
```

**Enums:**
```sql
-- PostgreSQL enum
CREATE TYPE user_role AS ENUM ('user', 'admin', 'moderator');

-- Or check constraint
ALTER TABLE users ADD COLUMN status VARCHAR(20)
CHECK (status IN ('active', 'inactive', 'suspended'));
```

### 3. Indexing Strategy

**Always index:**
- Primary keys (automatic)
- Foreign keys
- Columns in WHERE clauses
- Columns in ORDER BY
- Columns in JOIN conditions

**Composite indexes (order matters):**
```sql
-- Good for: WHERE user_id = X ORDER BY created_at DESC
CREATE INDEX idx_posts_user_created
ON posts(user_id, created_at DESC);
```

**Partial indexes:**
```sql
-- Index only published posts
CREATE INDEX idx_posts_published
ON posts(created_at)
WHERE published = true;
```

### 4. Relationship Patterns

**One-to-Many:**
```sql
CREATE TABLE users (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid()
);

CREATE TABLE posts (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  user_id UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE
);
```

**Many-to-Many:**
```sql
-- Junction table
CREATE TABLE user_roles (
  user_id UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
  role_id UUID NOT NULL REFERENCES roles(id) ON DELETE CASCADE,
  granted_at TIMESTAMP NOT NULL DEFAULT NOW(),
  PRIMARY KEY (user_id, role_id)
);

CREATE INDEX idx_user_roles_user ON user_roles(user_id);
CREATE INDEX idx_user_roles_role ON user_roles(role_id);
```

### 5. Safe Migration Patterns

**Adding NOT NULL column (3 steps):**
```sql
-- Step 1: Add as nullable
ALTER TABLE users ADD COLUMN phone VARCHAR(20);

-- Step 2: Backfill data
UPDATE users SET phone = '000-000-0000' WHERE phone IS NULL;

-- Step 3: Make NOT NULL (separate migration)
ALTER TABLE users ALTER COLUMN phone SET NOT NULL;
```

**Renaming column (zero downtime):**
```sql
-- Step 1: Add new column
ALTER TABLE users ADD COLUMN full_name VARCHAR(255);

-- Step 2: Dual write in app + backfill
UPDATE users SET full_name = name;

-- Step 3: Switch reads to new column in app

-- Step 4: Drop old column (after deployment)
ALTER TABLE users DROP COLUMN name;
```

### 6. Query Optimization

**Avoid N+1 queries:**
```typescript
// Bad
const users = await db.user.findMany();
for (const user of users) {
  user.posts = await db.post.findMany({ where: { userId: user.id } });
}

// Good
const users = await db.user.findMany({
  include: { posts: true }
});
```

**Use EXPLAIN ANALYZE:**
```sql
EXPLAIN ANALYZE
SELECT * FROM users WHERE email = 'test@example.com';
```

### 7. Data Types

**PostgreSQL recommendations:**
- Text: `VARCHAR(n)` or `TEXT`
- Numbers: `INTEGER`, `BIGINT`, `NUMERIC`
- Dates: `TIMESTAMP`, `DATE`
- UUID: `UUID` (use `gen_random_uuid()`)
- JSON: `JSONB` (not `JSON`)
- Arrays: `TEXT[]`, `INTEGER[]`
- Boolean: `BOOLEAN`

### 8. Transactions

```typescript
await db.$transaction(async (tx) => {
  const user = await tx.user.create({ data: userData });
  await tx.profile.create({ data: { userId: user.id, ...profileData } });
  await tx.audit.create({ data: { action: 'user_created', userId: user.id } });
});
```

## When This Skill Activates

Automatically provides this context when you mention:
- "database" or "DB"
- "schema" or "table"
- "migration" or "migrate"
- "SQL" or "query"
- "PostgreSQL" or "Postgres"
- "Prisma" or "TypeORM"
- "index" or "foreign key"
