# E-CommerceSystem

A layered **ASP.NET Core Web API** project that provides a backend for managing **users, products, orders, and reviews**.  
It uses **Entity Framework Core** with SQL Server, **JWT authentication** for security, and follows a **clean separation of concerns** with Controllers → Services → Repositories → DbContext.

---

## Project Structure

E-CommerceSystem/
│
├── Program.cs # Startup pipeline, DI, EF Core, JWT, Swagger
├── ApplicationDbContext.cs # DbContext with DbSets + EF configs
├── appsettings.json # Connection string, JwtSettings
│
├── Controllers/ # API endpoints (entry points)
│ ├── UserController.cs
│ ├── ProductController.cs
│ ├── OrderController.cs
│ └── ReviewController.cs
│
├── Models/ # Domain entities + DTOs
│ ├── Entities/
│ │ ├── User.cs
│ │ ├── Product.cs
│ │ ├── Order.cs
│ │ ├── OrderProducts.cs # Junction (Order–Product)
│ │ └── Review.cs
│ └── DTOs/
│ ├── UserDTO.cs
│ ├── ProductDTO.cs
│ ├── OrderItemDTO.cs
│ ├── OrdersOutputOTD.cs
│ └── ReviewDTO.cs
│
├── Repositories/ # Data access (EF Core operations)
│ ├── Interfaces/
│ │ ├── IUserRepo.cs
│ │ ├── IProductRepo.cs
│ │ ├── IOrderRepo.cs
│ │ ├── IOrderProductsRepo.cs
│ │ └── IReviewRepo.cs
│ └── Implementations/
│ ├── UserRepo.cs
│ ├── ProductRepo.cs
│ ├── OrderRepo.cs
│ ├── OrderProductsRepo.cs
│ └── ReviewRepo.cs
│
├── Services/ # Business logic layer
│ ├── Interfaces/
│ │ ├── IUserService.cs
│ │ ├── IProductService.cs
│ │ ├── IOrderService.cs
│ │ └── IReviewService.cs
│ └── Implementations/
│ ├── UserService.cs
│ ├── ProductService.cs
│ ├── OrderService.cs
│ └── ReviewService.cs
│
├── Migrations/ # EF Core migrations (schema changes)
│ ├── 20241217080107_initialCreate.cs
│ ├── 20241217082938_UpdateUserEmail.cs
│ ├── 20241217082956_UpdateUserEmail2.cs
│ └── 20241218115023_editUserIDColumninOrderTable.cs
│
└── README.md