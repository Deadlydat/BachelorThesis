using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Answers : MonoBehaviour
{

    [System.Serializable]
    public class Answer
    {
        public string question;
        public bool correctness;
        public int time;
        public bool tool; //ob mit oder ohne ai tool
        public int degree; // welche studienrichtung

    }



    [System.Serializable]
    public class AnswerList
    {
        public Answer[] answers = new Answer[5];

    }


    //public void WriteAnswersToFile(AnswerList answerList)
    //{

    //    string fileName = "bla";
    //    string json = JsonUtility.ToJson(answerList);
    //    string path = GetFilePath(fileName);
    //    FileStream fileStream = new FileStream(path, FileMode.Create);

    //    using (StreamWriter writer = new StreamWriter(fileStream))
    //    {
    //        writer.Write(json);
    //    }

    //}
    //private string GetFilePath(string fileName)
    //{
    //    return Application.persistentDataPath + "/" + fileName;
    //}

    public void SaveItemInfo(AnswerList answerList)
    {


       string  path = "Assets/Answers/Answers.json";


        string str = JsonUtility.ToJson(answerList);
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(str);
            }
        }

        UnityEditor.AssetDatabase.Refresh();

    }


}
