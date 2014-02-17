using UnityEngine;
using System.Collections;

public class Navigator : MonoBehaviour {
	
	public float moveSpeed = 5f;
	
	private GameObject moveTarget;
	private Vector3 moveTargetPosition;
	private Transform thisTransform;
	private CharacterController characterController;
	
	#region Events
	
	public delegate void MovingEventHandler(Vector3 targetCoordinate);
	public delegate void EventHandler();
	public event EventHandler OnStartWalk;
	public event MovingEventHandler OnWalking;
	public event EventHandler OnStopWalk;
	
	#endregion Events
	
	void Awake()
	{
		thisTransform = transform;
		characterController = GetComponent<CharacterController>();
	}
	
	public void MoveToward(GameObject targetCharacter)
	{
		if(!targetCharacter.Equals(moveTarget))
		{
			moveTarget = targetCharacter;
			MoveTo(targetCharacter.transform.position);
		}
		else
		{
			moveTargetPosition = targetCharacter.transform.position;
			//moveTargetPosition.z = 0;
		}
	}
	
	public void StopMoving()
	{
		if(OnStopWalk != null)
			OnStopWalk();
		StopCoroutine("walk");
		moveTarget = null;
	}
	
	public void MoveTo(Vector3 position)
	{
				if(OnStartWalk != null)
			OnStartWalk();
			
		
		StopCoroutine("walk");
		moveTargetPosition = position;
		//moveTargetPosition.z = 0;
		StartCoroutine("walk");
	}
	
	private IEnumerator walk()
	{
		while(thisTransform.position != moveTargetPosition)
		{
			if(OnWalking != null)
				OnWalking(moveTargetPosition);
			characterController.SimpleMove((moveTargetPosition - thisTransform.position).normalized * moveSpeed);
			//thisTransform.position = Vector3.MoveTowards(thisTransform.position, moveTargetPosition, moveSpeed * Time.deltaTime);
			yield return 0;
		}
	}
}
