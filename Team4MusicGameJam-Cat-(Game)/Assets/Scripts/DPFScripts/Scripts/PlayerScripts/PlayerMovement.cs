using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    float horizontalMove = 0f;

    public float runSpeed = 40f;

    bool jump = false;
    bool crouch = false;

    public bool wallToRight;
    public bool wallToLeft;

    public GameObject rightWall;
    public GameObject leftWall;

    [SerializeField] float dashCooldownTimer = 0, dashCooldown = 2;
    [SerializeField] ParticleSystem dashParticle;

    [SerializeField] private Vector3 dashVelocity = new Vector3(50, 0, 0);

    // Update is called once per frame
    void Update()
    {
        dashCooldownTimer += Time.deltaTime;


        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        if (Input.GetButtonDown("Move Left"))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
            rightWall.GetComponent<DetectWalls>().right = false;
            leftWall.GetComponent<DetectWalls>().right = true;
        }

        if (Input.GetButtonDown("Move Right"))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            leftWall.GetComponent<DetectWalls>().right = true;
            rightWall.GetComponent<DetectWalls>().right = false;
        }

        if (Input.GetButtonDown("Dash"))
        {
            if (dashCooldownTimer >= dashCooldown)
            {
                StartCoroutine("DashParticle");
                dashCooldownTimer = 0;
                transform.GetComponent<Rigidbody>().AddRelativeForce(dashVelocity, ForceMode.Impulse);
                
            }
            
        }

    }

    IEnumerator DashParticle()
    {
        ParticleSystem particle = Instantiate(dashParticle, transform);
        particle.transform.localPosition = new Vector3(0, 0, 0);
        particle.Play();
        yield return new WaitForSeconds(0.2f);
        Destroy(particle);
    }

    void FixedUpdate()
    {
        //Move our character
        if (wallToRight && horizontalMove > 0)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y);
            controller.Move(0, crouch, jump);
            jump = false;
        }
        else if (wallToLeft && horizontalMove < 0)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y);
            controller.Move(0, crouch, jump);
            jump = false;
        }
        else
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
            jump = false;
        }
        
    }
}
