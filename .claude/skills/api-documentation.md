---
name: api-documentation
description: Provides API documentation context when discussing endpoints, REST APIs, or HTTP requests
---

# API Documentation Skill

## Purpose

Auto-invoked when conversation involves API endpoints, REST APIs, HTTP methods, or web service discussions.

## Context Provided

When discussing APIs, remember to include:

### 1. API Design Best Practices

- **RESTful conventions**:
  - GET /resources - List all
  - GET /resources/:id - Get one
  - POST /resources - Create new
  - PUT /resources/:id - Full update
  - PATCH /resources/:id - Partial update
  - DELETE /resources/:id - Delete

- **HTTP Status Codes**:
  - 200 OK - Successful GET, PUT, PATCH
  - 201 Created - Successful POST
  - 204 No Content - Successful DELETE
  - 400 Bad Request - Validation errors
  - 401 Unauthorized - Not authenticated
  - 403 Forbidden - Authenticated but not authorized
  - 404 Not Found - Resource doesn't exist
  - 429 Too Many Requests - Rate limit exceeded
  - 500 Internal Server Error - Server error

### 2. Request/Response Patterns

**Standard Request:**
```typescript
interface APIRequest {
  method: 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE';
  headers: {
    'Content-Type': 'application/json';
    'Authorization': 'Bearer <token>';
  };
  body?: Record<string, any>;
}
```

**Standard Response:**
```typescript
interface APIResponse<T> {
  data: T;
  meta?: {
    page?: number;
    total?: number;
    limit?: number;
  };
}

interface APIError {
  error: {
    code: string;
    message: string;
    details?: Array<{
      field: string;
      message: string;
    }>;
  };
}
```

### 3. Authentication Patterns

- JWT Bearer tokens
- API keys in headers
- OAuth 2.0 flows
- Session cookies

### 4. API Documentation Standards

- OpenAPI/Swagger specifications
- Request/response examples
- Error codes documentation
- Rate limiting information
- Authentication requirements

## When This Skill Activates

Automatically provides this context when you mention:
- "API endpoint"
- "REST API"
- "HTTP request"
- "POST/GET/PUT/DELETE"
- "API design"
- "Web service"
- OpenAPI/Swagger
