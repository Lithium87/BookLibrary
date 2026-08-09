using BookLibrary.Models;

namespace BookLibrary.Services
{
    public class Library : ILibrary
    {
        private readonly List<Book> _books;

        public Library()
        {
            this._books = new List<Book>();
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

            this._books.Add(book);
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
        }

        public Book FindBookByIsbn(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentException("ISBN is required.", nameof(isbn));
            }
            Book book = this._books.SingleOrDefault(b => b.Isbn == isbn);

            if (book is null)
            {
                throw new InvalidOperationException($"No book with ISBN {isbn} found in the library.");
            }

            return book;
        }

        public IReadOnlyList<Book> FindBooksByAuthor(string author)
        {
            if(string.IsNullOrWhiteSpace(author))
            {
                throw new ArgumentException("Author is required.", nameof(author));
            }

            return this._books.Where(book => book.Author == author).ToList();
        }

        public IReadOnlyList<Book> FindBooksByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title is required.", nameof(title));
            }
            return this._books.Where(book => book.Title == title).ToList();
        }

        public IReadOnlyList<Book> FindBooksByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("Category is required.", nameof(category));
            }
            return this._books.Where(book => book.Category == category).ToList();
        }
    }
}
