//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;
using System.Threading.Tasks;
using System.Globalization;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

/// <summary>
/// SpeechRecognition class lets the user use Speech-to-Text to convert spoken words
/// into text strings. There is an optional mode that can be enabled to also translate
/// the text after the recognition returns results. Both modes also support interim
/// results (i.e. recognition hypotheses) that are returned in near real-time as the 
/// speaks in the microphone.
/// </summary>
public class SpeechRecorder : MonoBehaviour
{
    // Public fields in the Unity inspector
    [Tooltip("Unity UI Text component used to report potential errors on screen.")]
    public Text RecognizedText;
    [Tooltip("Unity UI Text component used to post recognition results on screen.")]
    public Text ErrorText;

    //The Speechmanager needs the ChatGPtConnector Object
    public GPTInterface chatGPTConnector;
    public AiController aiController;

    // Words that activate the Tool
    public string acitvatedWordTool1 = "Hey";
    public string acitvatedWordTool2 = "Hey Tool";

    // Used to show live messages on screen, must be locked to avoid threading deadlocks since
    // the recognition events are raised in a separate thread
    private string recognizedString = "";
    private string errorString = "";
    private System.Object threadLocker = new System.Object();

    // Speech recognition key, required
    [Tooltip("Connection string to Cognitive Services Speech.")]
    public string SpeechServiceAPIKey = string.Empty;
    [Tooltip("Region for your Cognitive Services Speech instance (must match the key).")]
    public string SpeechServiceRegion = "westus";

    // Cognitive Services Speech objects used for Speech Recognition
    private SpeechRecognizer recognizer;
    private TranslationRecognizer translator;
    // The current language of origin is locked to English-US in this sample. Change this
    // to another region & language code to use a different origin language.
    // e.g. fr-fr, es-es, etc.
    string fromLanguage = "de-de";

    private bool micPermissionGranted = true; //kann man wegmachen



    /// <summary>
    /// Attach to button component used to launch continuous recognition (with or without translation)
    /// </summary>
    public void StartContinuous()
    {
        errorString = "";
        if (micPermissionGranted)
        {
            StartContinuousRecognition();
        }
        else
        {
            recognizedString = "This app cannot function without access to the microphone.";
            errorString = "ERROR: Microphone access denied.";
            Debug.LogError(errorString);
        }
    }



    /// <summary>
    /// Creates a class-level Speech Recognizer for a specific language using Azure credentials
    /// and hooks-up lifecycle & recognition events
    /// </summary>
    void CreateSpeechRecognizer()
    {
        if (SpeechServiceAPIKey.Length == 0 || SpeechServiceAPIKey == "YourSubscriptionKey")
        {
            recognizedString = "You forgot to obtain Cognitive Services Speech credentials and inserting them in this app." + Environment.NewLine +
                               "See the README file and/or the instructions in the Awake() function for more info before proceeding.";
            errorString = "ERROR: Missing service credentials";
            Debug.LogError(errorString);
            return;
        }
        Debug.Log("Creating Speech Recognizer.");
        recognizedString = "Initializing speech recognition, please wait...";

        if (recognizer == null)
        {
            SpeechConfig config = SpeechConfig.FromSubscription(SpeechServiceAPIKey, SpeechServiceRegion);
            config.SpeechRecognitionLanguage = fromLanguage;
            recognizer = new SpeechRecognizer(config);

            if (recognizer != null)
            {
                // Subscribes to speech events.
                recognizer.Recognizing += RecognizingHandler;
                recognizer.Recognized += RecognizedHandler;
                recognizer.SpeechStartDetected += SpeechStartDetectedHandler;
                recognizer.SpeechEndDetected += SpeechEndDetectedHandler;
                recognizer.Canceled += CanceledHandler;
                recognizer.SessionStarted += SessionStartedHandler;
                recognizer.SessionStopped += SessionStoppedHandler;
            }
        }
        Debug.Log("CreateSpeechRecognizer exit");
    }

