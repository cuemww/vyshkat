using Moq;
using VyshKat.App.Domain;
using VyshKat.App.Services;

namespace VyshKat.Tests;

public class ParkManagerTests
{
    // Вспомогательный метод: менеджер с "всегда пропускающим" техосмотром.
    static ParkManager MakeManagerWithInspect(bool result)
    {
        var mockInspect = new Mock<ITechInspect>();
        mockInspect.Setup(m => m.CheckTransport(It.IsAny<ITransport>())).Returns(result);
        return new ParkManager(mockInspect.Object);
    }

    [Fact]
    public void AddTransport_WhenInspectOk_ReturnsTrue()
    {
        var manager = MakeManagerWithInspect(true);
        var testScooter = new Scooter(1, "Test", 5, 1.0);

        var res = manager.AddTransport(testScooter);

        Assert.True(res);
        Assert.Contains<IInventoryItem>(testScooter, manager.GetAllItems());
    }

    [Fact]
    public void AddTransport_WhenInspectFails_ReturnsFalse()
    {
        // Подменяем техосмотр на отказ через Moq.
        var manager = MakeManagerWithInspect(false);
        var testScooter = new Scooter(1, "Test", 5, 1.0);

        var res = manager.AddTransport(testScooter);

        Assert.False(res);
        Assert.Empty(manager.GetAllItems());
    }

    [Fact]
    public void CalcTotalEnergy_OnlyCountsEnergyConsumers()
    {
        var manager = MakeManagerWithInspect(true);

        manager.AddTransport(new Scooter(1, "S1", 10, 2.0)); // +2.0
        manager.AddTransport(new Bicycle(2, "B1", 10));      // +0 (велик)
        manager.AddThing(new ChargeStation(3, "C1", 5.0));   // +5.0
        manager.AddThing(new Helmet(4, "H1"));               // +0 (шлем)

        var sum = manager.CalcTotalEnergy();

        Assert.Equal(7.0, sum);
    }

    [Fact]
    public void CountTransports_And_CountThings_AreCorrect()
    {
        var manager = MakeManagerWithInspect(true);

        manager.AddTransport(new Scooter(1, "S1", 10, 1.0));
        manager.AddTransport(new EBike(2, "E1", 8, 1.0));
        manager.AddThing(new Helmet(3, "H1"));
        manager.AddThing(new ChargeStation(4, "C1", 5.0));

        Assert.Equal(2, manager.CountTransports());
        Assert.Equal(2, manager.CountThings());
    }

    [Fact]
    public void GetForNoobs_FiltersCorrectly()
    {
        var manager = MakeManagerWithInspect(true);

        var hard = new Scooter(1, "Hard", 4, 1.0);
        var easy = new Scooter(2, "Easy", 7, 1.0);

        manager.AddTransport(hard);
        manager.AddTransport(easy);

        var list = manager.GetForNoobs().ToList();

        Assert.Single(list);
        Assert.Equal("Easy", list[0].Name);
    }
}
