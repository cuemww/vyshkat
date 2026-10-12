using VyshKat.App.Domain;

namespace VyshKat.App.Services;

public class ParkManager
{
    private readonly ITechInspect _techInspect;
    private readonly List<IInventoryItem> _things = new();

    // Зависимость на интерфейс, а не на конкретный класс (DIP).
    public ParkManager(ITechInspect inspect)
    {
        _techInspect = inspect;
    }

    public bool AddTransport(ITransport t)
    {
        if (!_techInspect.CheckTransport(t))
            return false;

        _things.Add(t);
        return true;
    }

    public void AddThing(IInventoryItem item)
    {
        _things.Add(item);
    }

    // Считаем потребителей энергии.
    public double CalcTotalEnergy()
    {
        return _things
            .OfType<IEnergyConsumer>()
            .Sum(x => x.EnergyPerDay);
    }

    // Сколько техники и сколько вещей на балансе.
    public int CountTransports()
    {
        return _things.OfType<ITransport>().Count();
    }

    public int CountThings()
    {
        return _things.Count(x => x is not ITransport);
    }

    // Техника для новичкам.
    public IEnumerable<ITransport> GetForNoobs()
    {
        return _things
            .OfType<ITransport>()
            .Where(x => x.NoobScore >= 6);
    }

    public IEnumerable<IInventoryItem> GetAllItems()
    {
        return _things.AsReadOnly();
    }
}
