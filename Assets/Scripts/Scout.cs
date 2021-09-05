using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MLAPI;

public class Scout : MonoBehaviour
{

    public Camera playerCamera;
    public float speed;                //Floating point variable to store the player's movement speed.
    public int gains;
    public int dashCount;
    public int legs;
    public string uiText = "Dash Remaining: ";
    public bool isLocalPlayer = true;

    private Arena arena;
    private Rigidbody2D scout;        //Store a reference to the Rigidbody2D component required to use 2D Physics.
    private SpriteRenderer player;
    private Rigidbody2D ball;
    private Rigidbody2D disc;
    private Text dashUI;

    public bool trainingMode = false;

    private bool possession = false;

    // Use this for initialization
    void Start()
    {
        
        if (isLocalPlayer)
        {
            playerCamera.enabled = true;
        }
        else
        {
            playerCamera.enabled = false;
        }
        
        arena = FindObjectOfType<Arena>();
        scout = GetComponent<Rigidbody2D>();
        player = GetComponent<SpriteRenderer>();
        disc = GameObject.Find("Disc").GetComponent<Rigidbody2D>();
        
        dashUI = GameObject.Find("dashUI").GetComponent<Text>();
        scout.freezeRotation = true;

        // Possibly change to a label or other identification later
        trainingMode = arena.name == "TrainingArena";
        // If Training is Enabled, enable the training mods
        if (true == trainingMode)
        {
            TrainingMods training = gameObject.AddComponent<TrainingMods>() as TrainingMods;
            // Give the Training Mods a reference to the disc
            training.Load(disc);
        }
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
        scout.AddForce (movement * speed);
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
        if (col.gameObject.tag == "disc" && !possession) {
            RelativeJoint2D joint = scout.gameObject.AddComponent<RelativeJoint2D>();
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
        else if (col.gameObject.tag == "spike") {
            arena.Respawn(gameObject);
        }
        

    }

    IEnumerator DelayCollision() {
        //Declare a yield instruction.
        WaitForSeconds wait = new WaitForSeconds(1);

        yield return wait; //Pause the loop for 1 second.
        if (ball)
            Physics2D.IgnoreCollision(ball.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
   }

   void Dash()
   {
        Vector2 movement = getMovementInput();
        scout.velocity = movement * legs;
        dashCount--;
   }

    void Throw()
    {
        Destroy(GetComponent<RelativeJoint2D> () );
        Physics2D.IgnoreCollision(ball.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
        ball.mass = 5;
        
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint( mousePos );
        
        Vector2 mouseDelta =  cursorInWorldPos - scout.position;
        mouseDelta.Normalize();
        ball.velocity = scout.velocity + mouseDelta * gains;
        
        possession = false;
        float start = Time.time;
        float curr = start;
        StartCoroutine(DelayCollision());
    }

}