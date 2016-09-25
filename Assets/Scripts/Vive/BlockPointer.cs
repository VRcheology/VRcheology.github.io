using UnityEngine;
using System.Collections;

public class BlockPointer : ViveController
{
    public LayerMask blockLayer;
    public Block selectedBlock;
    public GameObject shovel;
    public GameObject hand;
    public GameObject ground;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ground)
        {
            if (!shovel.activeSelf)
            {
                shovel.SetActive(true);
                if (hand.transform.childCount > 0)
                {
                    hand.SetActive(false);
                }
            }
            GetSelectedBlock();
            if (selectedBlock != null)
            {
                Debug.Log("excavate");
                selectedBlock.Excavate();
            }
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.gameObject == ground)
        {
            if (shovel.activeSelf)
            {
                shovel.SetActive(false);
                hand.SetActive(true);
            }
        }
    }

    void GetSelectedBlock ()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, blockLayer))
        {
            Block block = hit.transform.GetComponent<Block>();
            if (block == null)
            {
                block = hit.transform.GetComponent<AnimalBoneBlock>();
            }
            if (block == null)
            {
                block = hit.transform.GetComponent<PotteryBlock>();
            }
            if (block != selectedBlock)
            {
                selectedBlock = block;
                if (selectedBlock != null)
                {
                    Debug.Log(selectedBlock.name);
                }
                else
                {
                    Debug.Log("selectedBlock is null");
                }
            }
        }
        else
        {
            Debug.Log("null");
            selectedBlock = null;
        }
    }

    public override void OnTriggerPressed()
    {
        Debug.Log("trigger " + selectedBlock.name + " " + selectedBlock.GetType());
        if (selectedBlock != null)
        {
            (selectedBlock as Block).ToggleImage();
        }
    }
}
