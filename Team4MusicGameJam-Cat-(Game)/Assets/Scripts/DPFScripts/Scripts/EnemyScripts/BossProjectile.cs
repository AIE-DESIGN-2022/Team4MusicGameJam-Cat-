using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public Vector3 fireingForce;
    public float damage;
    public float lifetime;
    public float bulletSpread;

    private Rigidbody bulletRB;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bulletRB = GetComponent<Rigidbody>();

        transform.LookAt(player.transform.position + new Vector3(0, Random.Range(-bulletSpread, bulletSpread), 0));
        //Randomise x and y values of firingForce
        //fireingForce.x += Random.Range(-bulletSpread, bulletSpread);
        //fireingForce.y += Random.Range(-bulletSpread, bulletSpread);

        bulletRB.AddForce(fireingForce.x * transform.TransformDirection(Vector3.forward), ForceMode.Impulse);

        StartCoroutine("BulletTimer");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator BulletTimer()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Bullet")
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<HealthManager>().TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
