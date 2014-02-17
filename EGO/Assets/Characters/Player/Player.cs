using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public GameObject model;
	public float energyRegenRate = 3f;
	public int abilityDamageModNum = 40;
	public GameObject energyBarPrefab;
	public GameObject corpse;
	
	#region Events
	public delegate void EventHandler();
	public delegate void AmmoEventHandler(int ammoAmount);
	public event EventHandler OnPlayerGotAttacked;
	public event AmmoEventHandler OnPlayerAttacking;
	#endregion Events
	
/*	
	private float energy = 100f;
	public float Energy
	{
		get { return energy; }
		set {
			energy = Mathf.Max(0, Mathf.Min(100, value));
			energyBar.DisplayValue = energy;
		}
	}
	*/
	
	private DisplayBar energyBar;
	private Attacker attacker;
	protected Navigator navigator;
	private Life life;
	private FXPlayer fxPlayer;
	private Transform thisTransform;
	
	void Start () {
		thisTransform = transform;
		fxPlayer = GetComponent<FXPlayer>();
		
		//GameObject spawnedBar = Instantiate(energyBarPrefab) as GameObject;
		//spawnedBar.transform.parent = transform;
		//spawnedBar.transform.localPosition = new Vector3(0,1f,0);
		//energyBar = spawnedBar.GetComponent<DisplayBar>();
		//energyBar.DisplayValue = energy;
		
		attacker = GetComponent<Attacker>();
		attacker.OnTargetLeaveRange += targetLeftRange;
		attacker.OnTargetInRange += targetInRange;
		attacker.OnTargetDeath += targetDied;
		//attacker.OnBeginAttacking += startedAttacking;
		attacker.OnActualAttack += attack;
		
		navigator = GetComponent<Navigator>();
		if(navigator != null)
		{
			navigator.OnWalking += walking;
			navigator.OnStartWalk += startWalking;
			navigator.OnStopWalk += stopWalking;
		}	
		life = GetComponent<Life>();
		life.OnBeingAttacked += wasAttacked;
		
		//StartCoroutine(energyRegen());
		SubscribeToInputDelegates();
	}
	
	void OnDestroy()
	{
		attacker.OnTargetLeaveRange -= targetLeftRange;
		attacker.OnTargetInRange -= targetInRange;
		attacker.OnTargetDeath -= targetDied;
		//attacker.OnBeginAttacking -= startedAttacking;
		attacker.OnActualAttack -= attack;
		if(navigator != null)
		{
			navigator.OnWalking -= walking;
			navigator.OnStartWalk -= startWalking;
			navigator.OnStopWalk -= stopWalking;
		}
		life.OnBeingAttacked -= wasAttacked;
		
		UnsubscribeToInputDelegates();
	}
	
	virtual protected void SubscribeToInputDelegates()
	{
		GameController.OnPlayerTouchTarget += OnTouchTarget;
		GameController.OnPlayerTouchEnvironment += OnTouchEnv;
	}
	
	virtual protected void UnsubscribeToInputDelegates()
	{
		GameController.OnPlayerTouchTarget -= OnTouchTarget;
		GameController.OnPlayerTouchEnvironment -= OnTouchEnv;
	}
	
	private void targetLeftRange()
	{
		gameObject.SendMessage("MoveToward", attacker.TargetCharacter);
	}
	
	private void targetInRange()
	{
		gameObject.SendMessage("StopMoving");
	}
	
	/// <summary>
	/// Target Died regardless of whether player killed them.
	/// </summary>
	private void targetDied()
	{
		
	}
	
	/// <summary>
	/// Player got last hit on enemy.
	/// </summary>
	public void OnAttackKill(Minion minion)
	{
		if(minion.GetComponent<Targetable>().faction != GameController.instance.PlayerFaction)
			GameController.instance.PlayerKilledEnemy(minion);
	}
	
	public void OnAttackNoKill()
	{
		//attackFailed();
		GameController.instance.PlayerAttackedEnemyNoKill();
	}
	
	public void OnAttackFizzle(Vector3 lastKnownPosition)
	{
		//attackFailed();
		GameController.instance.PlayerAttackFizzle();
	}
	
	public void HitAmmoPickup(int ammoAmount)
	{
		attacker.Ammo += ammoAmount;
	}
	
	public void UseAbility()
	{
		attacker.OnAttackApplyDamageMod += abilityDamageMod;
	}
	
	public void Die()
	{
		GameController.instance.PlayerDied();
		Vector3 position = thisTransform.position;
		position.y -= 1;
		Instantiate(corpse, position, thisTransform.rotation);
		Destroy(gameObject);
	}
	
	private void abilityDamageMod(Projectile projectile)
	{
		projectile.damage += abilityDamageModNum;
		print("---"+projectile.damage);
		attacker.OnAttackApplyDamageMod -= abilityDamageMod;
	}
	
	private void attackFailed()
	{
		GameController.instance.PlayerAttackedEnemyNoKill();
	}
	
	private void wasAttacked(GameObject whoAttackedMe)
	{
		if(OnPlayerGotAttacked != null)
			OnPlayerGotAttacked();
		
		if(!life.IsAlive)
		{
			Die();
		}
		else
		{
			playAnim("gothit");
		}
	}
	
	private void OnTouchTarget(GameObject touched)
	{
		gameObject.SendMessage("ShootTarget", touched);
	}
	
	private void OnTouchEnv(TouchData data)
	{
		gameObject.SendMessage("StopAttacking");
		gameObject.SendMessage("MoveTo", data.touchPosition);
	}
	
	private IEnumerator energyRegen()
	{
		while(true)
		{
			yield return new WaitForSeconds(1f);
			//Energy += energyRegenRate;
		}
	}
	
	private void playAnim(string animationName)
	{
		if(model != null)
			model.animation.Play(animationName);
	}
	
	private void attack(GameObject target)
	{
		if(OnPlayerAttacking != null)
			OnPlayerAttacking(attacker.Ammo);
		
		if(model != null)
		{
		 	thisTransform.LookAt(target.transform);
			model.animation.Play("punch");
		}
	}
	
	private void startWalking()
	{
		if(model != null)
			model.animation.Play("run");
	}
	
	private void walking(Vector3 walkTarget)
	{
		if(model != null)
		{
			walkTarget.y = thisTransform.position.y;
			//print(walkTarget);
			thisTransform.LookAt(walkTarget);
		}
	}
	
	private void stopWalking()
	{
		if(model != null)
			model.animation.Play("idle");
	}
}
