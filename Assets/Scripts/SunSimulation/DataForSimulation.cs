using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UI.Dates;
using UnityEngine;

public static class DataForSimulation
{
    public static int Year = 2025;
    public static int Month = 5;
    public static int Day = 27;
    public static int Hour = 12;
    public static int Minutes = 0;

    public static void UpdateDate(int year, int month, int day)
    {
        Year = year;
        Month = month;
        Day = day;
    }

    public static void UpdateHour(int hour)
    {
        Hour = hour;
    }
    public static void UpdateMinutes(int minutes)
    {
        Minutes = (minutes % 5) < 3 ? minutes - (minutes % 5) : minutes + (5 - (minutes % 5));
    }

}
