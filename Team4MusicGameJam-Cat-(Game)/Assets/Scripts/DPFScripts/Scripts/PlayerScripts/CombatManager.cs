using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{

    public Transform gunRotation;
    public GameObject gunObject;

    public Transform spawnPosition;
    public GameObject pistolProjectile;

    public GameObject pistol;

    public float spawnDelay;
    private float spawnTime;

    public Image reticleImage;

    private GameObject player;

    // Start is called before the first frame update.
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        reticleImage.enabled = false;

        //Sets the pistol model and childern to be active.
        pistol.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<CharacterController>().canInteract == true)
        {
        spawnTime += Time.deltaTime;


        if (spawnTime >= spawnDelay)
        {
            //Checks if the "Fire1 is pressed"
            if (Input.GetButton("Fire1"))
            {
                //spawns in pistol bullet projectile.
                GameObject projectileClone = Instantiate(pistolProjectile, spawnPosition.position, spawnPosition.rotation);
                //sets the spawn time to 0,
                spawnTime = 0;
            }
        }
        //Moves the reticle to the position of the mouse.
        reticleImage.transform.position = Input.mousePosition;

        //Rotates the gun to face the mouse.
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(gunRotation.transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        gunRotation.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }
    }
}
