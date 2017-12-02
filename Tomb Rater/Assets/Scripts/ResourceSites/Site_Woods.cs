using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Site_Woods : ResourceSite {

	public Site_Woods () {
		this.setMessage ("Woods:\nGather lumber for building");
		this.setPos (3);
		this.setAvailable (true);
	}

	public override string collectResources (int numWorkers) {
		return addAndRecord(Res_Oak.self, numWorkers);
	}
}
