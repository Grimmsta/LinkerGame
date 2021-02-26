using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private Camera m_Cam;
    private List<Tile> m_CurrentLinkingTiles = new List<Tile>();
    private Tile m_CurrentTile;
    private Grid m_Grid;
    private GameManager m_Manager;

    void Start()
    {
        m_Cam = Camera.main;
        m_Grid = FindObjectOfType<Grid>();
        m_Manager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 ray = m_Cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.transform.TryGetComponent(out Tile t)) //t being the current tile the mouse is on
                {
                    if (m_CurrentLinkingTiles.Count > 0 && m_CurrentLinkingTiles[0].TypeOfTile == t.TypeOfTile) //If the list has elements and the tile we are on is the same type as the first in the list
                    {
                        if (!m_CurrentLinkingTiles.Contains(t)) //We check if the list already contains the Tile we are on
                        {
                            if (Vector2.Distance(m_CurrentTile.transform.position, t.transform.position) <= m_Grid.PlayingGrid.MaxDistance)
                            {
                                m_CurrentLinkingTiles.Add(t);
                                m_CurrentTile = t;
                                t.HighlightTile();
                            }
                        }
                        else if (m_CurrentLinkingTiles.Count > 1 && t == m_CurrentLinkingTiles[m_CurrentLinkingTiles.Count - 2]) //We check if the tile we are on is the previous tile visited
                        {
                            m_CurrentTile.RestoreTile();
                            m_CurrentLinkingTiles.Remove(m_CurrentTile);
                            m_CurrentTile = t;
                        }
                    }
                    else if (m_CurrentLinkingTiles.Count == 0)
                    {
                        m_CurrentTile = t;
                        m_CurrentLinkingTiles.Add(t);
                        t.HighlightTile();
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (m_CurrentLinkingTiles.Count >= 3)
            {
                foreach (Tile t in m_CurrentLinkingTiles)
                {
                    t.DeactivateTile();
                }
                m_Grid.PlayingGrid.UpdateGrid();

                m_Manager.Manager.CountScore(m_CurrentLinkingTiles.Count);
                m_Manager.Manager.CountMoves();
            }
            else
            {
                foreach (Tile t in m_CurrentLinkingTiles)
                {
                    t.RestoreTile();
                }
            }

            m_CurrentLinkingTiles.Clear();
        }

        if (Input.GetMouseButtonDown(1))
        {
            foreach (Tile t in m_CurrentLinkingTiles)
            {
                t.RestoreTile();
            }

            m_CurrentLinkingTiles.Clear();
        }
    }
}
