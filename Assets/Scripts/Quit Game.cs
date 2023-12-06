using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // This method can be called to quit the game
    public void Quit()
    {
        // This will only work in a standalone build (not in the Unity Editor)
#if UNITY_STANDALONE
        Application.Quit();
#endif

        // If you are running the game in the Unity Editor, this will stop the play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
