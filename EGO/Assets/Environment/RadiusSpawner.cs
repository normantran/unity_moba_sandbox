using UnityEngine;
using System.Collections;

public class RadiusSpawner : BaseMinionSpawner {
	public float minRadius = 5f;
	public float maxRadius = 12f;
	
	private int numMinionsToSpawn = 2;
	
	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, minRadius);
		Gizmos.DrawWireSphere(transform.position, maxRadius);
	}
	
	void Start () {
		StartCoroutine(spawnLoop());
		walkTarget = transform.position;
		//spawnWave();
		InvokeRepeating("increment", 20f,20f); 
	}
	
	private void increment()
    {
		if(numMinionsToSpawn < 5)
			numMinionsToSpawn++;
	}
		      
	private void spawnWave()
	{
		int numToSpawn = numMinionsToSpawn;
		int randMinion;
		float randAngle, randRadius;
		Vector3 spawnPos = transform.position;
		GameObject minionPrefab;
		Minion.MinionEventHandler deathCallBack;
		GameObject spawnEffects;
		deathCallBack = spawnedEnemyDied;
		randMinion = Random.Range(0, enemyMinions.Length);
		minionPrefab = enemyMinions[randMinion];
		numSpawnedEnemies += numToSpawn;
		spawnEffects = enemySpawnEffect;
	
	
		for(int i = 0; i < numToSpawn; i++)
		{
			randRadius = Random.Range(minRadius, maxRadius);
			randAngle = Random.Range(0f, 360f);
			spawnPos.x = Mathf.Cos(randAngle) * randRadius;
			spawnPos.z = Mathf.Sin(randAngle) * randRadius;	
		
			StartCoroutine(spawnMinion(minionPrefab, spawnPos, deathCallBack, spawnEffects));
		}
	}
	
	private IEnumerator spawnLoop()
	{
		int randMinion;
		float randAngle, randRadius;
		Vector3 spawnPos = transform.position;
		GameObject minionPrefab;
		Minion.MinionEventHandler deathCallBack;
		GameObject spawnEffects;
		
		while(true)
		{
			//print("checking numbers: allies"+numSpawnedAllies + ", enemies"+numSpawnedEnemies);
			int numToSpawn = numMinionsToSpawn;//Mathf.Max(numMinionsToSpawn, Mathf.Abs(numSpawnedAllies - numSpawnedEnemies));
			if(numSpawnedAllies <= 0)//numSpawnedAllies < numSpawnedEnemies)
			{
				deathCallBack = spawnedAllyDied;
				randMinion = Random.Range(0, allyMinions.Length);
				minionPrefab = allyMinions[randMinion];
				numSpawnedAllies += numToSpawn;
				spawnEffects = allySpawnEffect;
				for(int i = 0; i < numToSpawn; i++)
				{
					randRadius = Random.Range(minRadius, maxRadius);
					randAngle = Random.Range(0f, 360f);
					spawnPos.x = Mathf.Cos(randAngle) * randRadius;
					spawnPos.z = Mathf.Sin(randAngle) * randRadius;	
				
					StartCoroutine(spawnMinion(minionPrefab, spawnPos, deathCallBack, spawnEffects));
				}
			}
			else if (numSpawnedEnemies <= 0)
			{
				deathCallBack = spawnedEnemyDied;
				randMinion = Random.Range(0, enemyMinions.Length);
				minionPrefab = enemyMinions[randMinion];
				numSpawnedEnemies += numToSpawn;
				spawnEffects = enemySpawnEffect;
			
			
				for(int i = 0; i < numToSpawn; i++)
				{
					randRadius = Random.Range(minRadius, maxRadius);
					randAngle = Random.Range(0f, 360f);
					spawnPos.x = Mathf.Cos(randAngle) * randRadius;
					spawnPos.z = Mathf.Sin(randAngle) * randRadius;	
				
					StartCoroutine(spawnMinion(minionPrefab, spawnPos, deathCallBack, spawnEffects));
				}
			}
			yield return 0; //new WaitForSeconds(4f);
		}
	}
	
	protected override void spawnedAllyDied (GameObject ally)
	{
		base.spawnedAllyDied (ally);
		//if is zero spawn more allies
	}
	
	protected override void spawnedEnemyDied (GameObject enemy)
	{
		base.spawnedEnemyDied (enemy);
		if(numSpawnedEnemies <= 0)
			spawnWave();
	}
}
