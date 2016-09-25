using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestViveController : ViveController
{
    public Button upButton;
    public Button leftButton;
    public Button rightButton;
    public Button downButton;

    public override void OnDPadUpEnter()
    {
        EventSystem.current.SetSelectedGameObject(upButton.gameObject);
    }

    public override void OnDPadLeftEnter()
    {
        EventSystem.current.SetSelectedGameObject(leftButton.gameObject);
    }

    public override void OnDPadRightEnter()
    {
        EventSystem.current.SetSelectedGameObject(rightButton.gameObject);
    }

    public override void OnDPadDownEnter()
    {
        EventSystem.current.SetSelectedGameObject(downButton.gameObject);
    }

    public override void OnDPadExit()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public override void OnDPadUpPressed()
    {
        SetCameraBackground( upButton.colors.normalColor );
    }

    public override void OnDPadLeftPressed()
    {
        SetCameraBackground( leftButton.colors.normalColor );
    }

    public override void OnDPadRightPressed()
    {
        SetCameraBackground( rightButton.colors.normalColor );
    }

    public override void OnDPadDownPressed()
    {
        SetCameraBackground( downButton.colors.normalColor );
    }

    void SetCameraBackground(Color color)
    {
        GameObject.Find("Camera (eye)").GetComponent<Camera>().backgroundColor = color;
    }
}
