using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public Slider healthSlider;
    public AudioClip damageSFX;

    int currentHealth;

    void Start()
    {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 25 || transform.position.x < -25 ||
            transform.position.z > 25 || transform.position.z < -25)
        {
            PlayerDies();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damageAmount;
            healthSlider.value = currentHealth;
            AudioSource.PlayClipAtPoint(damageSFX, transform.position);
        }

        if(currentHealth <= 0)
        {
            PlayerDies();
        }
    }

    void PlayerDies()
    {
        transform.Rotate(-90, 0, 0, Space.Self);

        FindObjectOfType<LevelManager>().GameOver();
    }
}
