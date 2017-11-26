using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource {
	public enum resourceType
	{
		WOOD,
		STONE,
		METAL,
		FOOD,
		OTHER
	};

	public string name;
	public resourceType type;
	public int value;
}

public class Res_Oak : Resource {
	public static Res_Oak self;
	public Res_Oak () {
		self = this;
		this.name = "Oak";
		this.type = resourceType.WOOD;
		this.value = 5;
	}
}
