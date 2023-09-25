using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject crossFade;
    [SerializeField] private Animator sceneTransition;
    [SerializeField] private float transitionTime = 1f;

    private void Awake()
    {
        Time.timeScale = 1f;
        crossFade.SetActive(false);
        crossFade.SetActive(true);
    }

    /// <summary>
    /// Loads the scene with the same name as the sceneName paramater.
    /// <a herf = "https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html"></a>
    /// </summary>
    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(LoadSceneWithFade(sceneName));
    }

    /// <summary>
    /// Restarts the current scene. Will be used for when player dies.
    /// </summary>
    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Quits the game regardless if it's a build or in the engine editor.
    /// < a href = "https://docs.unity3d.com/ScriptReference/EditorApplication-isPlaying.html"></a>
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// https://www.youtube.com/watch?v=CE9VOZivb3I
    /// Creates a fade transition and loads the next scene.
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadSceneWithFade(string sceneName)
    {
        sceneTransition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}