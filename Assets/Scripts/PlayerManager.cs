using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    /*
     * The PlayerManager will handle all player actions that would be specific to a single player
     * for instance selecting a unit, player team information ect.
     */
    public int team;

    public int MAP_SIZE { set; get; }
    public float TILE_SIZE { set; get; }
    public float TILE_OFFSET { set; get; }
}
