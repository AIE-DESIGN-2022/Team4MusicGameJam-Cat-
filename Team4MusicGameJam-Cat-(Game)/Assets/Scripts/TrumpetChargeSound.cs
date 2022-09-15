using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetChargeSound : MonoBehaviour
{
    public AudioSource trumpetCharging;
    public AudioSource trumpetCharged;

    // Start is called before the first frame update
    void Start()
    {
       // trumpetCharging = GetComponent<AudioSource>();
        //trumpetCharged = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!trumpetCharging.isPlaying && !trumpetCharged.isPlaying)
        {
            trumpetCharged.Play();
        }
    }
}
