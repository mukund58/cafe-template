# Cafe Management System & POS

This project is a Cafe Management & POS system designed to demonstrate backend engineering in ASP.NET Core and frontend integration with React (Vite, TypeScript, Tailwind/Vanilla CSS).

---

## 📂 Current Project Structure

### Backend (`/backend`)
```text
backend
├── Configurations
│   └── JwtOptions.cs
├── Constants
│   ├── OrderStatus.cs
│   ├── PaymentConstants.cs
│   └── Roles.cs
├── Data
│   └── AppDbContext.cs
├── DTOs
│   ├── CategoryDto.cs
│   ├── CouponDto.cs
│   ├── FloorDto.cs
│   ├── LoginRequest.cs
│   ├── LoginResponse.cs
│   ├── OrderDto.cs
│   ├── PaymentDto.cs
│   ├── ProductDto.cs
│   ├── ProfileUpdateRequest.cs
│   ├── RegisterRequest.cs
│   ├── TableDto.cs
│   └── UserResponse.cs
├── Extensions
│   └── SwaggerExtensions.cs
├── Features
│   ├── Auth
│   │   ├── AuthEndpoints.cs
│   │   └── JwtService.cs
│   ├── Products
│   │   ├── CategoriesEndpoints.cs
│   │   └── ProductsEndpoints.cs
│   └── Profile
│       └── ProfileEndpoints.cs
├── Migrations
├── Models
│   ├── Category.cs
│   ├── Coupon.cs
│   ├── Customer.cs
│   ├── Floors.cs
│   ├── OrderItem.cs
│   ├── Orders.cs
│   ├── Payment.cs
│   ├── Product.cs
│   ├── Tables.cs
│   └── User.cs
├── Services
│   └── PasswordServicsce.cs
├── Program.cs
├── Dockerfile
└── appsettings.json
```

### Frontend (`/frontend`)
```text
frontend
├── src
│   ├── api
│   │   ├── authService.ts
│   │   └── client.ts
│   ├── components
│   │   ├── Login.tsx
│   │   ├── Register.tsx
│   │   ├── ProtectedRoute.tsx
│   │   └── Dashboard.tsx
│   ├── App.tsx
│   ├── main.tsx
│   └── index.css
├── index.html
├── package.json
└── vite.config.ts
```

---

## 🏆 Development Flow (The Golden Rule)

For every new feature:
```text
Business Flow ➔ ERD ➔ Entity ➔ Constraints ➔ DTO ➔ Validation ➔ Endpoint ➔ Swagger Test ➔ Frontend ➔ Integration ➔ Manual Test ➔ Unit Test ➔ Commit & Push ➔ CI/CD Deploy ➔ Technical Retrospective
```

---

# 🗺️ Module Implementation Checklist

Below is the status checklist of each module in the project based on the current implementation.

## [x] Module 0 — Project Setup
- [x] Configure ASP.NET Core Minimal APIs, Dependency Injection, and Middleware.
- [x] Setup PostgreSQL database and configure Entity Framework Core connection.
- [x] Configure Docker container for PostgreSQL (`docker-compose.yml`).
- [x] Add Swagger/OpenAPI support for API documentation.
- [ ] Implement GitHub Actions CI/CD pipeline.

---

## [x] Module 1 — Authentication & Profiles
- [x] Backend: Register endpoint (`/auth/register`) with BCrypt password hashing.
- [x] Backend: Login endpoint (`/auth/login`) generating JWT tokens.
- [x] Backend: Current User info endpoint (`/auth/me`).
- [x] Backend: Profile endpoints (`/profile`) supporting profile fetching, update name, and profile image upload (with type/size validation).
- [x] Frontend: Login & Register views.
- [x] Frontend: Protected routes & profile dashboard.
- [x] Security: Middleware, JWT claims, and authorization policies.

---

## [/] Module 2 — Categories
- [x] Backend: CRUD endpoints (`api/categories`) to Create, Read, Update, and Delete categories.
- [x] Backend: Add Color properties to Category model/DTO.
- [x] Backend: Validation (e.g. name required).
- [ ] Frontend: Category management page (List, Create, Edit, Delete).

