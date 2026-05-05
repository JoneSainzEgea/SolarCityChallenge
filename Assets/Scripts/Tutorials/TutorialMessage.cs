using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMessage : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text messageTextUI;
    [SerializeField] private float messageDuration = 5f;

    private System.Action onTutorialCompleted;
    private bool waitForCompletion;

    public CanvasGroup CanvasGroup { get; private set; }

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        CanvasGroup.alpha = 0f;
    }

    public void Initialize(Tutorial tutorial, System.Action callback = null)
    {
        messageTextUI.text = tutorial.tutorialText;
        waitForCompletion = tutorial.waitForCompletion;

        onTutorialCompleted = callback;

        if (!waitForCompletion)
            StartCoroutine(WaitAndDeactivate());
    }

    private IEnumerator WaitAndDeactivate()
    {
        float elapsedTime = 0f;
        while (elapsedTime < messageDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        onTutorialCompleted?.Invoke();
    }
}
