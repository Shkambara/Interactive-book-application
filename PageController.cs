using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageController : MonoBehaviour
{
    private System.Action leftKeyAction;
    private System.Action rightKeyAction;
    private bool onPageControll = false;

    [SerializeField]
    private Button leftButton;
    [SerializeField]
    private Button rightButton;

    public void Init(System.Action onLeftKeyPressAction, System.Action onRightKeyPressAction)
    {
        leftKeyAction = onLeftKeyPressAction;
        rightKeyAction = onRightKeyPressAction;
        leftButton.onClick.AddListener(leftKeyAction.Invoke);
        rightButton.onClick.AddListener(rightKeyAction.Invoke);
    }

    public void OnUpdate()
    {

        if (Input.GetMouseButtonDown(0) && onPageControll && rightKeyAction != null)
            rightKeyAction.Invoke();
    }

    public void OnPageClickTransfer(bool trigger)
    {
        onPageControll = trigger;
    }

    public void ToggleLeftKey(bool show) { leftButton.gameObject.SetActive(show); }
    public void ToggleRightKey(bool show) { rightButton.gameObject.SetActive(show); }
}
