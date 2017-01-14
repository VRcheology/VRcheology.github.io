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
	public Vector2 timePeriod;
    public Sprite image1;
    public Sprite image2;
    public string description;

    public Feature (Vector3 _location, FeatureType _type)
	{
		location = _location;
		type = _type;
	}
}
