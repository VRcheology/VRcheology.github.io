using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class Block : MonoBehaviour 
{
    public int excavated = 2;

    public MeshRenderer fullGeometry;
    public MeshRenderer halfGeometry;
    MeshRenderer currentGeometry;

    public Feature feature;

    BlockFactory _factory;
    BlockFactory factory
    {
        get
        {
            if (_factory == null)
            {
                _factory = GetComponentInParent<BlockFactory>();
            }
            return _factory;
        }
    }

    MeshRenderer _meshRenderer;
    public MeshRenderer meshRenderer
    {
        get
        {
            if (_meshRenderer == null)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
            }
            return _meshRenderer;
        }
    }

    BoxCollider _boxCollider;
    BoxCollider boxCollider
    {
        get
        {
            if (_boxCollider == null)
            {
                _boxCollider = GetComponent<BoxCollider>();
            }
            return _boxCollider;
        }
    }

    public virtual void Init (Feature _feature)
	{
        feature = _feature;
		transform.localPosition = new Vector3(feature.location.x, -feature.location.y, feature.location.z);
		name = GetType().ToString() + "_" + feature.location.x + ":" + feature.location.y + ":" + feature.location.z;
        //CheckVisibility();
	}

    public virtual void Excavate ()
    {
        if (excavated > 0)
        {
            excavated--;
            if (fullGeometry != null)
            {
                fullGeometry.enabled = false;
                boxCollider.enabled = false;
            }
        }
    }

    bool AtSurface
    {
        get
        {
            Block blockAbove = factory.GetBlockAbove(feature.location);
            return excavated > 0 && (blockAbove == null || blockAbove.excavated == 0);
        }
    }
}
