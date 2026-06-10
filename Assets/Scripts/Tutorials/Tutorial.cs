// Jone Sainz Egea
// 05/05/2026
using UnityEngine;

public enum TutorialType { Gameplay, Learning, Tip}

[System.Serializable]
public class Tutorial
{
    [Tooltip("Nombre del tutorial.")]
    [TextArea]
    public string tutorialName;

    [Tooltip("Tipo de tutorial.")]
    public TutorialType tutorialType;

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
}
