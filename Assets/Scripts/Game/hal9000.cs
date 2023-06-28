using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class hal9000 : MonoBehaviour
{
    // Start is called before the first frame update
    public VideoPlayer player;
    private bool start;
    public AiController controller;
    private void Start()
    {
        player.Prepare();
    }




    // Update is called once per frame
    void Update()
    {

       start= controller.aiIsRunnung;

      

        if (start==true)
        {

            player.Play();
        }
        else
        {
            player.Pause();
            player.Prepare();
        }
    }
}
