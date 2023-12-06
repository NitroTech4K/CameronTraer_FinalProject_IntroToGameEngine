using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public Button resumeButton;
    public Button restartButton;
    public CharacterController characterController;
    public MonoBehaviour[] scriptsToDisableOnPause;

    [SerializeField]
    private PlayableDirector timeline;

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
            originalSpeed = 5f;
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

        if (timeline != null)
        {
            timeline.played += OnTimelineStart; // Subscribe to the timeline start event
        }
    }

    void OnTimelineStart(PlayableDirector director)
    {
        UnlockCursor(); // Unlock the cursor when the timeline starts playing
    }

    void Update()
    {
        if (timeline != null && timeline.state == PlayState.Playing)
        {
            return; // Do not process input if the timeline is playing
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
            characterController.Move(Vector3.zero);
        }

        EnableScripts();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    void OnDestroy()
    {
        if (timeline != null)
        {
            timeline.played -= OnTimelineStart; // Unsubscribe from the timeline start event to prevent memory leaks
        }
    }
}
