using UnityEngine;
using System.Collections;

public enum Factions
{
	RED,
	BLUE
}

public class Targetable : MonoBehaviour {
	
	public Factions faction;
	
	public void OnMouseStart(TouchData data)
	{
		print("touch" + gameObject.name);
		GameController.instance.PlayerTouchedTarget(gameObject);
	}
}
