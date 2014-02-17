using UnityEngine;
using System.Collections;

public class PlayerCorpse : MonoBehaviour {
	
	void Start () {
		animation.Play("deathfall");
		Invoke("Die", 2f);
	}
	
	void Die()
	{
		Destroy(gameObject);
	}
}
