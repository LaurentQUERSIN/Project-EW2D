using UnityEngine;
using System.Collections;

public class markerBehaviour : MonoBehaviour {
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.LookAt(Vector3.zero);
	}
}
