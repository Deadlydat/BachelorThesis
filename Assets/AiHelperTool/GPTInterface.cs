using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Events;

using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using System.Threading.Tasks;

public class GPTInterface : MonoBehaviour
{

    public TextToSpeech textToSpeech;

    public Button butt;

    private bool start;


    private OpenAIAPI api = new OpenAIAPI("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");


    public async void GetResponse(string question)
    {

        Debug.Log(question+"question");

        var result = await api.Completions.CreateCompletionAsync(new CompletionRequest(question + "\n", model: Model.DavinciText, max_tokens: 100));

        Debug.Log(result.ToString()+ "result");


        textToSpeech.speech = result.ToString();




        start = true;

    }



    private void Update()
    {

        if (start == true)
        {
            butt.onClick.Invoke();

            start = false;
        }
    }

}
