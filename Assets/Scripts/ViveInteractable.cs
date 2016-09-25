using UnityEngine;
using System.Collections;

public class ViveInteractable : MonoBehaviour {

	private Transform parent;

	public Transform Parent 
	{
		get
		{
			return parent;
		}
	}

	void Awake()
	{
		parent = transform.parent;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
