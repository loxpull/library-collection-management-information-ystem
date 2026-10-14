using LibraryFund.Core.Models;
using LibraryFund.Core.Services;

namespace LibraryFund.UnitTests;

public class BookCatalogServiceTests
{
    [Fact]
    public void Add_ValidBook_AddsBookToCatalog()
    {
        var catalog = new BookCatalogService();
        var book = CreateBook();

        catalog.Add(book);

        Assert.Single(catalog.GetAll());
        Assert.Equal(book, catalog.GetAll().First());
    }

    [Fact]
    public void FindById_ExistingBook_ReturnsBook()
    {
        var catalog = new BookCatalogService();
        var book = CreateBook();
        catalog.Add(book);

        var result = catalog.FindById(book.Id);

        Assert.Equal(book, result);
    }

    [Fact]
    public void Search_ByTitle_ReturnsMatchingBook()
    {
        var catalog = new BookCatalogService();
        var book = CreateBook(title: "Война и мир");
        catalog.Add(book);

        var result = catalog.Search("война");

        Assert.Contains(book, result);
    }

    [Fact]
    public void Search_ByAuthor_ReturnsMatchingBook()
    {
        var catalog = new BookCatalogService();
        var book = CreateBook(author: "Лев Толстой");
        catalog.Add(book);

        var result = catalog.Search("толстой");

        Assert.Contains(book, result);
    }

    [Fact]
    public void Add_DuplicateIsbn_ThrowsException()
    {
        var catalog = new BookCatalogService();
        catalog.Add(CreateBook(isbn: "9781234567890"));

        var duplicate = CreateBook(
            title: "Другая книга",
            isbn: "9781234567890");

        Assert.Throws<InvalidOperationException>(
            () => catalog.Add(duplicate));
    }

    [Fact]
    public void Search_NoMatches_ReturnsEmptyCollection()
    {
        var catalog = new BookCatalogService();
        catalog.Add(CreateBook());

        var result = catalog.Search("несуществующая книга");

        Assert.Empty(result);
    }

    private static Book CreateBook(
        string title = "Тестовая книга",
        string author = "Тестовый автор",
        string isbn = "9781234567890",
        int publicationYear = 2020,
        string genre = "Роман")
    {
        return new Book(title, author, isbn, publicationYear, genre);
    }
}