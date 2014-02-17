using UnityEngine;
using System.Collections;

public class JoypadPlayer : Player {
	
	const string kHAxis1 = "H1";
	const string kVAxis1 = "V1";
	const string kHAxis2 = "H2";
	const string kVAxis2 = "V2";
	
	CharacterController m_characterController;
	SkillShotAttacker m_skillShotAttacker;
	Vector3 m_lookDir;
	
	void Awake()
	{
		m_characterController = GetComponent<CharacterController>();
		if(m_characterController == null)
			Debug.LogError("Missing CharacterController component");
		
		m_skillShotAttacker = GetComponent<SkillShotAttacker>();
		if(m_skillShotAttacker == null)
			Debug.LogError("Missing SkillShotAttacker component");
		
		m_lookDir = Vector3.forward;
		
		StartCoroutine(InputPoll());
	}
	
	IEnumerator InputPoll()
	{
		float h;
		float v;
		while(true)
		{
			yield return 0;
			
			h = Input.GetAxis(kHAxis1);
			v = Input.GetAxis(kVAxis1);
			
			if(h != 0 || v != 0)
				MoveInput(h, v);
			
			//Debug.Log("h1 " + h + " v1 " + v);  
			
			h = Input.GetAxis(kHAxis2);
			v = Input.GetAxis(kVAxis2);
			
			if(h != 0 || v != 0)
				LookInput(h, v);
			
			//Debug.Log("h2 " + h + " v2 " + v);
			
			if(Input.GetKeyDown(KeyCode.JoystickButton5))// || Input.Get("Fire1"))
				FireInput();
		}
	}
	
	protected override void SubscribeToInputDelegates ()
	{
		//JoypadControls.Instance.OnMoveInput += MoveInput;
		//JoypadControls.Instance.OnLookInput += LookInput;
	}
	
	protected override void UnsubscribeToInputDelegates ()
	{
		//JoypadControls.Instance.OnMoveInput -= MoveInput;
		//JoypadControls.Instance.OnLookInput -= LookInput;
	}
	
	private void MoveInput(float horizontal, float vertical)
	{
		Vector3 target = new Vector3(horizontal, 0, vertical);
		//print(target);
		//navigator.MoveTo(target);
		m_characterController.Move(target);
	}
	
	private void LookInput(float horizontal, float vertical)
	{	
		m_lookDir = new Vector3(horizontal, 0, vertical);
		
		transform.LookAt(transform.position + m_lookDir);
	}
	
	private void FireInput()
	{
		m_skillShotAttacker.Attack(m_lookDir);
	}
}
