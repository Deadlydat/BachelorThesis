using UnityEngine;
using UnityEngine.UI;
using static Questions;
using static Answers;
using System.Diagnostics;
using System;

public class TabletButtonController : MonoBehaviour
{
    private Text Header;
    private Text TextButtonA;
    private Text TextButtonB;
    private Text TextButtonC;
    private Text TextButtonD;

    public Questions questionsScript;
    public Answers answerScript;
    public GameObject endeObject;
    public GameObject uiTablet;

    private QuestionList questionsList;
    private int questionNumber = 0;

    private AnswerList answerList;

    private Stopwatch stopWatch;


    void Start()
    {
        Header = GameObject.Find("HeaderUi").GetComponentInChildren<Text>();
        TextButtonA = GameObject.Find("TextButtonA").GetComponentInChildren<Text>();
        TextButtonB = GameObject.Find("TextButtonB").GetComponentInChildren<Text>();
        TextButtonC = GameObject.Find("TextButtonC").GetComponentInChildren<Text>();
        TextButtonD = GameObject.Find("TextButtonD").GetComponentInChildren<Text>();

        questionsList = questionsScript.mQuestionList;

        SetNewQuestion();

    }



    void OnEnable()
    {
        answerList = new AnswerList();
        //darf das aller erstemal nicht gecalled werden
        SetNewQuestion();

    }



    private void SetQuestion(int questionNumber)
    {

        Header.text = questionNumber + 1 + "/" + questionsList.questions.Length + " :  " +
            questionsList.questions[questionNumber].question;

        TextButtonA.text = questionsList.questions[questionNumber].answerA;
        TextButtonB.text = questionsList.questions[questionNumber].answerB;
        TextButtonC.text = questionsList.questions[questionNumber].answerC;
        TextButtonD.text = questionsList.questions[questionNumber].answerD;

        stopWatch = Stopwatch.StartNew();

    }
    private void CreateAnswer(bool correctness)
    {
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;

        Answer answer = new()
        {
            question = questionsList.questions[questionNumber - 1].question,
            correctness = correctness,
            time = ts.Seconds
        };

        answerList.answers[questionNumber - 1] = answer;

    }

    private void SetNewQuestion()
    {
        if (questionNumber < questionsList.questions.Length)

        {
            SetQuestion(questionNumber);
            questionNumber++;
        }
        else
        {
            answerScript.SaveItemInfo(answerList);

            questionNumber = 0;
            endeObject.SetActive(true);

            uiTablet.SetActive(false);
        }

    }


    public void TaskOnClickA()
    {
        //Debug.Log("You have clicked the button a!");


        CreateAnswer(questionsList.questions[questionNumber - 1].boolA);
        SetNewQuestion();

    }
    public void TaskOnClickB()
    {
        //Debug.Log("You have clicked the button b!");


        CreateAnswer(questionsList.questions[questionNumber - 1].boolB);
        SetNewQuestion();
    }
    public void TaskOnClickC()
    {
        //Debug.Log("You have clicked the button c!");


        CreateAnswer(questionsList.questions[questionNumber - 1].boolC);
        SetNewQuestion();
    }
    public void TaskOnClickD()
    {
        //Debug.Log("You have clicked the button d!");

        CreateAnswer(questionsList.questions[questionNumber - 1].boolD);
        SetNewQuestion();
    }
}
