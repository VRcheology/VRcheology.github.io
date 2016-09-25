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

	public Feature (Vector3 _location, FeatureType _type, Sprite _image)
	{
		location = _location;
		type = _type;
		image = _image;
	}
}
