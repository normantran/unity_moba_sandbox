using UnityEngine;
using System.Collections;

public struct TouchData
{
	public int touchIndex;
	public Vector3 touchPosition;
}

/// <summary>
/// Currently placed inside the MainCamera in the scene.
/// Shoots Raycasts and calls events on the objects it hits.
/// </summary>
public class RaycastMouseBetter : MonoBehaviour 
{
	public float dragThreshold;
	
	private Transform touchedTransform;
	private Touch touch0;
	//private Touch[] touches;
	private Transform[] touchedTransforms;
	//private bool touchIsStillValid;
	private int click;

	public void Start () 
	{
		//touches = new Touch[10];
		touchedTransforms = new Transform [10];
		
		if(Application.platform == RuntimePlatform.IPhonePlayer)
			StartCoroutine (iPhoneInputWatch ());
		else
			StartCoroutine (InputWatch ());
	}
	
	private IEnumerator iPhoneInputWatch ()
	{
		while (true)
		{
			#region iPhone/iPad Controls
			if (Input.touchCount <= 10)
			{
				for (int i = 0; i < Input.touchCount; ++i)
					DoTouchPhase (i);
			}
		/*	if (Input.touchCount == 1) 
		 * {
		
				touch0 = Input.GetTouch (0);
				
				if (touch0.phase == TouchPhase.Began)
				{
					InitMouseTouchRaycast ();
					touchIsStillValid = true;
				}
				
				if (touch0.phase == TouchPhase.Moved && touchedTransform != null && touchIsStillValid)
				{
					ContinueMouseTouchRaycast ();
				}
				
				if (touch0.phase == TouchPhase.Ended && touchedTransform != null && touchIsStillValid)
				{
					EndMouseTouchRaycast();
				}
			}
			else if (Input.touchCount > 1)
			{
				touchIsStillValid = false;
				if (touchedTransform != null)
					touchedTransform.SendMessage ("CancelMouseTouchRayCast");
			}
			*/
			#endregion
			yield return 0;
		}
	}
	
	private void DoTouchPhase (int touchIndex)
	{
		//print("Touch " + touchIndex);
		Touch touch = Input.GetTouch (touchIndex);
		
		Transform touchedTransform = touchedTransforms [touchIndex];
		
		Vector3 position = new Vector3 (touch.position.x, touch.position.y, Camera.main.nearClipPlane);
		RaycastHit hit = new RaycastHit ();
		if (ShootRaycast (position, out hit))
		{
			if( hit.point == null)
				return;
			
			if (touch.phase == TouchPhase.Began)
			{
				InitMouseTouchRaycast (touchIndex, hit);
				//touchIsStillValid = true;
			}
			else if (touch.phase == TouchPhase.Moved && touchedTransform != null)
			{
				ContinueMouseTouchRaycast (touchIndex, touchedTransform, hit);
			}
			else if (touch.phase == TouchPhase.Ended && touchedTransform != null)
			{
				EndMouseTouchRaycast (touchIndex, touchedTransform, hit);
				touchedTransforms [touchIndex] = null;
			}
		}
	}
	
	private IEnumerator InputWatch ()
	{
		while (true)
		{
			Vector3 position = Input.mousePosition;
			position.z       = Camera.main.nearClipPlane;
			
			Transform touchedTransform = touchedTransforms [0];
			
			RaycastHit hit = new RaycastHit ();
			if (ShootRaycast (position, out hit))
			{
				if(hit.point != null)
				{
					if (Input.GetMouseButtonDown (0))
					{//print("Click " + click++);
						InitMouseTouchRaycast (0, hit);
					}
					if (Input.GetMouseButton (0) && touchedTransform != null)
					{
						ContinueMouseTouchRaycast (0, touchedTransform, hit);
					}
					
					if (Input.GetMouseButtonUp (0) && touchedTransform != null)
					{//print("mouseup " + click);
						EndMouseTouchRaycast (0, touchedTransform, hit);
						touchedTransforms [0] = null;
					}
				}
			}
			
			yield return 0;
		}
	}
	
	private void InitMouseTouchRaycast (int touchIndex, RaycastHit hit)
	{
		//RaycastHit hit = new RaycastHit ();
		//if (ShootRaycast (out hit))
	//	{
			if (hit.transform.tag != "HUD")
			{
				//Make sure no object is effected by multiple touches at the same time.
				// Might want to make this more elegant later. Like have the objects themselves
				// 	choose what to do with multiple touches.
				/*for (int i = 0; i < touchedTransforms.Length; ++i)
					if (touchedTransforms [i] != null)
						if (touchedTransforms [i] == hit.transform)
							return;
			*/
				TouchData data = new TouchData();
				data.touchIndex = touchIndex;
				data.touchPosition = hit.point;
				touchedTransforms [touchIndex] = hit.transform;
				touchedTransforms [touchIndex].SendMessage ("OnMouseStart", data, SendMessageOptions.DontRequireReceiver);
			}
	//	}
	}
	
	private Vector3 oldHitPoint;
	private void ContinueMouseTouchRaycast (int touchIndex, Transform touchedTransform, RaycastHit hit)
	{
		//RaycastHit hit = new RaycastHit ();
		//if (ShootRaycast (out hit))
		//{
			TouchData data = new TouchData();
			data.touchIndex = touchIndex;
			data.touchPosition = hit.point;
			if (hit.point != oldHitPoint)
				touchedTransform.SendMessage ("OnMouseDraggingMoved", data, SendMessageOptions.DontRequireReceiver);
			else
				touchedTransform.SendMessage ("OnMouseDragging", data, SendMessageOptions.DontRequireReceiver);
			
			//oldHitPoint = hit.point;
	/*	}
		else //If the user drags the mouse into empty space.
		{
			touchedTransform.SendMessage ("OnMouseDraggedOff", SendMessageOptions.DontRequireReceiver);
		}
		*/
		oldHitPoint = hit.point;
	}
	
	private void EndMouseTouchRaycast (int touchIndex, Transform touchedTransform, RaycastHit hit)
	{
	//	RaycastHit hit = new RaycastHit ();
	//	if (ShootRaycast (out hit)) 
	//	{	
			TouchData data = new TouchData();
			data.touchIndex = touchIndex;
			data.touchPosition = hit.point;
			touchedTransform.SendMessage ("OnMouseStop", data, SendMessageOptions.DontRequireReceiver);
	/*	}
		else //If user lifts finger in empty space.
		{
			touchedTransform.SendMessage ("OnMouseDraggedOffUp", SendMessageOptions.DontRequireReceiver);
		}
	*/	
	}
	
	private bool ShootRaycast (Vector3 position, out RaycastHit hit)
	{	
		/*
		Vector3 mousePos;
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			mousePos = new Vector3 (touch0.position.x, touch0.position.y, Camera.main.nearClipPlane);
		else
		{		
			mousePos   = Input.mousePosition;
			mousePos.z = Camera.main.nearClipPlane;
		}
		*/
		Vector3 startPos = Camera.main.ScreenToWorldPoint (position);
		Debug.DrawLine (startPos, startPos + (Camera.main.transform.forward * 1000), Color.red);
		int layerMask = 1 << LayerMask.NameToLayer ("Touchable"); //Bit shift the layermask so that this raycast only hits actors we want it to.
		int layerMask2 = 1 << LayerMask.NameToLayer("Environment");
		layerMask = layerMask | layerMask2;
		//layerMask = ~layerMask;
		return Physics.Raycast (startPos, Camera.main.transform.forward, out hit,1000, layerMask); //Don't use layerMask. Simply dont' spawn the lantern if you touch an object.
	}
}