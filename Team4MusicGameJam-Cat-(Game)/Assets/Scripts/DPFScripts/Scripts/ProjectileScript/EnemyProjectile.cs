using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public Vector3 fireingForce;
    public float damage;
    public float lifetime;
    public float bulletSpread;

    private Rigidbody bulletRB;

    // Start is called before the first frame update
    void Start()
    {
        bulletRB = GetComponent<Rigidbody>();

        //Randomise x and y values of firingForce
        fireingForce.x += Random.Range(-bulletSpread, bulletSpread);
        fireingForce.y += Random.Range(-bulletSpread, bulletSpread);

        bulletRB.AddRelativeForce(fireingForce, ForceMode.Impulse);

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
