using System;

namespace WatchList;

public class ConsoleMenu
{
    private readonly CatalogService _catalog;
    private readonly AnalyticsService _analytics;
    private readonly FileManager _fileManager;
    private readonly string _favoritesPath;

    public ConsoleMenu(CatalogService catalog, AnalyticsService analytics, FileManager fileManager, string favoritesPath)
    {
        _catalog = catalog;
        _analytics = analytics;
        _fileManager = fileManager;
        _favoritesPath = favoritesPath;
    }

    public void Run()
    {
        bool isRunning = true;

        while (isRunning)
        {
            Console.WriteLine("\n=== КАТАЛОГ ФИЛЬМОВ И СЕРИАЛОВ ===");
            Console.WriteLine("1. Показать весь каталог");
            Console.WriteLine("2. Добавить фильм или сериал");
            Console.WriteLine("3. Редактировать запись (статус / оценка / серия)");
            Console.WriteLine("4. Удалить запись");
            Console.WriteLine("5. Поиск и фильтры");
            Console.WriteLine("6. Статистика и экспорт избранного");
            Console.WriteLine("7. Что посмотреть вечером? (случайный выбор)");
            Console.WriteLine("0. Выход и сохранение");
            Console.Write("Выберите пункт: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        _catalog.Items.PrintAll();
                        break;
                    case "2":
                        ShowAddMenu();
                        break;
                    case "3":
                        ShowEditMenu();
                        break;
                    case "4":
                        ShowDeleteMenu();
                        break;
                    case "5":
                        ShowSearchMenu();
                        break;
                    case "6":
                        ShowStatsMenu();
                        break;
                    case "7":
                        ShowRandomPlanned();
                        break;
                    case "0":
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Неверный пункт меню.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }

    private void ShowAddMenu()
    {
        Console.WriteLine("1. Фильм");
        Console.WriteLine("2. Сериал");
        Console.WriteLine("0. Назад");
        Console.Write("Выберите тип: ");
        string? typeChoice = Console.ReadLine();

        if (typeChoice == "0")
        {
            return;
        }

        if (typeChoice != "1" && typeChoice != "2")
        {
            Console.WriteLine("Неверный выбор.");
            return;
        }

        Console.Write("Название: ");
        string title = Console.ReadLine() ?? string.Empty;

        Console.Write("Жанр: ");
        string genre = Console.ReadLine() ?? string.Empty;

        int year = ReadInt("Год выхода: ", 1900, DateTime.Now.Year + 5);

        if (typeChoice == "1")
        {
            int duration = ReadInt("Длительность (в минутах): ", 1, 1000);
            _catalog.AddItem(new Movie(title, genre, year, duration));
            Console.WriteLine("Фильм добавлен.");
        }
        else
        {
            int seasons = ReadInt("Количество сезонов: ", 1, 100);
            _catalog.AddItem(new Series(title, genre, year, seasons));
            Console.WriteLine("Сериал добавлен.");
        }
    }

    private void ShowEditMenu()
    {
        if (_catalog.Items.Count == 0)
        {
            Console.WriteLine("Каталог пуст.");
            return;
        }

        _catalog.Items.PrintAll();
        int number = ReadInt("Введите номер записи (0 - назад): ", 0, _catalog.Items.Count);
        if (number == 0)
        {
            return;
        }

        int index = number - 1;

        Console.WriteLine("1. Изменить статус");
        Console.WriteLine("2. Поставить оценку и отзыв");
        Console.WriteLine("3. Отметить +1 просмотренную серию (для сериалов)");
        Console.WriteLine("0. Назад");
        Console.Write("Выберите действие: ");
        string? action = Console.ReadLine();

        switch (action)
        {
            case "1":
                Console.WriteLine("Статусы: 1 - Planned, 2 - Watching, 3 - Completed, 4 - Dropped");
                int statusNum = ReadInt("Новый статус: ", 1, 4);
                _catalog.ChangeStatus(index, (WatchStatus)statusNum);
                break;
            case "2":
                int score = ReadInt("Оценка (от 1 до 10): ", 1, 10);
                Console.Write("Короткий отзыв: ");
                string comment = Console.ReadLine() ?? string.Empty;
                _catalog.SetRating(index, score, comment);
                Console.WriteLine("Оценка сохранена.");
                break;
            case "3":
                if (_catalog.AddWatchedEpisode(index))
                {
                    Console.WriteLine("Счётчик серий увеличен.");
                }
                else
                {
                    Console.WriteLine("Эта запись не является сериалом.");
                }
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Неверный пункт.");
                break;
        }
    }

    private void ShowDeleteMenu()
    {
        if (_catalog.Items.Count == 0)
        {
            Console.WriteLine("Каталог пуст.");
            return;
        }

        _catalog.Items.PrintAll();
        int number = ReadInt("Введите номер для удаления (0 - назад): ", 0, _catalog.Items.Count);
        if (number == 0)
        {
            return;
        }

        if (_catalog.RemoveByIndex(number - 1))
        {
            Console.WriteLine("Запись удалена.");
        }
    }

    private void ShowSearchMenu()
    {
        Console.WriteLine("1. Поиск по названию");
        Console.WriteLine("2. Фильтр по статусу");
        Console.WriteLine("3. Топ по рейтингу");
        Console.WriteLine("4. Показать только сериалы");
        Console.WriteLine("0. Назад");
        Console.Write("Выберите пункт: ");
        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Введите часть названия: ");
                string query = Console.ReadLine() ?? string.Empty;
                _analytics.SearchByTitle(_catalog.Items, query).PrintAll();
                break;
            case "2":
                Console.WriteLine("Статусы: 1 - Planned, 2 - Watching, 3 - Completed, 4 - Dropped");
                int statusNum = ReadInt("Выберите статус: ", 1, 4);
                _analytics.FilterByStatus(_catalog.Items, (WatchStatus)statusNum).PrintAll();
                break;
            case "3":
                _analytics.GetTopRated(_catalog.Items).PrintAll();
                break;
            case "4":
                _catalog.FindItems(x => x is Series).PrintAll();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Неверный пункт.");
                break;
        }
    }

    private void ShowStatsMenu()
    {
        Console.WriteLine("1. Статистика по жанрам");
        Console.WriteLine("2. Экспортировать избранное (8+) в файл");
        Console.WriteLine("0. Назад");
        Console.Write("Выберите пункт: ");
        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                _analytics.PrintGenreStatistics(_catalog.Items);
                break;
            case "2":
                _fileManager.ExportFavoritesToTxt(_catalog.Items, _favoritesPath);
                Console.WriteLine($"Избранное сохранено в файл: {_favoritesPath}");
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Неверный пункт.");
                break;
        }
    }

    private void ShowRandomPlanned()
    {
        MediaItem? item = _catalog.GetRandomPlanned();
        if (item == null)
        {
            Console.WriteLine("В списке «В планах» пока ничего нет.");
        }
        else
        {
            Console.WriteLine("Сегодня можно посмотреть:");
            Console.WriteLine(item.GetInfo());
        }
    }

    private int ReadInt(string prompt, int min, int max)
    {
        int value;
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (int.TryParse(input, out value) && value >= min && value <= max)
            {
                return value;
            }
            Console.WriteLine($"Введите число от {min} до {max}.");
        }
    }
}