    /// <summary>
    /// Initiate continuous speech recognition from the default microphone.
    /// </summary>
    private async void StartContinuousRecognition()
    {
        Debug.Log("Starting Continuous Speech Recognition.");
        CreateSpeechRecognizer();

        if (recognizer != null)
        {
            Debug.Log("Starting Speech Recognizer.");
            await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);


            recognizedString = "Speech Recognizer is now running.";
            Debug.Log("Speech Recognizer is now running.");



        }
        Debug.Log("Start Continuous Speech Recognition exit");






    }

    #region Speech Recognition event handlers
    private void SessionStartedHandler(object sender, SessionEventArgs e)
    {
        Debug.Log($"\n    Session started event. Event: {e.ToString()}.");
    }

    private void SessionStoppedHandler(object sender, SessionEventArgs e)
    {
        Debug.Log($"\n    Session event. Event: {e.ToString()}.");
        Debug.Log($"Session Stop detected. Stop the recognition.");
    }

    private void SpeechStartDetectedHandler(object sender, RecognitionEventArgs e)
    {
        Debug.Log($"SpeechStartDetected received: offset: {e.Offset}.");
    }

    private void SpeechEndDetectedHandler(object sender, RecognitionEventArgs e)
    {
        Debug.Log($"SpeechEndDetected received: offset: {e.Offset}.");
        Debug.Log($"Speech end detected.");
    }

    // "Recognizing" events are fired every time we receive interim results during recognition (i.e. hypotheses)
    private void RecognizingHandler(object sender, SpeechRecognitionEventArgs e)
    {
        if (e.Result.Reason == ResultReason.RecognizingSpeech)
        {
            Debug.Log($"HYPOTHESIS: Text={e.Result.Text}");
            lock (threadLocker)
            {
                recognizedString = $"HYPOTHESIS: {Environment.NewLine}{e.Result.Text}";
            }
        }
    }

    // "Recognized" events are fired when the utterance end was detected by the server
    private void RecognizedHandler(object sender, SpeechRecognitionEventArgs e)
    {
        if (e.Result.Reason == ResultReason.RecognizedSpeech)
        {
            Debug.Log($"RECOGNIZED: Text={e.Result.Text}");

            if (e.Result.Text.StartsWith(acitvatedWordTool1, true, new CultureInfo("en-US")) || e.Result.Text.StartsWith(acitvatedWordTool2, true, new CultureInfo("en-US")))
            {

                if (!e.Result.Text.Equals("Hey.", StringComparison.OrdinalIgnoreCase) && !e.Result.Text.Equals("Hey Tool.", StringComparison.OrdinalIgnoreCase))
                {
                    aiController.aiIsRunnung = true;
                    chatGPTConnector.GetResponse(e.Result.Text);
                }
            }

            lock (threadLocker)
            {
                recognizedString = $"RESULT: {Environment.NewLine}{e.Result.Text}";
            }
        }
        else if (e.Result.Reason == ResultReason.NoMatch)
        {
            Debug.Log($"NOMATCH: Speech could not be recognized.");
        }
    }

    // "Canceled" events are fired if the server encounters some kind of error.
    // This is often caused by invalid subscription credentials.
    private void CanceledHandler(object sender, SpeechRecognitionCanceledEventArgs e)
    {
        Debug.Log($"CANCELED: Reason={e.Reason}");

        errorString = e.ToString();
        if (e.Reason == CancellationReason.Error)
        {
            Debug.LogError($"CANCELED: ErrorDetails={e.ErrorDetails}");
            Debug.LogError("CANCELED: Did you update the subscription info?");
        }
    }
    #endregion


    void OnEnable()
    {
        StartContinuous();
    }

    void OnDisable()
    {
        StopRecognition();
    }

    /// <summary>
    /// Stops the recognition on the speech recognizer or translator as applicable.
    /// Important: Unhook all events & clean-up resources.
    /// </summary>
    public async void StopRecognition()
    {
        if (recognizer != null)
        {
            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
            recognizer.Recognizing -= RecognizingHandler;
            recognizer.Recognized -= RecognizedHandler;
            recognizer.SpeechStartDetected -= SpeechStartDetectedHandler;
            recognizer.SpeechEndDetected -= SpeechEndDetectedHandler;
            recognizer.Canceled -= CanceledHandler;
            recognizer.SessionStarted -= SessionStartedHandler;
            recognizer.SessionStopped -= SessionStoppedHandler;
            recognizer.Dispose();
            recognizer = null;
            recognizedString = "Speech Recognizer is now stopped.";
            Debug.Log("Speech Recognizer is now stopped.");
        }
    }
}
