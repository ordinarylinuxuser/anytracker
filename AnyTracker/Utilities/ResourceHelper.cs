using System.Text.Json;

namespace AnyTracker.Utilities;

public static class ResourceHelper
{
    public static async Task<T> LoadJsonResourceFile<T>(string filename)
    {
        await using var stream = await FileSystem.OpenAppPackageFileAsync(filename);
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync() ?? throw new NullReferenceException();
        return JsonSerializer.Deserialize<T>(json) ?? throw new NullReferenceException();
    }
}