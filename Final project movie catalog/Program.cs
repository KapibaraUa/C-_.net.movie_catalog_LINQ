using System;
using System.Collections.Generic;
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
}