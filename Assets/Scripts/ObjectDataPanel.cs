using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectDataPanel : MonoBehaviour
{
    public Image image1;
    public Image image2;
    public Text description;

    static ObjectDataPanel _Instance;
    public static ObjectDataPanel Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = GameObject.FindObjectOfType<ObjectDataPanel>();
            }
            return _Instance;
        }
    }

    GameObject _canvases;
    GameObject canvases
    {
        get
        {
            if (_canvases == null)
            {
                _canvases = transform.FindChild("Canvases").gameObject;
            }
            return _canvases;
        }
    }

    public bool showing
    {
        get
        {
            return canvases != null && canvases.activeSelf;
        }
    }

    public void TogglePanel (Block block)
    {
        if (block != null && block.feature.type != FeatureType.None)
        {
            PopulateWith(block.feature);
            PositionAbove(block);
            LookAtCamera();
            Show(true);
        }
        else if (showing)
        {
            Show(false);
        }
    }

    void Show (bool show)
    {
        if (canvases != null)
        {
            canvases.SetActive(show);
        }
    }

    void PopulateWith (Feature feature)
    {
        if (feature.image1 != null)
        {
            image1.sprite = feature.image1;
        }
        if (feature.image2 != null)
        {
            image2.sprite = feature.image2;
        }
        description.text = feature.description;
    }

    void PositionAbove (Block block)
    {
        transform.position = new Vector3(block.transform.position.x, 1.5f, block.transform.position.z);
    }

    void LookAtCamera ()
    {
        transform.LookAt(Camera.main.transform);
    }
}
