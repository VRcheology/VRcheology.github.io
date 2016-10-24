using UnityEngine;
using System.Collections;

public class UIActivator : MonoBehaviour
{

    [SerializeField]
    private float delay;

    [SerializeField]
    private float maxActivationDistance = Mathf.Infinity;

    private float timer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (timer > 0)
	    {
	        timer -= Time.deltaTime;
	    }
	    else
	    {
	        timer = delay;

	        RaycastHit hitInfo;
	        Physics.Raycast(transform.position, transform.forward, out hitInfo, maxActivationDistance);

	        GameObject hitObject = hitInfo.transform.gameObject;

	        OnLook hitOnLook = hitObject.GetComponent<OnLook>();

	        if (hitOnLook != null)
	        {
	            hitOnLook.Visible = true;
	            hitOnLook.CameraTransform = transform;
	        }
	    }
	}
}
