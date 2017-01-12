using UnityEngine;
using System.Collections;

public class ObjectDataPanel : MonoBehaviour {

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

    public void OpenPanel (Block block)
    {
        transform.position = block.transform.position;
        Show(true);
    }

    void Show (bool show)
    {

    }
}
