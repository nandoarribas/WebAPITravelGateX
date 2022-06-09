using System;

namespace WebAPITravelGateX.Util
{
    [Flags]
    public enum MealPlan
    {
        sa,
        mp,
        ad,
        pc
    }

    [Flags]
    public enum RoomType
    {
        standard,
        suite
    }

    [Flags]
    public enum ResortRoomType
    {
        st,
        su
    }
}
