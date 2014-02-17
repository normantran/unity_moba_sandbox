using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour {
	
	public GameObject wayPointPrefab;
	public GameObject inputBlockerPrefab;
	public GameObject powerIconPrefab;
	
	private GameObject spawnedWayPoint;
	private GameObject spawnedBlocker;
	private GameObject spawnedPowerIcon;
	
	private Rect guiRect;
	private Rect pointsRect;
	private Rect multiplierRect;
	private Rect restartButtonRect;
	private Rect skillButtonRect;
	private Rect spawnMinionButtonRect;
	private Rect timerRect;
	
	void OnGUI()
	{
		//GUI.Box(guiBox, GameController.instance.Points.ToString());
		if(GameController.instance == null)
			return;
		
		GUI.Box(pointsRect, GameController.instance.Points.ToString());
		GUI.Box(multiplierRect, string.Concat('x', GameController.instance.Multiplier.ToString()));
		
		if(GUI.Button(restartButtonRect, "R"))
			GameController.instance.RestartLevel();
		
		if(GameController.instance.CanUseSkill(0))
		{
			if(GUI.Button(skillButtonRect, "S"))
			{
				GameController.instance.UseSkill(0);
			}
		}
		else
			GUI.Box(skillButtonRect, string.Empty);
		
		//if(GUI.Button(spawnMinionButtonRect, "Spawn"))
		//	GameController.instance.MinionButtonPress();
		
		GUI.Box(timerRect, GameController.instance.RoundTimer.ToString());
	}
	
	void Awake()
	{
		GameController.OnPlayerTouchEnvironment += touchEnv;
		GameController.OnPlayerTouchTarget += touchTarget;
		GameController.OnPlayerKillTarget += killedTarget;
		GameController.OnPlayerFailToKillTarget += killFail;
		GameController.OnTimerChange += timerChange;
	}
	
	void Start()
	{
		float pointsWidth = Screen.width/6;
		float multiWidth = Screen.width/10;
		float height = Screen.width/20;
		//guiBox = new Rect(0,0,Screen.width/4, Screen.height/20);
		pointsRect = new Rect(Screen.width/2 - pointsWidth, 0, pointsWidth, height);
		multiplierRect = new Rect(Screen.width/2, 0,  multiWidth, height);
		restartButtonRect = new Rect(Screen.width - multiWidth, 0, multiWidth, height);
		skillButtonRect = new Rect(0,Screen.height - height, height, height);
		spawnMinionButtonRect = new Rect(height, Screen.height- height, height,height);
		timerRect = new Rect(Screen.width/2 - pointsWidth, height, pointsWidth, height);
			
		spawnedWayPoint = Instantiate(wayPointPrefab) as GameObject;//, wayPointHoldingPosition, wayPointPrefab.transform.rotation);
		
		if(powerIconPrefab != null)
		{
			spawnedPowerIcon = Instantiate(powerIconPrefab) as GameObject;
			spawnedPowerIcon.transform.parent = Camera.main.transform;
			spawnedPowerIcon.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - pointsWidth, Screen.height -  height, 1));
			HidePowerIcon();
		}
	}
	
	void OnDestroy()
	{
		GameController.OnPlayerTouchEnvironment -= touchEnv;
		GameController.OnPlayerTouchTarget -= touchTarget;
		GameController.OnPlayerKillTarget -= killedTarget;
		GameController.OnPlayerFailToKillTarget -= killFail;
		GameController.OnTimerChange -= timerChange;
	}
	
	public void BlockInput()
	{
		if(spawnedBlocker == null)
		{
			spawnedBlocker = Instantiate(inputBlockerPrefab, Vector3.zero, Camera.main.transform.rotation) as GameObject;
			spawnedBlocker.transform.parent = Camera.main.transform;
			spawnedBlocker.transform.localPosition = new Vector3(0,0,1f);
		}
		
		spawnedBlocker.SetActiveRecursively(true);
	}
	
	public void UnBlockInput()
	{
		if(spawnedBlocker != null)
			spawnedBlocker.SetActiveRecursively(false);
	}
	
	public void ShowPowerIcon()
	{
		spawnedPowerIcon.SetActiveRecursively(true);
	}
	
	public void HidePowerIcon()
	{
		spawnedPowerIcon.SetActiveRecursively(false);
	}
	
	private void touchEnv(TouchData data)
	{
		//spawnedWayPoint.SendMessage("PointAt", data.touchPosition);
	}
	
	private void touchTarget(GameObject touched)
	{
		//spawnedWayPoint.SendMessage("PointAt", touched.transform.position);
		//spawnedWayPoint.SendMessage("Deactivate");
	}
	
	void timerChange(float time)
	{
		//Change clock when not using OnGUI
	}
	
	private void killedTarget(Minion minion)
	{
		
	}
	
	private void killFail()
	{
		
	}

}
