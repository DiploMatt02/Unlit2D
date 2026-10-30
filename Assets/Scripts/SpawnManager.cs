using UnityEngine;
using NavMeshPlus.Components;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] layouts;
    [SerializeField] private NavMeshSurface navSurface;
    [SerializeField] private GameObject[] items;
    [SerializeField] private GameObject[] enemies;

    void Start()
    {
        //Pick a layout and hide the rest
        int picked = Random.Range(0, layouts.Length);

        for (int i = 0; i < layouts.Length; i++)
        {
            layouts[i].gameObject.SetActive(i == picked);
        }

        //Rebuild Walkable Surface for chosen layout
        navSurface.BuildNavMesh();

        Transform layout = layouts[picked];
        PlaceObjects(items, GetChildren(layout.Find("ItemSpawnPoints")));
        PlaceObjects(enemies, GetChildren(layout.Find("EnemySpawnPoints")));
    }

    private Transform[] GetChildren(Transform parent)
    {
        Transform[] children = new Transform[parent.childCount];
        for (int i = 0; i < parent.childCount; i++)
        {
            children[i] = parent.GetChild(i);
        }
        return children;
    }

    private void Shuffle(Transform[] points)
    {
        for (int i = points.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1 );

            Transform temp = points[i];
            points[i] = points[randomIndex];
            points[randomIndex] = temp;
        }
    }

    private void PlaceObjects(GameObject[] objects, Transform[] points)
    {
        if (points.Length < objects.Length)
        {
            Debug.LogWarning("Not enough spawn points. Need at least" + objects.Length);
            return;
        }
        Shuffle(points);

        for (int i = 0; i < objects.Length; i++)
        {
            Vector3 position = points[i].position;

            NavMeshAgent agent = objects[i].GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.Warp(position);
            }
            else
            {
                objects[i].transform.position = position;
            }
        }
    }
}
