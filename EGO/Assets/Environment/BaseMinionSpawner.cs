using UnityEngine;
using System.Collections;

public abstract class BaseMinionSpawner : MonoBehaviour {
	
	public GameObject[] allyMinions;
	public GameObject[] enemyMinions;
	public GameObject allySpawnEffect;
	public GameObject enemySpawnEffect;
	public Vector3 walkTarget;
	protected Vector3 effectHoldingPosition = new Vector3(-100,-100,-100);
	protected int numSpawnedAllies;
	protected int numSpawnedEnemies;
	
	virtual public void SpawnAlly(Vector3 spawnPos)
	{
		StartCoroutine(spawnMinion(allyMinions[Random.Range(0, allyMinions.Length)], 
		                           spawnPos, 
		                           spawnedAllyDied, 
		                           allySpawnEffect));
	}
	
	virtual protected IEnumerator spawnMinion(GameObject prefab, Vector3 spawnPosition, Minion.MinionEventHandler deathCallBack, GameObject spawnEffects)
	{
		spawnEffects.transform.position = spawnPosition;
		//spawnEffects.SetActiveRecursively(true);
		
		yield return new WaitForSeconds(2f);
		
		//spawnEffects.SetActiveRecursively(false);
		spawnEffects.transform.position = effectHoldingPosition;
		
		GameObject spawned = Instantiate(prefab, spawnPosition, Quaternion.identity) as GameObject;
		//print("spawned: " + spawned.name);
		spawned.SendMessage("SetWalkTarget", walkTarget);
		Minion minionScript = spawned.GetComponent<Minion>();
		if(deathCallBack != null)
			minionScript.OnMinionDeath += deathCallBack;
	}
	
	virtual protected void spawnedAllyDied(GameObject ally)
	{
		GameController.instance.AllyMinionDeath(ally);
		numSpawnedAllies--;
	}
	
	virtual protected void spawnedEnemyDied(GameObject enemy)
	{
		GameController.instance.EnemyMinionDeath(enemy);
		numSpawnedEnemies--;
	}
}