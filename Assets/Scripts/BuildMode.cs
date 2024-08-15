using UnityEngine;

public class BuildMode : MonoBehaviour
{
    [SerializeField] private GameObject HutHousePrefab; // Reference to the prefab or GameObject for building preview
    [SerializeField] private GameObject OvenPrefab;
    public static bool IsInBuildMode { get; private set; } = false;

    private GameObject previewCube;
    private int chosenPrefab = 0;
    private bool prefabDrawn = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !prefabDrawn)
        {

            IsInBuildMode = !IsInBuildMode;

            if (IsInBuildMode)
            {
                HUDScript.Instance.UpdateBuildMode(true);
            }
            else
            {
                // close ui
                HUDScript.Instance.UpdateBuildMode(false);
            }
        }
        if (chosenPrefab == 0 && !prefabDrawn && IsInBuildMode)
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                chosenPrefab = 1;
            }
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                chosenPrefab = 2;
            }
        }

        if (chosenPrefab != 0 && !prefabDrawn)
        {
            HUDScript.Instance.UpdateBuildMode(false);
            DrawCubeInFrontOfMe();
        }

        if (IsInBuildMode && previewCube != null && prefabDrawn)
        {
            //logic what to do when in build mode, rotate, move, or initiate some methods
            //egzample: GetComponent<Oven>().OnBuild()
            Vector3 spawnPosition = transform.position + transform.forward * 20;
            Quaternion spawnRotation = transform.rotation;
            previewCube.transform.position = spawnPosition;
            previewCube.transform.rotation = spawnRotation;


            int moneyToPay = 0;
            IHasACost costComponent = previewCube.GetComponent<IHasACost>();
            if (costComponent != null)
            {
                moneyToPay = costComponent.Cost;
            }
            IHasStaticList hasStaticListComponent = previewCube.GetComponent<IHasStaticList>();
            if (IsInBuildMode && Input.GetKeyDown(KeyCode.Return) && Pizzeria.Instance.CanPayWithMoney(moneyToPay))
            {
                //here do logic on previewCube buy and init values
                Pizzeria.Instance.DeductMoney(moneyToPay);
                IsInBuildMode = false;
                previewCube = null;
                chosenPrefab = 0;
                prefabDrawn = false;
                hasStaticListComponent.Initiate();
                HUDScript.Instance.UpdateBuildMode(false);
                NavMeshRebuilder.Instance.ResetNavMeshSurface();
            }
            if (IsInBuildMode && Input.GetKeyDown(KeyCode.X)) // x for exit
            {
                // Exiting build mode: destroy the preview cube
                Destroy(previewCube);
                IsInBuildMode = false;
                chosenPrefab = 0;
                prefabDrawn = false;
                HUDScript.Instance.UpdateBuildMode(false);
            }

        }
    }

    void DrawCubeInFrontOfMe()
    {
        if (HutHousePrefab != null)
        {
            // Calculate position in front of the camera
            Vector3 spawnPosition = transform.position + transform.forward * 5;
            Quaternion spawnRotation = transform.rotation;
            //spawnRotation *= Quaternion.Euler(0, 180, 0);
            // Instantiate the HutHouse or a cube for preview
            switch (chosenPrefab)
            {
                case 0: previewCube = null; break;
                case 1: previewCube = HutHousePrefab; break;
                case 2: previewCube = OvenPrefab; break;

            }
            prefabDrawn = true;
            previewCube = Instantiate(previewCube, spawnPosition, spawnRotation);
        }
        else
        {
            Debug.LogWarning("HutHouse prefab is not assigned.");
        }
    }
}

public interface IHasACost
{
    int Cost { get; }
}
public interface IHasStaticList
{
    public void Initiate();
}