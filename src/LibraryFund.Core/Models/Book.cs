namespace LibraryFund.Core.Models;

public class Book
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Title { get; private set; }

    public string Author { get; private set; }

    public string Isbn { get; private set; }

    public int PublicationYear { get; private set; }

    public string Genre { get; private set; }

    public Book(
        string title,
        string author,
        string isbn,
        int publicationYear,
        string genre)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Название книги обязательно.", nameof(title));

        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Автор обязателен.", nameof(author));

        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN обязателен.", nameof(isbn));

        if (publicationYear < 1 || publicationYear > DateTime.UtcNow.Year)
            throw new ArgumentOutOfRangeException(
                nameof(publicationYear),
                "Некорректный год издания.");

        if (string.IsNullOrWhiteSpace(genre))
            throw new ArgumentException("Жанр обязателен.", nameof(genre));

        Title = title.Trim();
        Author = author.Trim();
        Isbn = isbn.Trim();
        PublicationYear = publicationYear;
        Genre = genre.Trim();
    }
}
