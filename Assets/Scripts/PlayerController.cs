using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera m_Cam;

    private List<Tile> m_CurrentLinkingTiles = new List<Tile>();
    private Tile m_CurrentTile;

    private Grid m_Grid;

    private GameManager m_GameManager;
    private UIManager m_UIManager;
    private static PlayerController s_PlayerController;
    public PlayerController PController => s_PlayerController;

    private bool m_ExitGamePromptActive = false;
    public bool ExitGamePromptActive
    {
        get { return m_ExitGamePromptActive; }
        set { m_ExitGamePromptActive = value; }
    }
 
    private bool m_CanPlay = true;
    public bool CanPlay
    {
        get { return m_CanPlay; }
        set { m_CanPlay = value; }
    }

    private bool m_ClearingStarted = false;

    private void Awake()
    {
        s_PlayerController = this;
    }

    void Start()
    {
        m_Cam = Camera.main;
        m_Grid = FindObjectOfType<Grid>();
        m_GameManager = FindObjectOfType<GameManager>();
        m_UIManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        if (m_CanPlay)
        {
            if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
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
                                    m_CurrentTile = t;
                                    m_CurrentLinkingTiles.Add(t);
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

            if (Input.GetMouseButtonUp(0)) //When we release left mouse button
            {
                if (m_CurrentLinkingTiles.Count >= 3)
                {
                    StartClearingLink();
                }
                else
                {
                    ResetLink();
                }
            }

            if (Input.GetMouseButton(1)) //Right-click to reset the current link
            {
                ResetLink();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !m_GameManager.CheckWinState()) //we dont want the exit game prompt when in the nectlevel prompt
        {
            if (!m_ExitGamePromptActive)
            {
                m_UIManager.ManagerOfUI.SetExitGamePromptActive(true);
                m_CanPlay = false;
                m_ExitGamePromptActive = true;
            }
            else
            {
                m_UIManager.ManagerOfUI.SetExitGamePromptActive(false);
                m_CanPlay = true;
                m_ExitGamePromptActive = false;
            }
        }
    }

    private void ResetLink()
    {
        foreach (Tile t in m_CurrentLinkingTiles)
        {
            t.RestoreTile();
        }
        m_CurrentLinkingTiles.Clear();
    }

    private void StartClearingLink()
    {
        if (m_ClearingStarted == false)
        {
            m_ClearingStarted = true;
            m_CanPlay = false;
            StartCoroutine(DisableTiles());
        }
    }

    private IEnumerator DisableTiles()
    {
        for (int i = 0; i < m_CurrentLinkingTiles.Count; i++)
        {
            StartCoroutine(m_CurrentLinkingTiles[i].DeactivateTile());

            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(m_GameManager.Manager.ExplosionDelay + 0.01f);

        FinishTurn();
    }
    
    private void FinishTurn()
    {
        StopAllCoroutines();
        m_ClearingStarted = false;
        m_CanPlay = true;
        m_CurrentLinkingTiles.Clear();

        if (!m_GameManager.Manager.CheckWinState())
        {
            m_GameManager.Manager.CountMoves();
            StartCoroutine(m_Grid.PlayingGrid.UpdateGrid());
            m_Grid.PlayingGrid.RepopulateGrid();
        }
    }
}