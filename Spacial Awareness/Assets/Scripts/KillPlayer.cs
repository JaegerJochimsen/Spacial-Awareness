using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class KillPlayer : MonoBehaviour
{
    //vars for flash when hit
    [SerializeField] FlashImage _flashImage = null;


    // Oxigen/Health Level
    public int maxHealth = 100;
    public float currentHealth;
    public GameObject player;
    public HealthBar healthBar;
    public float healthDecayCoef;
    // End health Variables
    //private float timeStart = 0;

    [Header("PostProcessingVars")]
    public PostProcessVolume postFXVol; 
    public float init_saturation;
    // End Post Process Vars

    // Start is called before the first frame update
    void Start()
    {
        // Health start
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        SetColorGrading(init_saturation);
    }

    // Update is called once per frame
    void Update()
    {
        // Change health over time
        //TakeDamage(healthDecayCoef * Time.deltaTime);

        // Kill the player when health is 0
        float health = healthBar.getHealth();
        if (health < 1f)
        {
            // Call the EndGame method to end the game when health is out

            FindObjectOfType<GameManager>().EndGame();
        }


    }

    /* TakeDamage():
 * :description: apply damage to player O2 level. Update O2/Health bar.
 * :param: float damage: the amount to reduce player's health by.
 * :dependency: n/a
 * 
 * :calls: n/a
 * :called by: Update(), JetPack().
 * 
 * :Credit: Jared Knofczynski
 */
    public void TakeDamage(float damage)
    {
        MovePlayer playerBod = player.GetComponent<MovePlayer>();


        // if we're shielding, negate damage and consume charge (disregard if the damage we are taking is due to jet-pack usage)
        if (playerBod.shielding && !playerBod.flying)
        {
            playerBod.ReduceShieldCharge(1);
            return;
        }
        else
        {
            currentHealth -= damage;
        }

        if (!playerBod.flying) {
        _flashImage.StartFlash(.25f, 1f, Color.red);

        // Update player visual based on O2 levels (want value to be -100 when player health is 0)
        // Also we don't want to wash out the visuals if we use the jet pack, only when we take other damage
        SetColorGrading(currentHealth - 100f + init_saturation);
        }

        // Update player health
        healthBar.SetHealth((int)currentHealth);
    }

    /* SetColorGrading():
     * :description: set's the post processing saturation based on player O2 levels.
     * :param: the saturation level as a float [-100,100]
     * 
     * :calls: N/a
     * :called by: Update()
     */
    void SetColorGrading(float saturation)
    { 
        if (postFXVol.profile.TryGetSettings(out ColorGrading colGrad))
        {
            colGrad.saturation.value = saturation;
        }
    }
}

