using System;
using System.Collections.Generic;

public class LocationAvailability
{
    public int openHour;
    public int closeHour; // не включая
    public LocationAvailability(int open, int close)
    {
        openHour = open; closeHour = close;
    }

    public bool IsOpen(int hour)
    {
        if (openHour < closeHour)
            return hour >= openHour && hour < closeHour;
        else
            return hour >= openHour || hour < closeHour;
    }
}
