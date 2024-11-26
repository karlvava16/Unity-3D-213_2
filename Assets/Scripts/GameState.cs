using System;
using System.Collections.Generic;

public class GameState
{
    public static bool isFpv { get; set; }
    public static bool isNight { get; set; }
    public static float flashCharge { get; set; }
    public static Dictionary<string, bool> collectedKeys { get; } = new Dictionary<string, bool>();

    #region effectsVolume

    private static float _effectsVolume = 1.0f;

    public static float effectsVolume
    {
        get => _effectsVolume;
        set
        {
            if (_effectsVolume != value)
            {
                _effectsVolume = value;
                NotifySubscribers(nameof(effectsVolume));
            }
        }
    }

    #endregion

    #region ambientVolume

    private static float _ambientVolume = 1.0f;

    public static float ambientVolume
    {
        get => _ambientVolume;
        set
        {
            if (_ambientVolume != value)
            {
                _ambientVolume = value;
                NotifySubscribers(nameof(ambientVolume));
            }
        }
    }
    #endregion

    #region isMuted ( Mute All )
    private static bool _isMuted = false;
    public static bool isMuted
    {
        get => _isMuted;
        set
        {

            if (_isMuted != value)

                _isMuted = value;
            NotifySubscribers(nameof(isMuted));

        }
    }
    #endregion


    #region ChangeNotifier

    private static readonly Dictionary<string, List<Action>> subscribers =
        new Dictionary<string, List<Action>>();

    private static void NotifySubscribers(String propertyName)
    {
        if (subscribers.ContainsKey(propertyName))
        {
            foreach (var action in subscribers[propertyName])
            {
                action();
            }
        }
    }
    public static void Subscribe(string propertyName, Action action)
    {
        if (!subscribers.ContainsKey(propertyName))
        {
            subscribers[propertyName] = new List<Action>();
        }
        subscribers[propertyName].Add(action);
    }
    public static void UnSubscribe(string propertyName, Action action)
    {
        if (!subscribers.ContainsKey(propertyName))
        {
            subscribers[propertyName].Remove(action);

        }
    }
    #endregion



}