using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Site_Quarry : ResourceSite {

	public Site_Quarry () {
		this.setMessage ("Quarry:\nGather stone for building");
		this.setPos (1);
		this.setAvailable (true);
	}

	public override string collectResources (int numWorkers) {
		return addAndRecord (Res_Stone.self, numWorkers);
	}
}
