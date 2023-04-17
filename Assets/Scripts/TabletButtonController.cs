using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Questions;
using static Answers;
using System.Linq;

public class TabletButtonController : MonoBehaviour
{
    private Text Header;
    private Text TextButtonA;
    private Text TextButtonB;
    private Text TextButtonC;
    private Text TextButtonD;

    public Questions questionsScript;
    public Answers answerScript;
    public GameObject startObject;
    public GameObject uiTablet;

    private QuestionList questionsList;
    private int questionNumber = 0;

    private AnswerList answerList ;


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

    }
    private void CreateAnswer(bool correctness)
    {
        
        Answer answer = new Answer();
        answer.question = questionsList.questions[questionNumber - 1].question;
        answer.correctness = correctness;
        answer.time = 1.0f;
        answer.tool = false;
        answer.degree = 0;
        
        answerList.answers.Append(answer);
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
            answerScript.WriteAnswersToFile(answerList);

            questionNumber = 0;
            startObject.SetActive(true);

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
