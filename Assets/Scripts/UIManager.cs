using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public static bool allowPause; // set to false across classes if you need to prevent pausing
    [SerializeField] private TextMeshProUGUI speedUI;
    [SerializeField] private GameObject crossFade;
    [SerializeField] private Animator sceneTransition;
    [SerializeField] private float transitionTime = 1f;
    
    private Drive playerDrive;

    private void Awake()
    {
        crossFade.SetActive(false);
        crossFade.SetActive(true);
    }


    // https://answers.unity.com/questions/1174255/since-onlevelwasloaded-is-deprecated-in-540b15-wha.html
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        scene = SceneManager.GetActiveScene();
        if (scene.name == "0_MainMenu")
        {
            Destroy(gameObject);
        }

        crossFade.SetActive(false);
        crossFade.SetActive(true);
    }

    private void Start()
    {
        playerDrive = GameObject.FindGameObjectWithTag("Player").GetComponent<Drive>();

        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "0_MainMenu")
        {
            allowPause = false;
        }
        else
        {
            allowPause = true;
        }
    }

    public void SetSpeed(float speed)
    {
        this.speedUI.text = (speed.ToString());
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