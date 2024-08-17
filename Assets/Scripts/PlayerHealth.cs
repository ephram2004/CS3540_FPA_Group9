using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public Slider healthSlider;
    public AudioClip damageSFX;
    public float invulnerableTime;

    int currentHealth;
    float invulnerableTimer;
    bool canGetHurt;

    void Start()
    {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
        canGetHurt = true;
    }

    // Update is called once per frame
    void Update()
    {
        updateTimers();
    }

    public void TakeDamage(int damageAmount)
    {
        if(currentHealth > 0 && canGetHurt)
        {
            invulnerableTimer = invulnerableTime;
            currentHealth -= damageAmount;
            healthSlider.value = currentHealth;
            AudioSource.PlayClipAtPoint(damageSFX, transform.position);
            canGetHurt = false;
        }

        if(currentHealth <= 0)
        {
            PlayerDies();
        }
    }

    private void updateTimers()
    {
        invulnerableTimer -= Time.deltaTime;
        if (invulnerableTimer <= 0)
        {
            canGetHurt = true;
        }
    }

    void PlayerDies()
    {
        transform.Rotate(-90, 0, 0, Space.Self);
        Debug.Log("died");

        FindObjectOfType<LevelManager>().GameOver();
    }
}
