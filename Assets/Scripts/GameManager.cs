using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    /*
     * This will be the overall game manager, to set up each match and
     * communicate between the Unitmanager, Board manager ect
     */
    public static GameManager Instance { set; get; }

    private const int MAP_SIZE = 20;
    private const float TILE_SIZE = 1f;
    private const float TILE_OFFSET = 0.5f;

    public GameObject UM;
    public GameObject BM;
    public GameObject EM;
    public UnitManager activeUM { set; get; }
    public BoardManager activeBM { set; get; }
    public EventManager activeEM { set; get; }

    public int playerTurn { set; get; }

	void Start () {
        Instance = this;
        playerTurn = 0;
        CreateUM();
        CreateBM();
        CreateEM();
	}
	
	void Update () {
		if(Input.GetKeyDown("space"))
        {
            EndTurn();
        }
	}

    private void CreateUM()
    {
        GameObject go = Instantiate(UM, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        activeUM = go.GetComponent<UnitManager>();
        activeUM.MAP_SIZE = MAP_SIZE;
        activeUM.TILE_SIZE = TILE_SIZE;
        activeUM.TILE_OFFSET = TILE_OFFSET;
    }

    private void CreateBM()
    {
        GameObject go = Instantiate(BM, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        activeBM = go.GetComponent<BoardManager>();
        activeBM.MAP_SIZE = MAP_SIZE;
        activeBM.TILE_SIZE = TILE_SIZE;
        activeBM.TILE_OFFSET = TILE_OFFSET;
    }

    private void CreateEM()
    {
        GameObject go = Instantiate(EM, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        activeEM = go.GetComponent<EventManager>();
    }

    private void EndTurn()
    {
        playerTurn++;
        playerTurn = playerTurn % 2;
        activeUM.ResetPlayer(playerTurn);
    }
}
