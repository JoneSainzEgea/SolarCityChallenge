using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private List<Tutorial> tutorials = new List<Tutorial>();

    private int currentIndex = 0;
    private TutorialMessage currentMessage;
    private bool triggered = false;

    private bool canceled = false;

    private void OnEnable()
    {
        foreach (var tutorial in tutorials)
        {
            if (tutorial.triggeredByAction)
                EventsManager.CallNormalEvents(tutorial.activationEventName, () => OnTriggeredByAction(tutorial));
        }
    }

    private void OnDisable()
    {
        foreach (var tutorial in tutorials)
        {
            if (tutorial.triggeredByAction)
                EventsManager.StopCallNormalEvents(tutorial.activationEventName, () => OnTriggeredByAction(tutorial));
        }
    }

    public void TriggerTutorial(string tutorialEventName)
    {
        EventsManager.TriggerNormalEvent(tutorialEventName);
    }


    private void OnTriggeredByAction(Tutorial tutorial)
    {
        if (canceled)
            return;

        if (triggered) return;

        if (tutorials[currentIndex] == tutorial)
        {
            triggered = true;
            DisplayTutorial(tutorial);
        }
    }

    private void ShowNextMessage()
    {
        if (canceled)
            return;

        // Debug.Log($"Showing next message at number {currentIndex}");
        if (currentMessage != null)
        {
            Debug.LogWarning("Ya hay un tutorial en marcha");
            return;
        }

        if (currentIndex >= tutorials.Count)
        {
            // Debug.Log("Todos los tutoriales de esta lista han sido completados");
            return;
        }

        Tutorial tutorial = tutorials[currentIndex];

        if (tutorial.triggeredByAction)
        {
            // Si el siguiente tutorial depende de un evento externo, dejamos que sea ese evento el que llame a DisplayTutorial
            triggered = false;
            return;
        }

        DisplayTutorial(tutorial);
    }


    private void DisplayTutorial(Tutorial tutorial)
    {
        if (canceled)
            return;

        currentMessage = TutorialManager.Instance.ShowMessage(tutorial);
        currentMessage.Initialize(tutorial, tutorial.waitForCompletion ? (System.Action)null : CompleteCurrentStep);
    }

    public void CompleteCurrentStep()
    {
        if (canceled)
            return;

        currentIndex++;
        StartCoroutine(TransitionToNextMessage());
    }


    private IEnumerator TransitionToNextMessage()
    {
        if (canceled)
            yield break;

        TutorialManager.Instance.RemoveMessage(currentMessage);
        yield return StartCoroutine(TutorialManager.Instance.FadeOutAndDestroy(currentMessage));
        currentMessage = null;

        ShowNextMessage();
    }

    public void HasBeenCanceled()
    {
        if (currentMessage != null)
        {
            TutorialManager.Instance.RemoveMessage(currentMessage);
        }
        canceled = true;
    }
}
