using UnityEngine;
using System.Collections;

public class DisplayBar : MonoBehaviour {
	public GameObject bar;
	public GameObject barFrame;
	public float maxX = 5f;
	public float maxValue = 100f;
	private float displayValue = 100f;
	public float DisplayValue
	{
		get { return displayValue; }
		set {
			StopCoroutine("goToValue");
			prevDisplayValue = displayValue;
			displayValue = Mathf.Max(0,value);
			StartCoroutine("goToValue");
		}
	}
	private float prevDisplayValue;
	private float lerpTime = 0.3f;
	private Vector3 initPosition;
	private Transform barTransform;
	
	void Awake()
	{
		barTransform = bar.transform;
		initPosition = barTransform.localPosition;
		
		
	}
	
	void Start()
	{
		Reset();
		barFrame.transform.localScale = new Vector3(scale(maxValue) + .5f,
		                                            barFrame.transform.localScale.y,
		                                            barFrame.transform.localScale.z);
	}
	
	public void Reset()
	{
		barTransform.localPosition = initPosition;
		prevDisplayValue = maxValue;
		displayValue = maxValue;
		Vector3 newScale = barTransform.localScale;
		newScale.x = scale(maxValue);
		barTransform.localScale = newScale;
	}
	
	private IEnumerator goToValue()
	{
		float goTime = 0f, lerpX;
		float targetLength = scale(displayValue);
		float prevTargetLength = scale(prevDisplayValue);
		Vector3 targetScale;
		Vector3 targetPosition;
		while(goTime <= lerpTime)
		{
			lerpX = Mathf.Lerp(prevTargetLength, targetLength, goTime / lerpTime); 
			targetScale = barTransform.localScale;
			targetScale.x = lerpX;
			
			targetPosition = barTransform.localPosition;
			targetPosition.x = (lerpX/2f) - (maxX/2f);//( * (displayValue/maxValue)
			
			barTransform.localPosition = targetPosition;
			//print(lerpX/2 + " " + maxX/2 + " " + barTransform.localPosition);
			
			barTransform.localScale = targetScale;
			yield return 0;
			goTime += Time.deltaTime;
		}
	}
	
	private float scale(float valToScale)
	{
		return (maxX * valToScale)/maxValue;
	}
}
