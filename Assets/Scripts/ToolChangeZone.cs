using UnityEngine;
using System.Collections;

public class ToolChangeZone : MonoBehaviour {

	[SerializeField]
	private GameObject toolPrefab;
	[SerializeField]
	private GameObject defaultToolPrefab;

	private GameObject nextTool;
	private GameObject changedHand;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (changedHand != null) {
			Transform parent = changedHand.transform.parent;
			SteamVR_RenderModel vrModel = changedHand.GetComponent<SteamVR_RenderModel> ();
			if (vrModel == null) {

				DestroyImmediate (changedHand);
			} else {
				vrModel.gameObject.SetActive (false);
			}
			if (nextTool.GetComponent<SteamVR_RenderModel> () == null) {
				GameObject newHandObj = GameObject.Instantiate (nextTool);
				newHandObj.transform.parent = parent;
				newHandObj.transform.localPosition = Vector3.zero;
//				newHandObj.SetActive (true);
			} else {
				vrModel = parent.GetComponentInChildren<SteamVR_RenderModel> (true);
				vrModel.gameObject.SetActive (true);
			}
			Debug.Log ("Changed hand to " + nextTool);
			changedHand = null;
		}
	}

	private void ChangeTool(GameObject hand, GameObject tool)
	{
		changedHand = hand;
		nextTool = tool;
	}

	private GameObject FindControllerModel(GameObject controller)
	{
		foreach (Transform child in controller.transform) {
			if (child.gameObject.activeSelf && child.tag == "ControllerModel") {
				return child.gameObject;
			}
		}
		return null;
	}


	void OnTriggerEnter(Collider collider)
	{
		SteamVR_TrackedObject trackedObject = collider.GetComponent<SteamVR_TrackedObject> ();
		if (trackedObject != null) {
//			if (ignoreNextCollision) {
//				ignoreNextCollision = false;
//				Debug.Log ("Ignoring collision");
//				return;
//			}

			Debug.Log ("TriggerEnter");

			ChangeTool (FindControllerModel(trackedObject.gameObject), toolPrefab);
//			ignoreNextCollision = true;
		}
	}

	void OnTriggerExit(Collider collider)
	{
//		Debug.LogError ("Pause");
		SteamVR_TrackedObject trackedObject = collider.GetComponent<SteamVR_TrackedObject> ();
		if (trackedObject != null) {
			Debug.Log ("TriggerExit");
			//change tool from active child
			ChangeTool (FindControllerModel(trackedObject.gameObject), defaultToolPrefab);
		}
	}
}
