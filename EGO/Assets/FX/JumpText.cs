using UnityEngine;
using System.Collections;

public class JumpText : MonoBehaviour {
	public float jumpForce = 5f;
	void Start () {
		rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		Invoke("Die", 1f);
	}
	
	void Die()
	{
		Destroy(gameObject);
	}
}
