namespace VyshKat.App.Domain;

// техника и вещи
public interface IInventoryItem
{
    int Number { get; }
    string Name { get; }
}

// Устройства тратят энергию
public interface IEnergyConsumer
{
    double EnergyPerDay { get; }
}

// Транспорт
public interface ITransport : IInventoryItem
{
    int NoobScore { get; }
}
