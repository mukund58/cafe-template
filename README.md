# Starter-Template
Backend
├── Features
│   ├── Auth
│   │   ├── AuthEndpoints.cs
│   │   ├── LoginRequest.cs
│   │   ├── RegisterRequest.cs
│   │   └── JwtService.cs
│   │
│   ├── Users
│   │   └── UserEndpoints.cs
│   │
│   └── Dashboard
│       └── DashboardEndpoints.cs
│
├── Data
│   ├── AppDbContext.cs
│   └── Migrations
│
├── Models
│   └── User.cs
│
├── Services
│   ├── JwtService.cs
│   └── PasswordService.cs
│
├── Extensions
│   ├── AuthenticationExtensions.cs
│   ├── SwaggerExtensions.cs
│   └── EndpointExtensions.cs
│
├── Program.cs
├── Dockerfile
├── docker-compose.yml
└── appsettings.json
