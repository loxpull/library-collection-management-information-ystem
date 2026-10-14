using LibraryFund.Core.Models;

namespace LibraryFund.Core.Services;

public class BookCatalogService
{
    private readonly List<Book> _books = new();

    public IReadOnlyCollection<Book> GetAll()
    {
        return _books.AsReadOnly();
    }

    public Book Add(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);

        if (_books.Any(existing =>
            string.Equals(existing.Isbn, book.Isbn, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                $"Книга с ISBN '{book.Isbn}' уже существует.");
        }

        _books.Add(book);
        return book;
    }

    public Book? FindById(Guid id)
    {
        return _books.FirstOrDefault(book => book.Id == id);
    }

    public IReadOnlyCollection<Book> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return GetAll();

        string normalizedQuery = query.Trim();

        return _books
            .Where(book =>
                book.Title.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase) ||
                book.Author.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase) ||
                book.Isbn.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
            .ToList()
            .AsReadOnly();
    }
}
