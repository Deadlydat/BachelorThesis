using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Questions;

public class TabletButtonController : MonoBehaviour
{
    private Text Header;
    private Text TextButtonA;
    private Text TextButtonB;
    private Text TextButtonC;
    private Text TextButtonD;

    public Questions questionsScript;
    public GameObject startObject;
    public GameObject uiTablet;

    private QuestionList questionsList;
    private int questionNumber = 0;


    void Start()
    {
        Header = GameObject.Find("HeaderUi").GetComponentInChildren<Text>();
        TextButtonA = GameObject.Find("TextButtonA").GetComponentInChildren<Text>();
        TextButtonB = GameObject.Find("TextButtonB").GetComponentInChildren<Text>();
        TextButtonC = GameObject.Find("TextButtonC").GetComponentInChildren<Text>();
        TextButtonD = GameObject.Find("TextButtonD").GetComponentInChildren<Text>();

        questionsList = questionsScript.mQuestionList;
        SetQuestion(questionNumber);
        questionNumber++;
    }



    void OnEnable()
    {
        //darf das aller erstemal nicht gecalled werden
        SetQuestion(questionNumber);
        questionNumber++;
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

    private void SetNewQuestion()
    {
        if (questionNumber < questionsList.questions.Length)
        {
            SetQuestion(questionNumber);
            questionNumber++;
        }
        else
        {
            questionNumber = 0;
            startObject.SetActive(true);

            uiTablet.SetActive(false);
        }

    }


    public void TaskOnClickA()
    {
        Debug.Log("You have clicked the button a!");
        SetNewQuestion();

    }
    public void TaskOnClickB()
    {
        Debug.Log("You have clicked the button b!");
        SetNewQuestion();
    }
    public void TaskOnClickC()
    {
        Debug.Log("You have clicked the button c!");
        SetNewQuestion();
    }
    public void TaskOnClickD()
    {
        Debug.Log("You have clicked the button d!");
        SetNewQuestion();


    }
}
