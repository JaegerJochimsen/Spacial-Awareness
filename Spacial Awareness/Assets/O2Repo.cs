using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O2Repo : MonoBehaviour
{
    [Header("O2 Repo Variables")]
    public float O2restoreVal;
    public float restoreDist;
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        // if we're within the restore distance units of O2 repo and we have yet to reach our max health
        if((player.position - transform.position).sqrMagnitude < restoreDist) {
            if (FindObjectOfType<KillPlayer>().currentHealth < FindObjectOfType<KillPlayer>().maxHealth)
            {
                // reducing health by a negative amount should restore it
                FindObjectOfType<KillPlayer>().TakeDamage(-O2restoreVal * Time.deltaTime);
            }
        }
    }
}
