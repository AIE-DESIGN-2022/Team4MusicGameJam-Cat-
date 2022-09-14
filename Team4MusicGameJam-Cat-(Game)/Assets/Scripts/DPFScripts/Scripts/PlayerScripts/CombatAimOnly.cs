using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponStateAimOnly
{
    Pistol,
    AssaultRifle,
    Shotgun
}

public class CombatAimOnly : MonoBehaviour
{
    public int pistolCurrentAmmoCount;
    public int pistolMaxAmmoCount;
    public int pistolReserveAmmoCount;


    public int assaultRifleCurrentAmmoCount;
    public int assaultRifleMaxAmmoCount;
    public int assaultRifleReserveAmmoCount;

    public int shotgunCurrentAmmoCount;
    public int shotgunMaxAmmo;
    public int shotgunReserveAmmoCount;


    // public int reserveAmmoCount;



    public Text ammoDesplayText;

    public int ammoToGive;

    public float reloadDelay;
    private bool isReloading;
    public Text relodingText;
    public Text reloadText;

    public Transform gunRotation;
    public GameObject gunObject;

    public Transform spawnPosition;
    public GameObject pistolProjectile;
    public GameObject assaultRifleProjectile;
    public GameObject shotgunProjectile;
    public Image pistolImage;
    public Image assaultRifleImage;
    public Image shotgunImage;

    public GameObject pistol;
    public GameObject assaultRifle;
    public GameObject shotgun;

    public float spawnDelay;
    private float spawnTime;

    public Image reticleImage;

    public WeaponState weaponState;

