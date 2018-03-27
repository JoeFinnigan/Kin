using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform if null.
	public Transform camTransform;

	// How long the object should shake for.
	public float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	public Button statsButton, menuButton;

	Vector3 originalPos;

	void Awake(){
		if (camTransform == null){
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable(){
		originalPos = camTransform.localPosition;
	}

	void Update(){
		ShakeCamera ();
	}

	public void ShakeCamera(){
		if (shakeDuration > 0){
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			shakeDuration -= Time.deltaTime * decreaseFactor;

			// Bug fix - clicking stats/menu buttons while shake was active caused them to permanently disppear from screen
			statsButton.interactable = false;
			menuButton.interactable = false;
		} else {
			shakeDuration = 0f;
			camTransform.localPosition = originalPos;

			statsButton.interactable = true;
			menuButton.interactable = true;
		}
	}
}