namespace WatchList;

public class Movie : MediaItem
{
    private int _durationMinutes;

    public int DurationMinutes
    {
        get { return _durationMinutes; }
        set
        {
            if (value > 0)
            {
                _durationMinutes = value;
            }
        }
    }

    public Movie()
    {
    }

    public Movie(string title, string genre, int releaseYear, int durationMinutes)
        : base(title, genre, releaseYear)
    {
        DurationMinutes = durationMinutes;
    }

    public override string GetInfo()
    {
        return $"[Фильм] {Title} ({ReleaseYear}) | Жанр: {Genre} | {DurationMinutes} мин. | Статус: {Status} | Оценка: {Rating}";
    }
}