using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    [SerializeField] private PlayerController m_player;
    [SerializeField] private AudioSource m_audioSource;

    [Space]
    [SerializeField] private Text m_text;
    [SerializeField] private Image m_image;
    [SerializeField] private Sprite m_alertSprite;
    [SerializeField] private GameObject m_alert;
    [SerializeField] private Text m_score;

    private Sprite m_defaultSprite;
    private float m_lastScore = 0;
    private bool m_activeAnim = false;

	// Use this for initialization
	void Start () {
        m_defaultSprite = m_image.sprite;
	}
	
	// Update is called once per frame
	void Update () {
        m_image.fillAmount = Mathf.Clamp01((float)m_player.m_boostPercent / 100);
        m_text.text = m_player.m_boostPercent + "%";

        if (m_image.fillAmount == 1)
        {
            m_image.sprite = m_alertSprite;
            m_alert.SetActive(true);
        }
        else
        {
            m_image.sprite = m_defaultSprite;
            m_alert.SetActive(false);
        }

        m_score.text = GameManager.instance.m_currentScore.ToString();

        if (m_lastScore < GameManager.instance.m_currentScore && !m_activeAnim)
            StartCoroutine(IScoreAnimation());
    }

    public void Boost()
    {
        if (m_player.Boost())
        {
            m_audioSource.Play();
            StartCoroutine(IBoostAnimation());
        }
    }

    private IEnumerator IBoostAnimation()
    {
        int l_nextPercent = m_player.m_boostPercent - 100;

        while (m_player.m_boostPercent > l_nextPercent)
        {
            m_player.m_boostPercent -= 2;
            yield return new WaitForEndOfFrame();
        }

        yield break;
    }

    private IEnumerator IScoreAnimation()
    {
        m_lastScore = GameManager.instance.m_currentScore;
        m_activeAnim = true;

        while (m_score.transform.localScale.x < 1.08f)
        {
            float l_currentScale = m_score.transform.localScale.x;
            m_score.transform.localScale = new Vector3(l_currentScale + 0.02f, l_currentScale + 0.02f, l_currentScale + 0.02f);
            yield return new WaitForEndOfFrame();
        }

        while (m_score.transform.localScale.x > 1f)
        {
            float l_currentScale = m_score.transform.localScale.x;
            m_score.transform.localScale = new Vector3(l_currentScale - 0.02f, l_currentScale - 0.02f, l_currentScale - 0.02f);
            yield return new WaitForEndOfFrame();
        }

        m_score.transform.localScale = Vector3.one;
        m_activeAnim = false;

        yield break;
    }
}
