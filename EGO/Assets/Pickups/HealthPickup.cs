using UnityEngine;
using System.Collections;

public class HealthPickup : Pickup {
	
	public float healthValue = 50f;
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
			col.gameObject.GetComponent<Life>().LifeValue += healthValue;
			fxPlayer.PlayEffects(col.transform.position);
			Die();
		}
	}
	
	public void Die()
	{
		Destroy(gameObject);
	}
}
