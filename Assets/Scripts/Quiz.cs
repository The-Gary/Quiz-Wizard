using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;

    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly;

    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;

    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;

    public bool isComplete;

    void Start()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;   
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
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";

        if (progressBar.value == progressBar.maxValue)
            isComplete = true;
    }

    void DisplayAnswer(int idx)
    {
        int correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
        Image buttonImage;
        if (idx == correctAnswerIndex)
        {
            questionText.text = "Correct!";
            buttonImage = answerButtons[idx].GetComponent<Image>();
            scoreKeeper.IncrementCorrectAnswer();
        }
        else
        {
            questionText.text =
                "Incorrect! The correct answer is:\n"
                + currentQuestion.GetAnswer(correctAnswerIndex);
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
        }
        buttonImage.sprite = correctAnswerSprite;
    }

    void GetNextQuestion()
    {
        if (questions.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            ++progressBar.value;
            scoreKeeper.IncrementQuestionsSeen();
        }
    }

    void GetRandomQuestion()
    {
        int idx = Random.Range(0, questions.Count);
        currentQuestion = questions[idx];
        if (questions.Contains(currentQuestion))
            questions.Remove(currentQuestion);
    }

    void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text =
                currentQuestion.GetAnswer(i);
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
