using LibraryFund.Core.Models;
using LibraryFund.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<BookCatalogService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/books", (BookCatalogService catalog) =>
{
    return Results.Ok(catalog.GetAll());
})
.WithName("GetAllBooks");

app.MapGet("/api/books/{id:guid}", (Guid id, BookCatalogService catalog) =>
{
    var book = catalog.FindById(id);

    return book is null
        ? Results.NotFound(new { message = "Книга не найдена." })
        : Results.Ok(book);
})
.WithName("GetBookById");

app.MapGet("/api/books/search", (string? q, BookCatalogService catalog) =>
{
    return Results.Ok(catalog.Search(q ?? string.Empty));
})
.WithName("SearchBooks");

app.MapPost("/api/books", (CreateBookRequest request, BookCatalogService catalog) =>
{
    if (string.IsNullOrWhiteSpace(request.Title) ||
        string.IsNullOrWhiteSpace(request.Author) ||
        string.IsNullOrWhiteSpace(request.Isbn) ||
        string.IsNullOrWhiteSpace(request.Genre))
    {
        return Results.BadRequest(new
        {
            message = "Название, автор, ISBN и жанр обязательны."
        });
    }

    if (request.PublicationYear < 1 ||
        request.PublicationYear > DateTime.UtcNow.Year)
    {
        return Results.BadRequest(new
        {
            message = "Указан некорректный год публикации."
        });
    }

    var book = new Book(
        request.Title,
        request.Author,
        request.Isbn,
        request.PublicationYear,
        request.Genre);

    try
    {
        catalog.Add(book);
        return Results.Created($"/api/books/{book.Id}", book);
    }
    catch (InvalidOperationException)
    {
        return Results.Conflict(new
        {
            message = "Книга с таким ISBN уже существует."
        });
    }
})
.WithName("CreateBook");

app.Run();

public record CreateBookRequest(
    string Title,
    string Author,
    string Isbn,
    int PublicationYear,
    string Genre);

public partial class Program;