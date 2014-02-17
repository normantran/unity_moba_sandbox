using UnityEngine;
using System.Collections;

public class AmmoCounter : MonoBehaviour {
	
	public Attacker attachedAttacker;
	
	private TextMesh text;
	
	void Start () {
		text = GetComponent<TextMesh>();
		attachedAttacker.OnAmmoChange += ammoChange;
	}
	
	void ammoChange(int ammo)
	{
		text.text = ammo.ToString();
	}
}
