using VyshKat.App.Domain;
using VyshKat.App.Services;

namespace VyshKat.Tests;

public class TechInspectServiceTests
{
    [Theory]
    [InlineData(1, true)]
    [InlineData(5, true)]
    [InlineData(10, true)]
    [InlineData(0, false)]
    [InlineData(11, false)]
    public void CheckTransport_ValidatesNoobScoreRange(int score, bool expected)
    {
        var inspect = new TechInspectService();
        var scooter = new Scooter(1, "Test", score, 1.0);

        var result = inspect.CheckTransport(scooter);

        Assert.Equal(expected, result);
    }
}
