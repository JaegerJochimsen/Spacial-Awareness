using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    public Transform player;

    private void LateUpdate()
    {
        // find where we are going to move camera focus to
        Vector3 newPosition = player.position;

        // maintain zoomed out
        newPosition.z = transform.position.z;

        // set position
        transform.position = newPosition;
    }
}
