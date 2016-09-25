using UnityEngine;
using System.Collections;

public class AnimalBoneBlock : Block 
{
    public void ToggleImage()
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
