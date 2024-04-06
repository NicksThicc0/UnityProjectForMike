using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	[Header("----Player Stats----")]
	public float movementSpeed = 5;
	[Header("----Jump Stats----")]
	public float jumpHeight = 200;
	[SerializeField] float JumpTimer = 0.08f;
	[SerializeField] float maxCoyoteTime = .2f;
	[SerializeField] int maxJumps = 1;
	//hidden
	float currentJumpTimer = .1f;
	float currentCoyoteTime = 1;
	int currentJumps;
	
	[Header("----Transforms----")]
	[SerializeField] Transform Hands;
	[SerializeField] Transform Feet;
	[Header("----Components----")]
	Rigidbody2D rb;
	Animator anim;
	[Header("----Layers----")]
	[SerializeField] LayerMask whatIsGround;
	//Bools
	bool isGrounded;
	bool isRight;
	//Other Hidden Vars
	float horInput;


	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>(); // getting the rigidbody component off the player
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		horInput = Input.GetAxisRaw("Horizontal"); // getting A and D or Left Arrow Key and Right Arrow key inputs


		//Functions
		Jumping();
		//rotateTowardsMouse();
	}

	void FixedUpdate()
	{
		Vector2 Movement = new Vector2(horInput * movementSpeed, rb.velocity.y) * Time.deltaTime; // setting a vector2 to Hor Input and * by moveSpeed. rb.velocity.y is the Rigidbody2D Component that u added and its getting the gravity basically and setting it to the players Postion 

		transform.Translate(Movement, Space.World); // Moving the player
	}

	void Jumping()
	{
			if (Input.GetKeyDown(KeyCode.Space) & isGrounded || Input.GetKeyDown(KeyCode.Space) & currentCoyoteTime > 0 || Input.GetKeyDown(KeyCode.Space) & currentJumps > 0)
			{
				currentJumpTimer = JumpTimer;
				currentJumps--;
			}
			
			if (Input.GetKey(KeyCode.Space))
			{
				currentJumpTimer -= 1 * Time.deltaTime;
				if(currentJumpTimer > 0)
				{
					rb.velocity = new Vector2(rb.velocity.x, jumpHeight) * Time.deltaTime;
				}
			}
			//
			if(isGrounded)
			{
				currentCoyoteTime = maxCoyoteTime;
				currentJumps = maxJumps;
			}
			else
			{
				currentCoyoteTime -= 1 *Time.deltaTime;
			}

		//Checking if ur grounded
		isGrounded = Physics2D.OverlapCircle(Feet.position, .1f, whatIsGround); // setting isGrounded Bool to true if that red circle is touching anything on the whatIsGround Layer!
	}

	/*void rotateTowardsMouse()
	{
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		//Look Direction
		if (mousePos.x < transform.position.x)
		{
			transform.localScale = new Vector3(-1, 1, 1);
			isRight = false;
		}
		else
		{
			transform.localScale = new Vector3(1, 1, 1);
			isRight = true;
		}

		//Rotate Hands
		Quaternion rotation = Quaternion.LookRotation(mousePos - (Vector2)Hands.position, Hands.TransformDirection(Vector3.up));
		Debug.Log(rotation.z);
		Hands.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
	}  
	this stuff is a little complex so i left it out*/

	private void OnDrawGizmos() 
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(Feet.position, .1f);
	}
}
