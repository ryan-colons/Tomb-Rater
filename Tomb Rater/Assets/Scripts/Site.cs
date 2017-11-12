using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteDragDestination))]
public class Site : MonoBehaviour {

	private int numOccupants = 0;
	private SpriteMouseOver mouseOver;

	private void Start () {
		//make sure the SpriteDragDestination component knows this gameobject is a Site
		this.GetComponent<SpriteDragDestination> ().setSite (true);
		mouseOver = this.gameObject.AddComponent<SpriteMouseOver> ();
		mouseOver.setMessage ("0");
	}

	public void handleDrop (GameObject obj) {
		numOccupants += 1;
		mouseOver.setMessage (numOccupants.ToString());
	}

}
