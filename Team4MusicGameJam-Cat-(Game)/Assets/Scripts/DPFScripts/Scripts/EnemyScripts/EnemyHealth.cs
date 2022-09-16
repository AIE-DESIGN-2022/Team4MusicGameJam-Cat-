using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;

    private Transform parent;

    private float colourChangeDelay = 0.1f;

    public bool boss;
    public AudioSource winSound;

    public AudioClip[] sounds;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TakeDamage(float damageToTake)
    {
        health -= damageToTake;
        // healthBar.fillAmount = health / maxHealth;

        source.clip = sounds[Random.Range(0, sounds.Length)];
        source.Play();

        if (health <= 0)
        {
            //They Dead
            onDeath();
        }

        StartCoroutine("ColourChangeWhenHit");
    }

    private void onDeath()
    {

        if (boss)
        {
            winSound.Play();
        }
        Destroy(gameObject);
    }
    IEnumerator ColourChangeWhenHit()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
        yield return new WaitForSeconds(colourChangeDelay);
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

}
