# eCommerce API – Product & Order Management

#  Overview
This is a   ASP.NET Core Web API, that manages a product catalog and supports transactional order, processing while preventing overselling under concurrent requests.
The system ensures data integrity using database transactions and row-level locking.
 
# Tech Stack
- *C# / .NET 8*
- *ASP.NET Core Web API*
- *Entity Framework Core*
- *SQL Server*
- *Swagger (OpenAPI)

#  Features
- Product CRUD operations
- Order placement with multiple products
- Prevention of overselling using transactions & row-level locks
- Partial order fulfillment (successful items are processed even if others fail)
- Clean separation of concerns using a service layer
 
##  Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio or Rider IDE or Vs Code




### Steps

1. Clone the repository
   git clone (https://github.com/Onoduchiagozie/eCommerceAPI.git)
   cd 

2. Update the connection string in appsettings.json

3. Restore dependencies
     dotnet restore
4. Apply database migrations
    dotnet ef database update
   
5. Run the application
    dotnet run
