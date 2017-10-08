using UnityEngine;

public class MuseumSceneManager : MonoBehaviour {

	public MuseumInputManager inputManager;
	
	void Start () {
		Input.backButtonLeavesApp = true;
	}
	
	void Update () {
		// Exit when (X) is tapped.
    	if (Input.GetKeyDown(KeyCode.Escape)) {
      		Application.Quit();
    	}
		// Clear messages when the user clicks
		if (Input.anyKeyDown) {
			if (inputManager != null) {
				if (inputManager.messageCanvas.activeSelf) {
					inputManager.messageCanvas.SetActive(false);
				}
			}
		}
	}
}
