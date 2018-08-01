using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private Spawner m_spawner;
    [SerializeField] private List<Animator> m_deathAnimators;
    [SerializeField] private AudioSource m_audioSource;

    [Space]
    [Range(0, 20)]    [SerializeField] private float m_speed = 4f;
    [Range(0, 20)]    [SerializeField] private float m_maxSpeed = 10f;
    [Range(0, 5)]     [SerializeField] private float m_rotationAplifier = 2f;
    [Range(0, 5)]     [SerializeField] private float m_rotationSpeed = 3.5f;
    [Range(0, 90)]    [SerializeField] private float m_maxRotationAngle = 75f;
    [Range(0, 0.05f)] [SerializeField] private float m_acceleration = 0.018f;
    [Range(0, 2)]     [SerializeField] private float m_deceleration = 1.25f;

    [Space]
    public int m_boostPercent = 0;

    private bool m_boostActive = false;
    private float m_lastSpeed = 0;
    private bool m_death = false;

	// Use this for initialization
	void Start () {
        GameManager.instance.m_player = this;

        foreach (Animator l_anim in m_deathAnimators)
            l_anim.enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!m_death)
        {
            Vector2 l_target = this.transform.position;

            if (Input.touchCount > 0)
            {
                Collider2D l_collision = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), 0.1f);

                if (l_collision == null)
                    l_target = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                else if (l_collision.tag != "BoostZone")
                    l_target = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }

            l_target.y += m_rotationAplifier;

            Vector2 l_direction = l_target - (Vector2)this.transform.position;
            float l_angle = Mathf.Atan2(l_direction.y, l_direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion l_rotation = Quaternion.AngleAxis(l_angle, Vector3.forward);

            l_rotation = Quaternion.Slerp(this.transform.rotation, l_rotation, m_rotationSpeed * Time.deltaTime);

            if (l_rotation.eulerAngles.z < m_maxRotationAngle || l_rotation.eulerAngles.z > 360 - m_maxRotationAngle)
                this.transform.rotation = l_rotation;

            this.transform.Translate(Vector2.up * m_speed * Time.deltaTime);

            if (!m_boostActive)
            {
                if (m_speed < m_maxSpeed)
                    m_speed += m_speed * m_acceleration * Time.deltaTime;
                else
                    m_speed = m_maxSpeed;
            }
            else
            {
                m_speed -= m_speed * m_deceleration * Time.deltaTime;

                if (m_speed <= m_lastSpeed / 2)
                    m_boostActive = false;
            }
        }
    }

    public bool Boost()
    {
        if (!m_boostActive && m_boostPercent >= 100)
        {
            m_lastSpeed = m_speed;
            m_boostActive = true;
            return true;
        }
        return false;
    }

    private void F_Death()
    {
        m_death = true;
        foreach (Animator l_anim in m_deathAnimators)
        {
            l_anim.enabled = true;
            l_anim.SetBool("Death", true);
        }
    }

    public void F_EndGame()
    {
        GameManager.instance.F_ResetGame();
    }

    void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.tag == "Collectable")
        {
            StartCoroutine(IBoostUp());
            GameManager.instance.m_collectedBoosts++;
            m_audioSource.Play();
            Destroy(_collision.gameObject);
        }

        else if (_collision.gameObject.tag == "Death")
            F_Death();

        if (_collision.gameObject.tag == "Goal")
            m_spawner.Spawn();
    }

    private IEnumerator IBoostUp()
    {
        int l_nextPercent = m_boostPercent + GameManager.instance.m_collectableValue;

        while (m_boostPercent < l_nextPercent && m_boostPercent <= 150)
        {
            m_boostPercent++;
            yield return new WaitForEndOfFrame();
        }

        if (m_boostPercent > 150)
            m_boostPercent = 150;

        yield break;
    }
}