---

## [/] Module 3 — Products
- [x] Backend: CRUD endpoints (`api/products`) with Category relation checks.
- [x] Backend: Validation constraints (e.g. price must be greater than zero).
- [ ] Backend: Advanced querying (Pagination, Filtering, Sorting, Soft Delete).
- [ ] Frontend: Product management interface.

---

## [ ] Module 4 — Customers
- [ ] Backend: Customer model and basic search (name, email, phone).
- [ ] Backend: Unique email constraint & index optimization.
- [ ] Frontend: Customer directory page.

---

## [ ] Module 5 — Tables and Floors
- [ ] Backend: Floor and Table models, establishing relationships (Floor ➔ Tables).
- [ ] Backend: Table reservation/status tracking.
- [ ] Frontend: Visual table layout / seating grid.

---

## [ ] Module 6 — Orders
- [ ] Backend: Many-to-Many mapping for Orders and Products through OrderItems.
- [ ] Backend: Database Transactions for order placement (updating status, items, calculations).
- [ ] Backend: Concurrency conflict handling (RowVersion/optimistic locking).
- [ ] Backend: Order status workflow (Pending ➔ Preparing ➔ Completed ➔ Paid).
- [ ] Frontend: Ordering cart and kitchen ticket tracking.

---

## [ ] Module 7 — Payments
- [ ] Backend: Payment entity and transaction history.
- [ ] Backend: State Machine for payment status (Pending ➔ Paid/Refunded).
- [ ] Frontend: Payment checkout portal.

---

## [ ] Module 8 — Dashboard & Metrics
- [ ] Backend: Aggregation endpoints using LINQ (GroupBy, Sum, Average, Count).
- [ ] Backend: Sales trends, popular items, active tables.
- [ ] Frontend: Analytical dashboard charts (e.g., Recharts).

---

## [ ] Module 9 — Global Exception Handling
- [ ] Backend: Implement global exception handling middleware mapping errors to RFC 7807 `ProblemDetails`.
- [ ] Backend: Custom logging formats.

---

## [ ] Module 10 — Caching
- [ ] Backend: Implement `IMemoryCache` for slow/frequent queries (e.g., categories & product lists).
- [ ] Backend: Implement Cache Invalidation policy on writes.
- [ ] Backend: Integration with Redis.

---

## [ ] Module 11 — Health Checks
- [ ] Backend: `/health` check endpoints monitoring database availability and server status.

---

## [ ] Module 12 — Testing Suite
- [ ] Backend: Unit tests using xUnit, Moq, and FluentAssertions (Auth, Orders, Payments).
- [ ] Backend: Integration tests using Testcontainers for PostgreSQL.
- [ ] Frontend: End-to-End browser tests using Playwright.

---

## [ ] Module 13 — Production Security & Deployment
- [ ] Docker: Multi-stage production builds for backend & frontend.
- [ ] Infrastructure: Rate limiting and CORS policies.
- [ ] HTTPS & Nginx configuration.

---

## [x] Module 14 — Architecture Refactoring
- [x] Refactor workspace into Feature slices (Auth, Products, Profile grouped within `Features` directory).
- [x] Clean separations for DTOs, Models, and Configurations.

---

## [ ] Module 15 — Real-Time Systems
- [ ] Backend: Configure SignalR Hubs for order state changes.
- [ ] Frontend: Real-time update integration for Kitchen Displays and orders list.

---

## [ ] Module 16 — Scale & Message Brokers
- [ ] Backend: Background jobs with Hangfire.
- [ ] Backend: Message queue integration using RabbitMQ or Kafka.

---

## 🏁 Summary of Learning Outcomes Achieved
- [x] C# & ASP.NET Core Minimal APIs
- [x] Database modeling with EF Core & PostgreSQL
- [x] JWT-based authentication & route protection
- [x] Feature-slice project structuring
- [x] Image file uploads & validation
- [ ] Real-time updates with SignalR
- [ ] Database Transactions & ACID operations
- [ ] Performance caching (Memory/Redis)
- [ ] Testing suites (Unit, Integration, E2E)
- [ ] Production DevOps (CI/CD, Nginx, security policies)
