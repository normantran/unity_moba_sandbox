using UnityEngine;
using System.Collections;

public class CommandSpawner : BaseMinionSpawner {
	
	public float zSpawnLength = 5f;
	
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position, new Vector3(1,1,zSpawnLength));
		Gizmos.color = Color.red;
		Gizmos.DrawRay(new Ray(transform.position, walkTarget));
	}
	
	void Start () {
	}
	
	public override void SpawnAlly (Vector3 spawnPos)
	{
		Vector3 thisSpawnPos = transform.position;
		thisSpawnPos.z = Random.Range(-zSpawnLength/2, zSpawnLength/2);
		base.SpawnAlly (thisSpawnPos);
	}
}
