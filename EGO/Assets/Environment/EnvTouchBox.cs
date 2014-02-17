using UnityEngine;
using System.Collections;

public class EnvTouchBox : MonoBehaviour {
	
	void OnMouseStart(TouchData data)
	{
		GameController.instance.PlayerTouchedEnvironment(data);
	}
}
