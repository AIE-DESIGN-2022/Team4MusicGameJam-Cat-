using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockpickDoor : MonoBehaviour
{
    public GameObject LockWhole;
    public GameObject LockPick;
    public bool isLockpicking = false;
    public bool playerInTrigger = false;
    private GameObject player;
    public Text infoText;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        infoText.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInTrigger)
        {
            if (Input.GetButtonDown("Interact") && !isLockpicking)
            {
                LockWhole.SetActive(true);
                LockPick.SetActive(true);
             //   Debug.Log("Set Active");
                isLockpicking = true;
                player.GetComponent<CharacterController>().canInteract = false;
                infoText.text = "Press 'Esc' to exit";


            }

            if (Input.GetButtonDown("Cancel"))
            {
                player.GetComponent<CharacterController>().canInteract = true;

                LockWhole.SetActive(false);
                LockPick.SetActive(false);
               // Debug.Log("Disabled");
                isLockpicking = false;
                infoText.text = "";

            }
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        infoText.text = "Press 'E' to lockpick";

        if (other.transform.GetComponent<CharacterController>() != null)
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        infoText.text = "";
        if (other.transform.GetComponent<CharacterController>() != null)
        {
            playerInTrigger = false;
        }
    }
    

}
