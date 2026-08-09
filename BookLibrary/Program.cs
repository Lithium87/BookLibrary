using BookLibrary.Services;
using BookLibrary.UI;

namespace BookLibrary
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ILibrary library = new Library();

            ConsoleMenu consoleMenu = new ConsoleMenu(library);

            consoleMenu.Run();
        }
    }  
}
