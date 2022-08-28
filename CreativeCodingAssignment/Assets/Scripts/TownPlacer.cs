using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownPlacer : MonoBehaviour
{
    [Header("Positioning")]
    [SerializeField] private Vector2 Start = new Vector2(0f, 0f);
    [SerializeField] private Vector2 Size = new Vector2(1500f, 1500f);
    [SerializeField] private float MaxHeight = 600f; //this is my terrain height
    [SerializeField] private LayerMask ValidLayers;

    [SerializeField] private int townAttempts = 500;
    [SerializeField] private int townRadius = 100;
    [SerializeField] private int buildingAttempts = 15;

    [SerializeField] private TreeSetup[] buildingPrefabs; //Treesetup is fine because all I really need is the canopy size. No need to rewrite anything

    [SerializeField] private Transform townPoint;

    [ContextMenu("SpawnTown")]
    public void SpawnTown()
    {
        ClearObjects();
        FindTownLocation(townAttempts);
    }

    public void FindTownLocation(int attempts)
    {
        for(var i = 0; i < attempts; i++)
        {
            var rayPos = new Vector3(Start.x + Size.x * Random.value, MaxHeight, Start.y + Size.y * Random.value);

            if (!Physics.Raycast(new Ray(rayPos, Vector3.down), out var hit, MaxHeight)) continue;

            if (!MaskContainsLayer(ValidLayers, hit.collider.gameObject.layer)) continue;

            if (hit.point.y < 294) continue;

            var rayUP = new Vector3(hit.point.x, MaxHeight, hit.point.z + townRadius);
            var rayRIGHT = new Vector3(hit.point.x + townRadius, MaxHeight, hit.point.z);
            var rayDOWN = new Vector3(hit.point.x, MaxHeight, hit.point.z - townRadius);
            var rayLEFT = new Vector3(hit.point.x - townRadius, MaxHeight, hit.point.z);

            if (Physics.Raycast(new Ray(rayUP, Vector3.down), out var UP, Mathf.Infinity)) 
            {
                if (UP.point.y != hit.point.y && MaskContainsLayer(ValidLayers, UP.collider.gameObject.layer)) continue;
            }
            if (Physics.Raycast(new Ray(rayRIGHT, Vector3.down), out var RIGHT, Mathf.Infinity))
            {
                if (RIGHT.point.y != hit.point.y && MaskContainsLayer(ValidLayers, RIGHT.collider.gameObject.layer)) continue;
            }
            if (Physics.Raycast(new Ray(rayDOWN, Vector3.down), out var DOWN, Mathf.Infinity))
            {
                if (DOWN.point.y != hit.point.y && MaskContainsLayer(ValidLayers, DOWN.collider.gameObject.layer)) continue;
            }
            if (Physics.Raycast(new Ray(rayLEFT, Vector3.down), out var LEFT, Mathf.Infinity))
            {
                if (LEFT.point.y != hit.point.y && MaskContainsLayer(ValidLayers, LEFT.collider.gameObject.layer)) continue;
            }

            //Debug.LogWarning("TOWN AT: " + hit.point);
            townPoint.position = hit.point;

            FindHouses(buildingAttempts, hit.point);
        }
    }

    public void FindHouses(int attempts, Vector3 loc)
    {
        for (var i = 0; i < attempts; i++)
        {
            var building = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];

            //Choose a point for a new building by going some amount away from the town center location. 
            var rayPos = loc + new Vector3((Random.value * 2 - 1) * townRadius - 23, MaxHeight, (Random.value * 2 - 1) * townRadius - 23);

            //if building would spawn on very edge of map, don't spawn it.
            if (rayPos.x < Start.x + 16 || rayPos.x > Start.x + Size.x - 16 || rayPos.z < Start.y + 16 || rayPos.z > Start.y + Size.y - 16) continue;

            if (!Physics.SphereCast(new Ray(rayPos, Vector3.down), building.CanopyRadius * 1.7f, out var hit, MaxHeight)) continue;

            if (!MaskContainsLayer(ValidLayers, hit.collider.gameObject.layer)) continue;


            print("FindHouses");
            print(hit.collider.gameObject.layer);

            print("Hit At: " +  hit.transform.position);

            PlaceBuilding(building.Prefab, townPoint.position);
            HouseDebug(hit.point);
        }
    }

    public GameObject PlaceBuilding(GameObject prefab, Vector3 position)
    {
        print("New Building");

        var newObj = Instantiate(prefab);
        newObj.transform.parent = transform;
        newObj.transform.position = position;
        newObj.transform.eulerAngles = new Vector3(0, Random.value * 360, 0);

        Physics.SyncTransforms();

        return newObj;
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

    public void HouseDebug(Vector3 pos)
    {
        townPoint.position = pos;
        print("Debug");
    }

    public static bool MaskContainsLayer(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
