using BookLibrary.Models;
using Microsoft.Extensions.Logging;

namespace BookLibrary.Services
{
    public class Library : ILibrary
    {
        private readonly List<Book> _books;
        private readonly IStorage<Book> _storage;
        private readonly ILogger<Library> _logger;

        public Library(IStorage<Book> storage, ILogger<Library> logger)
        {
            _storage = storage;
            _logger = logger;
            try
            {
                _books = storage.Load().ToList();

                _logger.LogInformation("Library loaded successfully with {Count} books.", _books.Count);
            }
            catch(StorageException ex)
            {
                _logger.LogCritical(ex, "Failed to load the library from storage.");

                throw;
            }
        }

        public void AddBook(Book book)
        {
            if(book is null)
            {
                throw new ArgumentNullException(nameof(book), "Book cannot be null.");
            }
           
            foreach(Book existingBook in this._books)
            {
                if (existingBook.Isbn == book.Isbn)
                {
                    throw new InvalidOperationException($"A book with ISBN {book.Isbn} already exists in the library.");
                }
            }

            try
            {
                this._books.Add(book);

                _logger.LogInformation("Book added to the library. ISBN: {Isbn}, Title: {Title}.", book.Isbn, book.Title);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while adding the book with ISBN {Isbn} to the library.", book.Isbn);
                throw;
            }
        }

        public IReadOnlyList<Book> GetAllBooks()
        {
            return this._books;
        }

        public void RemoveBook(string isbn)
        {
            if(string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentException("ISBN is required.", nameof(isbn));
            }
            Book bookToRemove = this._books.FirstOrDefault(b => b.Isbn == isbn);
            if (bookToRemove is null)
            {
                throw new InvalidOperationException($"No book with ISBN {isbn} found in the library.");
            }
            this._books.Remove(bookToRemove);

            _logger.LogInformation("Book with ISBN {Isbn} removed from the library.", isbn);
        }

        public Book? FindBookByIsbn(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentException("ISBN is required.", nameof(isbn));
            }
            Book book = this._books.SingleOrDefault(b => b.Isbn == isbn);

            return book;
        }

        private IReadOnlyList<Book> FindBooks(Func<Book, bool> predicate)
        {
            return this._books.Where(predicate).ToList();
        }

        public IReadOnlyList<Book> FindBooksByAuthor(string author)
        {
            if(string.IsNullOrWhiteSpace(author))
            {
                throw new ArgumentException("Author is required.", nameof(author));
            }

            return FindBooks(book => book.Author == author);
        }

        public IReadOnlyList<Book> FindBooksByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title is required.", nameof(title));
            }
            return FindBooks(book => book.Title == title);
        }

        public IReadOnlyList<Book> FindBooksByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("Category is required.", nameof(category));
            }
            return FindBooks(book => book.Category == category);
        }

        public void SaveLibrary()
        {
            try
            {
                _storage.Save(_books);

                _logger.LogInformation("Library saved successfully with {Count} books.", _books.Count);
            }
            catch (StorageException ex)
            {
                _logger.LogError(ex, "An error occurred while saving the library.");
                throw;
            }
        }
    }
}
