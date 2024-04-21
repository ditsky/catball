using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    // Camera attached to this player
    public Camera playerCamera; 
    // Floating point variable to store the player's movement speed
    public float speed; 
    // Int to store player's throw power               
    public int gains;
    // Physics body for the game disc
    public Rigidbody2D disc;
    // Counter for remaining ability uses
    public int abilityCount;
    // SoundEffects object used to play all SFX
    public SoundEffects soundEffects;
    // Stores a reference to the Rigidbody2D component required to use 2D Physics
    public Rigidbody2D playerBody;        
    // Stores a reference to the arena being played in
    private Arena arena;
    // Stores a reference to the player sprite
    private SpriteRenderer sprite;
    // Bool which determines whether or not we are playing in training mode
    public bool trainingMode = false;
    // Bool which determines whether or not this player has possesion of the ball
    private bool possession = false;
    public NetworkVariable<bool> jointed = new NetworkVariable<bool>(false);
    // Team name for player
    public int team;
    public int id;

    // Use this for initialization
    

    
    public void Start()
    {        
        // If this is the local player
        if (IsLocalPlayer)
        {
            // Enable this player's camera
            playerCamera.enabled = true;
        }
        else
        {
            // Disable other player cameras
            playerCamera.enabled = false;
        }
        
        // Retrieve game object references from the component/scene
        arena = FindObjectOfType<Arena>();
        playerBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        soundEffects = GetComponent<SoundEffects>();
        disc = GameObject.Find("Disc").GetComponent<Rigidbody2D>();

        // Initialize the new player into the game
        arena.JoinGame(this);

        // Keep physics body from spinning
        playerBody.freezeRotation = true;

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

    public void Update()
    {
        // If player left clicks, throw the disc
        if (possession && Input.GetButtonDown("Fire1"))
            Throw();

        if (jointed.Value)
        {
            // Create a joint to connect the disc to the player
            RelativeJoint2D joint = gameObject.AddComponent<RelativeJoint2D>();

            // conects the joint to the other object
            joint.connectedBody = disc;

            // Stops objects from continuing to collide and creating more joints
            joint.enableCollision = false;

            // Slightly distance the disc from the player sprite
            joint.linearOffset = new Vector2(0, -0.3f);

            GrabDiscServerRpc(false);
        }
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        Vector2 movement = getMovementInput();
        playerBody.AddForce (movement * speed);
    }

    public Vector2 getMovementInput()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis ("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis ("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        return new Vector2 (moveHorizontal, moveVertical);
    }

    [ServerRpc]
    private void GrabDiscServerRpc(bool isJointed)
    {
        jointed.Value = isJointed;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // If the player collided with the disc and does not already have possession
        if (col.gameObject.tag == "disc" && !possession && IsLocalPlayer) {
            // Create a joint to connect the disc to the player
            RelativeJoint2D joint = playerBody.gameObject.AddComponent<RelativeJoint2D>();

            // conects the joint to the other object
            joint.connectedBody = disc;

            // Stops objects from continuing to collide and creating more joints
            joint.enableCollision = false;

            // Slightly distance the disc from the player sprite
            joint.linearOffset = new Vector2(0, -0.3f);

            // Remove mass from the disc to avoid slowing down player
            disc.mass = 0;

            // Set possession flag to True
            possession = true;
        }
        // If the player collided with an ability pickup
        else if (col.gameObject.tag == "dash") {
            // Play the ability powerup SFX
            soundEffects.Powerup();

            // Destroy the pickup
            Destroy(col.gameObject);

            // Increment ability charges
            abilityCount++;
        }
        // If the player collided with a spike
        else if (col.gameObject.tag == "spike") {
            soundEffects.Pop();
            //arena.Respawn(gameObject);
        }
        // If the player collided with a wall
        else if (col.gameObject.tag == "wall") {
            soundEffects.Bounce();
        }
    }

    void Throw()
    {
        // Destory the joint connecting the disc to the player
        Destroy(GetComponent<RelativeJoint2D> () );

        // Ignore collisions with the disc briefly while it moves away from the player
        Physics2D.IgnoreCollision(disc.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);

        // Return the disc's mass to normal
        disc.mass = 5;
        
        // Retrieve mouse in world position
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint( mousePos );
        
        // Calculate throw vector from the player towards the mouse
        Vector2 mouseDelta =  cursorInWorldPos - playerBody.position;
        mouseDelta.Normalize();

        // Play the SFX for shooting
        soundEffects.Shoot();

        // Update the disc's velocity with the new vector scaled by this player's throw power
        disc.velocity = playerBody.velocity + mouseDelta * gains;
        
        // Reset the possession flag to false
        possession = false;

        // Start a wait to return collision detection to the disc
        StartCoroutine(DelayCollision());
    }

    IEnumerator DelayCollision() {
        //Declare a yield instruction.
        WaitForSeconds wait = new WaitForSeconds(1);

        yield return wait; //Pause the loop for 1 second.
        if (disc)
            Physics2D.IgnoreCollision(disc.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
   }
}
