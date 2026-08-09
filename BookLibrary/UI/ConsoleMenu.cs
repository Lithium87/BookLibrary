using BookLibrary.Models;
using BookLibrary.Services;

namespace BookLibrary.UI
{
    internal class ConsoleMenu
    {
        private readonly ILibrary _library;

        public ConsoleMenu(ILibrary library)
        {
            _library = library;
        }

        private void ShowMenu()
        {
            Console.WriteLine("=================================");
            Console.WriteLine("         BOOK LIBRARY");
            Console.WriteLine("=================================");
            Console.WriteLine();
            Console.WriteLine("1. Show all books");
            Console.WriteLine("2. Add book");
            Console.WriteLine("3. Remove book");
            Console.WriteLine("4. Find book by ISBN");
            Console.WriteLine("5. Find books by title");
            Console.WriteLine("6. Find books by author");
            Console.WriteLine("7. Find books by category");
            Console.WriteLine("0. Exit");
            Console.WriteLine();
        }

        private string ReadString(string message)
        {
            while (true)
            {
                Console.Write(message);

                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                Console.WriteLine("Input cannot be empty.");
            }
        }

        private int ReadInt(string message)
        {
            while (true)
            {
                Console.Write(message);

                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    return value;
                }

                Console.WriteLine("Please enter a valid number.");
            }
        }

        private void AddBook()
        {
            try
            {
                string title = ReadString("Title: ");
                string author = ReadString("Author: ");
                int year = ReadInt("Year: ");
                int pages = ReadInt("Pages: ");
                string isbn = ReadString("ISBN: ");
                string category = ReadString("Category: ");

                Book book = new Book(title, author, year, pages, isbn, category);

                _library.AddBook(book);

                Console.WriteLine("Book added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding book: {ex.Message}");
            }
        }

        private void RemoveBook()
        {
            string isbn = ReadString("Enter ISBN of the book to remove: ");
            try
            {
                _library.RemoveBook(isbn);
                Console.WriteLine("Book removed successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error removing book: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error removing book: {ex.Message}");
            }
        }

        private void FindBookByIsbn()
        {
            string isbn = ReadString("ISBN: ");

            try
            {
                Book book = _library.FindBookByIsbn(isbn);

                Console.WriteLine();
                Console.WriteLine(book);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void FindBooksByTitle()
        {
            string title = ReadString("Title: ");
            
            try
            {
                IReadOnlyList<Book> books = _library.FindBooksByTitle(title);
                if (books.Count == 0)
                {
                    Console.WriteLine("No books found with the given title.");
                    return;
                }
                foreach (Book b in books)
                {
                    Console.WriteLine(b);
                    Console.WriteLine("-------------------------");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void FindBooksByAuthor()
        {
            string author = ReadString("Author: ");

            try
            {
                IReadOnlyList<Book> books = _library.FindBooksByAuthor(author);

                if (books.Count == 0)
                {
                    Console.WriteLine("No books found.");
                    return;
                }

                foreach (Book book in books)
                {
                    Console.WriteLine(book);
                    Console.WriteLine("---------------------");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void FindBooksByCategory()
        {
            string category = ReadString("Category: ");

            try
            {
                IReadOnlyList<Book> books = _library.FindBooksByCategory(category);

                if (books.Count == 0)
                {
                    Console.WriteLine("No books found.");
                    return;
                }

                foreach (Book book in books)
                {
                    Console.WriteLine(book);
                    Console.WriteLine("---------------------");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ShowAllBooks()
        {
            foreach (Book b in _library.GetAllBooks())
            {
                IReadOnlyList<Book> books = _library.GetAllBooks();

                if(books.Count == 0)
                {
                    Console.WriteLine("Library is empty!");
                    return;
                }

                Console.WriteLine(b);
                Console.WriteLine("-------------------------");
            }
        }

        public void Run()
        {
            while (true)
            {
                ShowMenu();

                Console.Write("Choose an option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllBooks();
                        break;

                    case "2":
                        AddBook();
                        break;

                    case "3":
                        RemoveBook();
                        break;

                    case "4":
                        FindBookByIsbn();
                        break;

                    case "5":
                        FindBooksByTitle();
                        break;

                    case "6":
                        FindBooksByAuthor();
                        break;

                    case "7":
                        FindBooksByCategory();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }
}
