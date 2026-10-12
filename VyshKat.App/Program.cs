using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using VyshKat.App.Domain;
using VyshKat.App.Services;

namespace VyshKat.App;

class Program
{
    private static ParkManager _manager = null;

    static void Main()
    {
        // Техосмотр подключаем по интерфейсу.
        var provider = new ServiceCollection()
            .AddTransient<ITechInspect, TechInspectService>()
            .AddSingleton<ParkManager>()
            .BuildServiceProvider();

        _manager = provider.GetRequiredService<ParkManager>();

        Console.WriteLine("ВышКат: учёт парка");

        while (true)
        {
            PrintMenu();
            var choice = ReadLine("> ");
            if (choice == null)
                break;

            switch (choice.Trim())
            {
                case "1": AddTransport(); break;
                case "2": AddThing(); break;
                case "3": ShowCounts(); break;
                case "4": ShowEnergy(); break;
                case "5": ShowForNoobs(); break;
                case "6": ShowInventory(); break;
                case "0":
                    Console.WriteLine("Выход.");
                    return;
                default:
                    Console.WriteLine("Такого пункта нет.");
                    break;
            }
        }
    }

    static void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("1 - добавить транспорт (с техосмотром)");
        Console.WriteLine("2 - добавить вещь на баланс");
        Console.WriteLine("3 - отчёт по количеству");
        Console.WriteLine("4 - суммарное энергопотребление");
        Console.WriteLine("5 - техника для новичков");
        Console.WriteLine("6 - вся инвентарная ведомость");
        Console.WriteLine("0 - выход");
    }

    static void AddTransport()
    {
        Console.WriteLine("Какой транспорт?");
        Console.WriteLine("  1 - самокат");
        Console.WriteLine("  2 - электровелик");
        Console.WriteLine("  3 - велосипед");
        var kind = ReadInt("Тип: ");
        if (kind == null) return;

        var num = ReadInt("Инвентарный номер: ");
        if (num == null) return;

        var name = ReadText("Название: ");
        if (name == null) return;

        var score = ReadInt("Простота для новичка (1-10): ");
        if (score == null) return;

        ITransport t;
        if (kind == 3)
        {
            t = new Bicycle(num.Value, name, score.Value);
        }
        else
        {
            var energy = ReadDouble("Суточный расход энергии: ");
            if (energy == null) return;

            t = kind == 2
                ? new EBike(num.Value, name, score.Value, energy.Value)
                : new Scooter(num.Value, name, score.Value, energy.Value);
        }

        var accepted = _manager.AddTransport(t);
        Console.WriteLine(accepted
            ? $"Принято: \"{t.Name}\" прошёл техосмотр."
            : $"Отклонено: \"{t.Name}\" не прошёл техосмотр.");
    }

    static void AddThing()
    {
        Console.WriteLine("Что добавляем?");
        Console.WriteLine("  1 - шлем");
        Console.WriteLine("  2 - зарядная станция");
        var kind = ReadInt("Тип: ");
        if (kind == null) return;

        var num = ReadInt("Инвентарный номер: ");
        if (num == null) return;

        var name = ReadText("Название: ");
        if (name == null) return;

        if (kind == 2)
        {
            var energy = ReadDouble("Суточный расход энергии: ");
            if (energy == null) return;
            _manager.AddThing(new ChargeStation(num.Value, name, energy.Value));
        }
        else
        {
            _manager.AddThing(new Helmet(num.Value, name));
        }

        Console.WriteLine("Вещь добавлена на баланс.");
    }

    static void ShowCounts()
    {
        Console.WriteLine($"Техники на балансе: {_manager.CountTransports()}");
        Console.WriteLine($"Вещей на балансе:   {_manager.CountThings()}");
    }

    static void ShowEnergy()
    {
        Console.WriteLine($"Суммарное суточное энергопотребление: {_manager.CalcTotalEnergy()} кВт·ч");
    }

    static void ShowForNoobs()
    {
        var list = _manager.GetForNoobs().ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("Техники для новичков пока нет.");
            return;
        }

        Console.WriteLine("Подходит новичкам (оценка >= 6):");
        foreach (var t in list)
            Console.WriteLine($"- {t.Name} (оценка: {t.NoobScore})");
    }

    static void ShowInventory()
    {
        var items = _manager.GetAllItems().ToList();
        if (items.Count == 0)
        {
            Console.WriteLine("Парк пуст.");
            return;
        }

        Console.WriteLine("Инвентарная ведомость:");
        foreach (var item in items)
            Console.WriteLine($"#{item.Number} - {item.Name} ({item.GetType().Name})");
    }
    static string ReadLine(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    static string ReadText(string prompt)
    {
        var s = ReadLine(prompt);
        return s;
    }

    static int? ReadInt(string prompt)
    {
        while (true)
        {
            var s = ReadLine(prompt);
            if (s == null) return null;
            if (int.TryParse(s.Trim(), out var v)) return v;
            Console.WriteLine("Нужно целое число.");
        }
    }

    static double? ReadDouble(string prompt)
    {
        while (true)
        {
            var s = ReadLine(prompt);
            if (s == null) return null;

            // Точка и запятая как разделитель.
            var normalized = s.Trim().Replace(',', '.');
            if (double.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
                return v;

            Console.WriteLine("Нужно число.");
        }
    }
}
