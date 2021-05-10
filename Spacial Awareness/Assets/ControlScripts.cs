using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlScripts : MonoBehaviour
{

    // Attach a box collider for each movement type
    // Each time the character collides put text on screen
    public Collider WSAD;
    public Collider Dash;
    public Collider Jetpack;
    public Collider Bubble;

    // UI object
    public Text myText;

    // Start is called before the first frame update
    void Start()
    {
        WSAD = GetComponent<Collider>();
        Dash = GetComponent<Collider>();
        Jetpack = GetComponent<Collider>();
        Bubble = GetComponent<Collider>();

        myText = GameObject.Find("Canvas/Text").GetComponent<Text>();
    }


    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Here");        

        if (col.gameObject.CompareTag("WSAD")) {
            myText.text = "Use WSAD to move and Space to jump.";
            //Debug.Log("WSAD");
        } 
        else if (col.gameObject.CompareTag("Dash"))
        {
            myText.text = "Use k to get a short dash forward";
            //Debug.Log("Dash");

        }
        else if (col.gameObject.CompareTag("Jetpack"))
        {
            myText.text = "Use j to activate your jetpack \nbut be careful it comes at \nthe expense of your oxygen";
            //Debug.Log("Jetpack");

        }
        /*else if (col.gameObject.CompareTag("Bubble"))
        {
            myText.text = "Use q to get a protection bubble.\n (This will be useful in later levels)";
            //Debug.Log("Bubble");

        }     */
    }

    void OnTriggerExit(Collider hit)
    {
        //Debug.Log("Exit");

        myText.text = "";
    }
}
