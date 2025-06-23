using UnityEngine;
using UniRx;
using Zenject;
using R3;

[RequireComponent(typeof(AudioSource))]
public class DayNightMusicController : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip dayTheme;
    [SerializeField] private AudioClip nightTheme;

    private AudioSource _source;
    private PlayerModel _playerModel;
    private enum Period { None, Day, Night }
    private Period _current = Period.None;

    private static bool _exists;

    [Inject]
    public void Construct(PlayerModel playerModel)
    {
        _playerModel = playerModel;
    }

    void Awake()
    {
        if (_exists)
        {
            Destroy(gameObject);
            return;
        }
        _exists = true;
        DontDestroyOnLoad(gameObject);

        _source = GetComponent<AudioSource>();
        _source.loop = true;
        _source.playOnAwake = false;
    }

    void Start()
    {
        _playerModel.Hours
            .Subscribe(UpdateTheme)
            .AddTo(this);

        UpdateTheme(_playerModel.Hours.Value);
    }

    private void UpdateTheme(int hour)
    {
        bool isDay = hour >= 7 && hour < 19;
        var period = isDay ? Period.Day : Period.Night;
        if (period == _current) return;

        _current = period;
        _source.clip = isDay ? dayTheme : nightTheme;
        _source.Play();
    }
}
