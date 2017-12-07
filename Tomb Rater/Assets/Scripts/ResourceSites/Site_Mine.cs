using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Site_Mine : ResourceSite {

	public Site_Mine () {
		this.setMessage ("Mine:\nObtain useful metals");
		this.setPos (0);
		this.setAvailable (true);
	}

	public override string collectResources (int numWorkers) {
		return addAndRecord(Res.iron, numWorkers);
	}
}
