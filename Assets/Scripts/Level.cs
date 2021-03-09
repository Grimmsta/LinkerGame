using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "NewLevel", order = 1)]
public class Level : ScriptableObject
{   
    [SerializeField, Min(1), Tooltip("The amount of score needed to win")]
    private int m_LevelGoalScore = 0;
    public int LevelGoalScore=> m_LevelGoalScore;

    [SerializeField, Min(1), Tooltip("Number of moves the user have before game over")]
    private int m_MaxMoves = 0;
    public int MaxMoves=> m_MaxMoves;

    [SerializeField, Tooltip("Size of the playing grid")]
    private Vector2 m_GridSize;
    public Vector2 GridSize => m_GridSize;
}
