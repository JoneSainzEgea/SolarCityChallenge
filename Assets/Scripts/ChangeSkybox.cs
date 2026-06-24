// Jone Sainz Egea
// 17/04/2026

using System.Collections;
using UnityEngine;

public class ChangeSkybox : MonoBehaviour
{
    [SerializeField] private Material skyboxDay;
    [SerializeField] private Material skyboxNight;
    [SerializeField] private float transitionDuration = 2f;

    private bool isDay = true;
    private Coroutine transitionCoroutine;

    public void OnChangeSkybox(bool toDay)
    {
        if (toDay == isDay)
            return;
        
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        if (isDay)
            transitionCoroutine = StartCoroutine(FadeSkybox(skyboxNight));
        else
            transitionCoroutine = StartCoroutine(FadeSkybox(skyboxDay));

        isDay = !isDay;
    }

    IEnumerator FadeSkybox(Material newSkybox)
    {
        float t = 0;

        while (t < transitionDuration/2)
        {
            RenderSettings.skybox.SetFloat("_Exposure", 1 - t);
            t += Time.deltaTime;
            yield return null;
        }

        RenderSettings.skybox = newSkybox;

        t = 0;
        while (t < transitionDuration/2)
        {
            RenderSettings.skybox.SetFloat("_Exposure", t);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
