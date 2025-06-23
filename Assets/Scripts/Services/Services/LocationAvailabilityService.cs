using System.Collections.Generic;

public class LocationAvailabilityService
{
    private readonly Dictionary<string, (int open, int close)> _hours =
        new Dictionary<string, (int, int)>
    {
        { "Bank",   (9, 18) },
        { "Casino", (22, 7) }, 
        { "Home",   (0,  24) },
        { "Work",   (0,  24) },
        { "Birzha",   (0,  24) },
        { "CryptoLab",   (0,  24) },
    };

    public bool IsOpen(string loc, int hour)
    {
        var (open, close) = _hours[loc];
        if (open < close)
            return hour >= open && hour < close;
        else 
            return hour >= open || hour < close;
    }

    public int GetOpenHour(string loc) => _hours[loc].open;
    public int GetCloseHour(string loc) => _hours[loc].close;
}
