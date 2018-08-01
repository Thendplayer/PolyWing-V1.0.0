using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public PlayerController m_player;
    [Range(0, 100)] public int m_collectableValue = 15; //Percent

    [HideInInspector] public float m_maxScore = 0;
    [HideInInspector] public float m_currentScore = 0;
    [HideInInspector] public int m_collectedBoosts = 0;

    #region Singletone
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        if (instance != this)
        {
            DestroyImmediate(this.gameObject);
        }

        if (PlayerPrefs.HasKey("MaxScore"))
            m_maxScore = PlayerPrefs.GetFloat("MaxScore");
    }
    #endregion

    // Update is called once per frame
    void Update () {
        if (m_player != null)
        {
            float l_playerDistance = Mathf.RoundToInt(m_player.transform.position.y);

            m_currentScore = l_playerDistance + m_collectedBoosts * 10;

            if (m_maxScore < m_currentScore)
                m_maxScore = m_currentScore;
        }
	}

    public void F_ResetGame()
    {
        PlayerPrefs.SetFloat("MaxScore", m_maxScore);
        GooglePlayConnection.AddScoreToLeaderboard((long)m_maxScore);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
