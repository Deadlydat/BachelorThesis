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
    }



    [System.Serializable]
    public class AnswerList
    {
        public Answer[] answers = new Answer[4];

    }


    private int CalculateAnswerNumber()
    {
        string path = "Assets/Answers";
        int fCount = Directory.GetFiles(path, "*.json", SearchOption.TopDirectoryOnly).Length;

        return fCount;
    }

    public void SaveItemInfo(AnswerList answerList)
    {
     
        int newFileNumber = CalculateAnswerNumber();
  

       string  path = "Assets/Answers/AnswersFile" + newFileNumber+".json";


        string str = JsonUtility.ToJson(answerList);
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(str);
            }
        }

        //UnityEditor.AssetDatabase.Refresh();

    }


}
