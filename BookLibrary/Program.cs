using Serilog;
using Serilog.Templates;
using Microsoft.Extensions.DependencyInjection;
using BookLibrary.Services;
using BookLibrary.UI;
using BookLibrary.Models;

namespace BookLibrary
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(new ExpressionTemplate(
                   "{@t:HH:mm:ss} " +
            "{#if @l = 'Information'}\u001b[32m[INF]\u001b[0m" +
            "{#else if @l = 'Warning'}\u001b[33m[WRN]\u001b[0m" +
            "{#else if @l = 'Error'}\u001b[31m[ERR]\u001b[0m" +
            "{#else if @l = 'Fatal'}\u001b[35m[FTL]\u001b[0m" +
            "{#else}[{@l}]{#end} " +
            "{@m}\n"))
                .WriteTo.File(Path.Combine("Logs", "app.log"))
                .CreateLogger();

            ServiceCollection services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.AddSerilog(Log.Logger);
            });

            services.AddSingleton<IStorage<Book>>(
                _ => new JsonStorage<Book>(Path.Combine("Data", "books.json")));

            services.AddSingleton<ILibrary, Library>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            
            try
            {
                ILibrary library = serviceProvider.GetRequiredService<ILibrary>();

                ConsoleMenu consoleMenu = new ConsoleMenu(library);

                consoleMenu.Run();

                library.SaveLibrary();
            }
            catch (StorageException ex)
            {

                Console.WriteLine(
                    "The library couldn't be loaded or saved. The data file may be corrupted.");
            }
        }
    }  
}
