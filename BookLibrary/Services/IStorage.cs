
namespace BookLibrary.Services
{
    public interface IStorage<T>
    {
        void Save(IEnumerable<T> items);
        IReadOnlyList<T> Load();
    }
}
