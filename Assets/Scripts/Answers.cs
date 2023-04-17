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
        public float time;
        public bool tool; //ob mit oder ohne ai tool
        public int degree; // welche studienrichtung
    }

    [System.Serializable]
    public class AnswerList
    {
        public Answer[] answers;

    }

    public void WriteAnswersToFile(AnswerList answerList)
    {

        string fileName = "bla";
        string json = JsonUtility.ToJson(answerList);
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }

    }
    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }




}
