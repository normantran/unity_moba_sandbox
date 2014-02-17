using UnityEngine;
using System.Collections;

public class Minion : MonoBehaviour {
	
	public int pointWorth = 100;
	public float maxChaseRange = 75f;
	public AwarenessBubble awarenessBubble;
	public GameObject model;
	
	public GameObject lineOfSightPrefab; 	
	
	protected LineRenderer lineOfSight;
	protected Vector3 mainWalkTarget;
	protected Attacker attacker;
	protected Navigator navigator;
	protected Life life;
	protected Vector3 startPosition;
	protected Vector3 initialAttackPosition;
	protected Transform thisTransform;
	
	#region Events
	public delegate void MinionEventHandler(GameObject thisMinion);
	public event MinionEventHandler OnMinionDeath;
	#endregion Events
	
	void Start () {
		thisTransform = transform;
		startPosition = thisTransform.position;
		attacker = GetComponent<Attacker>();
		attacker.OnTargetLeaveRange += targetLeftRange;
		attacker.OnTargetInRange += targetInRange;
		attacker.OnTargetDeath += targetDied;
		attacker.OnBeginAttacking += startedAttacking;
		attacker.OnActualAttack += attack;
		
		navigator = GetComponent<Navigator>();
		navigator.OnWalking += walking;
		navigator.OnStartWalk += startWalking;
		navigator.OnStopWalk += stopWalking;
		
		life = GetComponent<Life>();
		life.OnBeingAttacked += wasAttacked;
		StartCoroutine("prowl");
		//StartCoroutine(repeatHurt());
		
		lineOfSight = (Instantiate(lineOfSightPrefab) as GameObject).GetComponent<LineRenderer>();
		lineOfSight.gameObject.SetActiveRecursively(false);
	}
	
	void OnDestroy()
	{
		attacker.OnTargetLeaveRange -= targetLeftRange;
		attacker.OnTargetInRange -= targetInRange;
		attacker.OnTargetDeath -= targetDied;
		attacker.OnBeginAttacking -= startedAttacking;
		attacker.OnActualAttack -= attack;
		
		navigator.OnWalking -= walking;
		navigator.OnStartWalk -= startWalking;
		navigator.OnStopWalk -= stopWalking;
		
		life.OnBeingAttacked -= wasAttacked;
		
		if(lineOfSight != null)
			Destroy(lineOfSight.gameObject);
	}
	
	virtual protected IEnumerator prowl()
	{
		GameObject target;
		attacker.StopAttacking();
		gameObject.SendMessage("MoveTo", mainWalkTarget);
		do
		{
			yield return 0;
			target = awarenessBubble.GetTarget();
		}
		while(target == null);
		gameObject.SendMessage("StopMoving");
		//print("Found target!! ATTACK!");
		attacker.ShootTarget(target);
	}
	
	public void SetWalkTarget(Vector3 position)
	{
		mainWalkTarget = position;
	}
	
	public void Die()
	{
		if(OnMinionDeath != null)
			OnMinionDeath(gameObject);
		gameObject.SendMessage("PlayEffect", 0, SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
		//gameObject.SetActiveRecursively(false);
		//transform.position = new Vector3(-100,-100,-100);
		//StartCoroutine(cycleBack());
	}
	
	protected void startedAttacking(GameObject target)
	{
		//Set for kiting limits.
		initialAttackPosition = thisTransform.position;
		
		StartCoroutine(flashLOS(target.transform.position));
	}
	
	private IEnumerator flashLOS(Vector3 to)
	{
		if(lineOfSight != null)
		{
			lineOfSight.gameObject.SetActiveRecursively(true);
			lineOfSight.SetPosition(0, thisTransform.position);
			lineOfSight.SetPosition(1, to);
			
			yield return new WaitForSeconds(.3f);
			
			lineOfSight.gameObject.SetActiveRecursively(false);
		}
	}
	
	virtual protected void targetLeftRange()
	{
		//If we're targeting a player, give up.
		//otherwise, pursue
		if(attacker.TargetCharacter != null 
		   && attacker.TargetCharacter.tag == "Player"
		   && Vector3.SqrMagnitude( thisTransform.position - initialAttackPosition ) > maxChaseRange)
			StartCoroutine("prowl");
		else
			gameObject.SendMessage("MoveToward", attacker.TargetCharacter);
	}
	
	protected void targetInRange()
	{
		gameObject.SendMessage("StopMoving");
	}
	
	protected void targetDied()
	{
		//Find a new target.
		StopCoroutine("prowl");
		StartCoroutine("prowl");
	}
	
	virtual protected void wasAttacked(GameObject whoAttackedMe)
	{
		if(whoAttackedMe != null)
		{
			reportToAttacker(whoAttackedMe);
			if(life.IsAlive)	
				reactToAttacker(whoAttackedMe);
		}
		
		if(!life.IsAlive)
			Die();
		else
			playAnim("gothit");
	}
	
	virtual protected void reportToAttacker(GameObject whoAttackedMe)
	{
		if(life.IsAlive)
			whoAttackedMe.SendMessage("OnAttackNoKill", SendMessageOptions.DontRequireReceiver);
		else
		{
			whoAttackedMe.SendMessage("OnAttackKill", this, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	virtual protected void reactToAttacker(GameObject whoAttackedMe)
	{
		//If who's attacking me is the Player, target them.
				//if I'm attacking the Player, stop attacking Player and attack minion, so the player can escape.
		if(GameController.instance.IsPlayer(whoAttackedMe) ||
		   (attacker.TargetCharacter != null && attacker.TargetCharacter.tag == "Player"))
		{
			attacker.ShootTarget(whoAttackedMe);
		}
	}	
	
	protected void playAnim(string animationName)
	{
		if(model != null)
			model.animation.Play(animationName);
	}
	
	protected void attack(GameObject target)
	{
		if(model != null)
			model.animation.Play("attackrun");
	}
	
	protected void startWalking()
	{
		if(model != null)
			model.animation.Play("threaten");
	}
	
	protected void walking(Vector3 walkTarget)
	{
		//if(model != null)
		//{
			walkTarget.y = thisTransform.position.y;
			//print(walkTarget);
			thisTransform.LookAt(walkTarget);
		//}
	}
	
	protected void stopWalking()
	{
		if(model != null)
			model.animation.Play("idle");
	}
}
