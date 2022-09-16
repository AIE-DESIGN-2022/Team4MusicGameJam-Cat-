using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCharge : MonoBehaviour
{
    public ParticleSystem particleLauncher;
    public GameObject particle;
    public float chargeTime;
    private ParticleSystem.MainModule psMain;

    // Start is called before the first frame update
    void Start()
    {
        particle.SetActive(false);
        psMain = particleLauncher.main;
        ParticleSystem.ShapeModule psShape = particleLauncher.shape;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButton("Fire2"))
        {
            if (!transform.GetComponent<CombatManager>().isOnCooldown)
            {
                particle.SetActive(true);
            }
            else
            {
                particle.SetActive(false);
            }
            
        }
        if (Input.GetButtonDown("Fire2"))
        {
            psMain.startSpeed = 0;
            StartCoroutine(IncreaseSpeed());
        }
        if (Input.GetButtonUp("Fire2"))
        {
            particle.SetActive(false);
            psMain.startSpeed = 0;
            StopAllCoroutines();
        }
    }

    IEnumerator IncreaseSpeed()
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime / chargeTime;
            psMain.startSpeed = Mathf.Lerp(-0.5f, -3, timer);
            yield return null;
        }


    }
}
