using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSpawner : MonoBehaviour
{
    [SerializeField] private TownPlacer townplace;
    [SerializeField] private TreePlacer_3 forestplace;
    [SerializeField] private TreePlacer_3 treeplace;
    [SerializeField] private CrystalPlacer crystalplace;

    [ContextMenu("SpawnWorld")]
    public void SpawnWorld()
    {
        townplace.SpawnTown();
        forestplace.SpawnDebug();
        treeplace.SpawnDebug();
        crystalplace.SpawnDebug();
    }

    [ContextMenu("ClearWorld")]
    public void ClearWorld()
    {
        townplace.ClearObjects();
        forestplace.ClearObjects();
        treeplace.ClearObjects();
        crystalplace.ClearObjects();
    }
}
