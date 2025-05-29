using System.Collections.Immutable;
using System.Text.Json;

public static class FileReader {
    public static ImmutableArray<string> readJSONFile(string path) {
        using (StreamReader r = new StreamReader(path))
        {
            var json = r.ReadToEnd();
            return JsonSerializer.Deserialize<List<string>>(json)?.ToImmutableArray() ?? ImmutableArray<string>.Empty;
        }
    }
}