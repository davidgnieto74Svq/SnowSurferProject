using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float boostMultiplier = 30f; // Multiplier for the boost speed
    [SerializeField] private float baseSpeed = 15f; 
    [SerializeField] private ScoreManager scoreManager;   
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private float jumpForce = 5f; 

    [SerializeField] private AudioClip jumpSound; // Audio clip for jump sound
    
    private AudioSource audioSource; // Audio source component to play sounds
    
    Rigidbody2D rb;
    SurfaceEffector2D[] surfaceEffectors; // All the floors (iceberg + platforms) that push the player

    InputAction moveAction;
    InputAction jumpAction;
    bool isGrounded; 
    Vector2 moveValue ; 
    int flipCount;
    float totalRotation;
    float previousRotation;

    float maxTorque = 500f;

    private bool isPaused = false; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f; 

        //Active the selected character sprite 
        // If the saved index doesn't exist (e.g. a button with a wrong index), use the first character
        Transform characters = transform.GetChild(0);
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);
        if (selectedCharacter < 0 || selectedCharacter >= characters.childCount)
        {
            Debug.LogWarning($"SelectedCharacter {selectedCharacter} no existe (hay {characters.childCount} personajes). Se usa el 0.");
            selectedCharacter = 0;
            PlayerPrefs.SetInt("SelectedCharacter", 0);
        }
        characters.GetChild(selectedCharacter).gameObject.SetActive(true);

        // Get the Move action from the Input System
        moveAction = InputSystem.actions.FindAction("Move");
        
        jumpAction = InputSystem.actions.FindAction("Jump");

        audioSource = GetComponent<AudioSource>();
    
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        surfaceEffectors = FindObjectsByType<SurfaceEffector2D>();
    
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the Escape key is pressed
        if (Keyboard.current != null && 
        Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ToogglePause();            
        }

        RotatePlayer();
        BoostPlayer();
        CalculateFlips();
        Jump();
    }

    void Jump()
    {
        if (isPaused || !isGrounded) return; 

        bool jumpPressed = jumpAction != null ? jumpAction.WasPressedThisFrame() : Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        
        if (jumpPressed)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            audioSource.PlayOneShot(jumpSound); // Play the jump sound
            isGrounded = false;
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            isGrounded = false;
        }
    }

    private void ToogglePause()
    {
        isPaused = !isPaused; 

        pauseMenuPanel.SetActive(isPaused); 

        Time.timeScale = isPaused ? 0f : 1f;        
    }

    /// <summary>
    /// Calculates the number of flips the player has performed based on their rotation and updates the score accordingly.
    /// </summary>
    private void CalculateFlips()
    {
        float currentRotation = transform.rotation.eulerAngles.z; 
        totalRotation += Mathf.DeltaAngle(previousRotation, currentRotation); 
        if (Mathf.Abs(totalRotation) >= 340f ) 
        {
            flipCount++;
            totalRotation = 0f;            
            scoreManager.AddPoints(100); // Each flip gives 100 points
            scoreManager.ShowMessage("¡Giro! +100", 1f);
        }
        previousRotation = currentRotation;
    }

    private void BoostPlayer()
    {
        // if the player is moving  (up), increase the speed of the SurfaceEffector2D
        // Increase speed when moving forward, reset it when not
        float speed = moveValue.y > 0 ? boostMultiplier : baseSpeed;
        foreach (SurfaceEffector2D effector in surfaceEffectors)
        {
            effector.speed = speed;
        }

    }


private void RotatePlayer()
    {
        // Read the value of the Move action and store it in moveValue
        moveValue = moveAction.ReadValue<Vector2>();

        if (moveValue.x > 0)
        {
            rb.AddTorque(-2f); // Apply torque to rotate the player clockwise
            if (rb.angularVelocity < -maxTorque)
            {
                rb.angularVelocity = -maxTorque; // Limit the maximum clockwise rotation speed
            }
        }
        else if (moveValue.x < 0)
        {
            rb.AddTorque(2f); // Apply torque to rotate the player counterclockwise
            if (rb.angularVelocity > maxTorque)
            {
                rb.angularVelocity = maxTorque; // Limit the maximum counterclockwise rotation speed
            }
        }
    }
}
