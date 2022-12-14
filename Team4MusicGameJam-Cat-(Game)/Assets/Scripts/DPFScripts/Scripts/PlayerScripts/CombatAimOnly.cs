using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatAimOnly : MonoBehaviour
{
   
    public Transform gunRotation;
    public GameObject gunObject;

    public Transform spawnPosition;
    public GameObject pistolProjectile;

    public GameObject pistol;

    public float spawnDelay;
    private float spawnTime;

    public Image reticleImage;

    // Start is called before the first frame update.
    void Start()
    {
        reticleImage.enabled = enabled;
        pistol.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        //Moves the reticle to the position of the mouse.
        reticleImage.transform.position = Input.mousePosition;

        //Rotates the gun to face the mouse.
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(gunRotation.transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        gunRotation.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //if the reserve ammo count gets over 999 ammo it gets set to 999.

        spawnTime += Time.deltaTime;

        if (spawnTime >= spawnDelay)
        {
            //Checks if any of the weapons ammo count are equal to 0 and checks if the player is not reloading.
           

            //Checks if the "Fire1 is pressed and if reloading is false"
            if (Input.GetButton("Fire1"))
            {
                    GameObject projectileClone = Instantiate(pistolProjectile, spawnPosition.position, spawnPosition.rotation);
                //sets the spawn time to 0,
                spawnTime = 0;
            }
        }

        //checks if "Fire2" button has been released.
        else if (Input.GetButtonUp("Fire2"))
        {
            //Disables the reticle on the UI.
            reticleImage.enabled = false;

            //Sets the Rotation of the gun to 0.
            gunObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }
}
