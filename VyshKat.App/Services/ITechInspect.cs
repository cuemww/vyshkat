using VyshKat.App.Domain;

namespace VyshKat.App.Services;

public interface ITechInspect
{
    bool CheckTransport(ITransport t);
}
