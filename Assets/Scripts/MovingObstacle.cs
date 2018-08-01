using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour {

    [SerializeField] private bool v_cyclic;
    [SerializeField] private float v_speed;
    [SerializeField] private float v_waitTime;
    [SerializeField] private float v_startTime;
    [Range(0, 5)] [SerializeField] private float v_smooth;
    [SerializeField] private Vector2[] v_localWaypoints;
    private Vector2[] v_globalWaypoints;

    private int vAux_fromWaypointIndex;
    private float vAux_percentBetweenWaypoints;
    private float vAux_nextMoveTime;

    // Use this for initialization
    void Start () {
        vAux_nextMoveTime = Time.time + v_startTime;
        v_globalWaypoints = new Vector2[v_localWaypoints.Length];
        for (int i = 0; i < v_localWaypoints.Length; i++)
            v_globalWaypoints[i] = v_localWaypoints[i] + (Vector2)transform.position;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 l_velocity = F_Movement();
        this.transform.Translate(l_velocity);
    }

    private Vector3 F_Movement()
    {
        if (Time.time < vAux_nextMoveTime)
            return Vector3.zero;

        vAux_fromWaypointIndex %= v_globalWaypoints.Length;
        int l_toWaypointIndex = (vAux_fromWaypointIndex + 1) % v_globalWaypoints.Length;
        float l_distanceBetweenWaypoints = Vector3.Distance(v_globalWaypoints[vAux_fromWaypointIndex], v_globalWaypoints[l_toWaypointIndex]);

        vAux_percentBetweenWaypoints += Time.deltaTime * v_speed / l_distanceBetweenWaypoints;
        vAux_percentBetweenWaypoints = Mathf.Clamp01(vAux_percentBetweenWaypoints);

        float l_smoothPercentBetweenWaypoints = F_SmoothBetweenWaypoints(vAux_percentBetweenWaypoints);
        Vector3 l_newPos = Vector3.Lerp(v_globalWaypoints[vAux_fromWaypointIndex], v_globalWaypoints[l_toWaypointIndex], l_smoothPercentBetweenWaypoints);

        if (vAux_percentBetweenWaypoints >= 1)
        {
            vAux_percentBetweenWaypoints = 0;
            vAux_fromWaypointIndex++;

            if (!v_cyclic)
            {
                if (vAux_fromWaypointIndex >= v_globalWaypoints.Length - 1)
                {
                    vAux_fromWaypointIndex = 0;
                    System.Array.Reverse(v_globalWaypoints);
                }
            }

            vAux_nextMoveTime = Time.time + v_waitTime;
        }

        return l_newPos - transform.position;
    }

    private float F_SmoothBetweenWaypoints(float _percent) // y = (x^a) / (x^a + (1-x)^a)
    {
        float l_pow = v_smooth + 1;
        return Mathf.Pow(_percent, l_pow) / (Mathf.Pow(_percent, l_pow) + Mathf.Pow(1 - _percent, l_pow));
    }

    void OnDrawGizmos()
    {
        if (v_localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float l_size = 0.15f;

            for (int i = 0; i < v_localWaypoints.Length; i++)
            {
                Vector2 l_globalWaypointPos = (Application.isPlaying) ? v_globalWaypoints[i] : v_localWaypoints[i] + (Vector2)transform.position;
                Gizmos.DrawLine(l_globalWaypointPos - Vector2.up * l_size, l_globalWaypointPos + Vector2.up * l_size);
                Gizmos.DrawLine(l_globalWaypointPos - Vector2.left * l_size, l_globalWaypointPos + Vector2.left * l_size);
            }
        }
    }
}
