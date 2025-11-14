---
name: testing-patterns
description: Provides testing best practices and patterns when discussing tests, TDD, or test coverage
---

# Testing Patterns Skill

## Purpose

Auto-invoked when conversation involves testing, test-driven development, or test coverage.

## Context Provided

When discussing testing, remember:

### 1. Test Types

**Unit Tests (80% of tests)**
- Test individual functions/methods in isolation
- Mock dependencies
- Fast execution
- Example: Testing a single service method

**Integration Tests (15% of tests)**
- Test multiple components together
- Real database connections (test DB)
- Test API endpoints end-to-end
- Example: Testing API route → controller → service → database

**E2E Tests (5% of tests)**
- Test complete user workflows
- Browser automation
- Slowest but most comprehensive
- Example: User sign up → login → perform action

### 2. Test Structure (AAA Pattern)

```typescript
describe('Feature', () => {
  it('should behave as expected', () => {
    // Arrange - Set up test data and dependencies
    const input = { data: 'test' };
    const mockService = { method: vi.fn() };

    // Act - Execute the code being tested
    const result = functionUnderTest(input);

    // Assert - Verify the outcome
    expect(result).toBe(expectedValue);
    expect(mockService.method).toHaveBeenCalledWith(input);
  });
});
```

### 3. Coverage Goals

- **Statements**: 80% minimum
- **Branches**: 80% minimum
- **Functions**: 90% minimum
- **Lines**: 80% minimum

### 4. What to Test

**✅ DO test:**
- Business logic
- Edge cases
- Error handling
- User workflows
- Integration points
- Security validation

**❌ DON'T test:**
- Third-party libraries
- Framework code
- Trivial getters/setters
- Auto-generated code

### 5. TDD Workflow

```
1. Write failing test (RED)
2. Write minimal code to pass (GREEN)
3. Refactor for quality (REFACTOR)
4. Repeat
```

### 6. Testing Best Practices

- One assertion per test (generally)
- Descriptive test names
- Independent tests (no shared state)
- Fast execution
- Deterministic (no flaky tests)
- Use test factories/fixtures
- Mock external dependencies
- Clean up after tests

### 7. Common Testing Tools

**JavaScript/TypeScript:**
- Vitest / Jest (test runner)
- React Testing Library (component tests)
- Playwright / Cypress (E2E)
- Supertest (API testing)

**Python:**
- pytest (test runner)
- unittest.mock (mocking)
- pytest-cov (coverage)

### 8. Example Test Patterns

**API Endpoint Test:**
```typescript
describe('POST /api/users', () => {
  it('should create user with valid data', async () => {
    const response = await request(app)
      .post('/api/users')
      .send({ email: 'test@example.com', password: 'password123' })
      .expect(201);

    expect(response.body.data).toHaveProperty('id');
    expect(response.body.data.email).toBe('test@example.com');
  });

  it('should return 400 for invalid email', async () => {
    await request(app)
      .post('/api/users')
      .send({ email: 'invalid', password: 'password123' })
      .expect(400);
  });
});
```

**Component Test:**
```typescript
describe('LoginForm', () => {
  it('should submit form with valid credentials', async () => {
    const onSubmit = vi.fn();
    render(<LoginForm onSubmit={onSubmit} />);

    await userEvent.type(screen.getByLabelText('Email'), 'test@example.com');
    await userEvent.type(screen.getByLabelText('Password'), 'password123');
    await userEvent.click(screen.getByRole('button', { name: 'Login' }));

    expect(onSubmit).toHaveBeenCalledWith({
      email: 'test@example.com',
      password: 'password123',
    });
  });
});
```

## When This Skill Activates

Automatically provides this context when you mention:
- "test" or "tests"
- "TDD" or "test-driven"
- "coverage"
- "jest" or "vitest" or "pytest"
- "unit test" or "integration test"
- "e2e" or "end-to-end"
