using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Seasons { Spring,Summer,Fall,Winter}
public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;
    
    [Header("Time And Day")]
    public int currentHour;
    public int currentDay;
    public bool am;
    [Header("Seasons")]
    public Seasons currentSeason;
    [Header("SFX")]
    public SoundScript SFX;

    //

    private void Awake()
    {
        Instance = this;
    }

    public void switchTime()
    {
        currentHour++;
        if (currentHour == 13)
        {
            am = !am;
            currentHour = 1;

            if (!am)
            {
                currentDay++;
            }
        }
        
    }

    // Day Time
    public void dayTime()
    {

    }

    //Night Time
    public void nightTime()
    {

    }
}
