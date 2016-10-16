using UnityEngine;
using System.Collections;

public enum FeatureType
{
	None,
	Object,
	Pottery,
	AnimalBone,
    ArchitecturalElement
}

[System.Serializable]
public class Feature 
{
	public Vector3 location;
	public FeatureType type;
	public Sprite image;
	public Vector2 timePeriod;
    private GameObject obj;

	public Feature (Vector3 _location, FeatureType _type, Sprite _image)
	{
		location = _location;
		type = _type;
		image = _image;
	}

    public GameObject CreateGameObject()
    {
        Debug.Log(obj);
        if (obj == null)
        {

            obj = GameObject.Instantiate(DataImporter.Instance.FeaturePrefab);

            obj.GetComponent<Transform>().position = location;
            
        }

        return obj;
    }
}
