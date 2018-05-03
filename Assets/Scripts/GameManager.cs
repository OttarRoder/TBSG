using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    //This will be the overall game manager, to set up each match and communicate between the Unitmanager, Board manager ect
    public static GameManager Instance { set; get; }

    private const int MAP_SIZE = 20;
    private const float TILE_SIZE = 1f;
    private const float TILE_OFFSET = 0.5f;

    public GameObject UM;
    public GameObject BM;
    private UnitManager activeUM;
    private BoardManager activeBM;

    public int playerTurn = 0;

	void Start () {
        Instance = this;
        CreateUM();
        CreateBM();
	}
	
	void Update () {
		if(Input.GetKeyDown("e"))
        {
            EndTurn();
        }
	}

    private void CreateUM()
    {
        Vector3 center = Vector3.zero;
        GameObject go = Instantiate(UM, center, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        activeUM = go.GetComponent<UnitManager>();
        activeUM.MAP_SIZE = MAP_SIZE;
        activeUM.TILE_SIZE = TILE_SIZE;
        activeUM.TILE_OFFSET = TILE_OFFSET;
    }

    private void CreateBM()
    {
        Vector3 center = Vector3.zero;
        GameObject go = Instantiate(BM, center, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        activeBM = go.GetComponent<BoardManager>();
        activeBM.MAP_SIZE = MAP_SIZE;
        activeBM.TILE_SIZE = TILE_SIZE;
        activeBM.TILE_OFFSET = TILE_OFFSET;
    }

    private void EndTurn()
    {
        playerTurn++;
        playerTurn = playerTurn % 2;
        activeUM.ResetPlayer(playerTurn);
    }
}
