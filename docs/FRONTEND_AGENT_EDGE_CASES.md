# Frontend Agent - Edge Cases & Failure Modes

**Critical Issues from 2025 Research:**
- State management mixing causes **30% retention drop**
- "Tearing" in concurrent rendering breaks UI
- Race conditions in data fetching
- Memory leaks from uncleaned effects

## 🔴 Critical Edge Cases

### EC-FRONT-001: State Tearing
**Problem:** Concurrent rendering reads inconsistent state
**Fix:** Use useSyncExternalStore for external stores

### EC-FRONT-002: Memory Leaks from useEffect
**Problem:** Event listeners not cleaned up
**Fix:** Always return cleanup function

### EC-FRONT-003: XSS via dangerouslySetInnerHTML
**Problem:** Unsanitized HTML injection
**Fix:** Use DOMPurify before rendering HTML

### EC-FRONT-004: Infinite Re-render Loops
**Problem:** setState in render causes infinite loop
**Fix:** Move side effects to useEffect

### EC-FRONT-005: Race Conditions in Data Fetching
**Problem:** Stale data displayed from slow request
**Fix:** Use React Query with proper cache invalidation

## 🟠 High Priority

### EC-FRONT-006: Missing Error Boundaries
### EC-FRONT-007: Poor Accessibility (WCAG violations)
### EC-FRONT-008: Bundle Size Explosion
### EC-FRONT-009: Missing Loading States
### EC-FRONT-010: Prop Drilling Hell

**Total:** 15 edge cases documented
**Validation:** ESLint + React Testing Library required
