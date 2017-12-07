using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Site_Farm : ResourceSite {

	public Site_Farm () {
		this.setMessage ("Farm\nMakes food. Every year, you need at least 1 Food per citizen.");
		this.setPos (2);
		this.setAvailable (true);
	}

	public override string collectResources (int numWorkers) {
		return addAndRecord(Res.food, numWorkers * 2);
	}
}
