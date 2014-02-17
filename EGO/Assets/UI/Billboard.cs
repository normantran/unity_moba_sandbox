using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {
	
	private Transform cameraTransform;
	private Transform thisTransform;
	
	void Start () {
		thisTransform = transform;
		cameraTransform = Camera.main.transform;
		StartCoroutine(billboard());
	}
	
	private IEnumerator billboard()
	{
		while(true)
		{
			//Vector3 point = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(thisTransform.position));
			thisTransform.LookAt(thisTransform.position - cameraTransform.position, cameraTransform.up);
			yield return 0;
		}
	}
}
