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
        //surface.UpdateNavMesh(surface.navMeshData);
        surface.BuildNavMesh();
        //ResetAll();
    }
    private void ResetAll()
    {
        foreach(var o in Oven.ovens)
        {
            o.Reset();
        }
        foreach(var g in GoblinTransporter.Goblins)
        {
            g.State.Reset();
        }
    }
}