    // Start is called before the first frame update.
    void Start()
    {
        //Sets the current ammo of the weapon to the max ammo count of the weapon.
        pistolCurrentAmmoCount = pistolMaxAmmoCount;
        assaultRifleCurrentAmmoCount = assaultRifleMaxAmmoCount;
        shotgunCurrentAmmoCount = shotgunMaxAmmo;

        isReloading = false;
        relodingText.enabled = false;
        reloadText.enabled = false;
        reticleImage.enabled = enabled;


        TurnOffGuns();
        TurnOffImages();

        //enables the pistol image on the UI.
        pistolImage.enabled = true;
        //changes the players weapon state to use the pistol.
        weaponState = WeaponState.Pistol;
        //Sets the pistol model and childern to be active.
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
        if (pistolReserveAmmoCount > 999)
        {
            pistolReserveAmmoCount = 999;
        }
        if (assaultRifleReserveAmmoCount > 999)
        {
            assaultRifleReserveAmmoCount = 999;
        }
        if (shotgunReserveAmmoCount > 999)
        {
            shotgunReserveAmmoCount = 999;
        }


        spawnTime += Time.deltaTime;

        //When the "Pistol bullon is pressed"
        if (Input.GetButtonDown("Pistol"))
        {
            //changes the players weapon state to use the pistol. 
            weaponState = WeaponState.Pistol;
            TurnOffGuns();
            //sets the spawn delay of the bullet projectile to 0.3f.
            spawnDelay = 0.3f;
            TurnOffImages();
            pistolImage.enabled = true;
            pistol.SetActive(true);
        }

        if (Input.GetButtonDown("AssaultRifle"))
        {
            //changes the players weapon state to use the AssaultRifle. 
            weaponState = WeaponState.AssaultRifle;
            TurnOffGuns();
            //sets the spawn delay of the bullet projectile to 0.2f.
            spawnDelay = 0.2f;
            TurnOffImages();
            assaultRifleImage.enabled = true;
            assaultRifle.SetActive(true);
        }

        if (Input.GetButtonDown("Shotgun"))
        {
            //changes the players weapon state to use the Shotgun. 
            weaponState = WeaponState.Shotgun;
            TurnOffGuns();
            //sets the spawn delay of the bullet projectile to 1f.
            spawnDelay = 1f;
            TurnOffImages();
            shotgunImage.enabled = true;
            shotgun.SetActive(true);
        }


        if (spawnTime >= spawnDelay)
        {
            //Checks if any of the weapons ammo count are equal to 0 and checks if the player is not reloading.
            if (pistolCurrentAmmoCount == 0 || assaultRifleCurrentAmmoCount == 0 || shotgunCurrentAmmoCount == 0 && isReloading == false)
            {
                //enables reloading to be true
                reloadText.enabled = true;
            }
            else
            {
                reloadText.enabled = false;
            }

            //Checks if the "Fire1 is pressed and if reloading is false"
            if (Input.GetButton("Fire1") && isReloading == false)
            {
                //Checks if the weapoan state is set to pistol and that there is more than 0 current ammo.
                if (weaponState == WeaponState.Pistol && (pistolCurrentAmmoCount > 0))
                {
                    //removes one ammo from the weapons current ammo.
                    pistolCurrentAmmoCount--;

                    //spawns in pistol bullet projectile.
                    GameObject projectileClone = Instantiate(pistolProjectile, spawnPosition.position, spawnPosition.rotation);
                }

                //Checks if the weapoan state is set to AssaultRifle and that there is more than 0 current ammo.
                if (weaponState == WeaponState.AssaultRifle && (assaultRifleCurrentAmmoCount > 0))
                {
                    //removes one ammo from the weapons current ammo.
                    assaultRifleCurrentAmmoCount--;

                    //spawns in assaultRifle bullet projectile.
                    GameObject projectileClone = Instantiate(assaultRifleProjectile, spawnPosition.position, spawnPosition.rotation);
                }

                //Checks if the weapoan state is set to shotgun and that there is more than 0 current ammo.
                if (weaponState == WeaponState.Shotgun && (shotgunCurrentAmmoCount > 0))
                {
                    //removes one ammo from the weapons current ammo.
                    shotgunCurrentAmmoCount--;

                    //spawns in shotgun bullet projectile.
                    GameObject projectileClone = Instantiate(shotgunProjectile, spawnPosition.position, spawnPosition.rotation);
                }

                //sets the spawn time to 0,
                spawnTime = 0;
            }
        }

        //Checks the weapon state than changes the desplayed weapon ammo to the correct ammo.
        if (weaponState == WeaponState.Pistol)
        {
            ammoDesplayText.text = "Ammo: " + pistolCurrentAmmoCount + "/" + pistolReserveAmmoCount;
        }
        if (weaponState == WeaponState.AssaultRifle)
        {
            ammoDesplayText.text = "Ammo: " + assaultRifleCurrentAmmoCount + "/" + assaultRifleReserveAmmoCount;
        }
        if (weaponState == WeaponState.Shotgun)
        {
            ammoDesplayText.text = "Ammo: " + shotgunCurrentAmmoCount + "/" + shotgunReserveAmmoCount;
        }

    

        //checks if "Fire2" button has been released.
        else if (Input.GetButtonUp("Fire2"))
        {
            //Disables the reticle on the UI.
            reticleImage.enabled = false;
            //Cursor.lockState = CursorLockMode.Locked;
            //Rotation.transform.Rotate(0.0f, 0.0f, 0.0f);

            //Sets the Rotation of the gun to 0.
            gunObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        //Checks if the R key has been pressed and what weapon state has been selected and checkes the reserve ammo is higher than 0.
        if (Input.GetKeyDown(KeyCode.R) && weaponState == WeaponState.Pistol && (pistolReserveAmmoCount > 0))
        {
            reloadText.enabled = false;
            Invoke("Reload", reloadDelay);
            isReloading = true;
            relodingText.enabled = true;
        }

        //Checks if the R key has been pressed and what weapon state has been selected and checkes the reserve ammo is higher than 0.
        if (Input.GetKeyDown(KeyCode.R) && weaponState == WeaponState.AssaultRifle && (assaultRifleReserveAmmoCount > 0))
        {
            reloadText.enabled = false;
            Invoke("Reload", reloadDelay);
            isReloading = true;
            relodingText.enabled = true;
        }

        //Checks if the R key has been pressed and what weapon state has been selected and checkes the reserve ammo is higher than 0.
        if (Input.GetKeyDown(KeyCode.R) && weaponState == WeaponState.Shotgun && (shotgunReserveAmmoCount > 0))
        {
            reloadText.enabled = false;
            Invoke("Reload", reloadDelay);
            isReloading = true;
            relodingText.enabled = true;
        }
    }

    public void ReceivePistolAmmo(int ammoToReceive)
    {
        //checks if the weapon state is set to pistol
        if (weaponState == WeaponState.Pistol)
        {
            //adds ammo to pistol reserve from "ammoToReceive". 
            pistolReserveAmmoCount += ammoToReceive;
            if (pistolReserveAmmoCount > 999)
            {
                pistolReserveAmmoCount += ammoToGive;
            }
        }
    }
    public void ReceiveAssaultRifleAmmo(int ammoToReceive)
    {
        if (weaponState == WeaponState.AssaultRifle)
        {
            //adds ammo to assaultRifle reserve from "ammoToReceive". 
            assaultRifleReserveAmmoCount += ammoToReceive;
            if (assaultRifleReserveAmmoCount > 999)
            {
                assaultRifleReserveAmmoCount += ammoToGive;
            }
        }
    }

    public void ReceiveShotgunAmmo(int ammoToReceive)
    {
        if (weaponState == WeaponState.Shotgun)
        {
            //adds ammo to shotgun reserve from "ammoToReceive". 
            shotgunReserveAmmoCount += ammoToReceive;
            if (shotgunReserveAmmoCount > 999)
            {
                shotgunReserveAmmoCount += ammoToGive;
            }
        }
    }


    void Reload()
    {
        //Checks the weapon state.
        if (weaponState == WeaponState.Pistol)
        {
            pistolReserveAmmoCount -= (pistolMaxAmmoCount - pistolCurrentAmmoCount);
            UpdatePistolAmmoCount();
            isReloading = false;
            relodingText.enabled = false;
        }

        //Checks the weapon state.
        if (weaponState == WeaponState.AssaultRifle)
        {
            assaultRifleReserveAmmoCount -= (assaultRifleMaxAmmoCount - assaultRifleCurrentAmmoCount);
            UpdateAssaultRifleAmmoCount();
            isReloading = false;
            relodingText.enabled = false;
        }

        //Checks the weapon state.
        if (weaponState == WeaponState.Shotgun)
        {
            shotgunReserveAmmoCount -= (shotgunMaxAmmo - shotgunCurrentAmmoCount);
            UpdateShotgunAmmoCount();
            isReloading = false;
            relodingText.enabled = false;
        }
    }

    //Disable all gun models
    void TurnOffGuns()
    {
        pistol.SetActive(false);
        assaultRifle.SetActive(false);
        shotgun.SetActive(false);
    }

    //Disable all gun hud icons.
    void TurnOffImages()
    {
        pistolImage.enabled = false;
        assaultRifleImage.enabled = false;
        shotgunImage.enabled = false;
    }

    //updates ammo count to the max ammo.
    private void UpdatePistolAmmoCount()
    {
        pistolCurrentAmmoCount = pistolMaxAmmoCount;
    }

    //updates ammo count to the max ammo.
    private void UpdateAssaultRifleAmmoCount()
    {
        assaultRifleCurrentAmmoCount = assaultRifleMaxAmmoCount;
    }

    //updates ammo count to the max ammo.
    private void UpdateShotgunAmmoCount()
    {
        shotgunCurrentAmmoCount = shotgunMaxAmmo;
    }
}
