using UnityEngine;
using System.Collections;

public class Shovel : MonoBehaviour
{
    public GameObject ground;
    public bool inGround;
    public GameObject geometry;
    bool disabled = false;

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

    void Update ()
    {

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

    //void OnTriggerExit (Collider other)
    //{
    //    if (other.gameObject == ground)
    //    {
    //        if (pointer.selectedBlock != null)
    //        {
    //            pointer.selectedBlock.Excavate();
    //        }
    //    }
    //}
}
