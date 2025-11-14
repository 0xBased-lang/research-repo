# Create Database Migration

Generate a database migration with up/down scripts and update the schema.

## Migration Details

Description: $DESCRIPTION
Type: $TYPE (create_table/alter_table/add_column/add_index/data_migration)

## Instructions

1. **Plan Migration**
   - What needs to change?
   - Impact on existing data?
   - Reversibility requirements
   - Performance considerations
   - Zero-downtime requirements

2. **Create Migration File**
   ```typescript
   // Using Prisma
   // Run: npx prisma migrate dev --name $DESCRIPTION

   // Or manual SQL migration
   // migrations/YYYYMMDDHHMMSS_$DESCRIPTION.up.sql
   // migrations/YYYYMMDDHHMMSS_$DESCRIPTION.down.sql
   ```

3. **Write Up Migration**
   ```sql
   -- Example: Add new table
   CREATE TABLE new_table (
     id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
     name VARCHAR(255) NOT NULL,
     created_at TIMESTAMP NOT NULL DEFAULT NOW(),
     updated_at TIMESTAMP NOT NULL DEFAULT NOW()
   );

   CREATE INDEX idx_new_table_name ON new_table(name);
   ```

4. **Write Down Migration**
   ```sql
   -- Rollback script
   DROP TABLE IF EXISTS new_table;
   ```

5. **Handle Data Migration (if needed)**
   ```sql
   -- Backfill data
   UPDATE users
   SET full_name = CONCAT(first_name, ' ', last_name)
   WHERE full_name IS NULL;
   ```

6. **Safe Migration Patterns**

   **Adding a Column:**
   ```sql
   -- Step 1: Add nullable
   ALTER TABLE users ADD COLUMN phone VARCHAR(20);

   -- Step 2: Backfill (separate migration)
   UPDATE users SET phone = '000-000-0000' WHERE phone IS NULL;

   -- Step 3: Make NOT NULL (separate migration)
   ALTER TABLE users ALTER COLUMN phone SET NOT NULL;
   ```

   **Adding an Index:**
   ```sql
   -- For large tables, use CONCURRENTLY (PostgreSQL)
   CREATE INDEX CONCURRENTLY idx_users_email ON users(email);
   ```

7. **Test Migration**
   - Run up migration on development database
   - Verify schema changes
   - Test down migration (rollback)
   - Verify data integrity
   - Check performance impact

8. **Update ORM Schema**
   ```prisma
   // schema.prisma
   model NewTable {
     id        String   @id @default(uuid())
     name      String
     createdAt DateTime @default(now()) @map("created_at")
     updatedAt DateTime @updatedAt @map("updated_at")

     @@index([name])
     @@map("new_table")
   }
   ```

9. **Generate Types**
   ```bash
   # Prisma
   npx prisma generate

   # TypeORM
   npm run typeorm:generate
   ```

10. **Document Migration**
    - Add to CHANGELOG.md
    - Note any manual steps required
    - Document rollback procedure
    - Update schema documentation

## Migration Safety Checklist

- [ ] Backwards compatible (if zero-downtime required)
- [ ] Down migration written and tested
- [ ] No data loss in down migration
- [ ] Indexes added CONCURRENTLY for large tables
- [ ] Default values provided for NOT NULL columns
- [ ] Large data migrations done in batches
- [ ] Performance tested on production-like data
- [ ] Rollback plan documented

## Common Migration Patterns

### Add Column (Safe)
1. Add column as nullable
2. Backfill data
3. Make NOT NULL
4. Add index

### Rename Column (Zero Downtime)
1. Add new column
2. Dual write to both columns
3. Backfill old → new
4. Switch reads to new column
5. Stop dual writes
6. Drop old column

### Split Table
1. Create new table
2. Dual write
3. Backfill data
4. Switch reads
5. Stop dual writes
6. Drop old columns

Use the database-agent for implementation.
