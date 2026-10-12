namespace VyshKat.App.Domain;

public class Scooter : ITransport, IEnergyConsumer
{
    public int Number { get; }
    public string Name { get; }
    public int NoobScore { get; }
    public double EnergyPerDay { get; }

    public Scooter(int num, string name, int noobScore, double energy)
    {
        Number = num;
        Name = name;
        NoobScore = noobScore;
        EnergyPerDay = energy;
    }
}

// Электровелосипед:едет,ест ток.
public class EBike : ITransport, IEnergyConsumer
{
    public int Number { get; }
    public string Name { get; }
    public int NoobScore { get; }
    public double EnergyPerDay { get; }

    public EBike(int num, string name, int noobScore, double energy)
    {
        Number = num;
        Name = name;
        NoobScore = noobScore;
        EnergyPerDay = energy;
    }
}

// Обычный велик: энергию не тратит
public class Bicycle : ITransport
{
    public int Number { get; }
    public string Name { get; }
    public int NoobScore { get; }

    public Bicycle(int num, string name, int noobScore)
    {
        Number = num;
        Name = name;
        NoobScore = noobScore;
    }
}

// Шлем
public class Helmet : IInventoryItem
{
    public int Number { get; }
    public string Name { get; }

    public Helmet(int num, string name)
    {
        Number = num;
        Name = name;
    }
}

// Зарядная станция: потребляет энергию.
public class ChargeStation : IInventoryItem, IEnergyConsumer
{
    public int Number { get; }
    public string Name { get; }
    public double EnergyPerDay { get; }

    public ChargeStation(int num, string name, double energy)
    {
        Number = num;
        Name = name;
        EnergyPerDay = energy;
    }
}
