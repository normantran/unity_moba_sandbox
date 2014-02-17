 using UnityEngine;
using System.Collections;

public class StoreUI : MonoBehaviour {

	void OnGUI()
	{
		GUI.Box(new Rect(0,0, Screen.width/10, Screen.height/10), SaveData.TotalCoins.ToString());
	
		if(GUI.Button(new Rect(Screen.width/10,
		                       Screen.height/10,
		                       Screen.width/10, 
		                       Screen.height/10)
		              , "BOW")
		   )
		{
			IntersceneData.instance.AddEquipment(new BetterBow());
			SaveData.TotalCoins -= BetterBow.Cost;
		}
			
		if(GUI.Button(new Rect(Screen.width - Screen.width/10,
		                       Screen.height - Screen.height/10,
		                       Screen.width/10, 
		                       Screen.height/10)
		              ,"PLAY")
		   )
			Application.LoadLevel(1);
		   
	}
}
