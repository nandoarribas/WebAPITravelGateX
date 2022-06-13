using System;

namespace WebAPITravelGateX.Util
{
    [Flags]
    public enum MealPlan
    {
        sa=0,
        ad=1,
        mp=2,
        pc=3
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
