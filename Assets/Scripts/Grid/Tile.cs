using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType
    {
        Red,
        Green,
        Blue,
        Pink,
        Yellow,
    }

    private TileType m_TileType;

    public TileType TypeOfTile => m_TileType;

    private Vector3 m_NormalScale;
    private SpriteRenderer m_Renderer;
    Array tileTypeCount = Enum.GetValues(typeof(TileType));

    private void Start()
    {
        m_NormalScale = transform.localScale;
        m_Renderer = GetComponent<SpriteRenderer>();
        AssingColor();
    }

    public void HighlightTile()
    {
        transform.localScale = transform.localScale * 1.15f;
    }

    public void RestoreTile()
    {
        transform.localScale = m_NormalScale;
    }

    public void DeactivateTile()
    {
        gameObject.SetActive(false);
    }

    public void ActivateTile()
    {
        gameObject.SetActive(true);
        transform.localScale = m_NormalScale;
        AssingColor();
    }

    private void AssingColor() //Assigns a random color depending on the ones we have in our enum
    {
        m_TileType = (TileType)UnityEngine.Random.Range(0, tileTypeCount.Length);

        switch (m_TileType)
        {
            case TileType.Red:
                m_Renderer.color = Color.red;
                break;
            case TileType.Green:
                m_Renderer.color = Color.green;
                break;
            case TileType.Blue:
                m_Renderer.color = Color.blue;
                break;
            case TileType.Pink:
                m_Renderer.color = Color.magenta;
                break;
            case TileType.Yellow:
                m_Renderer.color = Color.yellow;
                break;
            default:
                break;
        }
    }
}