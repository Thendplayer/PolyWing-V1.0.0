using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    [SerializeField] private GameObject m_menu;
    [SerializeField] private GameObject m_canvas;
    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_spawner;
    [SerializeField] private Text m_maxScore;

    [Space]
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_clip;

    void Start()
    {
        GameManager.instance.m_currentScore = 0;
        GameManager.instance.m_collectedBoosts = 0;

        if (PlayerPrefs.HasKey("MaxScore"))
            m_maxScore.text = PlayerPrefs.GetFloat("MaxScore").ToString();
        else
            m_maxScore.text = "0";
    }

    public void StartGame()
    {
        m_canvas.SetActive(true);
        m_player.SetActive(true);
        m_spawner.SetActive(true);
        m_menu.SetActive(false);
    }

    public void StartSound()
    {
        if (m_audioSource.clip != m_clip)
        {
            m_audioSource.clip = m_clip;
            m_audioSource.Play();
        }
    }

    public void ShowLeaderboard()
    {
        GooglePlayConnection.ShowLeaderboard();
    }
}
