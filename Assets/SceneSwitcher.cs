using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField]
    private string sceneToSwitchTo; // Assign the scene name in the Inspector

    // Call this function to switch to the assigned scene
    public void SwitchScene()
    {
        if (!string.IsNullOrEmpty(sceneToSwitchTo))
        {
            SceneManager.LoadScene(sceneToSwitchTo);
        }
        else
        {
            Debug.LogWarning("Scene name not assigned in the Inspector!");
        }
    }
}
