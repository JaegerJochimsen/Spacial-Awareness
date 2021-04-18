using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    // Oxigen/Health Level
    public int maxHealth = 100;
    public float currentHealth;

    public HealthBar healthBar;
    public float healthDecayCoef;
    // End health Variables
    private float timeStart = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Health start
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        // Change health over time
        TakeDamage(healthDecayCoef * Time.deltaTime);

        // Kill the player when health is 0
        float health = healthBar.getHealth();
        if (health < 1f)
        {
            // Call the EndGame method to end the game when health is out
            FindObjectOfType<GameManager>().EndGame();
        }
        // TODO add a check for if the player fell off the map and call EndGame
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
        currentHealth -= damage;

        healthBar.SetHealth((int)currentHealth);
    }
}
