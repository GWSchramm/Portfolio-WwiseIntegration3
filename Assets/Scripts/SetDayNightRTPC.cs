using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDayNightRTPC : MonoBehaviour
{

    public AK.Wwise.RTPC TimeOfDayRTPC;

    public SimpleDayNightCycle dayNightCycle;

    // Update is called once per frame
    void Update()
    {
        TimeOfDayRTPC.SetGlobalValue(dayNightCycle.timeOfDay);
    }
}
