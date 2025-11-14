# Create UI Component

Generate a React component with TypeScript, styling, tests, and documentation.

## Component Specification

Component name: $COMPONENTNAME
Type: $TYPE (presentational/container/compound)

## Instructions

1. **Plan Component Structure**
   - Component responsibilities
   - Props interface
   - State requirements
   - Child components (if any)
   - Styling approach

2. **Create Component File**
   ```typescript
   // src/components/$COMPONENTNAME.tsx
   import { type ReactNode } from 'react';

   interface $COMPONENTNAMEProps {
     // Define props
   }

   export function $COMPONENTNAME({ ...props }: $COMPONENTNAMEProps) {
     // Implementation
   }
   ```

3. **Add TypeScript Types**
   - Strict type definitions
   - No `any` types
   - Proper null handling
   - Generic types if needed

4. **Implement Styling**
   ```typescript
   // Using Tailwind CSS
   <div className="p-4 rounded-lg shadow-md">
     {/* Component content */}
   </div>
   ```

5. **Add Accessibility**
   - Semantic HTML
   - ARIA labels
   - Keyboard navigation
   - Focus management

6. **Create Tests**
   ```typescript
   // src/components/$COMPONENTNAME.test.tsx
   import { render, screen } from '@testing-library/react';
   import userEvent from '@testing-library/user-event';
   import { $COMPONENTNAME } from './$COMPONENTNAME';

   describe('$COMPONENTNAME', () => {
     it('should render correctly', () => {
       render(<$COMPONENTNAME />);
       expect(screen.getByRole('...')).toBeInTheDocument();
     });

     it('should handle user interactions', async () => {
       const onClick = vi.fn();
       render(<$COMPONENTNAME onClick={onClick} />);
       await userEvent.click(screen.getByRole('button'));
       expect(onClick).toHaveBeenCalled();
     });
   });
   ```

7. **Create Storybook Story** (if applicable)
   ```typescript
   // src/components/$COMPONENTNAME.stories.tsx
   import type { Meta, StoryObj } from '@storybook/react';
   import { $COMPONENTNAME } from './$COMPONENTNAME';

   const meta: Meta<typeof $COMPONENTNAME> = {
     component: $COMPONENTNAME,
   };

   export default meta;
   type Story = StoryObj<typeof $COMPONENTNAME>;

   export const Default: Story = {
     args: {
       // Default props
     },
   };
   ```

8. **Export Component**
   ```typescript
   // src/components/index.ts
   export { $COMPONENTNAME } from './$COMPONENTNAME';
   ```

9. **Document Usage**
   ```typescript
   /**
    * $COMPONENTNAME component
    *
    * @example
    * ```tsx
    * <$COMPONENTNAME prop1="value" prop2={value} />
    * ```
    */
   ```

## Component Patterns

### Presentational Component
- Pure function
- No business logic
- Receives data via props
- Focuses on UI rendering

### Container Component
- Handles data fetching
- Manages state
- Passes data to presentational components

### Compound Component
- Parent component with related children
- Shares implicit state
- Flexible composition

## Success Criteria

- [ ] Component created
- [ ] TypeScript types defined
- [ ] Styling implemented
- [ ] Accessibility ensured
- [ ] Tests written and passing
- [ ] Storybook story created (if applicable)
- [ ] Documentation added
- [ ] Exported from index

Use the frontend-agent for implementation.
