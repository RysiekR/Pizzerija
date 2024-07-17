using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public List<Scene> SceneList = new List<Scene>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            AddInitialScene();
            StartCoroutine(LoadAllScenes());
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void AddInitialScene()
    {
        SceneList.Add(SceneManager.GetActiveScene());
    }
    private IEnumerator LoadAllScenes()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            // Check if the scene is already loaded
            Scene scene = SceneManager.GetSceneByBuildIndex(i);
            if (!SceneList.Contains(scene))
            {
                // Load the scene additively and wait until it is fully loaded
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(i, LoadSceneMode.Additive);
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }

                // Add the loaded scene to the list
                SceneList.Add(SceneManager.GetSceneByBuildIndex(i));
            }
        }
    }
}


