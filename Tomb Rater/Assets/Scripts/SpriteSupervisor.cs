using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSupervisor : MonoBehaviour {

	private GameObject draggedSpr;

	public GameObject getDraggedSpr () {
		return draggedSpr;
	}
	public void setDraggedSpr (GameObject spr) {
		draggedSpr = spr;
	}

}
