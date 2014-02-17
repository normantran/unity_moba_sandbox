using UnityEngine;
using System.Collections;

public class Life : MonoBehaviour {

	public GameObject lifeBarPrefab;
	private DisplayBar lifeBar;
	
	#region Events
	public delegate void AttackedEventHandler(GameObject whoAttackedMe);
	public event AttackedEventHandler OnBeingAttacked;
	#endregion Events
	
	public bool IsAlive
	{
		get { return life > 0; }
	}
	
	public float maxLife = 100f;
	private float life;
	public float LifeValue
	{
		get{ return life;}
		set{
			float newVal =  Mathf.Max(0, Mathf.Min(maxLife, value));
			life = newVal;
			lifeBar.DisplayValue = newVal;
		}
	}
	
	void Awake()
	{
		life = maxLife;
		GameObject spawnedLifeBar = Instantiate(lifeBarPrefab, transform.position + new Vector3(0,3f,0), lifeBarPrefab.transform.rotation) as GameObject;
		spawnedLifeBar.transform.parent = transform;
		lifeBar = spawnedLifeBar.GetComponent<DisplayBar>();
	}
	
	void Start()
	{
		lifeBar.maxValue = maxLife;
	}
	
	public void Hurt(float damage, GameObject attacker)
	{
		life -= damage;
		lifeBar.DisplayValue = life;
		OnBeingAttacked(attacker);
		//if(life <= 0)
		//{
			//if(attacker != null && attacker.tag == "Player")
			//	gameObject.SendMessage("specialDeath");
			//else
		//		gameObject.SendMessage("Die");
		//}
	}
}
