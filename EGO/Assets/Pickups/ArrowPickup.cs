using UnityEngine;
using System.Collections;

public class ArrowPickup : Pickup {
	
	public int arrowAmount = 3;
	private FXPlayer fxPlayer;
	
	protected override void Start ()
	{
		base.Start ();
		fxPlayer = GetComponent<FXPlayer>();
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
			col.gameObject.GetComponent<Player>().HitAmmoPickup(arrowAmount);
			fxPlayer.PlayEffects(col.transform.position);
			Die();
		}
	}
	
	public void Die()
	{
		Destroy(gameObject);
	}
}
