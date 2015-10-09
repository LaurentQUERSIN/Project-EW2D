using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

    public GameObject playerShip;
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(playerShip.transform.position.x, playerShip.transform.position.y, -20f), Time.deltaTime * 3);
	}
}
