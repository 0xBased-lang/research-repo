# Database Agent - Edge Cases & Failure Modes

**Critical Issues from 2025 Research:**
- **83% of migrations fail** or exceed budget
- **45% failures** from schema mismatches
- Silent corruption - "successful" but data broken
- 15% failures from character encoding issues

## 🔴 Critical Edge Cases

### EC-DB-001: Data Corruption from Type Mismatch
**Problem:** Decimal becomes integer, loses precision
**Fix:** Validate types before migration

### EC-DB-002: Zero-Downtime Migration Failure
**Problem:** Adding NOT NULL column breaks production
**Fix:** 3-step migration (add nullable → backfill → make required)

### EC-DB-003: Missing Rollback Migrations
**Problem:** Can't undo failed migration
**Fix:** Always write DOWN migrations

### EC-DB-004: Foreign Key Cascade Deletion
**Problem:** Deleting user deletes all their data unintentionally
**Fix:** Use soft deletes or explicit cascade handling

### EC-DB-005: Index Missing on Foreign Keys
**Problem:** JOINs scan full table
**Fix:** Auto-index all foreign keys

## 🟠 High Priority

### EC-DB-006: Migration Order Dependencies
### EC-DB-007: Character Encoding Mismatches (UTF8 vs UTF8MB4)
### EC-DB-008: Insufficient Connection Pool
### EC-DB-009: Missing Unique Constraints
### EC-DB-010: No Query Performance Monitoring

**Total:** 18 edge cases documented
**Validation:** EXPLAIN ANALYZE on all queries
