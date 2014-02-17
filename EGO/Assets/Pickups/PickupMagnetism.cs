using UnityEngine;
using System.Collections;

public class PickupMagnetism : MonoBehaviour {
	
	private bool magnetized;
	private Transform target;
	private Transform parentTransform;
	
	void Start()
	{
		parentTransform = transform.parent;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(!magnetized && col.tag == "Player")
		{
			StartCoroutine(magnetize(col.transform));
			magnetized = true;
		}
	}
	
	private IEnumerator magnetize(Transform target)
	{
		float speed;
		while(target != null)
		{
			yield return 0;
			speed = 50 * Time.deltaTime / Vector3.SqrMagnitude(target.position - parentTransform.position);
			parentTransform.position = Vector3.MoveTowards(parentTransform.position, target.position, speed);
		}
	}
}
