using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed;                //Floating point variable to store the player's movement speed.
    public int gains;
    public int dashCount;
    public int legs;
    public string uiText = "Dash Remaining: ";

    private Rigidbody2D player;        //Store a reference to the Rigidbody2D component required to use 2D Physics.
    private Rigidbody2D ball;
    private Camera cam;
    private Text dashUI;
    
    

    private bool possession = false;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<Rigidbody2D> ();
        cam = GetComponent<Camera>();
        dashUI = GameObject.Find("dashUI").GetComponent<Text>();
        player.freezeRotation = true;
    }

    void Update()
    {
        if (possession && Input.GetButtonDown("Fire1"))
            Throw();

        if (dashCount > 0 && Input.GetButtonDown("Jump"))
            Dash();

        dashUI.text = uiText + dashCount.ToString();
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        Vector2 movement = getMovementInput();
        player.AddForce (movement * speed);
    }

    Vector2 getMovementInput()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis ("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis ("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        return new Vector2 (moveHorizontal, moveVertical);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ball" && !possession) {
            Debug.Log("OnCollisionEnter2D");
            RelativeJoint2D joint = player.gameObject.AddComponent<RelativeJoint2D>();
            ball = col.rigidbody;
            // conects the joint to the other object
            joint.connectedBody = ball;
            // Stops objects from continuing to collide and creating more joints
            joint.enableCollision = false;
            joint.linearOffset = new Vector2 (0, -0.3f);

            ball.mass = 0;
            possession = true;
        }
        else if (col.gameObject.tag == "dash") {
            Destroy(col.gameObject);
            dashCount++;
        }
        

    }

    IEnumerator DelayCollision() {
        //Declare a yield instruction.
        WaitForSeconds wait = new WaitForSeconds(1);

        yield return wait; //Pause the loop for 3 seconds.
        if (ball)
            Physics2D.IgnoreCollision(ball.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
   }

   void Dash()
   {
        Vector2 movement = getMovementInput();
        player.velocity = movement * legs;
        dashCount--;
   }

    void Throw()
    {
        Destroy(GetComponent<RelativeJoint2D> () );
        Physics2D.IgnoreCollision(ball.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
        ball.mass = 5;
        
        Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        Vector2 mouseDelta = cursorInWorldPos - player.position;
        mouseDelta.Normalize();

        ball.velocity = mouseDelta * gains;
        
        possession = false;
        float start = Time.time;
        float curr = start;
        StartCoroutine(DelayCollision());
    }

}