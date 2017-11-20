using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNavigation : MonoBehaviour {

	/* This should probably be attached to the main camera */ 

	//for constraining how far the map can be panned horizontally/vertically
	public int horizBound, vertBound;

	private void Update() {
		float horiz = Input.GetAxis ("Horizontal");
		float vert = Input.GetAxis ("Vertical");

		if (Mathf.Abs (this.transform.position.x + horiz) > horizBound)
			horiz = 0f;
		if (Mathf.Abs (this.transform.position.y + vert) > vertBound)
			vert = 0f;
		Vector3 move = new Vector3 (horiz, vert, 0f);
		this.transform.Translate (move);
	}

}
