using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshRebuilder : MonoBehaviour
{
    public static NavMeshRebuilder Instance;
    NavMeshSurface surface;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        surface = GetComponent<NavMeshSurface>();
    }
    public void ResetNavMeshSurface()
    {
        StartCoroutine(StartUpdatingNavMesh());
        //surface.BuildNavMesh();
        //ResetAll();
    }
    private IEnumerator StartUpdatingNavMesh()
    {
        yield return StartCoroutine(UpdateNavMeshAsync());
    }
    private IEnumerator UpdateNavMeshAsync()
    {
        AsyncOperation asyncOp = surface.UpdateNavMesh(surface.navMeshData);

        while (!asyncOp.isDone)
        {
            // You can check progress here if needed
            float progress = asyncOp.progress;
            Debug.Log($"NavMesh update progress: {progress * 100}%");

            yield return null;
        }

        Debug.Log("NavMesh update completed");
    }

    private void ResetAll()
    {
        foreach (var o in Oven.ovens)
        {
            o.ResetOven();
        }
        foreach (var g in GoblinTransporter.Goblins)
        {
            g.State.Reset();
        }
    }
}