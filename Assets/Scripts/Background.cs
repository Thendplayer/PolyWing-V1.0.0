using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    [SerializeField] private SpriteRenderer m_current;
    [SerializeField] private SpriteRenderer m_next;
	
	// Update is called once per frame
	void Update () {
        float l_maxDistance = m_current.bounds.size.y / 2 - 1;
        float l_currentDistance = (Camera.main.transform.position.y + Camera.main.orthographicSize) - m_current.transform.position.y;

        if (l_maxDistance <= l_currentDistance)
        {
            m_next.transform.position = new Vector3(0, m_current.transform.position.y + m_current.bounds.size.y);

            SpriteRenderer l_change = m_current;
            m_current = m_next;
            m_next = l_change;
        }
	}
}
