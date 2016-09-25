using UnityEngine;
using System.Collections;
using Valve.VR;

public enum DPadDirection
{
    Up,
    Left,
    Right,
    Down
}

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ViveController : MonoBehaviour
{
    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device controller
    {
        get
        {
            if (trackedObj == null)
            {
                trackedObj = GetComponent<SteamVR_TrackedObject>();
            }
            return SteamVR_Controller.Input( (int)trackedObj.index );
        }
    }

    Vector2 dPadPosition;
    bool[] dPadHovering = new bool[4];
	
	// Update is called once per frame
	void Update ()
    {
        GetInput();
	}

    void GetInput ()
    {
        if (controller == null)
        {
            Debug.LogWarning("Vive controller not initialized");
            return;
        }
        
        GetDPadHover();
        GetDPadPress();
        GetTrigger();
    }

    void GetDPadHover ()
    {
        dPadPosition = controller.GetAxis();

        if (dPadPosition.y > 0.4f)
        {
            if (!dPadHovering[(int)DPadDirection.Up])
            {
                GetDPadExit();
                dPadHovering[(int)DPadDirection.Up] = true;
                OnDPadUpEnter();
            }
        }
        else if (dPadPosition.y < -0.4f)
        {
            if (!dPadHovering[(int)DPadDirection.Down])
            {
                GetDPadExit();
                dPadHovering[(int)DPadDirection.Down] = true;
                OnDPadDownEnter();
            }
        }
        else if (dPadPosition.x < -0.4f)
        {
            if (!dPadHovering[(int)DPadDirection.Left])
            {
                GetDPadExit();
                dPadHovering[(int)DPadDirection.Left] = true;
                OnDPadLeftEnter();
            }
        }
        else if (dPadPosition.x > 0.4f)
        {
            if (!dPadHovering[(int)DPadDirection.Right])
            {
                GetDPadExit();
                dPadHovering[(int)DPadDirection.Right] = true;
                OnDPadRightEnter();
            }
        }
    }

    void GetDPadExit ()
    {
        for (int i = 0; i < dPadHovering.Length; i++)
        {
            if (dPadHovering[i])
            {
                dPadHovering[i] = false;
                OnDPadExit();
            }
        }
    }

    void GetDPadPress ()
    {
        if (controller.GetPressUp(EVRButtonId.k_EButton_SteamVR_Touchpad))
        {
            if (dPadHovering[(int)DPadDirection.Up])
            {
                OnDPadUpPressed();
            }
            else if (dPadHovering[(int)DPadDirection.Left])
            {
                OnDPadLeftPressed();
            }
            else if (dPadHovering[(int)DPadDirection.Right])
            {
                OnDPadRightPressed();
            }
            else if (dPadHovering[(int)DPadDirection.Down])
            {
                OnDPadDownPressed();
            }
        }
    }

    void GetTrigger ()
    {
        if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            OnTriggerPressed();
        }
    }

    public virtual void OnDPadUpEnter () { }

    public virtual void OnDPadLeftEnter () { }

    public virtual void OnDPadRightEnter () { }

    public virtual void OnDPadDownEnter () { }

    public virtual void OnDPadExit () { }

    public virtual void OnDPadUpPressed () { }

    public virtual void OnDPadLeftPressed () { }

    public virtual void OnDPadRightPressed () { }

    public virtual void OnDPadDownPressed () { }

    public virtual void OnTriggerPressed () { }
}
