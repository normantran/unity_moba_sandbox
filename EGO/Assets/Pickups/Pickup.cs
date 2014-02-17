using UnityEngine;
using System.Collections;

public abstract class Pickup : MonoBehaviour {
	
	public GameObject pickupTriggerPrefab;
	
	private float lifeSpan = 2;
	
	virtual protected void Start () {
		GameObject spawned = Instantiate(pickupTriggerPrefab) as GameObject;
		spawned.transform.parent = transform;
		spawned.transform.localPosition = Vector3.zero;
		
		StartCoroutine(fadeAndDie());
	}
	
	IEnumerator fadeAndDie()
	{
		float lifeTime = 0;
		Color orig = renderer.material.GetColor("_Color");
		while(lifeTime <= lifeSpan)
		{
			if(lifeSpan - lifeTime < 5f)
			{
				orig.a = Mathf.Max(0,Mathf.Cos(lifeTime * lifeTime));
				//print(orig);
				renderer.material.SetColor("_Color", orig);
			}
			yield return 0;
			lifeTime += Time.deltaTime;
		}
		Destroy(gameObject);
	}
}
