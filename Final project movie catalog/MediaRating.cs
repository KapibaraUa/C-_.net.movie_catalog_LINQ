namespace WatchList;

public struct MediaRating
{
    private int _score;
    private string _comment;

    public int Score
    {
        get { return _score; }
        set
        {
            if (value >= 0 && value <= 10)
            {
                _score = value;
            }
        }
    }

    public string Comment
    {
        get { return _comment; }
        set { _comment = value ?? string.Empty; }
    }

    public MediaRating(int score, string comment)
    {
        _score = 0;
        _comment = string.Empty;
        Score = score;
        Comment = comment;
    }

    public override string ToString()
    {
        if (_score == 0)
        {
            return "Без оценки";
        }
        return string.IsNullOrEmpty(_comment) ? $"{_score}/10" : $"{_score}/10 ({_comment})";
    }
}