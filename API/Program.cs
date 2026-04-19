using API.Extension;
using API.Extensions;
using App.Managers;
using App.Managers.Contract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration, builder.Environment);

// Register authentication services
builder.Services.AddAuthenticationServices(builder.Configuration);

// Register Auth Service
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add Swagger Documentation with JWT support
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.RoutePrefix = "swagger"; // Change this from string.Empty to "swagger"
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();  // Must be before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
