using Microsoft.EntityFrameworkCore;
using NexusBooks.Book.Application.Dtos;
using NexusBooks.Book.Application.Interfaces;
using NexusBooks.Book.Application.Services;
using NexusBooks.Book.Infraestructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<NexusDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BookDb")));
builder.Services.AddScoped<IBookService, BookService>();

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

var books = app.MapGroup("/api/books");

books.MapGet("/", async (IBookService service, CancellationToken cancellation) =>
{
    var result = await service.GetAll(cancellation);
    return Results.Ok(result.Value);
});

books.MapGet("/{id:long}", async (long id, IBookService service, CancellationToken cancellation) =>
{
    var result = await service.GetById(id, cancellation);

    return result.StatusCode switch
    {
        404 => Results.NotFound(result.Message),
        _ => Results.Ok(result.Value)
    };
});

books.MapPost("/", async (DtoBook book, IBookService service, CancellationToken cancellation) =>
{
    var result = await service.Create(book, cancellation);

    return result.StatusCode switch
    {
        400 => Results.BadRequest(result.Message),
        _ when result.Value is not null => Results.Created($"/api/books/{result.Value.Id}", result.Value),
        _ => Results.Problem(result.Message, statusCode: result.StatusCode)
    };
});

books.MapPut("/{id:long}", async (long id, DtoBook book, IBookService service, CancellationToken cancellation) =>
{
    var payload = book with { Id = id };
    var result = await service.Update(payload, cancellation);

    return result.StatusCode switch
    {
        404 => Results.NotFound(result.Message),
        400 => Results.BadRequest(result.Message),
        _ => Results.Ok(result.Value)
    };
});

books.MapDelete("/{id:long}", async (long id, IBookService service, CancellationToken cancellation) =>
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
