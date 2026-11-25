using System;
using System.Collections.Generic;
using System.Linq;

namespace WatchList;

public delegate void MediaEventHandler(string message);

public class CatalogService
{
    private readonly List<MediaItem> _items;
    private readonly Random _random = new Random();

    public event MediaEventHandler? OnItemCompleted;

    public List<MediaItem> Items
    {
        get { return _items; }
    }

    public CatalogService(List<MediaItem> initialItems)
    {
        _items = initialItems;
    }

    public void AddItem(MediaItem item)
    {
        _items.Add(item);
    }

    public bool RemoveByIndex(int index)
    {
        if (index < 0 || index >= _items.Count)
        {
            return false;
        }

        _items.RemoveAt(index);
        return true;
    }

    public void ChangeStatus(int index, WatchStatus newStatus)
    {
        if (index < 0 || index >= _items.Count)
        {
            return;
        }

        MediaItem item = _items[index];
        item.Status = newStatus;

        if (newStatus == WatchStatus.Completed)
        {
            OnItemCompleted?.Invoke($"Поздравляем! Вы завершили просмотр: \"{item.Title}\"");
        }
    }

    public void SetRating(int index, int score, string comment)
    {
        if (index < 0 || index >= _items.Count)
        {
            return;
        }

        _items[index].Rating = new MediaRating(score, comment);
    }

    public bool AddWatchedEpisode(int index)
    {
        if (index < 0 || index >= _items.Count)
        {
            return false;
        }

        Series? series = _items[index] as Series;
        if (series == null)
        {
            return false;
        }

        series.WatchedEpisodes++;
        if (series.Status == WatchStatus.Planned)
        {
            series.Status = WatchStatus.Watching;
        }
        return true;
    }

    public List<MediaItem> FindItems(Predicate<MediaItem> predicate)
    {
        List<MediaItem> result = new List<MediaItem>();
        foreach (var item in _items)
        {
            if (predicate(item))
            {
                result.Add(item);
            }
        }
        return result;
    }

    public MediaItem? GetRandomPlanned()
    {
        var planned = _items.Where(x => x.Status == WatchStatus.Planned).ToList();
        if (planned.Count == 0)
        {
            return null;
        }

        int randomIndex = _random.Next(0, planned.Count);
        return planned[randomIndex];
    }
}
