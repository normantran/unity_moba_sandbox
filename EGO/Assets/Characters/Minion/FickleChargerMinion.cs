using UnityEngine;
using System.Collections;

public class FickleChargerMinion : Minion {

	protected override IEnumerator prowl ()
	{
		GameObject player = GameController.instance.SpawnedPlayer;	
		if(player != null)
			attacker.ShootTarget(player);
		else
			Die();
		
		yield return 0;
	}
	
	protected override void reactToAttacker (GameObject whoAttackedMe)
	{
		if(GameController.instance.IsPlayer(whoAttackedMe))
		{
			attacker.StopAttacking();
			navigator.MoveTo(thisTransform.position + ((thisTransform.position - whoAttackedMe.transform.position).normalized * 1000));
		}
	}
}
