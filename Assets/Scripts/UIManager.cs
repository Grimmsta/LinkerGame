using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text m_GoalText;
    
    [SerializeField]
    private Text m_ScoreText;
    
    [SerializeField]
    private Text m_MovesText;

    [SerializeField]
    private GameObject m_GameOverScreen;
    
    [SerializeField]
    private GameObject m_LevelCompleteScreen;

    private int m_GoalScore = 0;
    private int m_CurrentScore = 0;
    private int m_MovesLeft = 0;
    private GameManager m_GameManager;

    private static UIManager s_UIManager;
    public UIManager ManagerOfUI => s_UIManager;

    private void Awake()
    {
        s_UIManager = this;        
    }

    void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();

        m_MovesLeft = m_GameManager.Manager.Moves;
        m_MovesText.text = m_MovesLeft.ToString();

        m_GoalScore = m_GameManager.Manager.GoalScore;
        m_GoalText.text = m_GoalScore.ToString();

        m_GameOverScreen.SetActive(false);
        m_LevelCompleteScreen.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        m_CurrentScore = score;
        m_ScoreText.text = m_CurrentScore.ToString();
    }

    public void UpdateMovesCount(int movesLeft)
    {
        m_MovesLeft = movesLeft;
        m_MovesText.text = m_MovesLeft.ToString();
    }
    public void ShowGameOverPrompt()
    {
        m_GameOverScreen.SetActive(true);
    }
    
    public void ShowLevelCompletePrompt()
    {
        m_LevelCompleteScreen.SetActive(true);
    }
}
