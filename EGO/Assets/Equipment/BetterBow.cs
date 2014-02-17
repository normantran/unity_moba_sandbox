using UnityEngine;
using System.Collections;

public class BetterBow : Equipment {
	
	public static int Cost
	{
		get { return 60; }
	}
	
	public override void ApplyStats (GameObject player)
	{
		player.GetComponent<Attacker>().attackDamage += 50;
	}
}
