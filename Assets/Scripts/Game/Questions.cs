using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Questions : MonoBehaviour
{

    public TextAsset questionsJSON;
    [System.Serializable]
    public class Question
    {
        public string question;

        public string answerA;
        public bool boolA;

        public string answerB;
        public bool boolB;


        public string answerC;
        public bool boolC;


        public string answerD;
        public bool boolD;

    }

    [System.Serializable]
    public class QuestionList
    {
        public Question[] questions;

    }



    public QuestionList mQuestionList = new QuestionList();

    // Start is called before the first frame update
    void Start()
    {

        mQuestionList = JsonUtility.FromJson<QuestionList>(questionsJSON.text);
        



    }

}
