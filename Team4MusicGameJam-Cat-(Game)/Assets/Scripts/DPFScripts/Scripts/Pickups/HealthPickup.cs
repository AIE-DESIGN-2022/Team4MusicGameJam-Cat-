using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healthToGive;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //The script triggers when an object enters its trigger box. 
    private void OnTriggerEnter(Collider other)
    {
        // Checks if the object is tagged as "Player".
        if (other.gameObject.tag == "Player")
        {

            other.gameObject.GetComponent<HealthManager>().ReceiveHealth(healthToGive);
            Destroy(gameObject);
        }
    }
}
