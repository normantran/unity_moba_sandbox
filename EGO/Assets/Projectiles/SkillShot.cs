using UnityEngine;
using System.Collections;

public class SkillShot : MonoBehaviour {
	
	[SerializeField] private float m_speed = 5f;
	[SerializeField] private int m_damage = 10;
	
	private GameObject m_whoShotMe;
	private const float kMaxLifeTime = 5f;
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject == m_whoShotMe)
			return;
		
		Life life = col.GetComponent<Life>();
		if(life != null)
		{
			life.Hurt(m_damage, m_whoShotMe);
			Die();
		}
	}
	
	public void Shoot(Vector3 direction, GameObject shooter)
	{
		m_whoShotMe = shooter;
		
		transform.LookAt(direction + transform.position);
		
		StartCoroutine(Fly(direction));
	}
	
	public void Die()
	{
		gameObject.SendMessage("PlayEffects");
		Destroy(gameObject);
	}
	
	private IEnumerator Fly(Vector3 direction)
	{
		float flyingTime = 0;
		Vector3 currentPos = transform.position;
		direction.y = 0;
		direction *= 10;
		print(currentPos + " + " + direction);
		while(flyingTime <= kMaxLifeTime) //&& thisTransform.position != targetCharacter.transform.position)// && targetEnemy.IsAlive)
		{
			currentPos = transform.position;
			//print(currentPos + " + " + direction);
			transform.position = Vector3.MoveTowards(currentPos, currentPos + direction, m_speed * Time.deltaTime);
			
			yield return 0;
			
			flyingTime += Time.deltaTime;
		}
		
		Die();
	}
}
