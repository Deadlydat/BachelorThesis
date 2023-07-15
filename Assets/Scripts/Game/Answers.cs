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
        public Answer[] answers = new Answer[10];

    }


    private int CalculateAnswerNumber(string path)
    {
        string pathh = "Assets/Answers";
        int fCount = Directory.GetFiles(pathh, "*.json", SearchOption.TopDirectoryOnly).Length;

        return fCount;
    }

    public void SaveItemInfo(AnswerList answerList)
    {
        string folderName = "MyGameFolder";
        string folderPath = System.IO.Path.Combine(Application.persistentDataPath, folderName);

        // Überprüfen, ob der Ordner bereits existiert, andernfalls erstellen
        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }

        int newFileNumber = CalculateAnswerNumber(folderPath);

        // Erstellen des Dateinamens und des vollständigen Dateipfads
        string filename = "AnswersFile" + newFileNumber + ".json";
        string filePath = System.IO.Path.Combine(folderPath, filename);// für build version





        Debug.Log("persistentDataPath: " + Application.persistentDataPath);




        string path = "Assets/Answers/AnswersFile" + newFileNumber + ".json";


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
