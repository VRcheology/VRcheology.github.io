using UnityEngine;
using System.Collections;

public class DigDetector : MonoBehaviour
{
    GameObject shovelPrefab;
    GameObject groundPrefab;

    GameObject ground;
    GameObject shovel;

	void Start ()
    {
        if (shovelPrefab != null)
        {
            shovel = Instantiate(shovelPrefab);
            shovel.transform.SetParent(transform);
            shovel.transform.localPosition = 50 * Vector3.forward;
        }

        if (groundPrefab != null)
        {
            ground = Instantiate(shovelPrefab);
            ground.transform.SetParent(transform.parent);
            ground.transform.localPosition = Vector3.zero;
        }
    }
}
