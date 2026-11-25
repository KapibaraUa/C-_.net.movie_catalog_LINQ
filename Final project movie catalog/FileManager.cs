using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace WatchList;

public class FileManager
{
    private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto,
        Formatting = Formatting.Indented
    };

    public void SaveToJson(List<MediaItem> items, string filePath)
    {
        string json = JsonConvert.SerializeObject(items, _settings);
        File.WriteAllText(filePath, json);
    }

    public List<MediaItem> LoadFromJson(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<MediaItem>();
        }

        string json = File.ReadAllText(filePath);
        List<MediaItem>? items = JsonConvert.DeserializeObject<List<MediaItem>>(json, _settings);
        return items ?? new List<MediaItem>();
    }

    public void ExportFavoritesToTxt(List<MediaItem> items, string filePath)
    {
        var favorites = items
            .Where(x => x.Rating.Score >= 8)
            .OrderByDescending(x => x.Rating.Score)
            .ToList();

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("ИЗБРАННЫЕ ФИЛЬМЫ И СЕРИАЛЫ (оценка 8+)");
            writer.WriteLine(new string('-', 50));

            if (favorites.Count == 0)
            {
                writer.WriteLine("Список избранного пуст.");
                return;
            }

            foreach (var item in favorites)
            {
                writer.WriteLine(item.GetInfo());
            }
        }
    }
}