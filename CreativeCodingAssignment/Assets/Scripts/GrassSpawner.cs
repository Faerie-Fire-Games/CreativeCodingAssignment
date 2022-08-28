using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
    [Header("Positioning")]
    public Vector2 Start = new Vector2(0f, 0f);
    public Vector2 Size = new Vector2(1500f, 1500f);
    public float MaxHeight = 600f; //this is my terrain height
    public LayerMask ValidLayers;

    [Header("Appearance")]
    public GameObject[] GrassPrefabs;
    public float sizeVariance = 0.2f;

    [Header("Placement Restrictions")]
    public float NoiseScale;
    [Range(0f, 1f)] public float NoiseThreshold;

    [Header("Debug")]
    public int DebugSpawnCount = 1000;

    [ContextMenu("Spawn Debug")]
    public void SpawnDebug()
    {
        ClearObjects();
        Spawn(DebugSpawnCount);
    }

    [ContextMenu("Check Object Counts")]
    public void CheckObjectCounts()
    {
        var totalCount = 0;
        for (var i = 0; i < transform.childCount; i++)
        {
            totalCount += transform.GetChild(i).childCount;
        }

        Debug.Log($"{transform.childCount} top-level objects, {totalCount} objects in total");
    }

    public void Spawn(int count)
    {
        print("Trying");
        var noiseOffset = Random.value * 10000f;

        for (var i = 0; i < count; i++)
        {
            //We choose the tree and scale now, because we need that information to perform the Spherecast

            var rayPos = new Vector3(Start.x + Size.x * Random.value, MaxHeight, Start.y + Size.y * Random.value);

            //This time, we do a sphere cast, which is kinda like a 'thick' raycast with a provided radius
            //We use the tree's canopy radius (set in the scriptable object) to make sure that it won't intersect any other trees
            if (!Physics.Raycast(new Ray(rayPos, Vector3.down), out var hit, MaxHeight)) continue;

            //If it hit an object on an invalid layer, we also stop here
            if (!MaskContainsLayer(ValidLayers, hit.collider.gameObject.layer)) continue;

            //If the hit point is above the tree line, we don't spawn a tree
            if (hit.point.y < 295) continue;

            //We use some simple noise to vary the tree density
            var noiseVal = Mathf.PerlinNoise(hit.point.x * NoiseScale + noiseOffset, hit.point.z * NoiseScale);
            if (noiseVal < NoiseThreshold) continue;

            Debug.LogWarning("Grass At: " + hit.point);

            //But if it *does* hit something, a bunch of information about the ray hit is stored in the 'hit' variable
            PlaceGrassAt(transform, hit.point);
        }
    }

    public void PlaceGrassAt(Transform parent, Vector3 position)
    {
        var newObj = Instantiate(GrassPrefabs[Random.Range(0, GrassPrefabs.Length)]);
        newObj.transform.position = position + Vector3.up * 0.6f;
        newObj.transform.parent = parent;
        newObj.transform.eulerAngles = new Vector3(0f, Random.value * 360f, 0f);
        newObj.transform.localScale = Vector3.one * Random.Range(1f - sizeVariance, 1f + sizeVariance);
    }

    [ContextMenu("Clear Objects")]
    public void ClearObjects()
    {
        //We iterate backwards through children, since if we go forwards we end up changing child indices
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            //DestroyImmediate is for edit-mode, Destroy is for play-mode (including in the built application)
            if (Application.isPlaying) Destroy(transform.GetChild(i).gameObject);
            else DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    //I swear I've rewritten this function so many times, so now I just copy-paste it whenever I need it
    //Don't worry if you don't understand it, bitwise coding is used for basically nothing other than this specific purpose
    public static bool MaskContainsLayer(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
