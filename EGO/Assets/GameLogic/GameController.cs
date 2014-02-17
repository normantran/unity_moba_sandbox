using UnityEngine;
using System.Collections;

public enum GameState
{
	Playing,
	PlayerDead,
	Paused
}

public class GameController : MonoBehaviour {
	
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private GameState gameState;
	[SerializeField] private Factions playerFaction;
	public Factions PlayerFaction
	{
		get { return playerFaction; }	
	}
	[SerializeField] private GameObject spawnedPlayer;
	public GameObject SpawnedPlayer
	{
		get { return spawnedPlayer; }
	}
	
	[SerializeField] private GameObject minionSpawner;
	
	[SerializeField] private GameObject coinPrefab;
	[SerializeField] private float pickUpSpawnRate = .33f;
	[SerializeField] private GameObject[] pickUps;
	
	private FXPlayer fxPlayer;
	private GameUI gameUI;
	
	private int minionCost = 20;
	
	private int points = 0;
	
	private int skillThreshold = 2;
	
	[SerializeField] private bool isTimed = false;
	[SerializeField] private float roundTimer = 30;
	public float RoundTimer
	{
		get {return roundTimer;}
		set {
			roundTimer = value;
			if(OnTimerChange != null)
				OnTimerChange(roundTimer);
		}
	}
	
	public int Points
	{
		get { return points; }
		set { points = value; }
	}
	
	private int multiplier = 1;
	public int Multiplier
	{
		get { return multiplier; }
		set {
			multiplier = value;
			EscalationLevel = multiplier / 3;
		}
	}
	
	private int skillPoints = 0;
	public int SkillPoints
	{
		get { return skillPoints; } 
		set {
			if(value >= skillThreshold) //value is above threshold
			{ 
				if(skillPoints < skillThreshold) //value is newly above threshold.
				{	
					gameUI.ShowPowerIcon();
				}
			}
			else if(skillPoints >= skillThreshold) //value has dropped below threshold.
			{	
				gameUI.HidePowerIcon();
			}
			
			skillPoints = Mathf.Max(0,value);
		}
	}
	
	private int escalationLevel;
	public int EscalationLevel
	{
		get { return escalationLevel; }
		set {
			escalationLevel = value;
			if(OnEscalationChange != null)
				OnEscalationChange(escalationLevel);
		}
	}
	
	#region Events
	public delegate void TouchEventHandler(TouchData data);
	public delegate void TargetEventHandler(GameObject touched);
	public delegate void EventHandler();
	public delegate void MinionEventHandler(Minion minion);
	public delegate void EscalationEventHandler(int escalationLvl);
	public delegate void TimerEvent(float time);
	
	public static event EventHandler OnPlayerAttack;
	public static event TouchEventHandler OnPlayerTouchEnvironment;
	public static event TargetEventHandler OnPlayerTouchTarget;
	public static event MinionEventHandler OnPlayerKillTarget;
	public static event EventHandler OnPlayerFailToKillTarget;
	public static event EscalationEventHandler OnEscalationChange;
	public static event TimerEvent OnTimerChange;
	#endregion Events
	
	private static GameController sInstance;
	public static GameController instance
	{
		get{
			if(sInstance == null)
				Debug.LogError("Scene missing GameController Object.");

			return sInstance;
		}
	}
		
