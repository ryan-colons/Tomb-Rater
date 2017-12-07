using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageResources {

	private Dictionary<Resource, int> resourceDict;

	public ManageResources () {
		resourceDict = new Dictionary<Resource, int> ();
		incrementResource (Res.iron, 500);
		incrementResource (Res.oak, 200);
	}

	//return the number associated with a resource in the resource dictionary
	//i.e. how much of that resource does the player have
	public int getResourceAmount (Resource r) {
		if (resourceDict.ContainsKey (r)) {
			return resourceDict [r];
		} else {
			Debug.Log ("Couldn't find the entry! " + r.name);
			return 0;
		}
	}

	public void incrementResource (Resource r, int n) {
		if (resourceDict.ContainsKey (r)) {
			resourceDict [r] += n;
		} else {
			resourceDict.Add (r, n);
		}
		if (resourceDict [r] < 0) {
			Debug.Log ("Something tried to lower resource " + r.name + " below 0.");
			resourceDict [r] = 0;
		}
	}

	public int getResourceTypeAmount (ResourceType rType) {
		int count = 0;
		foreach (KeyValuePair<Resource, int> kvp in resourceDict) {
			Debug.Log (kvp.Key.type);
			if (kvp.Key.type == rType) {
				count += kvp.Value;
			}
		}
		return count;
	}
}
