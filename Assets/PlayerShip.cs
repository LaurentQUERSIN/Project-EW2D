using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerShip : MonoBehaviour 
{
	public bool up;
	public bool down;
	public bool left;
	public bool right;
	public float speed;

	public void getKeys()
	{
		if (Input.GetKeyDown(KeyCode.Z))
			up = true;
		if (Input.GetKeyDown(KeyCode.S))
			down = true;
		if (Input.GetKeyDown(KeyCode.D))
			right = true;
		if (Input.GetKeyDown(KeyCode.Q))
			left = true;
		if (Input.GetKeyUp(KeyCode.Z))
			up = false;
		if (Input.GetKeyUp(KeyCode.S))
			down = false;
		if (Input.GetKeyUp(KeyCode.D))
			right = false;
		if (Input.GetKeyUp(KeyCode.Q))
			left = false;
	}



	void Start ()
	{
		up = false;
		down = false;
		left = false;
		right = false;
	}

	void Update ()
	{
		if (this.GetComponent<GameManager>().gamePaused == false)
		{
			getKeys ();
			if (up == true)
				this.GetComponent<Rigidbody>().AddForce(0f, 1f * speed, 0f);
			if (down == true)
				this.GetComponent<Rigidbody>().AddForce(0f, -1f * speed, 0f);
			if (left == true)
				this.GetComponent<Rigidbody>().AddForce(-1f * speed, 0f, 0f);
			if (right == true)
				this.GetComponent<Rigidbody>().AddForce(1f * speed, 0f, 0f);
		}
	}
}
