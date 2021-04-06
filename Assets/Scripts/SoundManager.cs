using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip m_SelectSound;
    
    [SerializeField] private AudioClip m_TilePopSound;
    
    private AudioSource m_AudioSource;

    private SoundManager m_SoundManager;
    public SoundManager ManagerSound => m_SoundManager;

    private float m_HighPitch = 3;
    private float m_NormalPitch;

    private void Awake()
    {
        m_SoundManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();

        m_NormalPitch = m_AudioSource.pitch;
    }

    public void PlayPopSound()
    {
        m_AudioSource.pitch = m_NormalPitch;
        m_AudioSource.PlayOneShot(m_TilePopSound);
    }

    public void PlaySelectSound()
    {
        m_AudioSource.pitch = m_HighPitch;
        m_AudioSource.PlayOneShot(m_SelectSound);
    }
}
