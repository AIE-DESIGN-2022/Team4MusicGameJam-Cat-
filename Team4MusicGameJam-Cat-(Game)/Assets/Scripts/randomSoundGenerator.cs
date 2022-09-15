using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomSoundGenerator : MonoBehaviour
{
    public AudioClip[] sounds;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            source.clip = sounds[Random.Range(0, sounds.Length)];
            source.Play();

        }
        //if 
    }
}
