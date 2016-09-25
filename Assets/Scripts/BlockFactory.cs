using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockFactory : MonoBehaviour 
{
	public Block blockPrefab;
	public Block animalBonePrefab;
	public Block potteryPrefab;
	public Block objectPrefab;
    public GameObject ground;
    public DigData data;

	public Vector3 size;
	public Block[,,] blocks;
	public Queue<Feature> features = new Queue<Feature>();

	Feature nextFeature = null;
    float maxHeight = 0;
    Vector3 positionOffset;

	void Start ()
	{
        CenterSite();
        //MakeTestFeatures();
        features = new Queue<Feature>(data.features);
		GenerateBlocks();
	}

    void Update ()
    {
        if (Camera.main.transform.position.y > maxHeight)
        {
            maxHeight = Camera.main.transform.position.y;
        }
        ground.transform.position = transform.position - positionOffset + Mathf.Clamp(0.2f * maxHeight / 1.6f, 0.1f, 2f) * Vector3.up;
        //Debug.Log(ground.transform.position.y + 2f);
    }

    void CenterSite ()
    {
        positionOffset = new Vector3((1 - size.x) / 2, -1, (1 - size.z) / 2);
        transform.position = positionOffset;
        ground.transform.localScale = new Vector3(size.x, 0.2f, size.z);
    }

	void GenerateBlocks ()
	{
		if (blockPrefab != null && blockPrefab.GetComponent<Block>())
		{
			if (features.Count > 0) 
			{
				nextFeature = features.Dequeue();
			}

			blocks = new Block[Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.y), Mathf.RoundToInt(size.z)];
			for (int u = 0; u < size.x; u++)
			{
				for (int v = 0; v < size.y; v++)
				{
					for (int w = 0; w < size.z; w++)
					{
						Vector3 location = new Vector3(u, v, w);

						blocks[u, v, w] = CreateBlock( GetType(location) );
						blocks[u, v, w].transform.SetParent(transform);
						blocks[u, v, w].Init(location);
					}
				}
			}
		}
	}

	FeatureType GetType (Vector3 location)
	{
		FeatureType type = FeatureType.None;
		if (nextFeature != null && nextFeature.location == location) 
		{
			type = nextFeature.type;
			Debug.Log("Making " + type.ToString() + " at " + location);
			if (features.Count > 0) 
			{
				nextFeature = features.Dequeue();
			}
			else
			{
				nextFeature = null;
			}
		}
		return type;
	}

	Block CreateBlock (FeatureType type)
	{
		switch (type)
		{
		case FeatureType.AnimalBone :
			
			return Instantiate(animalBonePrefab).GetComponent<Block>();

		case FeatureType.Object :

			return Instantiate(objectPrefab).GetComponent<Block>();

		case FeatureType.Pottery :

			return Instantiate(potteryPrefab).GetComponent<Block>();

		default :

			return Instantiate(blockPrefab).GetComponent<Block>();
		}
	}

	void MakeTestFeatures ()
	{
		features.Enqueue(new Feature(new Vector3(0, 5, 2), FeatureType.Pottery, null));
		features.Enqueue(new Feature(new Vector3(1, 0, 0), FeatureType.Pottery, null));
		features.Enqueue(new Feature(new Vector3(4, 3, 1), FeatureType.Object, null));
		features.Enqueue(new Feature(new Vector3(5, 5, 2), FeatureType.AnimalBone, null));
	}

    public Block GetBlockAbove (Vector3 location)
    {
        if (location.y > 0)
        {
            return blocks[Mathf.RoundToInt(location.x), Mathf.RoundToInt(location.y) - 1, Mathf.RoundToInt(location.z)];
        }
        else
        {
            return null;
        }
    }

    public Block GetBlockBelow (Vector3 location)
    {
        if (location.y < size.y - 1)
        {
            return blocks[Mathf.RoundToInt(location.x), Mathf.RoundToInt(location.y) + 1, Mathf.RoundToInt(location.z)];
        }
        else
        {
            return null;
        }
    }
}
