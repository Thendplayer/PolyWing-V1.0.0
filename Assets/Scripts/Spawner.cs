using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField] private List<GameObject> m_prefabs;
    [SerializeField] private GameObject m_collectable;

    private int m_nextCollectable;

    void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        float l_screenHeight = Camera.main.orthographicSize * 2;

        Vector2 l_spawnPos = new Vector2(0, this.transform.position.y + l_screenHeight / 1.35f);
        Instantiate(m_prefabs[Random.Range(0, m_prefabs.Count - 1)], l_spawnPos, Quaternion.identity);

        m_nextCollectable -= 1;
        if (m_nextCollectable <= 0)
            F_SpawnCollectable();
    }

    private void F_SpawnCollectable()
    {
        float l_screenHeight = Camera.main.orthographicSize * 2;
        float l_screenWidth = l_screenHeight * Camera.main.aspect;

        Vector2 l_randomPos = new Vector2(Random.Range(-l_screenWidth / 2, l_screenWidth / 2), this.transform.position.y + l_screenHeight);
        Instantiate(m_collectable, l_randomPos, Quaternion.identity);

        m_nextCollectable = Random.Range(1, 5);
    }
}
