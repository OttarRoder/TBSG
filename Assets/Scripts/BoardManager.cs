using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        int counter = 0;
        float h = 0.6f;
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        List<Vector3> verticies = new List<Vector3>();

        List<int> indicies = new List<int>();
        for (int i = 0; i < MAP_SIZE; i++)
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                h = GetHeight(i, j);
                for(int k = 0; k < 8; k++)
                {
                    indicies.Add(counter);
                    counter++;
                }

                verticies.Add(new Vector3(i, h, j));
                verticies.Add(new Vector3(i, h, j+1));

                verticies.Add(new Vector3(i, h, j));
                verticies.Add(new Vector3(i+1, h, j));

                verticies.Add(new Vector3(i, h, j+1));
                verticies.Add(new Vector3(i+1, h, j+1));

                verticies.Add(new Vector3(i+1, h, j));
                verticies.Add(new Vector3(i+1, h, j+1));
            }
        }

        mesh.vertices = verticies.ToArray();
        mesh.SetIndices(indicies.ToArray(), MeshTopology.Lines, 0);
        filter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        meshRenderer.material.color = Color.grey;
    }

    //Spawn map will create a grid of tiles, the size determined by MAP_SIZE, in the future this function needs to
    //take parameters to dictate the kind of map that will be created
    private void SpawnMap()
    {
        TileMap = new Tile[MAP_SIZE, MAP_SIZE];
        HeightMap = new int[MAP_SIZE, MAP_SIZE];
        float seed = (DateTime.UtcNow.Millisecond);
        Debug.Log(seed);

        for (int i = 0; i < MAP_SIZE; i++)
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                HeightMap[i, j] = (int)(10 * Mathf.PerlinNoise((i + seed)* 0.1f, (j + seed)* 0.1f));
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

    //Returns the coordinate positon a tile should be at given x, y and height
    private Vector3 GetGridCenter(int x, int y, int z)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        origin.y -= 0.11f;
        origin.y += z * TILE_HEIGHT;
        return origin;
    }

    //Returns the Height of the tile at x, y
    public float GetHeight(int x, int y)
    {
        return (HeightMap[x, y] * TILE_HEIGHT);
    }
}
