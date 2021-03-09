using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text m_LevelIDText;
    [SerializeField] private Text m_GoalText;
    [SerializeField] private Text m_ScoreText;
    [SerializeField] private Text m_MovesText;
    [SerializeField] private GameObject m_GameOverScreen;
    [SerializeField] private GameObject m_LevelCompleteScreen;
    [SerializeField] private GameObject m_ExitGameScreen;
    [SerializeField] private GameObject m_GameCompletePrompt;

    private int m_GoalScore = 0;
    private int m_CurrentScore = 0;
    private int m_MovesLeft = 0;
    
    private GameManager m_GameManager;
    private PlayerController m_PlayerController;

    private static UIManager s_UIManager;
    public UIManager ManagerOfUI => s_UIManager;

    private void Awake()
    {
        s_UIManager = this;
    }

    void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        m_PlayerController = FindObjectOfType<PlayerController>();
     
        m_GameOverScreen.SetActive(false);
        m_LevelCompleteScreen.SetActive(false);
        m_ExitGameScreen.SetActive(false);
        m_GameCompletePrompt.SetActive(false);
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

    #region Prompts
    public void SetGameOverPromptActive(bool state)
    {
        m_GameOverScreen.SetActive(state);
    }

    public void SetLevelCompletePromptActive(bool state)
    {
        m_LevelCompleteScreen.SetActive(state);
        m_PlayerController.PController.CanPlay = !state;
    }

    public void SetExitGamePromptActive(bool state)
    {
        m_ExitGameScreen.SetActive(state);
        m_PlayerController.PController.CanPlay = !state;

        m_PlayerController.PController.ExitGamePromptActive = state;
    }

    public void SetGameCompletePrompActive(bool state)
    {
        m_GameCompletePrompt.SetActive(state);
        m_PlayerController.PController.CanPlay = !state;
    }
    #endregion

    public void SetUIElements()
    {
        m_MovesLeft = m_GameManager.Manager.Moves;
        m_MovesText.text = m_MovesLeft.ToString();
        UpdateMovesCount(m_MovesLeft);

        m_GoalScore = m_GameManager.Manager.GoalScore;
        m_GoalText.text = m_GoalScore.ToString();
        UpdateScore(0);

        m_LevelIDText.text = (m_GameManager.Manager.LevelID + 1).ToString();
    }
}