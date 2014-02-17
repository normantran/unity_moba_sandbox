using UnityEngine;
using System.Collections;

public class Killzone : MonoBehaviour {

	void OnTriggerEnter(Collider col)
	{
		Minion m = col.GetComponent<Minion>();
		if(m != null)
			m.Die();
	}
}
