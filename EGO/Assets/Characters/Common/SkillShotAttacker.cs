using UnityEngine;
using System.Collections;

public class SkillShotAttacker : MonoBehaviour {
	
	[SerializeField] private SkillShot m_skillShotPrefab;
	
	void Awake()
	{
		if(m_skillShotPrefab == null)
			Debug.LogError("Missing reference to skillshotPrefab.");
		
	}
	
	public void Attack(Vector3 direction)
	{
		SkillShot spawned = Instantiate(m_skillShotPrefab, transform.position, Quaternion.identity) as SkillShot;
		spawned.Shoot(direction, gameObject);
	}
}
