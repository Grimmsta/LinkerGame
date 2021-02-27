using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int m_ScorePerTile = 100;

    [SerializeField, Min(1), Tooltip("Number of moves the user have before game over")]
    private int m_Moves = 12;
    public int Moves => m_Moves;

    [SerializeField, Min(1), Tooltip("The amount of score needed to win")]
    private int m_GoalScore = 2000;
    public int GoalScore => m_GoalScore;

    private static GameManager s_Manager;
    public GameManager Manager => s_Manager;

    private UIManager m_UIManager;

    private int m_CurrentScore = 0;
    
    private void Awake()
    {
        s_Manager = this;
    }

    void Start()
    {
        m_UIManager = FindObjectOfType<UIManager>();
    }

    public void CountScore(int amountOfTiles)
    {
        m_CurrentScore += amountOfTiles * m_ScorePerTile;
        m_UIManager.ManagerOfUI.UpdateScore(m_CurrentScore);

        if (m_CurrentScore >= m_GoalScore)
        {
            m_UIManager.ManagerOfUI.ShowLevelCompletePrompt();
        }
    }

    public void CountMoves()
    {
        m_Moves--;
        m_UIManager.ManagerOfUI.UpdateMovesCount(m_Moves);

        if (m_Moves == 0)
        {
            m_UIManager.ManagerOfUI.ShowGameOverPrompt();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}