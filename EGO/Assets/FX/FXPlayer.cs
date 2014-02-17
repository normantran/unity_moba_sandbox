using UnityEngine;
using System.Collections;

public class FXPlayer : MonoBehaviour {
	
	public GameObject[] effectPrefabs;

	void Start () {
	
	}
	
	public void PlayEffect(int effectIndex)
	{
		PlayEffect(effectIndex, transform.position);
	}
	
	public void PlayEffect(int effectIndex, Vector3 position)
	{
		Instantiate(effectPrefabs[effectIndex], position, Quaternion.identity);
	}
	
	public void PlayEffects()
	{
		PlayEffects(transform.position);
	}
	
	public void PlayEffects(Vector3 position)
	{
		for(int i = 0; i < effectPrefabs.Length; i++)
		{
			PlayEffect(i, position);
		}
	}
}
