using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFirstTime : MonoBehaviour
{
    private bool firstTime = true;

    public void triggerTutorials()
    {
        if (!firstTime)
            return;

        firstTime = false;

        EventsManager.TriggerNormalEvent("BuildingOpen");
    }
}
