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

    public float maxSheild;
    public float currentSheild;
    public Slider sheildSlider;

    public float timeToStartRegen;
    public float timeToRegen;

    private float timeSinceDamage;

    private float colourChangeDelay = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = currentHealth;

        currentSheild = maxSheild;
        sheildSlider.maxValue = currentSheild;

        UpdateHealthBar();
        UpdateSheildBar();
    }

    private void Update()
    {
        timeSinceDamage += Time.deltaTime;

        if (timeSinceDamage >= timeToStartRegen && currentSheild < maxSheild)
        {
            StartCoroutine(ShieldRegen());
        }
    }
    private IEnumerator ShieldRegen()
    {
        float regenTime = 0f;
        float shieldValue = currentSheild;

        while (regenTime < timeToRegen && timeSinceDamage > timeToStartRegen)
        {
            regenTime *= 1.005f;
            regenTime += Time.deltaTime;
            float lerpTime = regenTime / timeToRegen;
            sheildSlider.value = Mathf.Lerp(shieldValue, maxSheild, regenTime);
            currentSheild = sheildSlider.value;
            yield return null;
        }
    }

    public void TakeDamage(float damageToTake)
    {
        if (currentSheild > 0)
        {
            currentSheild -= damageToTake;

            if (currentSheild < 0)
            {
                currentHealth += currentSheild;
            }
        }
        else
        {
            currentHealth -= damageToTake;

            if (currentHealth <= 0)
            {
                //Death stuff
                Debug.Log("You Died");
                SceneManager.LoadScene("LooseScene");


            }
        }
        timeSinceDamage = 0;

        UpdateHealthBar();
        UpdateSheildBar();
        StartCoroutine("ColourChangeWhenHit");

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
    private void UpdateSheildBar()
    {
        sheildSlider.value = currentSheild;
    }

   IEnumerator ColourChangeWhenHit()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
        yield return new WaitForSeconds(colourChangeDelay);
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }
}
