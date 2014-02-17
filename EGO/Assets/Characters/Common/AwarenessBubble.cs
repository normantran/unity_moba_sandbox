using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AwarenessBubble : MonoBehaviour {
	
	public Targetable myTargetable;
	private List<GameObject> playerTargets;
	private List<GameObject> minionTargets;
	
	void Awake()	
	{
		playerTargets = new List<GameObject>();
		minionTargets = new List<GameObject>();
	}
	
	public GameObject GetTarget()
	{
		if(minionTargets.Count > 0)
		{
			foreach(GameObject minion in minionTargets)
			{
				if(minion != null)
					return minion;
			}
			minionTargets.TrimExcess();
		}
		
		if(playerTargets.Count > 0)
		{
			foreach(GameObject player in playerTargets)
			{
				if(player != null)
					return player;
			}
			playerTargets.TrimExcess();
		}
		
		
		return null;
	}
		                     
	void OnTriggerEnter(Collider col)
	{
		Targetable t = col.GetComponent<Targetable>();
		if(t != null)
		{
			if(myTargetable.faction != t.faction)
			{
				if(col.tag == "Player")
					playerTargets.Add(t.gameObject);
				else
					minionTargets.Add(t.gameObject);
			}
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		Targetable t = col.GetComponent<Targetable>();
		if(t != null)
		{
			if(col.tag == "Player")
				playerTargets.Remove(t.gameObject);
			else
				minionTargets.Remove(t.gameObject);
		}
	}
}