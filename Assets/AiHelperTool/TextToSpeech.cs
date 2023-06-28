
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
//using UnityEngine.UnityMainThreadDispatcher;
using Debug = UnityEngine.Debug;
using Newtonsoft.Json.Linq;

public class TextToSpeech : MonoBehaviour
{

    public AudioSource audioSource;
    public AiController aiController;

    // Replace with your own subscription key and service region (e.g., "westus").
    private const string SubscriptionKey = "82edbc66833b4157938a7ae24905a2a6";
    private const string Region = "westeurope";

    private const int SampleRate = 24000;

    private object threadLocker = new object();
    private bool waitingForSpeak;
    private bool audioSourceNeedStop;
    private string message;

    private SpeechConfig speechConfig;
    private SpeechSynthesizer synthesizer;

    private static readonly string synthesisLanguage = "de-de";
    private static readonly string snythesisVoiceName = "de-DE-RalfNeural";

    public string speech = "";


    //de-DE-AmalaNeural(Female)
    //de-DE-BerndNeural(Male)
    //de-DE-ChristophNeural(Male)
    //de-DE-ConradNeural1(Male)
    //de-DE-ElkeNeural(Female)
    //de-DE-GiselaNeural(Female, Child)
    //de-DE-KasperNeural(Male)
    //de-DE-KatjaNeural(Female)
    //de-DE-KillianNeural(Male)
    //de-DE-KlarissaNeural(Female)
    //de-DE-KlausNeural(Male)
    //de-DE-LouisaNeural(Female)
    //de-DE-MajaNeural(Female)
    //de-DE-RalfNeural(Male)
    //de-DE-TanjaNeural(Female)


    public void StartSpeech()
    {
        lock (threadLocker)
        {
            waitingForSpeak = true;
        }

        //string newMessage = null;
        //var startTime = DateTime.Now;

        // Starts speech synthesis, and returns once the synthesis is started.
        using (var result = synthesizer.StartSpeakingTextAsync(speech).Result)
        {
            // Native playback is not supported on Unity yet (currently only supported on Windows/Linux Desktop).
            // Use the Unity API to play audio here as a short term solution.
            // Native playback support will be added in the future release.
            var audioDataStream = AudioDataStream.FromResult(result);

            //var isFirstAudioChunk = true;
            var audioClip = AudioClip.Create(
                "Speech",
                SampleRate * 600, // Can speak 10mins audio as maximum
                1,
                SampleRate,
                true,
                (float[] audioChunk) =>
                {
                    var chunkSize = audioChunk.Length;
                    var audioChunkBytes = new byte[chunkSize * 2];
                    var readBytes = audioDataStream.ReadData(audioChunkBytes);
                  
                    for (int i = 0; i < chunkSize; ++i)
                    {
                        if (i < readBytes / 2)
                        {
                            audioChunk[i] = (short)(audioChunkBytes[i * 2 + 1] << 8 | audioChunkBytes[i * 2]) / 32768.0F;
                        }
                        else
                        {
                            audioChunk[i] = 0.0f;

                            //stops the animation
                            if (aiController.aiIsRunnung == true)
                            {
                                aiController.aiIsRunnung = false;

                            }
                        }
                    }
                    if (readBytes == 0)
                    {
                        Thread.Sleep(200); // Leave some time for the audioSource to finish playback
                        audioSourceNeedStop = true;
                    }
                });
       
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        lock (threadLocker)
        {
            waitingForSpeak = false;
        }
    }


    void Start()
    {


        // Creates an instance of a speech config with specified subscription key and service region.
        speechConfig = SpeechConfig.FromSubscription(SubscriptionKey, Region);

        speechConfig.SpeechSynthesisLanguage = synthesisLanguage;

        speechConfig.SpeechSynthesisVoiceName = snythesisVoiceName;

        // The default format is RIFF, which has a riff header.
        // We are playing the audio in memory as audio clip, which doesn't require riff header.
        // So we need to set the format to raw (24KHz for better quality).
        speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw24Khz16BitMonoPcm);

        // Creates a speech synthesizer.
        // Make sure to dispose the synthesizer after use!
        synthesizer = new SpeechSynthesizer(speechConfig, null);

        synthesizer.SynthesisCanceled += (s, e) =>
        {
            var cancellation = SpeechSynthesisCancellationDetails.FromResult(e.Result);
            message = $"CANCELED:\nReason=[{cancellation.Reason}]\nErrorDetails=[{cancellation.ErrorDetails}]\nDid you update the subscription info?";
        };

    }

    void Update()
    {
        lock (threadLocker)
        {

            if (audioSourceNeedStop)
            {
                audioSource.Stop();
                audioSourceNeedStop = false;
            }
        }
    }

    void OnDestroy()
    {
        if (synthesizer != null)
        {
            synthesizer.Dispose();
        }
    }
}


