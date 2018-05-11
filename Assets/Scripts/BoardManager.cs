using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    /*
     * This class should handle all terrain behaviour and tile generation
     * the goal is to pass some kind of id for the map
     * and for this class to fetch the approriate resource to generate the map tiles.
     */
    public static BoardManager Instance { set; get; }

    public List<GameObject> tilePrefabs;

    private Tile[,] TileMap { set; get; }
    private int[,] HeightMap { set; get; }

    public int MAP_SIZE { set; get; }
    public float TILE_SIZE { set; get; }
    public float TILE_OFFSET { set; get; }

    private void Start()
    {
        SpawnMap();
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
                SpawnTile(0, i, j);
            }
        }
    }

    //Spawns a single tile in the location given, of type index
    private void SpawnTile(int index, int x, int y)
    {
        GameObject go = Instantiate(tilePrefabs[index], GetGridCenter(x, y), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        TileMap[x, y] = go.GetComponent<Tile>();
    }


    //SUPPORT FUNCTIONS
    private Vector3 GetGridCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        origin.y -= 0.11f;
        return origin;
    }
}
