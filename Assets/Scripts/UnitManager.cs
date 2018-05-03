﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public int MAP_SIZE { set; get; }
    public float TILE_SIZE { set; get; }
    public float TILE_OFFSET { set; get; }

    private int selectedX = -1;
    private int selectedY = -1;
    private const float hitRange = 50f;

    public static UnitManager instance { set; get; }
    public GameObject Plane;
    public List<GameObject> unitPrefabs;
    private List<GameObject> activeUnits;
    public Unit[,] units { set; get; }

    private int[,] allowedMoves { set; get; }

    private Unit selectedUnit;



    private void Start()
    {
        instance = this;
        SpawnAllUnits();
        Vector3 h = new Vector3();
        h.x = (MAP_SIZE / 2);
        h.z = (MAP_SIZE / 2);
        this.Plane.transform.localPosition = h;
        this.Plane.transform.localScale = new Vector3(MAP_SIZE, 0, MAP_SIZE);

    }

    private void Update ()
    {
        UpdateSelection();
        DrawSelection();

        if(Input.GetMouseButtonDown(0))
        {
            if(selectedX >= 0 && selectedY >= 0)
            {
                SelectUnit(selectedX, selectedY);
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            if (selectedX >= 0 && selectedY >= 0 && selectedUnit != null)
            {
                MoveUnit(selectedX, selectedY);
            }
        }
	}

    public void ResetPlayer(int p)
    {
        DeselectUnit();
        foreach(GameObject u in activeUnits)
        {
            if(u.GetComponent<Unit>().Team == p)
            {
                u.GetComponent<Unit>().RefreshUnit();
            }
        }
    }

    private void SpawnAllUnits()
    {
        activeUnits = new List<GameObject>();
        units = new Unit[MAP_SIZE, MAP_SIZE];

        for (int i = 5; i < MAP_SIZE - 5; i++)
        {
            SpawnUnit(0, i, 1, 0, 0);
        }
        for (int i = 5; i < MAP_SIZE - 5; i++)
        {
            SpawnUnit(0, i, MAP_SIZE-2, 1, 180);
        }
    }

    private void SpawnUnit(int index, int x, int y, int t, float a)
    {
        GameObject go = Instantiate(unitPrefabs[index], GetTileCenter(x, y), Quaternion.Euler(0f, a, 0f)) as GameObject;
        go.transform.SetParent(transform);
        units[x, y] = go.GetComponent<Unit>();
        units[x, y].SetPosition(x, y);
        units[x, y].Team = t;
        activeUnits.Add(go);
    }

    private void SelectUnit(int x, int y)
    {
        if (units[x, y] == null)
            return;

        else if (units[x, y].Team != GameManager.Instance.playerTurn)
            return;

        allowedMoves = units[x, y].PossibleMove();
        DeselectUnit();
        selectedUnit = units[x, y];
        Board_Highlights.Instance.HighLightAllowedMoves(allowedMoves);
    }

    private void DeselectUnit()
    {
        Board_Highlights.Instance.HideHighlights();
        selectedUnit = null;
    }

    private void MoveUnit(int x, int y)
    {
        if (allowedMoves[x, y] == 1)
        {
            units[selectedUnit.currentX, selectedUnit.currentY] = null;
            selectedUnit.transform.position = GetTileCenter(x, y);
            selectedUnit.SetPosition(x, y);
            units[x, y] = selectedUnit;
            units[x, y].moveRem = 0;
        }
        DeselectUnit();
    }

    private void Combat(Unit a, Unit b)
    {

    }

    private void DrawSelection()
    {
        if(selectedX >= 0 && selectedY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectedY + Vector3.right * selectedX,
                Vector3.forward * (selectedY + 1) + Vector3.right * (selectedX + 1));
            Debug.DrawLine(
                 Vector3.forward * (selectedY + 1) + Vector3.right * selectedX,
                 Vector3.forward * selectedY + Vector3.right * (selectedX + 1));
        }
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, hitRange, LayerMask.GetMask("BoardPlane")))
        {
            selectedX = (int)hit.point.x;
            selectedY = (int)hit.point.z;
        }
        else
        {
            selectedX = -1;
            selectedY = -1;
        }
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

}
