using Zenject;
using UnityEngine;

public class PlayerLocationManager : IInitializable
{
    public string CurrentLocation { get; private set; }

    public void Initialize()
    {
        if (!PlayerPrefs.HasKey("CurrentLocation"))
        {
            PlayerPrefs.SetString("CurrentLocation", "Home");
        }
        CurrentLocation = PlayerPrefs.GetString("CurrentLocation");
        Debug.Log($"[PlayerLocationManager] Initialized. Current location: {CurrentLocation}");
    }

    public void SetLocation(string newLocation)
    {
        CurrentLocation = newLocation;
        PlayerPrefs.SetString("CurrentLocation", newLocation);
        PlayerPrefs.Save();
    }
}
