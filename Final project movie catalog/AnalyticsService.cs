using System;
using System.Collections.Generic;
using System.Linq;

namespace WatchList;

public class AnalyticsService
{
    public List<MediaItem> SearchByTitle(List<MediaItem> items, string query)
    {
        return items
            .Where(x => x.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<MediaItem> FilterByStatus(List<MediaItem> items, WatchStatus status)
    {
        return items
            .Where(x => x.Status == status)
            .ToList();
    }

    public List<MediaItem> GetTopRated(List<MediaItem> items)
    {
        return items
            .Where(x => x.Rating.Score > 0)
            .OrderByDescending(x => x.Rating.Score)
            .ThenByDescending(x => x.ReleaseYear)
            .ToList();
    }

    public void PrintGenreStatistics(List<MediaItem> items)
    {
        if (items.Count == 0)
        {
            Console.WriteLine("Каталог пуст.");
            return;
        }

        var groups = items.GroupBy(x => x.Genre);

        foreach (var group in groups)
        {
            var ratedItems = group.Where(x => x.Rating.Score > 0).ToList();
            double avgScore = ratedItems.Count > 0 ? ratedItems.Average(x => x.Rating.Score) : 0;
            Console.WriteLine($"Жанр: {group.Key} | Количество: {group.Count()} | Средняя оценка: {avgScore:F1}");
        }
    }
}