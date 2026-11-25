using System;
using System.Collections.Generic;

namespace WatchList;

public static class CatalogExtensions
{
    public static void PrintAll(this IEnumerable<MediaItem> items)
    {
        int index = 1;
        foreach (var item in items)
        {
            Console.WriteLine($"{index}. {item.GetInfo()}");
            index++;
        }

        if (index == 1)
        {
            Console.WriteLine("Список пуст.");
        }
    }
}
