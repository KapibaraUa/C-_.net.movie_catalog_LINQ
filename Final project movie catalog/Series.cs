namespace WatchList;

public class Series : MediaItem
{
    private int _totalSeasons;
    private int _watchedEpisodes;

    public int TotalSeasons
    {
        get { return _totalSeasons; }
        set
        {
            if (value > 0)
            {
                _totalSeasons = value;
            }
        }
    }

    public int WatchedEpisodes
    {
        get { return _watchedEpisodes; }
        set
        {
            if (value >= 0)
            {
                _watchedEpisodes = value;
            }
        }
    }

    public Series()
    {
    }

    public Series(string title, string genre, int releaseYear, int totalSeasons)
        : base(title, genre, releaseYear)
    {
        TotalSeasons = totalSeasons;
        WatchedEpisodes = 0;
    }

    public override string GetInfo()
    {
        return $"[Сериал] {Title} ({ReleaseYear}) | Жанр: {Genre} | Сезонов: {TotalSeasons}, просмотрено серий: {WatchedEpisodes} | Статус: {Status} | Оценка: {Rating}";
    }
}