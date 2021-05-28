using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class MoveScene : MonoBehaviour
{
    [SerializeField] FlashImage _flashImage = null;
    public string nextScene;
    public PostProcessVolume volume;
    public float changeValue;

    //void OnCollisionEnter(Collision other)
    void OnTriggerEnter(Collider other)
    {
        LensDistortion lensDist;
        volume.profile.TryGetSettings(out lensDist);
        while(lensDist.intensity > -100f)
        {
            lensDist.intensity.value -= changeValue*Time.deltaTime;
            //volume.profile.settings
        }


        _flashImage.StartFlash(.25f, 1f, Color.white);
        if (other.gameObject.CompareTag("Player")) 
        {
            Invoke("NextLevel", 1.0f);  
        }
    }


    void NextLevel()
    {
        SceneManager.LoadScene(nextScene);
    }
}


