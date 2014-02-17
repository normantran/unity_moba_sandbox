using UnityEngine;
using System.Collections;

public class LaneSpawner : BaseMinionSpawner {
	
	public int activeEscalationLevel = 0;
	
	Vector3 allySpawnPoint;
	Vector3 enemySpawnPoint;
	
	bool isActive;
	
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position, transform.localScale);
		Gizmos.color = Color.red;
		Gizmos.DrawRay(new Ray(transform.position, transform.right * 5));
	}
	
	void Start()
	{
		allySpawnPoint = transform.position;
		enemySpawnPoint = allySpawnPoint;
		allySpawnPoint.x -= transform.localScale.x/2;
		enemySpawnPoint.x += transform.localScale.x/2;
		
		GameController.OnEscalationChange += checkEscalationChanged;
		checkEscalationChanged(GameController.instance.EscalationLevel);
	}
	
	protected void checkEscalationChanged(int escalationLevel)
	{
		if(escalationLevel >= activeEscalationLevel)
		{
			if(!isActive)
			{
				StartCoroutine(SpawnAlly());
				StartCoroutine(SpawnEnemy());
				isActive = true;
			}
		}
		else
			isActive = false;
	}
	
	protected IEnumerator SpawnAlly()
	{
		if( numSpawnedAllies <=0)
		{
			yield return new WaitForSeconds(Random.Range(0f,1f));
			numSpawnedAllies++;
			SpawnMinion(allyMinions[0], allySpawnPoint, spawnedAllyDied);
		}
	}
	
	protected IEnumerator SpawnEnemy()
	{
		if(numSpawnedEnemies <=0)
		{
			yield return new WaitForSeconds(Random.Range(0f,1f));
			numSpawnedEnemies++;
			SpawnMinion(enemyMinions[0], enemySpawnPoint, spawnedEnemyDied);
		}
	}
	
	protected override void spawnedAllyDied (GameObject ally)
	{
		base.spawnedAllyDied (ally);
		if(isActive)
			StartCoroutine(SpawnAlly());
	}
	
	protected override void spawnedEnemyDied (GameObject enemy)
	{
		base.spawnedEnemyDied (enemy);
		if(isActive)
			StartCoroutine(SpawnEnemy());
	}
	
	protected void SpawnMinion(GameObject prefab, Vector3 position, Minion.MinionEventHandler deathCallBack)
	{
		GameObject spawned = Instantiate(prefab, position, Quaternion.identity) as GameObject;
		spawned.SendMessage("SetWalkTarget", transform.position);
		Minion minionScript = spawned.GetComponent<Minion>();
		minionScript.OnMinionDeath += deathCallBack;
	}
}
