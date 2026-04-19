Scaffold-DbContext ` "Server=.;Database=GarageDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true" ` Microsoft.EntityFrameworkCore.SqlServer ` -OutputDir Models ` -Context AppDbContext ` -NoOnConfiguring ` -Force



with table:


Scaffold-DbContext ` "Server=.;Database=GarageDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true" ` Microsoft.EntityFrameworkCore.SqlServer ` -OutputDir Models ` -Tables Customers ` -Context AppDbContext ` -NoOnConfiguring ` -Force
