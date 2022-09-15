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

    private GameObject player;

    public AudioClip[] sounds;
    private AudioSource source;

    // charging variables
    [SerializeField] private float 
    m_chargeProgress = 0, m_chargeProgressPercent = 0, m_lastChargeProgressPercent = 0, 
    m_chargeTime = 2, 
    m_chargeShootThresholdPercent = 50, 
    m_chargeSpeed = 1.5f, 
    m_cooldownTimer = 0, m_cooldownTime = 3, m_CDChargeProgressModifier = 100, m_cooldownPercent;


    [SerializeField] private bool isFullyCharged = false, isOnCooldown = false;

    [SerializeField] private Slider m_chargeProgressBar;
    [SerializeField] private RectTransform m_cooldownDisplay;

    // Start is called before the first frame update.
    void Start()
    {

        source = GetComponent<AudioSource>();


        player = GameObject.FindGameObjectWithTag("Player");

        //Sets the pistol model and childern to be active.
        pistol.SetActive(true);

        m_cooldownDisplay.offsetMax = new Vector3(m_cooldownDisplay.offsetMax.x, -(100 - m_cooldownPercent));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnCooldown)
        {
            if (true/*!Input.GetButton("Fire1")*/)
            {
                // while charging charged attack
                if (Input.GetButton("Charge Attack"))
                {

                    if (m_chargeProgress >= m_chargeTime)
                    {
                        m_chargeProgress = m_chargeTime;
                        isFullyCharged = true;
                    }
                    else
                    {
                        m_chargeProgress += Time.deltaTime * m_chargeSpeed;
                    }

                    m_chargeProgressPercent = m_chargeProgress / m_chargeTime * 100;
                }

                // When charge is cancelled and attack is made
                if (Input.GetButtonUp("Charge Attack"))
                {
                    // only shoot projectile if it is charged past percent threshold
                    if (m_chargeProgressPercent >= m_chargeShootThresholdPercent)
                    {
                        // shoot
                        GameObject projectileClone = Instantiate(pistolProjectile, spawnPosition.position, spawnPosition.rotation);

                        //sets the spawn time to 0,
                        spawnTime = 0;
                    }

                    m_lastChargeProgressPercent = m_chargeProgressPercent;
                    isOnCooldown = true;
                    m_chargeProgressPercent = 0;
                    isFullyCharged = false;
                    m_chargeProgress = 0;
                }
            }

            m_chargeProgressBar.value = m_chargeProgressPercent;
        }
        else
        {
            m_cooldownTimer += Time.deltaTime / (m_lastChargeProgressPercent / m_CDChargeProgressModifier);

            if (m_cooldownTimer >= m_cooldownTime)
            {
                m_cooldownTimer = 0;
                isOnCooldown = false;
            }


            m_cooldownPercent = m_cooldownTimer / m_cooldownTime * 100;
            m_chargeProgressBar.value = 100 - m_cooldownPercent;
            m_cooldownDisplay.offsetMax = new Vector3(m_cooldownDisplay.offsetMax.x, -(100 - m_cooldownPercent));
        }
        
        
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

                source.clip = sounds[Random.Range(0, sounds.Length)];
                source.Play();
            }
        }

        //Rotates the gun to face the mouse.
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(gunRotation.transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        gunRotation.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }
    }
}
