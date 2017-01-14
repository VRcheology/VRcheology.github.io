using UnityEngine;
using System.Collections;

public class Shovel : MonoBehaviour
{
    public GameObject ground;
    public bool inGround;
    public GameObject geometry;

    BlockPointer _pointer;
    BlockPointer pointer
    {
        get
        {
            if (_pointer == null)
            {
                _pointer = GetComponentInParent<BlockPointer>();
            }
            return _pointer;
        }
    }

    void OnTriggerEnter (Collider other)
    {   
        if (other.gameObject == ground)
        {
            inGround = true;
            if (pointer.selectedBlock != null)
            {
                pointer.selectedBlock.Excavate();
            }
        }
    }

    public void SetEnabled (bool enable)
    {
        geometry.SetActive(enable);
    }
}
