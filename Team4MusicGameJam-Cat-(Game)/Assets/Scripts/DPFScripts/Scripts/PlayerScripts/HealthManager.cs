using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public Slider healthSlider;

    public float maxHealth;
    public float currentHealth;

    private float timeSinceDamage;

    [SerializeField] private float invinsibilityTime = 0.5f;

    private float colourChangeDelay = 0.1f;

    public GameObject deathCanvas;

    [SerializeField] GameObject shield;

    public AudioClip[] sounds;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = currentHealth;

        UpdateHealthBar();
    }

    private void Update()
    {
        timeSinceDamage += Time.deltaTime;

        if (timeSinceDamage > invinsibilityTime)
        {
            shield.SetActive(false);
        }
        else
        {
            shield.SetActive(true);
        }
    }


    public void TakeDamage(float damageToTake)
    {
        if (timeSinceDamage > invinsibilityTime)
        {
            currentHealth -= damageToTake;

            source.clip = sounds[Random.Range(0, sounds.Length)];
            source.Play();

            if (currentHealth <= 0)
            {
                Death();
            }
            else
            {
                timeSinceDamage = 0;

               
                StartCoroutine("ColourChangeWhenHit");
            }
            UpdateHealthBar();
        }
        

    }
    public void Death()
    {
        //Death stuff
        Debug.Log("You Died");
        deathCanvas.SetActive(true);
        gameObject.SetActive(false);
    }
    public void ReceiveHealth(float healthToReceive)
    {
        currentHealth += healthToReceive;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateHealthBar();

    }

    private void UpdateHealthBar()
    {
        healthSlider.value = currentHealth;
    }

   IEnumerator ColourChangeWhenHit()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
        yield return new WaitForSeconds(colourChangeDelay);
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }
}
