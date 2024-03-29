using System.Text.Json;
using WeavyChat.Entities;

namespace WeavyChat.Utils
{
    public static class Json
    {
        public static async Task<T?> ReadAsync<T>(string filePath)
        {
            using FileStream stream = File.OpenRead(filePath);

            if (stream.Length > 0)
            {
                return await JsonSerializer.DeserializeAsync<T>(stream);
            }            
            return default(T);
        }
    }
}
