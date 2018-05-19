using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    /*
     * The PlayerManager will handle all player actions that would be specific to a single player
     * for instance selecting a unit, player team information ect.
     */
    public int MAP_SIZE { set; get; }
    public float TILE_SIZE { set; get; }
    public float TILE_OFFSET { set; get; }

    public Camera playerCamera;
    public Text HealthText;
    public Text MoveText;
    public Text AttackText;
    public Text DefenceText;

    public int Team { set; get; }

    private float hitRange = 100f;
    private int SelectedX;
    private int SelectedY;
    private int SavedSelectedX;
    private int SavedSelectedY;
    private bool ClickMove;
    private Unit SelectedUnit;
    private MoveNode[,] MoveGrid;

    private void Start()
    {
        ClickMove = false;
    }

    private void Update()
    {
        UpdateSelection();
        if (Input.GetMouseButtonDown(0))
        {
            if (ClickMove)
            {
                ClickMove = false;
            }
            else if (SelectedX >= 0 && SelectedY >= 0)
            {
                SelectUnit(SelectedX, SelectedY);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (SelectedX >= 0 && SelectedY >= 0 && SelectedUnit != null)
            {
                ClickMove = true;
                SavedSelectedX = SelectedX;
                SavedSelectedY = SelectedY;
            }
        }
        if (Input.GetMouseButtonUp(1) && ClickMove == true)
        {
            int x = SavedSelectedX - SelectedX;
            int y = SavedSelectedY - SelectedY;
            GameManager.Instance.activeUnitManager.MoveUnit(SelectedUnit, MoveGrid, SavedSelectedX, SavedSelectedY, new Vector3(x,0,y));
            SelectUnit(SavedSelectedX, SavedSelectedY);
            ClickMove = false;
        }

        if (Input.GetKeyDown("escape"))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void SelectUnit(int x, int y)
    {
        //Clear selection
        DeselectUnit();

        Unit unit = GameManager.Instance.activeUnitManager.GetUnitAt(x, y);

        if (unit == null)
        {
            return;
        }

        SelectedUnit = unit;
        if (unit.Team == Team && GameManager.Instance.playerTurn == Team)
        {
            MoveGrid = GameManager.Instance.activeUnitManager.PossibleMove(unit);
            Board_Highlights.Instance.HighLightAllowedMoves(MoveGrid);
        }

        HealthText.text = (SelectedUnit.healthRem).ToString() + "/" + (SelectedUnit.Health).ToString();
        MoveText.text = ((SelectedUnit.moveRem).ToString() + "/" + (SelectedUnit.Move).ToString());
        AttackText.text = (SelectedUnit.AttackLow).ToString() + "-" + (SelectedUnit.AttackHigh).ToString();
        DefenceText.text = (SelectedUnit.Defence).ToString();
    }

    private void DeselectUnit()
    {
        Board_Highlights.Instance.HideHighlights();
        SelectedUnit = null;
        HealthText.text = "";
        MoveText.text = "";
        AttackText.text = "";
        DefenceText.text = "";
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out hit, hitRange, LayerMask.GetMask("BoardPlane")))
        {
            SelectedX = (int)hit.point.x;
            SelectedY = (int)hit.point.z;
        }
        else
        {
            SelectedX = -1;
            SelectedY = -1;
        }
    }
}
