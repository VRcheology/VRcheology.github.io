using UnityEngine;
using System.Collections;

public class ShowCanvasOnLook : OnLook {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected override void OnSetVisible()
    {
        foreach (Canvas canvas in gameObject.GetComponentsInChildren<Canvas>())
        {
            canvas.gameObject.SetActive(Visible);
        }
    }
}
