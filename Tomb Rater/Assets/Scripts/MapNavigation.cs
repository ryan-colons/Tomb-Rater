using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MapNavigation : MonoBehaviour {

	/* This should be attached to the main camera */ 

	//for constraining how far the map can be panned horizontally/vertically
	public int horizBound, vertBound;
	//for constraining how far the map can be zoomed in/out
	public int zoomMinBound, zoomMaxBound;
	public int zoomSpeed;
	private Camera cam;

	private void Start () {
		cam = this.GetComponent<Camera> ();
	}

	private void Update() {
		float horiz = Input.GetAxis ("Horizontal");
		float vert = Input.GetAxis ("Vertical");

		if (Mathf.Abs (this.transform.position.x + horiz) > horizBound)
			horiz = 0f;
		if (Mathf.Abs (this.transform.position.y + vert) > vertBound)
			vert = 0f;
		Vector3 move = new Vector3 (horiz, vert, 0f);
		this.transform.Translate (move);

		float scroll = Input.GetAxis ("Mouse ScrollWheel") * -1 * zoomSpeed;
		if (cam.orthographicSize + scroll > zoomMaxBound || cam.orthographicSize + scroll < zoomMinBound) {
			scroll = 0f;
		}
		cam.orthographicSize += scroll;
	}

}
