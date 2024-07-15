using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerManager : MonoBehaviour
{
    [SerializeField] int sceneId;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) ShowMenu();
    }

    private void Start()
    {
        if (Pizzeria.Instance == null) SceneManager.LoadScene(0);
        if (DeliveryTruck.Instance == null) SceneManager.LoadScene(1);

        if (SceneManager.GetActiveScene().buildIndex != 1) SceneManager.LoadScene(1);
    }


    public void ShowMenu()
    {
        PersistentUI.Instance.ShowMenu();
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneId);
    }
}
