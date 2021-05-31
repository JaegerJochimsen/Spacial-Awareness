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

    public PostProcessVolume postFXVol; // assign this to your post processing volume in the inspector
    public float init_saturation; // placeholder to cache the default settings so you can return to them

    // Used to kill the player if he falls off the world
    //MovePlayer player = GameObject.Find("Stylized Astronaut").GetComponent<MovePlayer>();

    // Start is called before the first frame update
    void Start()
    {
        // Health start
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        //postFXVol.profile.com
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

        // Update player visual
        SetColorGrading(currentHealth);
        // We don't need this code because the player can't fall off the map

        // Check for if the player fell off the map and call EndGame if he has
        //if (player.body.position.y < -1f)
        //{
        //  FindObjectOfType<GameManager>().EndGame();
        //}
    }

    /* TakeDamage():
 * :description: apply damage to player O2 level. Update O2/Health bar.
 * :param: float damage: the amount to reduce player's health by.
 * :dependency: n/a
 * 
 * :calls: n/a
 * :called by: Update(), JetPack().
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
        }

        // Update player health
        healthBar.SetHealth((int)currentHealth);
    }

    void SetColorGrading(float saturation)
    { // simple function to set the film grain intensity; call this from your Update loop or wherever you'd like
        if (postFXVol.TryGetComponent<ColorGrading>(out var colGrad))
        {
            colGrad.saturation.value = saturation;
        }
    }
}
