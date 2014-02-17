using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public float damage = 15f;
	public float speed = 5f;
	
	private GameObject characterWhoShotMe;
	private GameObject targetCharacter;
	private Vector3 initPosition;
	//private Vector3 targetPosition;
	
	private Transform thisTransform;
	
	void Awake()
	{
		thisTransform = transform;
		initPosition = thisTransform.position;
	}
	
	void OnTriggerEnter(Collider col)
	{
		//print(col.tag);
		if(col.gameObject == (targetCharacter))
		{
			print("---HIT "+damage);
			col.GetComponent<Life>().Hurt(damage, characterWhoShotMe);
			Die();
		}
	}
	
	public void Die()
	{
		gameObject.SendMessage("PlayEffects");
		Destroy(gameObject);
	}
	
	public void SetTarget(GameObject target)
	{
		targetCharacter = target;
		//targetPosition = target.transform.position;
		StartCoroutine(fly());
	}
	
	public void SetShooter(GameObject shooter)
	{
		characterWhoShotMe = shooter;
	}
	
	public void SetDamage(float dmg)
	{
		damage = dmg;
	}
	
	private IEnumerator fly()
	{
		Vector3 lastKnowPosition = targetCharacter.transform.position;
		while(targetCharacter != null) //&& thisTransform.position != targetCharacter.transform.position)// && targetEnemy.IsAlive)
		{
			yield return 0;
			if(targetCharacter != null)
			{
				lastKnowPosition = targetCharacter.transform.position;
				thisTransform.position = Vector3.MoveTowards(thisTransform.position,
				                                             lastKnowPosition,
				                                             speed * Time.deltaTime);
			}
			else
				break;
		}
		
		//finish by going to last known position of target.
		while(thisTransform.position != lastKnowPosition)
		{
			yield return 0;
			thisTransform.position = Vector3.MoveTowards(thisTransform.position,
			                                             lastKnowPosition,
			                                             speed * Time.deltaTime);
		}
		
		if(characterWhoShotMe != null)
			characterWhoShotMe.SendMessage("OnAttackFizzle", lastKnowPosition, SendMessageOptions.DontRequireReceiver);
		//if(GameController.instance.IsPlayer(characterWhoShotMe))
		//	GameController.instance.PlayerAttackFizzled();
		
		Die();
	}
}
