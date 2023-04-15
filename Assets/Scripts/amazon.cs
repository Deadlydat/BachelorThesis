using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class amazon : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public float range;
    public float height;


    public float speedR;
    public float angleR;
    public float rangeR;



    // Update is called once per frame
    void Update()
    {
        float y = Mathf.PingPong(Time.time * speed, 1) * range + height;

        float r = Mathf.PingPong(Time.time * speedR, 1) * angleR + rangeR;

        transform.position = new Vector3(0, y, 13.35f);
        transform.rotation = Quaternion.Euler(new Vector3(0, r, 0));

    }
}
