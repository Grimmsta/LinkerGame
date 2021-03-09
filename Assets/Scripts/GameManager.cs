using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int m_ScorePerTile = 100;
    public int ScorePerTile => m_ScorePerTile;

    [SerializeField] private List<Level> m_Levels = new List<Level>();

    [SerializeField, Tooltip("How much time will pass between each tile pop in seconds")]
    private float m_ExplodeDelay;
    public float ExplosionDelay => m_ExplodeDelay;

    private int m_LevelID = -1;
    public int LevelID => m_LevelID;

    private int m_Moves = 12;
    public int Moves => m_Moves;

    private int m_GoalScore = 2000;
    public int GoalScore => m_GoalScore;

    private static GameManager s_Manager;
    public GameManager Manager => s_Manager;

    private Vector2 m_GridSize;
    public Vector2 GridSize => m_GridSize;

    private UIManager m_UIManager;
    private Grid m_Grid;

    private int m_CurrentScore = 0;

    private void Awake()
    {
        s_Manager = this;
    }

    void Start()
    {
        m_UIManager = FindObjectOfType<UIManager>();
        m_Grid = FindObjectOfType<Grid>();

        LoadLevel();
    }

    public void CountScore()
    {
        m_CurrentScore += m_ScorePerTile;
        m_UIManager.ManagerOfUI.UpdateScore(m_CurrentScore);
    }

    public bool CheckWinState()
    {
        if (m_CurrentScore >= m_GoalScore)
        {
            if (m_LevelID == m_Levels.Count - 1)
            {
                m_UIManager.ManagerOfUI.SetGameCompletePrompActive(true);
            }
            else
            {
                m_UIManager.ManagerOfUI.SetLevelCompletePromptActive(true);
            }
            return true;
        }
        return false;
    }

    public void CountMoves()
    {
        m_Moves--;
        m_UIManager.ManagerOfUI.UpdateMovesCount(m_Moves);

        if (m_Moves == 0)
        {
            m_UIManager.ManagerOfUI.SetGameOverPromptActive(true);
        }
    }

    public void NextLevel()
    {
        LoadLevel();
        m_UIManager.ManagerOfUI.SetLevelCompletePromptActive(false);
        m_UIManager.ManagerOfUI.SetUIElements();
        m_CurrentScore = 0;
    }

    public void RestartLevel()
    {
        m_UIManager.SetGameOverPromptActive(false);
        m_Moves = m_Levels[m_LevelID].MaxMoves;
        m_Grid.PlayingGrid.DrawGrid();
        m_UIManager.ManagerOfUI.SetUIElements();
    }

    private void LoadLevel()
    {
        m_LevelID++;

        if (LevelID < m_Levels.Count)
        {
            m_Moves = m_Levels[m_LevelID].MaxMoves;
            m_GoalScore = m_Levels[m_LevelID].LevelGoalScore;
            m_GridSize = m_Levels[m_LevelID].GridSize;

            m_Grid.PlayingGrid.DrawGrid();
            m_UIManager.ManagerOfUI.SetUIElements();
        }
        else
        {
            m_UIManager.ManagerOfUI.SetGameCompletePrompActive(true);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}