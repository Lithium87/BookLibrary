
using BookLibrary.Models;

namespace BookLibrary.Services
{
    public interface ILibrary
    {
        void AddBook(Book book);
        void RemoveBook(string isbn);
        Book FindBookByIsbn(string isbn);
        IReadOnlyList<Book> FindBooksByTitle(string title);
        IReadOnlyList<Book> FindBooksByAuthor(string author);
        IReadOnlyList<Book> FindBooksByCategory(string category);
        IReadOnlyList<Book> GetAllBooks();
    }
}
