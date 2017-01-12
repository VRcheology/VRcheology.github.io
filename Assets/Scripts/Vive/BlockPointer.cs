using UnityEngine;

public class BlockPointer : ViveController
{
    public LayerMask blockLayer;
    public Block selectedBlock;
    public GameObject shovel;
    public GameObject hand;
    public GameObject ground;
    public GameObject beam;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ground)
        {
            shovel.transform.localRotation = Quaternion.identity;
            beam.SetActive(true);
            GetSelectedBlock();
            if (selectedBlock != null)
            {
                selectedBlock.Excavate();
            }
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.gameObject == ground)
        {
            shovel.transform.localRotation = Quaternion.Euler(-45f * Vector3.right);
            beam.SetActive(false);
        }
    }

    void GetSelectedBlock ()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, blockLayer))
        {
            Block block = hit.transform.GetComponent<AnimalBoneBlock>();
            if (block == null)
            {
                block = hit.transform.GetComponent<PotteryBlock>();
            }
            if (block == null)
            {
                block = hit.transform.GetComponent<Block>();
            }
            if (block != selectedBlock)
            {
                selectedBlock = block;
            }
        }
        else
        {
            selectedBlock = null;
        }
    }

    public override void OnTriggerPressed()
    {
        GetSelectedBlock();
        ObjectDataPanel.Instance.TogglePanel(selectedBlock);
    }
}