	void Awake()
	{
		if(sInstance != null)
			Debug.LogError("ERROR: More than one GameController in scene. There can only be one!");
		sInstance = this;
		
		spawnedPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity) as GameObject;
		Player script = spawnedPlayer.GetComponent<Player>();
		script.OnPlayerGotAttacked += playerGotAttacked;
	}
	
	void OnDestroy()
	{
		sInstance = null; //Prevents memory leaks.
		
		//Player script = spawnedPlayer.GetComponent<Player>();
		//script.OnPlayerGotAttacked -= playerGotAttacked;
	}
	
	void Start()
	{
		Time.timeScale = 1f;
		gameState = GameState.Playing;
		playerFaction = spawnedPlayer.GetComponent<Targetable>().faction;
		fxPlayer = GetComponent<FXPlayer>();
		gameUI = GetComponent<GameUI>();
		
		if(isTimed)
			InvokeRepeating("roundTimeCountdown", 1, 1);
		
		//spawnCoins(50, new Vector3(-1, 4, 8));
		
		IntersceneData.instance.ApplyEquipment(spawnedPlayer);
	}
	
	void roundTimeCountdown()
	{
		if(roundTimer <= 0)
		{
			endRound();
		}
		RoundTimer--;
	}
	
	void endRound()
	{
		Time.timeScale = 0;
		SaveData.TotalCoins += Points;
		Application.LoadLevel("Store");
	}
	
	public bool IsPlayer(GameObject character)
	{
		return character != null && character.tag == "Player";
	}
	
	public void PlayerTouchedEnvironment(TouchData data)
	{
		OnPlayerTouchEnvironment(data);
	}
	
	public void PlayerTouchedTarget(GameObject touched)
	{
		OnPlayerTouchTarget(touched);
	}
	
	public void PlayerKilledEnemy(Minion minion)
	{
		fxPlayer.PlayEffect(0, minion.transform.position);
		fxPlayer.PlayEffect(1, minion.transform.position);
		
		points += minion.pointWorth * multiplier;
		spawnCoins(minion.pointWorth, minion.transform.position);
		
		Multiplier++;
		SkillPoints++;
		
		spawnItem(minion.transform.position);
		
		if(OnPlayerKillTarget != null)
			OnPlayerKillTarget(minion);
		//print("Success: " + points + " x" + multiplier);
	}
	
	public void PlayerAttackedEnemyNoKill()
	{
		//print("FAIL!");
		Multiplier = 1;
		skillPoints = 0;
		if(OnPlayerFailToKillTarget != null)
			OnPlayerFailToKillTarget();
	}
	
	public void PlayerAttackFizzle()
	{
		Multiplier = 1;
		skillPoints = 0;
		if(OnPlayerFailToKillTarget != null)
			OnPlayerFailToKillTarget();
	}
	
	public void PlayerDied()
	{
		gameState = GameState.PlayerDead;
		gameUI.BlockInput();
		Time.timeScale = .8f;
		Invoke("RestartLevel", 3f);
	}
	
	public bool CanUseSkill(int skillIndex)
	{
		return SkillPoints >= skillThreshold;
	}
	
	public void UseSkill(int skillIndex)
	{
		print("skill activated!");
		SkillPoints = 0;
		spawnedPlayer.SendMessage("UseAbility");
	}
	
	private void playerGotAttacked()
	{
		Camera.main.SendMessage("FlashRed");
	}
	
	public void RestartLevel()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
	
	public void EnemyMinionDeath(GameObject minion)
	{
		
	}
	
	public void AllyMinionDeath(GameObject minion)
	{
		
	}
	
	public void AddToScore(int pointsToAdd)
	{
		Points += pointsToAdd * multiplier;
	}
	
	public void MinionButtonPress()
	{
		print("SPAWNMINIONS");
		Vector3 spawnPos =  spawnedPlayer.transform.position + spawnedPlayer.transform.forward * 2;
		
		if(Points >= minionCost)
		{
			Points -= minionCost;
			minionSpawner.SendMessage("SpawnAlly", spawnPos);
		}
	}
	
	private void spawnCoins(int pointWorth, Vector3 spawnPosition)
	{
		int numCoins = pointWorth/10;
		Vector3 coinPosition;
		float angleIncrement = 360f/(float)pointWorth;
		float radius = 0.5f, x, z;
		Vector3 force = spawnPosition;
		force.y -= 0f;
		GameObject spawnedCoin;
		
		for(int i = 1; i <= numCoins; i++)
		{
			x = radius * Mathf.Cos(angleIncrement * i);
			z = radius * Mathf.Sin(angleIncrement * i);
			coinPosition = spawnPosition;
			coinPosition.x += x;
			coinPosition.z += z;
			
			spawnedCoin = Instantiate(coinPrefab, coinPosition, Quaternion.identity) as GameObject;
			spawnedCoin.rigidbody.AddForce((coinPosition - spawnPosition + Vector3.up) * 5, ForceMode.Impulse);
			
			//spawnedCoin.rigidbody.AddExplosionForce(300f, spawnPosition,0,1);
		}
	}
	
	private void spawnItem(Vector3 spawnPosition)
	{
		float r = Random.Range(0f,1f);
		//print(r);
		//if(r <= pickUpSpawnRate)
		//{
			if(pickUps.Length > 0)
			{
				//TODO make more dynamic and less hardcoded.
				int rand = Random.Range(0, 100); //(0, pickUps.Length);
				if(rand < 10)
					Instantiate(pickUps[0], spawnPosition, Quaternion.identity);
				else if(rand < 20)
					Instantiate(pickUps[1], spawnPosition, Quaternion.identity);
				else
					Instantiate(pickUps[2], spawnPosition, Quaternion.identity);
			}
		//}
	}
}
