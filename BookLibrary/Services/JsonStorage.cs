using System.Text.Json;

namespace BookLibrary.Services
{
    internal class JsonStorage<T> : IStorage<T>
    {
        private readonly string _filePath;

        public JsonStorage(string filePath)
        {
            _filePath = filePath;
        }

        public void Save(IEnumerable<T> items)
        {
            try
            {
                string? directory = Path.GetDirectoryName(_filePath);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }


                string json = JsonSerializer.Serialize(items, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });

                File.WriteAllText(_filePath, json);
            }
            catch(Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new StorageException($"Failed to save to file '{_filePath}'.", ex);
            }
        }

        public IReadOnlyList<T> Load()
        {
            if(!File.Exists(_filePath))
            {
                return new List<T>();
            }
            

            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch(JsonException ex)
            {
                throw new StorageException($"Failed to deserialize JSON from file '{_filePath}'.", ex);
            }
        }
    }
}
