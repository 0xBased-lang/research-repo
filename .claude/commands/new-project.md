# Create New Project

Create a new project in the appropriate directory (backend, frontend, or database) with proper structure and boilerplate.

## Project Details

Project type: $PROJECTTYPE (backend/frontend/database)
Project name: $PROJECTNAME

## Instructions

1. **Analyze Request**
   - Confirm project type and name
   - Determine technology stack based on type
   - Identify required dependencies

2. **Create Directory Structure**

   For **backend** projects:
   ```
   backend/$PROJECTNAME/
   ├── src/
   │   ├── routes/
   │   ├── controllers/
   │   ├── services/
   │   ├── repositories/
   │   ├── models/
   │   ├── middlewares/
   │   ├── utils/
   │   └── types/
   ├── tests/
   │   ├── unit/
   │   ├── integration/
   │   └── e2e/
   ├── package.json
   ├── tsconfig.json
   ├── .env.example
   ├── .gitignore
   ├── README.md
   └── docker-compose.yml
   ```

   For **frontend** projects:
   ```
   frontend/$PROJECTNAME/
   ├── src/
   │   ├── components/
   │   ├── features/
   │   ├── hooks/
   │   ├── lib/
   │   ├── pages/
   │   ├── api/
   │   ├── styles/
   │   └── types/
   ├── public/
   ├── tests/
   ├── package.json
   ├── tsconfig.json
   ├── tailwind.config.js
   ├── .env.example
   ├── .gitignore
   └── README.md
   ```

   For **database** projects:
   ```
   database/$PROJECTNAME/
   ├── migrations/
   ├── seeds/
   ├── schema.prisma (or schema definition)
   ├── README.md
   └── .env.example
   ```

3. **Generate Boilerplate Files**
   - package.json with appropriate dependencies
   - TypeScript configuration
   - Environment variable template
   - README with setup instructions
   - Docker configuration (if needed)
   - Basic test setup

4. **Initialize Git**
   - Create .gitignore
   - Initial commit with message: "Initial setup for $PROJECTNAME"

5. **Document Project**
   - Update main README.md to include new project
   - Add project to CLAUDE.md
   - Create project-specific documentation

6. **Provide Next Steps**
   - Installation commands
   - Development commands
   - Configuration requirements
   - Integration points

## Success Criteria

- [ ] Directory structure created
- [ ] Dependencies configured
- [ ] Environment variables templated
- [ ] Documentation complete
- [ ] Git initialized
- [ ] Ready for development

Execute this plan step by step, confirming each section before proceeding.
