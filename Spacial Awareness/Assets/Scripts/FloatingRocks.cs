using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//public class Random;

public class FloatingRocks : MonoBehaviour
{

    public Rigidbody r1;
    public Rigidbody r2;
    public Rigidbody r3;
    public Rigidbody r4;

    private int frames = 0;



    // Apply random force to each rock to simulate z g
    void Update()
    {
        //public class Random;

        // Every 30 frames reverse direction so rocks dont get stuck
        if (frames == 120)
        {
            r1.velocity *= -1;
            r2.velocity *= -1;
            r3.velocity *= -1;
            r4.velocity *= -1;
            frames = 0;
        }
        frames ++;

        r1.AddForce(new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3)));
        r2.AddForce(new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3)));
        r3.AddForce(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)));
        r4.AddForce(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)));
    }
}
