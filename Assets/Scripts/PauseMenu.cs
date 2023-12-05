using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public GameObject resumeButton; // Reference to the Resume button in your UI
    public CharacterController characterController; // Reference to the CharacterController
    public MonoBehaviour[] scriptsToDisableOnPause; // Add the camera script or any other scripts you want to disable

    private bool isPaused = false;
    private float originalSlopeLimit;
    private float originalStepOffset;
    private CursorLockMode originalCursorLockMode;
    private bool originalCursorVisibility;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure that the pause menu canvas is initially disabled
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }

        // Store the original values when the script starts
        if (characterController != null)
        {
            originalSlopeLimit = characterController.slopeLimit;
            originalStepOffset = characterController.stepOffset;
        }

        // Store the original cursor state
        originalCursorLockMode = Cursor.lockState;
        originalCursorVisibility = Cursor.visible;

        // Set up the Resume button click event
        if (resumeButton != null)
        {
            resumeButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ResumeGame);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for the pause input, using the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        // Pause the game
        isPaused = true;
        Time.timeScale = 0f;

        // Disable character controller movement
        if (characterController != null)
        {
            // Set the values to zero to freeze the character in place
            characterController.slopeLimit = 0f;
            characterController.stepOffset = 0f;
        }

        // Disable specified scripts
        DisableScripts();

        // Unlock the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Show the pause menu canvas
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(true);
        }
    }

    void ResumeGame()
    {
        // Unpause the game
        isPaused = false;
        Time.timeScale = 1f;

        // Restore character controller values
        if (characterController != null)
        {
            // Restore the original values
            characterController.slopeLimit = originalSlopeLimit;
            characterController.stepOffset = originalStepOffset;
        }

        // Enable specified scripts
        EnableScripts();

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Hide the pause menu canvas
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }
    }

    void DisableScripts()
    {
        // Disable specified scripts
        foreach (var script in scriptsToDisableOnPause)
        {
            script.enabled = false;
        }
    }

    void EnableScripts()
    {
        // Enable specified scripts
        foreach (var script in scriptsToDisableOnPause)
        {
            script.enabled = true;
        }
    }
}
