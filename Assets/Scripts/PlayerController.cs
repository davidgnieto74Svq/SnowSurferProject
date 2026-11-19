using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] private float boostMultiplier = 30f; // Multiplier for the boost speed
    [SerializeField] private float baseSpeed = 15f; 

    [Header("Score Manager")]
    [SerializeField] private ScoreManager scoreManager;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenuPanel;

    


    Rigidbody2D rb;
    SurfaceEffector2D surfaceEffector2D;

    InputAction moveAction;
    Vector2 moveValue ; 
    int flipCount;
    float totalRotation;
    float previusRotation;
    float maxTorque = 400f;

    private bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Time.timeScale = 1f; // Ensure the game is not paused at the start

        // Active the selected character sprite

        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0); 
        transform.GetChild(0).GetChild(selectedCharacter).gameObject.SetActive(true);




        moveAction = InputSystem.actions.FindAction("Move");
    
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        surfaceEffector2D = FindAnyObjectByType<SurfaceEffector2D>();           
    
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }

        RotatePlayer();
        BoostPlayer();
        CalculateFlips();
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f; // Pause or resume the game
    }

    private void CalculateFlips()
    {
        float currentRotation = transform.rotation.eulerAngles.z;
        totalRotation += Mathf.DeltaAngle(previusRotation, currentRotation);
        previusRotation = currentRotation;

        if (Mathf.Abs(totalRotation) >= 360f)
        {
            flipCount++;
            totalRotation = 0f; 
            scoreManager.UpdateScore(flipCount*100); 
          
        }
        previusRotation = currentRotation;
    }

    private void BoostPlayer()
    {
        // if the player is moving  (up), increase the speed of the SurfaceEffector2D
        if (moveValue.y > 0)
        {
            surfaceEffector2D.speed = boostMultiplier; // Increase speed when moving forward
        }
        else
        {
            surfaceEffector2D.speed = baseSpeed; // Reset speed when not moving forward
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
