using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	Vector3 oldPosition;

	// Use this for initialization
	void Start ()
	{
		this.oldPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Move camera with WASD.
		if (Input.GetKey(KeyCode.W))
		{
			this.transform.position = this.transform.position + new Vector3(0, 0, 0.1f);
		}
		if (Input.GetKey(KeyCode.S))
		{
			this.transform.position = this.transform.position - new Vector3(0, 0, 0.1f);
		}
		if (Input.GetKey(KeyCode.A))
		{
			this.transform.position = this.transform.position - new Vector3(0.1f, 0, 0);
		}
		if (Input.GetKey(KeyCode.D))
		{
			this.transform.position = this.transform.position + new Vector3(0.1f, 0, 0);
		}

		// Move camera with click+drag.

		// Zoom in and out
		if (Input.mouseScrollDelta.y / 10 > 0)
		{
			this.transform.position = this.transform.position + this.transform.forward;
		}
		else if (Input.mouseScrollDelta.y / 10 < 0)
		{
			this.transform.position = this.transform.position - this.transform.forward;
		}

		this.CheckIfCameraMoved();
	}

	// Move camera to specified hex
	public void PanToHex (Hex hex)
	{

	}

	private void CheckIfCameraMoved()
	{
		if (this.oldPosition != this.transform.position)
		{
			this.oldPosition = this.transform.position;
		}
	}
}
