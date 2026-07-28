using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using NexusBooks.ShoppingCart.Application.Dtos;
using NexusBooks.ShoppingCart.Application.Interfaces;
using NexusBooks.ShoppingCart.Application.Services;
using NexusBooks.ShoppingCart.Infraestructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<NexusDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("ShoppingCartDb"),
        new MySqlServerVersion(new Version(8, 0, 36))));
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NexusDbContext>();
    await dbContext.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var shoppingCarts = app.MapGroup("/api/shopping-carts");

shoppingCarts.MapGet("/", async (IShoppingCartService service, CancellationToken cancellation) =>
{
    var result = await service.GetAll(cancellation);
    return Results.Ok(result.Value);
});

shoppingCarts.MapGet("/{id:guid}", async (Guid id, IShoppingCartService service, CancellationToken cancellation) =>
{
    var result = await service.GetById(id, cancellation);

    return result.StatusCode switch
    {
        404 => Results.NotFound(result.Message),
        _ => Results.Ok(result.Value)
    };
});

shoppingCarts.MapPost("/", async (DtoShoppingCartSession shoppingCart, IShoppingCartService service, CancellationToken cancellation) =>
{
    var result = await service.Create(shoppingCart, cancellation);

    return result.StatusCode switch
    {
        400 => Results.BadRequest(result.Message),
        _ when result.Value is not null => Results.Created($"/api/shopping-carts/{result.Value.Id}", result.Value),
        _ => Results.Problem(result.Message, statusCode: result.StatusCode)
    };
});

shoppingCarts.MapPut("/{id:guid}", async (Guid id, DtoShoppingCartSession shoppingCart, IShoppingCartService service, CancellationToken cancellation) =>
{
    var payload = shoppingCart with { Id = id };
    var result = await service.Update(payload, cancellation);

    return result.StatusCode switch
    {
        404 => Results.NotFound(result.Message),
        400 => Results.BadRequest(result.Message),
        _ => Results.Ok(result.Value)
    };
});

shoppingCarts.MapDelete("/{id:guid}", async (Guid id, IShoppingCartService service, CancellationToken cancellation) =>
{
    var result = await service.Delete(id, cancellation);

    return result.StatusCode switch
    {
        404 => Results.NotFound(result.Message),
        400 => Results.BadRequest(result.Message),
        _ => Results.NoContent()
    };
});

app.Run();
