using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button soundButton;
    [SerializeField]
    private Button menuButton;
    [SerializeField]
    private Button exitButton;
    [SerializeField]
    private Button gameEndExitButton;

    
    // Start is called before the first frame update
    public void Init(System.Action onStartAction, System.Action onExitAction, System.Action onMenuClickAction,System.Action onSettingsClick)
    {
        if (startButton != null)
            startButton.onClick.AddListener(onStartAction.Invoke);
        if(exitButton != null)
            exitButton.onClick.AddListener(onExitAction.Invoke);
        if (gameEndExitButton != null)
            gameEndExitButton.onClick.AddListener(onExitAction.Invoke);
        if (menuButton != null)
            menuButton.onClick.AddListener(onMenuClickAction.Invoke);
        if(soundButton != null)
            soundButton.onClick.AddListener(onSettingsClick.Invoke);
        

    }

    public void ToggleButtons(bool show)
    {
        startButton.gameObject.SetActive(show);
        soundButton.gameObject.SetActive(show);
        exitButton.gameObject.SetActive(show);
    }

    public void ToggleGameEndButtons(bool show)
    {
        menuButton.gameObject.SetActive(show);
        gameEndExitButton.gameObject.SetActive(show);
    }

    public void ToggleMenuButton(bool show)
    {
        menuButton.gameObject.SetActive(show);
    }

    public void SetMenuButtonAction(System.Action action)
    {
        menuButton.onClick.RemoveAllListeners();
        menuButton.onClick.AddListener(action.Invoke);
    }
}
