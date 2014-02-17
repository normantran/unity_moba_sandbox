using UnityEngine;
using System.Collections;

public static class SaveData
{
	public static int TotalCoins
	{
		get { return PlayerPrefs.GetInt("TotalCoins", 0); }
		set { PlayerPrefs.SetInt("TotalCoins", value); }
	}
}
