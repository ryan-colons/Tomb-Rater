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

/* This holds static references to each resource class 
 * This means we can always reference the right instance of a resource class
 */
public static class Res {
	//please keep these sorted by type (and the alphabetically sorted)
	//FOOD
	public static Food food = new Food();
	//METAL
	public static Iron iron = new Iron();
	//OTHER

	//STONE
	public static Stone stone = new Stone();
	//WOOD
	public static Oak oak = new Oak();
}

public class Oak : Resource {
	public Oak () {
		name = "Oak";
		type = ResourceType.WOOD;
		value = 5;
	}

}

public class Stone : Resource {
	public Stone () {
		name = "Stone";
		type = ResourceType.STONE;
		value = 5;
	}
}

public class Iron : Resource {
	public Iron() {
		name = "Iron";
		type = ResourceType.METAL;
		value = 5;
	}
}

public class Food : Resource {
	public Food () {
		name = "Food";
		type = ResourceType.FOOD;
		value = 5;
	}
}
