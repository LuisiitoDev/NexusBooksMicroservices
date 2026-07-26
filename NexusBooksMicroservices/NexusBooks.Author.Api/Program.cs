using Microsoft.EntityFrameworkCore;
using NexusBooks.Author.Application.Dtos;
using NexusBooks.Author.Application.Interfaces;
using NexusBooks.Author.Application.Services;
using NexusBooks.Author.Infraestructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<NexusDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthorDb")));
builder.Services.AddScoped<IAuthorService, AuthorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var authors = app.MapGroup("/api/authors");

authors.MapGet("/", async (IAuthorService service, CancellationToken cancellation) =>
{
    var result = await service.GetAll(cancellation);
    return Results.Ok(result.Value);
});

authors.MapGet("/{id:long}", async (long id, IAuthorService service, CancellationToken cancellation) =>
{
    var result = await service.GetById(id, cancellation);

    return result.StatusCode switch
    {
        404 => Results.NotFound(result.Message),
        _ => Results.Ok(result.Value)
    };
});

authors.MapPost("/", async (DtoAuthor author, IAuthorService service, CancellationToken cancellation) =>
{
    var result = await service.Create(author, cancellation);

    return result.StatusCode switch
    {
        400 => Results.BadRequest(result.Message),
        _ when result.Value is not null => Results.Created($"/api/authors/{result.Value.Id}", result.Value),
        _ => Results.Problem(result.Message, statusCode: result.StatusCode)
    };
});

authors.MapPut("/{id:long}", async (long id, DtoAuthor author, IAuthorService service, CancellationToken cancellation) =>
{
    var payload = author with { Id = id };
    var result = await service.Update(payload, cancellation);

    return result.StatusCode switch
    {
        404 => Results.NotFound(result.Message),
        400 => Results.BadRequest(result.Message),
        _ => Results.Ok(result.Value)
    };
});

authors.MapDelete("/{id:long}", async (long id, IAuthorService service, CancellationToken cancellation) =>
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
