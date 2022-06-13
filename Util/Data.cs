using System;

namespace WebAPITravelGateX.Util
{
    /// <summary>
    /// Represent all the available meal plans
    /// </summary>
    [Flags]
    public enum MealPlan
    {
        sa=0,
        ad=1,
        mp=2,
        pc=3
    }

    /// <summary>
    /// Represent the type of rooms
    /// </summary>
    [Flags]
    public enum RoomType
    {
        standard,
        suite
    }

    /// <summary>
    /// Represent the type of room for resort hotels
    /// </summary>
    [Flags]
    public enum ResortRoomType
    {
        st,
        su
    }
}
