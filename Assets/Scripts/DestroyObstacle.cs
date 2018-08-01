using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObstacle : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Vector2.Distance(Camera.main.transform.position, this.transform.position) > 15f)
            Destroy(this.gameObject);
	}
}
