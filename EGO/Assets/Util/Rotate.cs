using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
	
	public float rotationSpeed = 50f;
	public Vector3 rotateAxis = Vector3.up;
	private Transform thisTransform;
	
	// Use this for initialization
	void Start () {
		thisTransform = transform;
		StartCoroutine(rotate());
	}
	
	private IEnumerator rotate()
	{
		while(true)
		{
			thisTransform.Rotate(rotateAxis, rotationSpeed * Time.deltaTime);
			yield return 0;
		}
	}
}
