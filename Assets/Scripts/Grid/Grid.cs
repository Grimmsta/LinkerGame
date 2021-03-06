﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] 
    private Tile m_TilePrefab;
    [SerializeField, Min(1), Tooltip("Creates some space between the tiles")]
    private float m_Padding = 1;
    [SerializeField, Tooltip("Insert the width of the level info UI to correctly place the grid between the UI and the end of the screen")] 
    private float m_UIBoxWidth = 318;

    private static Grid s_Grid;
    public Grid PlayingGrid => s_Grid;

    private Vector2 m_GridSize;
    private Vector3 m_ScreenSize;

    private Tile[,] m_AllTiles; //The tiles we have spawned in
    private Vector3[,] m_GridPositions; //All the positions the tiles can have

    private const float k_SmallNumber = 0.001f;
    private float m_MaxDistance;
    public float MaxDistance => m_MaxDistance;

    private GameManager m_GameManager;

    private float m_BottomOfTheGrid;
    private float m_TileRadius = 0.5f; //I know the tiles size is one unit, this is hardcoded

    private void Awake()
    {
        s_Grid = this;
    }

    void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        m_ScreenSize = Camera.main.ScreenToWorldPoint(new Vector3((Screen.width + 318) / 2, Screen.height / 2, Camera.main.nearClipPlane)); //315 is the width of the UI on the left side, we want to place our grid  
        m_MaxDistance = ((Mathf.Sqrt(2f) + k_SmallNumber) * m_Padding); //Sqrt(2) is the distance between 2 positions ex. (0,0) and (1,1) aka diagonal 
    }

    public void DrawGrid()
    {
        ClearGrid();
        m_GridSize = m_GameManager.Manager.GridSize;

        m_AllTiles = new Tile[(int)m_GridSize.x, (int)m_GridSize.y];

        m_GridPositions = new Vector3[(int)m_GridSize.x, (int)m_GridSize.y];

        m_BottomOfTheGrid = ((m_ScreenSize.y - m_GridSize.y) / 2 + m_TileRadius) * m_Padding;

        for (int x = 0; x < m_GridSize.x; x++)
        {
            for (int y = 0; y < m_GridSize.y; y++)
            {
                SpawnTile(x, y);
            }
        }
    }

    private void SpawnTile(int x, int y)
    {
        Tile tileToSpawn = m_TilePrefab;
        Tile newTile = Instantiate(tileToSpawn, new Vector2(m_ScreenSize.x - m_GridSize.x / 2 + m_TileRadius + x, m_ScreenSize.y - m_GridSize.y / 2 + m_TileRadius + y) * m_Padding, Quaternion.identity);
        m_AllTiles[x, y] = newTile;
        m_GridPositions[x, y] = newTile.transform.position;
    }

    public IEnumerator UpdateGrid()
    {
        Tile previousTile = null;
        Tile currentTile;

        //Move all active tiles down to fill the empty slots beneath them
        for (int x = 0; x < m_GridSize.x; x++)
        {
            for (int y = 0; y < m_GridSize.y; y++)
            {
                if (GridPosHasATile(x, y)) //does this position have a tile?
                {
                    currentTile = GridPosHasATile(x, y);
                    if (currentTile.transform.position.y != m_BottomOfTheGrid)
                    {
                        if (previousTile == null) //If its the first tile we are looking at, we want to set it at the bottom
                        {
                            currentTile.transform.position = new Vector3(currentTile.transform.position.x, m_BottomOfTheGrid);
                        }
                        else if (previousTile.transform.position.y >= currentTile.transform.position.y) //if we the previous tile is higher than the current
                        {
                            currentTile.transform.position = new Vector3(currentTile.transform.position.x, m_BottomOfTheGrid);
                        }
                        else if (previousTile.transform.position.y <= currentTile.transform.position.y && previousTile.transform.position.x != currentTile.transform.position.x) //if the previous tile is lower but not in the same column as the current tile
                        {
                            currentTile.transform.position = new Vector3(currentTile.transform.position.x, m_BottomOfTheGrid);
                        }
                        else if (Vector2.Distance(currentTile.transform.position, previousTile.transform.position) >= m_Padding) //if the distance between the previous and current tile is bigger than the padding
                        {
                            currentTile.transform.position = new Vector3(currentTile.transform.position.x, previousTile.transform.position.y + m_Padding);
                        }
                    }
                    previousTile = currentTile;
                }
            }
        }
        yield return new WaitForSeconds(0.1f);

        //After we moved all active tiles, we add the inactive tiles to the grid again with a new color
        RepopulateGrid();
    }

    private Tile GridPosHasATile(int x, int y)
    {
        foreach (var t in m_AllTiles)
        {
            if (m_GridPositions[x, y] == t.transform.position)
            {
                if (t.isActiveAndEnabled)
                {
                    return t;
                }
            }
        }
        return null;
    }

    public void RepopulateGrid()
    {
        List<Vector3> emptyPositions = new List<Vector3>();
        List<Tile> disabledTiles = new List<Tile>();

        for (int x = 0; x < m_GridSize.x; x++) //Gets all empty slots on the grid
        {
            for (int y = 0; y < m_GridSize.y; y++)
            {
                if (GridPosHasATile(x, y) == false)
                {
                    emptyPositions.Add(m_GridPositions[x, y]);
                }
            }
        }

        foreach (var t in m_AllTiles) //Gets all inactivated tiles
        {
            if (!t.isActiveAndEnabled)
            {
                disabledTiles.Add(t);
            }
        }

        for (int i = 0; i < disabledTiles.Count; i++) // Gives the inactive tiles a slot on the grid
        {
            disabledTiles[i].transform.position = emptyPositions[i];

            disabledTiles[i].ActivateTile();
        }
    }

    public void ClearGrid()
    {
        for (int x = 0; x < m_GridSize.x; x++)
        {
            for (int y = 0; y < m_GridSize.y; y++)
            {
                Destroy(m_AllTiles[x, y].gameObject);
                m_AllTiles[x, y] = null;
                m_GridPositions[x, y] = Vector3.zero;
            }
        }
    }
}