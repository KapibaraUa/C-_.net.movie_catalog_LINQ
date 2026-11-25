using System;

namespace WatchList;

public abstract class MediaItem
{
    private int _releaseYear;

    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public WatchStatus Status { get; set; } = WatchStatus.Planned;
    public MediaRating Rating { get; set; }

    public int ReleaseYear
    {
        get { return _releaseYear; }
        set
        {
            if (value >= 1900 && value <= DateTime.Now.Year + 5)
            {
                _releaseYear = value;
            }
        }
    }

    protected MediaItem()
    {
    }

    protected MediaItem(string title, string genre, int releaseYear)
    {
        Title = title;
        Genre = genre;
        ReleaseYear = releaseYear;
        Status = WatchStatus.Planned;
        Rating = new MediaRating(0, string.Empty);
    }

    public abstract string GetInfo();

    public override string ToString()
    {
        return GetInfo();
    }
}