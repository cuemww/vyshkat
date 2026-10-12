using VyshKat.App.Domain;

namespace VyshKat.App.Services;

public class TechInspectService : ITechInspect
{
    public bool CheckTransport(ITransport t)
    {
        // Оценка простоты.
        return t.NoobScore >= 1 && t.NoobScore <= 10;
    }
}
