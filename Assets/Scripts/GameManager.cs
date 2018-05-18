using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*
     * This will be the overall game manager, to set up each match and
     * communicate between the Unitmanager, Board manager ect
     */
    public static GameManager Instance { set; get; }

    private const int MAP_SIZE = 30;
    private const float TILE_SIZE = 1f;
    private const float TILE_OFFSET = 0.5f;
    private const float TILE_HEIGHT = 0.2f;

    public GameObject UnitManager;
    public GameObject BoardManager;
    public GameObject EventManager;
    public GameObject AudioManager;
    public UnitManager activeUnitManager { set; get; }
    public BoardManager activeBoardManager { set; get; }
    public EventManager activeEventManager { set; get; }
    public AudioManager activeAudioManager { set; get; }
    public PlayerManager activePlayerManager { set; get; }

    public int playerTurn { set; get; }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100),  ((int) (1.0f / Time.smoothDeltaTime)).ToString());
    }

    void Start ()
    {
        Instance = this;
        playerTurn = 0;
        CreateBM();
        CreateEM();
        CreateUM();
	}
	
	void Update () {
		if(Input.GetKeyDown("space"))
        {
            EndTurn();
        }
	}

    private void CreateUM()
    {
        GameObject go = Instantiate(UnitManager, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        activeUnitManager = go.GetComponent<UnitManager>();
        activeUnitManager.MAP_SIZE = MAP_SIZE;
        activeUnitManager.TILE_SIZE = TILE_SIZE;
        activeUnitManager.TILE_OFFSET = TILE_OFFSET;
    }

    private void CreateBM()
    {
        GameObject go = Instantiate(BoardManager, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        activeBoardManager = go.GetComponent<BoardManager>();
        activeBoardManager.MAP_SIZE = MAP_SIZE;
        activeBoardManager.TILE_SIZE = TILE_SIZE;
        activeBoardManager.TILE_OFFSET = TILE_OFFSET;
        activeBoardManager.TILE_HEIGHT = TILE_HEIGHT;
    }

    private void CreateEM()
    {
        GameObject go = Instantiate(EventManager, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        activeEventManager = go.GetComponent<EventManager>();
    }

    private void EndTurn()
    {
        playerTurn++;
        playerTurn = playerTurn % 2;
        activeUnitManager.ResetPlayer(playerTurn);
    }
}
