using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaydreamContent : MonoBehaviour {

	private GvrVideoPlayerTexture videoPlayer;
	private GameObject cameraRig;
	public Vector3 position = Vector3.zero;
	private bool ready = false;

	void Start () {
		if (position == Vector3.zero) {
			position = gameObject.transform.position + Vector3.up * 1.75f;
		}
		cameraRig = Camera.main.transform.parent.gameObject;		
	}
	
	void Update () {
		if(cameraRig.transform.position == position) {
			//videoPlayer.ReInitializeVideo();
			if (!ready) {
				enableElements();
			}
			
			if (videoPlayer != null && videoPlayer.isActiveAndEnabled) {
				videoPlayer.Play();
			}
		} else {
			if (videoPlayer != null) {
				if (!videoPlayer.IsPaused) {
					videoPlayer.Pause();
				}
			}
		}
	}

	private void enableElements() {
		int children = gameObject.transform.childCount;
		Debug.Log("Enabling " + children + " children");
		for (int i = 0; i < children; i++) {
			gameObject.transform.GetChild(i).gameObject.SetActive(true);
		}
		videoPlayer = GetComponentInChildren<GvrVideoPlayerTexture>();
		if (videoPlayer != null) {
			videoPlayer.Init();
		} else {
			Debug.Log("GvrVideoPlayerTexture not found");
		}
		ready = true;
	}
}
