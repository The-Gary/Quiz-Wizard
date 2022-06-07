using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField]
    TextMeshProUGUI questionText;

    [SerializeField]
    QuestionSO question;

    [Header("Answers")]
    [SerializeField]
    GameObject[] answerButtons;

    int correctAnswerIndex;
    bool hasAnsweredEarly;

    [Header("Button Colors")]
    [SerializeField]
    Sprite defaultAnswerSprite;

    [SerializeField]
    Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField]
    Image timerImage;

    Timer timer;

    void Start()
    {
        timer = FindObjectOfType<Timer>();
        GetNextQuestion();
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if (timer.loadNextQuestion)
        {
            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int idx)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(idx);
        SetButtonState(false);
        timer.CancelTimer();
    }

    void DisplayAnswer(int idx)
    {
        int correctAnswerIndex = question.GetCorrectAnswerIndex();
        Image buttonImage;
        if (idx == correctAnswerIndex)
        {
            questionText.text = "Correct!";
            buttonImage = answerButtons[idx].GetComponent<Image>();
        }
        else
        {
            questionText.text =
                "Incorrect! The correct answer is:\n" + question.GetAnswer(correctAnswerIndex);
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
        }
        buttonImage.sprite = correctAnswerSprite;
    }

    void GetNextQuestion()
    {
        SetButtonState(true);
        SetDefaultButtonSprites();
        DisplayQuestion();
    }

    void DisplayQuestion()
    {
        questionText.text = question.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = question.GetAnswer(i);
    }

    void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
            answerButtons[i].GetComponent<Button>().interactable = state;
    }

    void SetDefaultButtonSprites()
    {
        for (int i = 0; i < answerButtons.Length; i++)
            answerButtons[i].GetComponent<Image>().sprite = defaultAnswerSprite;
    }
}
