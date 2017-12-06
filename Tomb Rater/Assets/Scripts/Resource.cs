using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
	WOOD,
	STONE,
	METAL,
	FOOD,
	OTHER
};

public class Resource {
	public string name;
	public ResourceType type;
	public int value;
}

public class Res_Oak : Resource {
	public static Res_Oak self = new Res_Oak();
	public static Res_Oak getSelf() {
		if (self == null) {
			self.name = "Oak";
			self.type = ResourceType.WOOD;
			self.value = 5;
		}
		return self;
	}

}

public class Res_Stone : Resource {
	public static Res_Stone self = new Res_Stone();
	public static Res_Stone getSelf() {
		if (self == null) {
			self.name = "Stone";
			self.type = ResourceType.STONE;
			self.value = 5;
		}
		return self;
	}
}

public class Res_Iron : Resource {
	public static Res_Iron self = new Res_Iron();
	public static Res_Iron getSelf() {
		if (self == null) {
			self.name = "Iron";
			self.type = ResourceType.METAL;
			self.value = 5;
		}
		return self;
	}
}

public class Res_Food : Resource {
	public static Res_Food self = new Res_Food();
	public static Res_Food getSelf() {
		if (self == null) {
			self.name = "Food";
			self.type = ResourceType.FOOD;
			self.value = 5;
		}
		return self;
	}
}
