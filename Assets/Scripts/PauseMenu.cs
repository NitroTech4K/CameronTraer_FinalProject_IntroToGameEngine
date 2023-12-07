using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System.Collections;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public Button resumeButton;
    public Button restartButton;
    public Button mainMenuButton;
    public CharacterController characterController;
    public MonoBehaviour[] scriptsToDisableOnPause;

    [SerializeField]
    private PlayableDirector timeline1; // First timeline
    [SerializeField]
    private PlayableDirector timeline2; // Second timeline

    [SerializeField]
    private string mainMenuSceneName = "MainMenu"; // Default scene name is "MainMenu"

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

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(BackToMainMenu);
        }
    }

    void Update()
    {
        // Check if the timeline is playing or the game is paused
        if ((timeline1 != null && timeline1.state == PlayState.Playing) ||
            (timeline2 != null && timeline2.state == PlayState.Playing) ||
            isPaused)
        {
            UnlockCursor(); // Unlock cursor when the timeline is playing or the game is paused
            return; // Do not process input if the timeline is playing or the game is paused
        }

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
            UnlockCursor(); // Unlock cursor when the game is paused
        }
    }

    void ResumeGame()
    {
        if (!isPaused)
        {
            return; // Ignore if the game is not paused
        }

        isPaused = false;
        Time.timeScale = 1f;

        if (characterController != null)
        {
            characterController.slopeLimit = originalSlopeLimit;
            characterController.stepOffset = originalStepOffset;
            characterController.Move(Vector3.zero);
        }

        EnableScripts();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);

            // Stop and reset the first timeline if it's playing
            if (timeline1 != null && timeline1.state == PlayState.Playing)
            {
                timeline1.time = 0.0;
                timeline1.Stop();
            }

            // Stop and reset the second timeline if it's playing
            if (timeline2 != null && timeline2.state == PlayState.Playing)
            {
                timeline2.time = 0.0;
                timeline2.Stop();
            }

            // Manually unlock the cursor after a short delay
            StartCoroutine(UnlockCursorWithDelay());
        }
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName); // Use the specified main menu scene name
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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

    IEnumerator UnlockCursorWithDelay()
    {
        // Add a short delay before unlocking the cursor
        yield return new WaitForSecondsRealtime(0.1f);

        // Unlock the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
