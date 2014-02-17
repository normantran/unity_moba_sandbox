using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntersceneData : MonoBehaviour {
	
	private static IntersceneData sInstance;
	public static IntersceneData instance
	{
		get{
			if(sInstance == null)
			{
				Debug.Log("Scene missing IntersceneData Object. Spawning");
				GameObject go = new GameObject("IntersceneData");
				go.AddComponent<IntersceneData>();
			}
			return sInstance;
		}
	}
	
	private List<Equipment> ownedEquipment;
	
	void Awake()
	{
		if(sInstance != null)
			Debug.LogError("ERROR: More than one IntersceneData in scene. There can only be one!");
		sInstance = this;
		
		DontDestroyOnLoad(this);
		
		ownedEquipment = new List<Equipment>();
	}
	
	// Use this for initialization
	void Start () {
	}
	
	public void AddEquipment(Equipment eq)
	{
		ownedEquipment.Add(eq);
	}
	
	public void ApplyEquipment(GameObject player)
	{
		foreach(Equipment eq in ownedEquipment)
		{
			eq.ApplyStats(player);
		}
	}
}
