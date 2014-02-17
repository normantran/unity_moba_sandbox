using UnityEngine;
using System.Collections;

public class Attacker : MonoBehaviour {
	public float attackDamage = 15;
	public float range = 5f;
	public float castTime = 0.5f;
	//public float castEnergyCost = 30f;
	public GameObject projectilePrefab;
	public bool limitedAmmo = false;
	public int maxAmmo = 5;
	
	private int ammo = 10;
	public int Ammo
	{
		get{return ammo;}
		set{
			int oldAmmo = ammo;
			ammo = Mathf.Max(0, Mathf.Min(maxAmmo, value));
			if(oldAmmo != ammo)
			{
				if(OnAmmoChange != null)
					OnAmmoChange(ammo);
			}
		}
	}
	
	private Color initColor;
	private GameObject targetCharacter;
	public GameObject TargetCharacter
	{
		get {return targetCharacter;}
	}
	private Transform thisTransform;
	
	#region Events
	public delegate void EventHandler();
	public delegate void TargetEventHandler(GameObject target);
	public delegate void AmmoEventHandler(int ammo);
	public delegate void AttackModEvent(Projectile projectile);
	public event EventHandler OnTargetLeaveRange;
	public event EventHandler OnTargetInRange;
	public event EventHandler OnTargetDeath;
	public event TargetEventHandler OnBeginAttacking;
	public event AttackModEvent OnAttackApplyDamageMod;
	public event TargetEventHandler OnActualAttack;
	public event TargetEventHandler OnStopAttacking;
	public event AmmoEventHandler OnAmmoChange;
	#endregion Events
	
	void Awake()
	{
		thisTransform = transform;
		initColor = renderer.material.color;
	}
	
	void Start()
	{
		
		
	}
	
	public void ShootTarget(GameObject target)
	{
		if(!target.Equals(targetCharacter))
		{
			if(OnBeginAttacking != null)
				OnBeginAttacking(target);
			
			StopCoroutine("castShot");
			targetCharacter = target;
			StartCoroutine("castShot");
		}
	}
	
	public void StopAttacking()
	{
		if(OnStopAttacking != null)
			OnStopAttacking(targetCharacter);
		
		StopCoroutine("castShot");
		targetCharacter = null;
		//renderer.material.SetColor("_Color", initColor);
	}
	
	private IEnumerator castShot()
	{
		//Life charLife = targetCharacter.GetComponent<Life>();
		while(targetCharacter != null)//charLife.IsAlive) //&& energy >= castEnergyCost)
		{
			if(range * range >= Vector3.SqrMagnitude(thisTransform.position - targetCharacter.transform.position)) 
			{
				if(OnTargetInRange != null)
					OnTargetInRange();
				/*
				//casting = true;
				float castingTime = 0;
				float red = 0;
				while(castingTime <= castTime)
				{
					red = Mathf.Lerp(0, 1, castingTime / castTime);
					renderer.material.SetColor("_Color", new Color(red, 0,0,1));
					yield return 0;
					castingTime += Time.deltaTime;                         
				}
				*/
				renderer.material.SetColor("_Color", initColor);
				
				if(targetCharacter != null)//charLife.IsAlive)
				{
					if(!limitedAmmo || (limitedAmmo && ammo > 0))
				    {
						if(OnActualAttack != null)
							OnActualAttack(targetCharacter);
						
						GameObject spawnedProjectile = Instantiate(projectilePrefab, transform.position, transform.rotation) as GameObject;
						Projectile projectile = spawnedProjectile.GetComponent<Projectile>();
						projectile.SetTarget( targetCharacter);
						projectile.SetShooter(gameObject);
						projectile.SetDamage(attackDamage);
						if(OnAttackApplyDamageMod != null)
						{
							OnAttackApplyDamageMod(projectile);
						}
						//Energy -= castEnergyCost;
						
						Ammo--;
						
						yield return new WaitForSeconds(castTime);
					}
					else // No ammo
					{
						
					}
				}
				else
				{
					break;
				}
			}
			else
				OnTargetLeaveRange();//gameObject.SendMessage("MoveToward", targetCharacter);
			
			yield return 0;
		}
		
		OnTargetDeath();
		targetCharacter = null;
	}
	
	
}
