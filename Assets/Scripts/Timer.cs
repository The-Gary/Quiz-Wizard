using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    float timeToComplete = 30f;

    [SerializeField]
    float timeToShowAnswer = 10f;
    float timerValue;

    public float fillFraction;
    public bool loadNextQuestion;
    public bool isAnsweringQuestion = false;

    void Update()
    {
        UpdateTimer();
    }

    public void CancelTimer()
    {
        timerValue = 0;
    }

    void UpdateTimer()
    {
        timerValue -= Time.deltaTime;

        if (timerValue > 0)
        {
            if (isAnsweringQuestion)
                fillFraction = timerValue / timeToComplete;
            else
                fillFraction = timerValue / timeToShowAnswer;
        }

        if (isAnsweringQuestion && timerValue <= 0)
        {
            isAnsweringQuestion = false;
            timerValue = timeToShowAnswer;
        }
        else if (timerValue <= 0)
        {
            isAnsweringQuestion = true;
            timerValue = timeToComplete;
            loadNextQuestion = true;
        }
    }
}
