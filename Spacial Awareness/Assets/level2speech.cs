using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class level2speech : MonoBehaviour
{
    public Collider IGotOut;

    public Text myText;
    // Start is called before the first frame update
    void Start()
    {
        IGotOut = GameObject.Find("Speech/IGotOut").GetComponent<Collider>();

        myText = GameObject.Find("Canvas/Text").GetComponent<Text>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("IGotOut"))
        {
            myText.text = "Woo! I got out. Now where the heck am I?";
            Debug.Log("I AM OUT!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        myText.text = "";
    }
}
