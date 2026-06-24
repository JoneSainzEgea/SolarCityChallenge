// Jone Sainz Egea
// 05/05/2026
using UnityEngine;

[System.Serializable]
public class Tutorial
{
    [Tooltip("Nombre del tutorial.")]
    [TextArea]
    public string tutorialName;

    [Tooltip("Texto del tutorial que se mostrará.")]
    [TextArea]
    public string tutorialText;

    [Tooltip("¿Este tutorial se activa por evento externo?")]
    public bool triggeredByAction;

    [Tooltip("Nombre del evento que activa este tutorial si triggeredByAction es true.")]
    [TextArea]
    public string activationEventName;

    [Tooltip("¿Debe esperar una confirmación externa para completar el tutorial?")]
    public bool waitForCompletion;

    [Tooltip("Nombre del evento que indica que el jugador completó la acción requerida.")]
    [TextArea]
    public string completionEventName;
}
