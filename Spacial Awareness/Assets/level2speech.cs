using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class level2speech : MonoBehaviour
{
    public Collider IGotOut;
    public Collider FindKey;
    public Collider MoreLava;

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
        else if (col.gameObject.CompareTag("FindKey"))
        {
            myText.text = "I'll come back with a key to open this hatch";
        }
        else if (col.gameObject.CompareTag("MoreLava"))
        {
            myText.text = "More Lava?! Hopefully thats an oxygen fillup on the other side";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        myText.text = "";
    }
}
