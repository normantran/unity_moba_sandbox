using UnityEngine;
using System.Collections;

public class PlayerWayPoint : MonoBehaviour {
	
	private Vector3 holdingPosition = new Vector3(-100,-100,-100);
	private bool isActive = false;
	private Transform thisTransform;
	
	void Start () {
		thisTransform = transform;
		thisTransform.position = holdingPosition;
	}
	
	private IEnumerator bob()
	{
		Vector3 pos = thisTransform.position;
		while(isActive)
		{
			pos.y += 0.1f * Mathf.Cos(Time.time * 10f);
			thisTransform.position = pos;
			yield return 0;
		}
	}
	
	public void PointAt(Vector3 position)
	{
		position.y += 1f;
		thisTransform.position = position;
		isActive = true;
		StartCoroutine(bob());
	}
	
	public void OnTriggerEnter(Collider col)
	{
		if(isActive)
		{
			Player player = col.GetComponent<Player>();
			if(player != null)
			{
				Deactivate();
			}
		}
	}
	
	public void Deactivate()
	{
		isActive = false;
		thisTransform.position = holdingPosition;
	}
}
