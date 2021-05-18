using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class hitFlash : MonoBehaviour
{
    public bool wantFlashOut = false;
    Image _image = null;
    Coroutine _currentFlashRoutine = null;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void StartFlash(float secondsForOneFlash, float maxAlpha, Color newColor)
    {
        // NOTICE: uncomment this to change color, though not sure where the color newColor gets assigned??
        //_image.color = newColor;

        // ensure maxAlpha isn't above 1
        maxAlpha = Mathf.Clamp(maxAlpha, 0, 1);

        if (_currentFlashRoutine != null)
        {
            StopCoroutine(_currentFlashRoutine);
        }
        _currentFlashRoutine = StartCoroutine(Flash(secondsForOneFlash, maxAlpha));

    }

    IEnumerator Flash(float secondsForOneFlash, float maxAlpha)
    {
        // animate flash in
        float flashInDuration = secondsForOneFlash / 6;
        for (float t = 0; t <= flashInDuration; t += Time.deltaTime)
        {
            // create a new color change
            Color colorThisFrame = _image.color;
            colorThisFrame.a = Mathf.Lerp(0, maxAlpha, t / flashInDuration);
            // apply it
            _image.color = colorThisFrame;
            // wait until the next frame
            yield return null;
        }

        // TODO IF YOU WANT TO HAVE THE FLASH COLOR GO AWAY COMMENT OUT THE BELOW CODE
        // animate flash out
        if (wantFlashOut == true)
        {
            float flashOutDuration = secondsForOneFlash / 2;
            for (float t = 0; t <= flashOutDuration; t += Time.deltaTime)
            {
                Color colorThisFrame = _image.color;
                colorThisFrame.a = Mathf.Lerp(maxAlpha, 0, t / flashOutDuration);
                _image.color = colorThisFrame;
                yield return null;
            }

            // ensure alpha is set to 0
            _image.color = new Color32(0, 0, 0, 0); // color32 allows for alpha value
        }
    }
}
