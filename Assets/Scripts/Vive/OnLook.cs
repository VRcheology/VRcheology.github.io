using UnityEngine;
using System.Collections;

public abstract class OnLook : MonoBehaviour
{
    private bool visible;

    public bool Visible
    {
        get { return visible; }
        set
        {
            if (visible != value)
            {
                visible = value;
                OnSetVisible();
            }
        }
    }

    public Transform CameraTransform { get; set; }

    [SerializeField]
    private float delay;

    [SerializeField]
    private float maxViewAngle = 5;

    private float maxViewCos;

    private float timer;

    // Use this for initialization
    void Start()
    {
        maxViewCos = Mathf.Cos(Mathf.Deg2Rad * maxViewAngle);
    }

    // Update is called once per frame
    void Update()
    {
        if (Visible)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = delay;

                Vector3 fromCamera = transform.position - CameraTransform.position;
                fromCamera.Normalize();

                if (Vector3.Dot(fromCamera, CameraTransform.forward) < maxViewCos)
                {
                    Visible = false;
                }
            }
        }
    }

    protected abstract void OnSetVisible();
}