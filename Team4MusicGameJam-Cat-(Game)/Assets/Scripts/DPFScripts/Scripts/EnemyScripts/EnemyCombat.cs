using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyWeaponState
{
    Pistol,
    AssaultRifle,
    Shotgun
}

public class EnemyCombat : MonoBehaviour
{
    public EnemyWeaponState enemyWeaponState;

    public GameObject enemyPistolProjectile;
    public GameObject enemyAssaultRifleProjectile;
    public GameObject enemyShotgunProjectile;

    public GameObject pistol;
    public GameObject assaultRifle;
    public GameObject shotgun;

    public bool canAttack = true;
    public float Meleedamage;
    public float attackTime;

    private GameObject player;

    public bool isRangedEnemy;
    //public GameObject enemyProjectile;
    public Transform spawnPosition;

   

    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (enemyWeaponState == EnemyWeaponState.Pistol)
        {
            TurnOffEnemyGuns();
            pistol.SetActive(true);
            attackTime = 0.4f;

        }
        if (enemyWeaponState == EnemyWeaponState.AssaultRifle)
        {
            TurnOffEnemyGuns();
            assaultRifle.SetActive(true);
            attackTime = 0.3f;

        }
        if (enemyWeaponState == EnemyWeaponState.Shotgun)
        {
            TurnOffEnemyGuns();
            shotgun.SetActive(true);
            attackTime = 1f;

        }

    }
    public IEnumerator Attack()
    {
        canAttack = false;
        if (isRangedEnemy)
        {
            if (enemyWeaponState == EnemyWeaponState.Pistol)
            {
                GameObject projectileClone = Instantiate(enemyPistolProjectile, spawnPosition.position, spawnPosition.rotation);
              
            }
            if (enemyWeaponState == EnemyWeaponState.AssaultRifle)
            {
                GameObject projectileClone = Instantiate(enemyAssaultRifleProjectile, spawnPosition.position, spawnPosition.rotation);

            }
            if (enemyWeaponState == EnemyWeaponState.Shotgun)
            {
                GameObject projectileClone = Instantiate(enemyShotgunProjectile, spawnPosition.position, spawnPosition.rotation);

            }
        }
        else
        {
            player.GetComponent<HealthManager>().TakeDamage(Meleedamage);

        }

        yield return new WaitForSeconds(attackTime);
        canAttack = true;
    }
    void TurnOffEnemyGuns()
    {
        pistol.SetActive(false);
        assaultRifle.SetActive(false);
        shotgun.SetActive(false);
    }
}
