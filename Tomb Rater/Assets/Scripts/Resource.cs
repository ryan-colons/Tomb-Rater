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

public class Res_Stone : Resource {
	public static Res_Stone self;
	public Res_Stone () {
		self = this;
		this.name = "Stone";
		this.type = resourceType.STONE;
		this.value = 5;
	}
}

public class Res_Iron : Resource {
	public static Res_Iron self;
	public Res_Iron () {
		self = this;
		this.name = "Iron";
		this.type = resourceType.METAL;
		this.value = 5;
	}
}

public class Res_Food : Resource {
	public static Res_Food self;
	public Res_Food () {
		self = this;
		this.name = "Food";
		this.type = resourceType.FOOD;
		this.value = 5;
	}
}
