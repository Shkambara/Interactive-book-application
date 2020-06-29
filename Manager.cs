using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private MainMenu menu;
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private PageController pageController;
    [SerializeField]
    private Image gameFadeImage;
    [SerializeField]
    private float transitionDelay = 1f;
    [SerializeField]
    private Page settingsPage;
    [SerializeField]
    private List<Page> pages;
    
    private bool pageSwitchAvailable
    {
        get {
            return currentPageIndex != 0 && currentPageIndex != pages.Count - 1 && pageController;
        }
    }

    private GameObject fadeImageParent;
    private Animator gameFadeAnimator;

    private Page currentPage;
    private int currentPageIndex;
    void Start()
    {

        if (!IsReady())
            return;

        gameFadeAnimator = gameFadeImage.GetComponent<Animator>();
        fadeImageParent = gameFadeImage.transform.parent.gameObject;

        currentPageIndex = 0;
        currentPage = pages[0];

        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].Init();
        }

        menu.Init(OnStartAction,OnExitAction,delegate { ChangePageToMenu(); }, OnSettingsPageClick);
        audioManager.Init();
        pageController.Init(() =>PreviousPage(), () => NextPage());
        LoadMenu();

    }

    bool IsReady()
    {
        if (pages.Count == 0)
        {
            Debug.LogError("You have not initialized pages, please add pages to the list");
            return false;
        }else if(menu == null)
        {
            Debug.LogError("You have not initialized the game menu, please add menu to the manager");
            return false;
        }

        return true;

    }

    public void OnSettingsPageClick()
    {
        StartCoroutine(ChangePage(settingsPage, transitionDelay, OnSettingsEnterEvent));
    }

    public void OnExitAction()
    {
        Application.Quit();
    }

    public void OnSettingsEnterEvent()
    {
        OnMenuExit();
        menu.ToggleMenuButton (true);
        menu.SetMenuButtonAction(delegate { ChangePageToMenu(OnSettingsExitEvent); });
        audioManager.ToggleSettingsSlider(true);
    }

    public void OnSettingsExitEvent()
    {
        OnMenuEnter();
        menu.ToggleMenuButton(false);
        menu.SetMenuButtonAction(delegate { ChangePageToMenu(); });
        audioManager.ToggleSettingsSlider(false);
    }

    public void OnStartAction()
    {
        NextPage(OnMenuExit);
    }

    void NextPage(System.Action onTransitionAction = null)
    {
        currentPageIndex++;
        StartCoroutine(ChangePage(pages[currentPageIndex], transitionDelay, onTransitionAction));
    }
    void PreviousPage(System.Action onTransitionAction = null)
    {
        currentPageIndex--;
        StartCoroutine(ChangePage(pages[currentPageIndex], transitionDelay,()=>menu.ToggleGameEndButtons(false)));
    }

    IEnumerator ChangePage(Page page, float delay,System.Action onTransitionAction = null)
    {
        gameFadeImage.transform.SetSiblingIndex(fadeImageParent.transform.childCount - 1); 

        FadeIn();
        audioManager.PlaySound(SoundMark.pageTurn);

        yield return new WaitForSeconds(delay / 2);

        currentPage.Stop();
        currentPage = page;
        ManagePageArrows();

        if (currentPageIndex == 1)
            pageController.OnPageClickTransfer(true);
        else
            pageController.OnPageClickTransfer(false);

        if (onTransitionAction != null)
            onTransitionAction.Invoke();

        FadeOut();
        PlayPage(currentPage);

        yield return new WaitForSeconds(delay / 2);

        gameFadeImage.transform.SetSiblingIndex(0);
       
    }


    void ManagePageArrows()
    {
        if (currentPageIndex > 1)
            pageController.ToggleLeftKey(true);
        else
            pageController.ToggleLeftKey(false);

        if (currentPageIndex < pages.Count - 1 && currentPageIndex >= 2)
            pageController.ToggleRightKey(true);
        else
            pageController.ToggleRightKey(false);
    }
    void FadeIn()
    {
        if (gameFadeAnimator != null)
            gameFadeAnimator.SetTrigger("FadeIn");
    }

    void FadeOut()
    {
        if (gameFadeAnimator != null)
            gameFadeAnimator.SetTrigger("FadeOut");
    }

    void Update()
    {
        if (!IsReady() || !pageSwitchAvailable)
            return;

        pageController.OnUpdate();
        
    }

    void LoadMenu()
    {
        currentPage = pages[0];
        currentPageIndex = 0;

        currentPage.Play();

        menu.ToggleButtons(true);
        menu.ToggleGameEndButtons(false);

        pageController.ToggleLeftKey(false);
        pageController.ToggleRightKey(false);
    }

    void ChangePageToMenu(System.Action action = null)
    {
        currentPageIndex = 0;
        if (action == null)
            action = OnMenuEnter;

        StartCoroutine(ChangePage(pages[0], transitionDelay, action));
    }

    void OnMenuExit()
    {
        menu.ToggleButtons(false);
    }

    void OnMenuEnter()
    {
        menu.ToggleButtons(true);
        menu.ToggleGameEndButtons(false);
    }

    void PlayPage(Page page)
    {
        page.Play();
        if (currentPageIndex == pages.Count - 1 && currentPageIndex != 0)
            menu.ToggleGameEndButtons(true);
    }
}
