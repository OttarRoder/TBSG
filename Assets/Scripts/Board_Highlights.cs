using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board_Highlights : MonoBehaviour
{
    //Handles all selection highlight functions
    public static Board_Highlights Instance { set; get; }

    public GameObject highlightPrefab;
    public Material def_mat;
    public Material sef_mat;
    public Material ene_mat;

    private const int MAP_SIZE = 20;
    private const float TILE_OFFSET = 0.5f;

    private List<GameObject> highlights;
    private Renderer rend;


    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
    }

    //Will find any current highlight objects and reuse them if possible, else generate new ones
    private GameObject GetHighlightObject()
    {
        GameObject go = highlights.Find(g => !g.activeSelf);

        if (go == null)
        {
            go = Instantiate(highlightPrefab);
            highlights.Add(go);
        }

        return go;
    }

    //Passed a grid of values, will display highlights on the map with the correspondance of
    //0 = none, 1 = white, 2 = red, 5 = green
    public void HighLightAllowedMoves(MoveNode[,] moves)
    {
        for(int i = 0; i < MAP_SIZE; i++)
        {
            for(int j = 0; j < MAP_SIZE; j++)
            {
                if(moves [i , j].code != 0)
                {
                    GameObject go = GetHighlightObject();
                    go.SetActive(true);
                    go.transform.position = new Vector3(i + TILE_OFFSET, 0, j + TILE_OFFSET);

                    rend = go.GetComponent<Renderer>();
                    if (moves[i, j].code == 1)
                    {
                        rend.sharedMaterial = def_mat;
                    }
                    else if (moves[i, j].code == 2)
                    {
                        rend.sharedMaterial = ene_mat;
                    }
                    else if (moves[i, j].code == 5)
                    {
                        rend.sharedMaterial = sef_mat;
                    }
                }
            }
        }
    }

    public void HideHighlights()
    {
        foreach (GameObject go in highlights)
        {
            go.SetActive(false);
        }
    }
}
