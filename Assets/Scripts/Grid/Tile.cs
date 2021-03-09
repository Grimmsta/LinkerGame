using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_PickupParticles;
    [SerializeField] private Text m_PointsLable;
    [SerializeField] private GameObject m_LableHolder;
    [SerializeField] private GameObject m_HighlightSprite;
    [SerializeField] private GameObject m_BlackHole;
        
    public enum TileType
    {
        Red,
        Green,
        Blue,
        Purple,
        Yellow,
    }
    private TileType m_TileType;
    public TileType TypeOfTile => m_TileType;

    private SpriteRenderer m_Renderer;
    private SoundManager m_SoundManager;
    private GameManager m_GameManager;

    private Array m_TileTypeCount = Enum.GetValues(typeof(TileType));
    
    #region Tile Colors
    private Color m_Red = new Color(0.8039216f, 0.4392157f, 0.4392157f);
    private Color m_Green = new Color(0.4666667f, 0.8039216f, 0.4392157f);
    private Color m_Blue = new Color(0.4392157f, 0.7372549f, 0.8039216f);
    private Color m_Purple = new Color(0.7490196f, 0.4392157f, 0.8039216f);
    private Color m_Yellow = new Color(0.8039216f, 0.8039216f, 0.4392157f);
    #endregion

    private void Start()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_GameManager = FindObjectOfType<GameManager>();
        m_SoundManager =  FindObjectOfType<SoundManager>();

        m_PointsLable.text = m_GameManager.Manager.ScorePerTile.ToString();
        m_HighlightSprite.SetActive(false);
        m_BlackHole.SetActive(false);

        m_LableHolder.SetActive(false);
        m_LableHolder.transform.position = Camera.main.WorldToScreenPoint(transform.position);

        AssingColor();
    }

    public void HighlightTile()
    {
        m_HighlightSprite.SetActive(true);
        m_SoundManager.ManagerSound.PlaySelectSound();
    }

    public void RestoreTile()
    {
        m_HighlightSprite.SetActive(false);
    }

    public IEnumerator DeactivateTile()
    {
        m_LableHolder.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        m_LableHolder.SetActive(true);
        m_BlackHole.SetActive(true);
        m_Renderer.color -= new Color(0,0,0,255);
        m_HighlightSprite.SetActive(false);
        m_GameManager.Manager.CountScore();
        m_SoundManager.ManagerSound.PlayPopSound();

        ParticleSystem go = Instantiate(m_PickupParticles, transform.position, Quaternion.identity);
       
        yield return new WaitForSeconds(m_GameManager.Manager.ExplosionDelay);
        
        Destroy(go.gameObject);
       
        gameObject.SetActive(false);
    }

    public void ActivateTile()
    {
        m_BlackHole.SetActive(false);
        m_LableHolder.SetActive(false);
        gameObject.SetActive(true);
      
        AssingColor();
    }

    private void AssingColor() //Assigns a random color depending on the ones we have in our enum
    {
        m_TileType = (TileType)UnityEngine.Random.Range(0, m_TileTypeCount.Length);

        switch (m_TileType)
        {
            case TileType.Red:
                m_Renderer.color = m_Red;
                break;
            case TileType.Green:
                m_Renderer.color = m_Green;
                break;
            case TileType.Blue:
                m_Renderer.color = m_Blue;
                break;
            case TileType.Purple:
                m_Renderer.color = m_Purple;
                break;
            case TileType.Yellow:
                m_Renderer.color = m_Yellow;
                break;
            default:
                break;
        }
    }
}