using UnityEngine;
using System.Collections;


// This class represents on 1x1 meter block from an excavation.
// It is used by BlockFactory.cs to create a number of blocks equal to 
// excavation coordinates from the OpenContext data and is filled with
// the artifacts which originate from those coordinates.

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class Block : MonoBehaviour 
{
    public int excavated = 2;
	public Vector3 location;

    public MeshRenderer fullGeometry;
    public MeshRenderer halfGeometry;
    MeshRenderer currentGeometry;
    public GameObject image;

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

    public virtual void Init (Vector3 _location)
	{
		location = _location;
		transform.localPosition = new Vector3(location.x, -location.y, location.z);
        //boxCollider.center = location.y * Vector3.up;
		name = GetType().ToString() + "_" + location.x + ":" + location.y + ":" + location.z;
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
            Block blockAbove = factory.GetBlockAbove(location);
            return excavated > 0 && (blockAbove == null || blockAbove.excavated == 0);
        }
    }

    public void ToggleImage ()
    {
        if (image != null)
        {
            if (image.activeSelf)
            {
                image.SetActive(false);
            }
            else
            {
                image.transform.position = new Vector3(image.transform.position.x, 1.5f, image.transform.position.z);
                image.SetActive(true);
            }
        }
    }
}
