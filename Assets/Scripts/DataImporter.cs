using UnityEngine;
using System.Collections;
using UnityEditor;

public class DataImporter : Singleton<DataImporter>
{

    public GameObject FeaturePrefab;

	// Use this for initialization
	void Start ()
	{
	    LoadDigData();
	}

    private void LoadDigData()
    {
        Debug.Log("LoadData");
        DigData data = AssetDatabase.LoadAssetAtPath<DigData>(DigData.PATHNAME);
        foreach (Feature feature in data.features)
        {
            feature.CreateGameObject();
        }
    }

    // Update is called once per frame
	void Update () {
	
	}
}
