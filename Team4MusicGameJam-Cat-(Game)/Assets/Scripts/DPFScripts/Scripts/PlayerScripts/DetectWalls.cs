using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWalls : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerMovement player;
    public bool right;
    private float lastX;
    private GameObject otherCollision;
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        lastX = transform.position.x;

        if (otherCollision == null || !otherCollision.activeInHierarchy)
        {
            if (right)
            {
                player.wallToRight = false;
            }
            else
            {
                player.wallToLeft = false;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (right && (collision.tag == "Floor" || collision.tag == "Enemy"))
        {
            player.transform.position = new Vector3(lastX, transform.position.y, transform.position.z);
            Debug.Log("hit wall to right");
            player.wallToRight = true;
            otherCollision = collision.gameObject;
        }
        else if(collision.tag == "Floor" || collision.tag == "Enemy")
        {
            player.transform.position = new Vector3(lastX, transform.position.y, transform.position.z);
            Debug.Log("hit wall to left");
            player.wallToLeft = true;
            otherCollision = collision.gameObject;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (right && (collision.tag == "Floor" || collision.tag == "Enemy"))
        {
            player.wallToRight = false;
            otherCollision = null;
        }
        else if (collision.tag == "Floor" || collision.tag == "Enemy")
        {
            player.wallToLeft = false;
            otherCollision = null;
        }
    }
}
