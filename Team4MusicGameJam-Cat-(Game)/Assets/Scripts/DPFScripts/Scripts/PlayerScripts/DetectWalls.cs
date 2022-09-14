using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWalls : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerMovement player;
    public bool right;
    private float lastX;
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        lastX = transform.position.x;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (right && collision.tag == "Floor")
        {
            player.transform.position = new Vector3(lastX, transform.position.y, transform.position.z);
            Debug.Log("hit wall to right");
            player.wallToRight = true;
        }
        else if(collision.tag == "Floor")
        {
            player.transform.position = new Vector3(lastX, transform.position.y, transform.position.z);
            Debug.Log("hit wall to left");
            player.wallToleft = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (right && collision.tag == "Floor")
        {
            player.wallToRight = false;
        }
        else if (collision.tag == "Floor")
        {
            player.wallToleft = false;
        }
    }
}
