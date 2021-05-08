using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraPan : MonoBehaviour
{

    // Get position of the player
    public Rigidbody body;

    // Get postition of the sphere we are following
    public Rigidbody follow;

    GameObject panCamera;
    GameObject realCamera;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        follow = GetComponent<Rigidbody>();

        panCamera = GameObject.Find("Camera/PanCamera");
        realCamera = GameObject.Find("Camera/VirtualCamera");
 
    } 

    // Update is called once per frame
    void Update()
    {

        follow.velocity = Vector3.zero;
        follow.AddForce(Physics.gravity * GetComponent<Rigidbody>().mass);

        // We have reached the player, so "merge" with him
        if (follow.position.y <= body.position.y)
        {

            panCamera.SetActive(false);
            realCamera.SetActive(true);

            Destroy(GameObject.Find("Camera/Follow"));

        }
    }
}
