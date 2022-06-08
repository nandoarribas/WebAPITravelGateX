using System;
using System.Runtime.Serialization;

namespace WebAPITravelGateX.Util
{
    [Flags]
    public enum Regimenes
    {
        sa,
        mp,
        ad,
        pc
    }

    [Flags]
    public enum Habitacion
    {
        standard,
        suite
    }
}
