using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class BoardManager : MonoBehaviour
{
    /*
     * This class should handle all terrain behaviour and tile generation
     * the goal is to pass some kind of id for the map
     * and for this class to fetch the approriate resource to generate the map tiles.
     */
    public int MAP_SIZE { set; get; }
    public float TILE_SIZE { set; get; }
    public float TILE_OFFSET { set; get; }
    public float TILE_HEIGHT { set; get; }

    public List<GameObject> tilePrefabs;

    private Tile[,] TileMap { set; get; }
    private int[,] HeightMap { set; get; }

    private void Start()
    {
        SpawnMap();
        SpawnGrid();
    }

    void SpawnGrid()
    {
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        var mesh = new Mesh();
        var verticies = new List<Vector3>();

        var indicies = new List<int>();
        for (int i = 0; i <= MAP_SIZE; i++)
        {
            verticies.Add(new Vector3(i, 0, 0));
            verticies.Add(new Vector3(i, 0, MAP_SIZE));

            indicies.Add(4 * i + 0);
            indicies.Add(4 * i + 1);

            verticies.Add(new Vector3(0, 0, i));
            verticies.Add(new Vector3(MAP_SIZE, 0, i));

            indicies.Add(4 * i + 2);
            indicies.Add(4 * i + 3);
        }

        mesh.vertices = verticies.ToArray();
        mesh.SetIndices(indicies.ToArray(), MeshTopology.Lines, 0);
        filter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        meshRenderer.material.color = Color.black;
    }

    //Spawn map will create a grid of tiles, the size determined by MAP_SIZE, in the future this function needs to
    //take parameters to dictate the kind of map that will be created
    private void SpawnMap()
    {
        TileMap = new Tile[MAP_SIZE, MAP_SIZE];
        HeightMap = new int[MAP_SIZE, MAP_SIZE];

        for (int i = 0; i < MAP_SIZE; i++)
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                HeightMap[i, j] = (int)(10 * Mathf.PerlinNoise(i * 0.1f, j * 0.1f));
            }
        }

        for (int i = 0; i < MAP_SIZE; i++)
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                SpawnTile((HeightMap[i, j] / 5), i, j, HeightMap[i, j]);
            }
        }
    }

    //Spawns a single tile in the location given, of type index
    private void SpawnTile(int index, int x, int y, int z)
    {
        GameObject go = Instantiate(tilePrefabs[index], GetGridCenter(x, y, z), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        TileMap[x, y] = go.GetComponent<Tile>();
    }

    //SUPPORT FUNCTIONS
    private Vector3 GetGridCenter(int x, int y, int z)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        origin.y -= 0.11f;
        origin.y += z * TILE_HEIGHT;
        return origin;
    }

    public float getHeight(int x, int y)
    {
        return (HeightMap[x, y] * TILE_HEIGHT);
    }
}
