using UnityEngine;
using System.Collections;

public class ScreenOverlay : MonoBehaviour {
	
	public GameObject redFlash;
	private Material flashMat;
	// Use this for initialization
	void Start () {
		flashMat = redFlash.renderer.material;
		Color color = flashMat.GetColor("_Color");
		color.a = 0;
		redFlash.renderer.material.SetColor("_Color", color);
	}
	
	public void FlashRed()
	{
		StartCoroutine(flash());
	}
	
	private IEnumerator flash()
	{
		float lerpTime = 0, lerpLength = 0.05f;
		Color color = flashMat.GetColor("_Color");
		/*while(lerpTime <= lerpLength)
		{
			color.a = Mathf.Lerp(0, 1, lerpTime/lerpLength);
			flashMat.SetColor("_Color", color);
			yield return 0;
			lerpTime += Time.deltaTime;
		}
		*/
		color.a = 1;
		flashMat.SetColor("_Color", color);
		
		lerpLength = 0.25f;
		lerpTime = 0;
		while(lerpTime <= lerpLength)
		{
			color.a = Mathf.Lerp(1, 0, lerpTime/lerpLength);
			flashMat.SetColor("_Color", color);
			yield return 0;
			lerpTime += Time.deltaTime;
		}
	}
}
