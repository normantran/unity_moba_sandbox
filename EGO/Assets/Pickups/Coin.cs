using UnityEngine;
using System.Collections;

public class Coin : Pickup {
	
	public int valuableness = 10;
	public GameObject txtPointsPrefab;
	
	private Transform thisTransform;
	
	protected override void Start ()
	{
		base.Start ();
		thisTransform = transform;
		StartCoroutine(spin());
	}
	
	private IEnumerator spin()
	{
		while(rigidbody != null)
		{
			thisTransform.RotateAround(Vector3.up, 1f * Time.deltaTime);
			yield return 0;
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(!rigidbody.isKinematic && col.name == "EnvTouchBox") //if touching the floor, stop;
			rigidbody.isKinematic = true;
		
		if(col.tag == "Player")
		{
			GameController.instance.AddToScore(valuableness);
			Die();
		}
	}
	
	public void Die()
	{
		//Destroy(transform.parent.gameObject);
		GameObject spawnedTXT = Instantiate(txtPointsPrefab, thisTransform.position, thisTransform.rotation) as GameObject;
		spawnedTXT.GetComponent<TextMesh>().text = valuableness + " x " + GameController.instance.Multiplier;
		Destroy(gameObject);
	}
}
