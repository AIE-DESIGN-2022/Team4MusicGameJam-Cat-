using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CharacterController : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheckBack;                          // A position marking where to check for ceilings
	[SerializeField] private Transform m_CeilingCheckFront;                          // A position marking where to check for ceilings
	[SerializeField] private Collider m_CrouchDisableCollider;				// A collider that will be disabled when crouching

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = 1f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody m_Rigidbody;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	public bool wasCrouched;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;
	public bool canInteract = true;

	public GameObject checkLeft;
	public GameObject checkRight;

	Animator animator;

	private void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

		animator = transform.Find("GamblerCat").GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider[] colliders = Physics.OverlapSphere(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject && colliders[i].gameObject.tag != "Detector")
			{
				m_Grounded = true;
				if (!wasGrounded)
				OnLandEvent.Invoke();
			}
		}
	}


	public void Move(float move, bool crouch, bool jump)
	{
		if (canInteract)
		{

			// If crouching, check to see if the character can stand up
			if (crouch)
			{
				// If the character has a ceiling preventing them from standing up, keep them crouching
				wasCrouched = true;
			}
			if (wasCrouched && (Physics.Raycast(m_CeilingCheckBack.position, transform.up, k_CeilingRadius, m_WhatIsGround) || Physics.Raycast(m_CeilingCheckFront.position, transform.up, k_CeilingRadius, m_WhatIsGround)))
			{
				crouch = true;
			}
			else
			{
				wasCrouched = false;
			}

			//only control the player if grounded or airControl is turned on
			if (m_Grounded || m_AirControl)
			{
				// If crouching
				if (crouch)
				{
					if (!m_wasCrouching)
					{
						m_wasCrouching = true;
						OnCrouchEvent.Invoke(true);
					}

					// Reduce the speed by the crouchSpeed multiplier
					move *= m_CrouchSpeed;
					transform.localScale = new Vector3(1, 0.5f, 1);


					// Disable one of the colliders when crouching
					if (m_CrouchDisableCollider != null)
						m_CrouchDisableCollider.enabled = false;
				}
				else
				{
					transform.localScale = Vector3.one;
					// Enable the collider when not crouching
					if (m_CrouchDisableCollider != null)
						m_CrouchDisableCollider.enabled = true;

					if (m_wasCrouching)
					{
						m_wasCrouching = false;
						OnCrouchEvent.Invoke(false);
					}
				}

				// Move the character by finding the target velocity
				Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody.velocity.y);
				// And then smoothing it out and applying it to the character
				m_Rigidbody.velocity = Vector3.SmoothDamp(m_Rigidbody.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !m_FacingRight)
				{
					// ... flip the player.
					//Flip();
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (move < 0 && m_FacingRight)
				{
					// ... flip the player.
					//Flip();
				}
			}



			// If the player should jump...
			if (m_Grounded && jump)
			{
				m_Grounded = true;
				animator.SetTrigger("Jump");
				animator.SetFloat("Walk", 0);
				m_Rigidbody.AddForce(new Vector2(0f, m_JumpForce), ForceMode.VelocityChange);

			}

		}
	}


	public void EndJump()
    {
		animator.SetTrigger("Land");
    }

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		/*Vector3 theScale  = transform.localScale;
		theScale.x *= -1;
		transform.localScale = new Vector3 (theScale.x, transform.localScale.y, transform.localScale.z);*/
		if (!m_FacingRight)
		{
			transform.rotation = new Quaternion(0, 180, 0, 0);
		}
		else if (m_FacingRight)
		{
			transform.rotation = new Quaternion(0, 0, 0, 0);

		}

	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "KillBox")
        {
			GetComponent<HealthManager>().Death();
        }
    }
}
