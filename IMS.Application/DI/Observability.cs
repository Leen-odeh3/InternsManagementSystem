

namespace IMS.Application.DI;
using System.Diagnostics;

public static class Observability
{
    public static readonly ActivitySource ActivitySource =
        new("IMS.API");
}
