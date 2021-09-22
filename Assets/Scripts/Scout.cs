using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MLAPI;

public class Scout : Player
{
    // Int representing dash power
    public int legs;                    

    // UI text displayed for Scouts
    public string uiText = "Dash Remaining: ";

    // UI element which holds the uiText
    private Text dashUI;                        

    // Overrides Player Start
    new void Start() {
        // Call Player Start
        base.Start();

        // Retrieve DashUI Text Component from the Scene
        dashUI = GameObject.Find("dashUI").GetComponent<Text>();

        // Initialize DashCount in the UI
        dashUI.text = uiText + base.abilityCount.ToString();
    }

    // Overrides Player Update
    new void Update()
    {
        // Call Player Update
        base.Update();

        // Check for scout specific Dash Input
        if (abilityCount > 0 && Input.GetButtonDown("Jump"))
        {
            // Call Dash Function
            Dash();
            // Update dash count on the UI
            dashUI.text = uiText + abilityCount.ToString();
        }
    }

   void Dash()
   {
        // Retrieve Key Inputs from player
        Vector2 movement = getMovementInput();

        // Play the Dash Sound Effect
        soundEffects.Dash();

        // Update Player velocity
        playerBody.velocity = movement * legs;
        
        // Decrement dash counter
        abilityCount--;
   }
}