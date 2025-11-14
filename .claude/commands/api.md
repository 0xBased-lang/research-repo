# Generate API Endpoint

Create a complete API endpoint with routes, controllers, services, validation, and tests.

## Endpoint Specification

Resource: $RESOURCE
Method: $METHOD (GET/POST/PUT/PATCH/DELETE)

## Instructions

1. **Design API**
   - HTTP method and route
   - Request schema (path params, query params, body)
   - Response schema
   - Error responses
   - Authentication/authorization requirements

2. **Create Route**
   ```typescript
   // src/routes/$RESOURCE.routes.ts
   router.$METHOD('/$RESOURCE',
     authenticate,
     validate($RESOURCESchema),
     $RESOURCEController.$METHOD
   );
   ```

3. **Create Controller**
   ```typescript
   // src/controllers/$RESOURCE.controller.ts
   export class $RESOURCEController {
     async $METHOD(req: Request, res: Response) {
       try {
         const result = await this.$RESOURCEService.$METHOD(req.body);
         res.status(200).json({ data: result });
       } catch (error) {
         next(error);
       }
     }
   }
   ```

4. **Create Service**
   ```typescript
   // src/services/$RESOURCE.service.ts
   export class $RESOURCEService {
     async $METHOD(data: $RESOURCEDto) {
       // Business logic
       return this.$RESOURCERepository.$METHOD(data);
     }
   }
   ```

5. **Create Repository**
   ```typescript
   // src/repositories/$RESOURCE.repository.ts
   export class $RESOURCERepository {
     async $METHOD(data: Partial<$RESOURCE>) {
       return this.db.$RESOURCE.$METHOD({ data });
     }
   }
   ```

6. **Create Validation Schema**
   ```typescript
   // src/schemas/$RESOURCE.schema.ts
   import { z } from 'zod';

   export const $RESOURCESchema = z.object({
     // Define schema
   });
   ```

7. **Write Tests**
   ```typescript
   // tests/integration/$RESOURCE.test.ts
   describe('$METHOD /$RESOURCE', () => {
     it('should $METHOD $RESOURCE successfully', async () => {
       // Test implementation
     });

     it('should return 400 for invalid data', async () => {
       // Test validation
     });

     it('should return 401 for unauthenticated requests', async () => {
       // Test auth
     });
   });
   ```

8. **Update API Documentation**
   - Add to API.md or OpenAPI spec
   - Document request/response examples
   - Note authorization requirements

9. **Run Tests**
   - Execute test suite
   - Verify all tests pass
   - Check coverage

## Success Criteria

- [ ] Route created and registered
- [ ] Controller implemented
- [ ] Service with business logic
- [ ] Repository for data access
- [ ] Input validation
- [ ] Error handling
- [ ] Tests written and passing
- [ ] Documentation updated

Implement using test-driven development: write tests first, then implementation.
