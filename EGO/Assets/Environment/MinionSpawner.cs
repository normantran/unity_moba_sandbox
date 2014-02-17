using UnityEngine;
using System.Collections;

public class MinionSpawner : MonoBehaviour {
	public int meleeWaveSize = 4;
	public int rangedWaveSize = 6;
	public GameObject meleeMinionPrefab;
	public GameObject rangedMinionPrefab;
	
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position, new Vector3(1,1,1));
		Gizmos.color = Color.red;
		Gizmos.DrawRay(new Ray(transform.position, transform.right * 5));
	}
	
	void Start () {
			StartCoroutine(spawnWave());
	}
	
	private IEnumerator spawnWave()
	{
		while(true)
		{
			for(int i=0; i < meleeWaveSize; i++)
			{
				spawn(meleeMinionPrefab);
				yield return new WaitForSeconds(1f);
			}
			
			for(int i=0; i < rangedWaveSize; i++)
			{
				spawn(rangedMinionPrefab);
				yield return new WaitForSeconds(1f);
			}
			
			yield return new WaitForSeconds(30f);
		}
	}
	
	private void spawn(GameObject prefab)
	{
		GameObject spawned = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
		spawned.SendMessage("SetWalkTarget", transform.right * 100);
	}
}
