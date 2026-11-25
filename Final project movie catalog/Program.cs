using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WatchList;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        string dataFilePath = "catalog.json";
        string favoritesFilePath = "favorites.txt";

        FileManager fileManager = new FileManager();
        AnalyticsService analyticsService = new AnalyticsService();

        List<MediaItem> loadedItems = fileManager.LoadFromJson(dataFilePath);
        if (!File.Exists(dataFilePath))
        {
            loadedItems = GetDefaultItems();
        }

        CatalogService catalogService = new CatalogService(loadedItems);

        catalogService.OnItemCompleted += ShowNotification;

        ConsoleMenu menu = new ConsoleMenu(catalogService, analyticsService, fileManager, favoritesFilePath);
        menu.Run();

        fileManager.SaveToJson(catalogService.Items, dataFilePath);
        Console.WriteLine("Данные сохранены. До встречи!");
    }

    static void ShowNotification(string message)
    {
        Console.WriteLine($"\n*** {message} ***");
    }

    /// <summary>
    /// Returns starter catalog items used on first launch when no saved data file exists.
    /// </summary>
    static List<MediaItem> GetDefaultItems()
    {
        return new List<MediaItem>
        {
            new Movie("Интерстеллар", "Фантастика", 2014, 169)
            {
                Status = WatchStatus.Completed,
                Rating = new MediaRating(9, "Отличный саундтрек")
            },
            new Movie("Начало", "Триллер", 2010, 148),
            new Movie("Дюна", "Фантастика", 2021, 155)
            {
                Status = WatchStatus.Planned
            },
            new Series("Во все тяжкие", "Драма", 2008, 5)
            {
                Status = WatchStatus.Completed,
                WatchedEpisodes = 62,
                Rating = new MediaRating(10, "Шедевр")
            },
            new Series("Очень странные дела", "Фантастика", 2016, 4)
            {
                Status = WatchStatus.Watching,
                WatchedEpisodes = 12
            }
        };
    }
}