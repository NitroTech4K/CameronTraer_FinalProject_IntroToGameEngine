using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables; // Import the Playables namespace

public class PauseScript : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public Button resumeButton;
    public Button restartButton;
    public CharacterController characterController;
    public MonoBehaviour[] scriptsToDisableOnPause;

    [SerializeField]
    private PlayableDirector timeline; // Reference to your timeline

    private bool isPaused = false;
    private float originalSlopeLimit;
    private float originalStepOffset;
    private float originalSpeed;
    private CursorLockMode originalCursorLockMode;
    private bool originalCursorVisibility;

    void Start()
    {
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }

        if (characterController != null)
        {
            originalSlopeLimit = characterController.slopeLimit;
            originalStepOffset = characterController.stepOffset;
            originalSpeed = 5f; // Replace 5f with your default speed value or set it to a default speed
        }

        originalCursorLockMode = Cursor.lockState;
        originalCursorVisibility = Cursor.visible;

        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
    }

    void Update()
    {
        if (timeline != null && timeline.state != PlayState.Playing)
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
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (characterController != null)
        {
            characterController.slopeLimit = 0f;
            characterController.stepOffset = 0f;
        }

        DisableScripts();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(true);
        }
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (characterController != null)
        {
            characterController.slopeLimit = originalSlopeLimit;
            characterController.stepOffset = originalStepOffset;
            characterController.Move(Vector3.zero); // This helps to stop the sliding that might occur after changing the speed
        }

        EnableScripts();

        Cursor.lockState = CursorLockMode.Locked; // Ensure the cursor is locked
        Cursor.visible = false;

        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }
    }

    void RestartGame()
    {
        Time.timeScale = 1f; // Ensure that time scale is set to 1 before reloading the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void DisableScripts()
    {
        foreach (var script in scriptsToDisableOnPause)
        {
            script.enabled = false;
        }
    }

    void EnableScripts()
    {
        foreach (var script in scriptsToDisableOnPause)
        {
            script.enabled = true;
        }
    }
}
