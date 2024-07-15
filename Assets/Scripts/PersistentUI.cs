using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    public static PersistentUI Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ShowMenu()
    {
        if (!gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }


}