using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }


    bool paused = false;
    [SerializeField] float fadeSpeed = 1f;

    //General Idea
    //Probably a UI on screen at all times
    //Whether its the newspaper screen, the main UI, pause, or whatever
    //Use StartCoroutine(Transtion(from, to)) to swap between them


    [Header("Newspaper Screen")]
    [SerializeField] CanvasGroup newspaperUI;
    [SerializeField] Animator newspaperAnim;

    [Header("Main UI")]
    [SerializeField] CanvasGroup mainUI;


    [Header("Pause UI")]
    [SerializeField] CanvasGroup pauseUI;



    CanvasGroup[] uis;

    [SerializeField] TextMeshProUGUI baitAmountText;



    private static WaitForEndOfFrame _EndOfFrame;
    private static WaitForFixedUpdate _FixedUpdate;
    public static WaitForFixedUpdate FixedUpdate
    {
        get { return _FixedUpdate ?? (_FixedUpdate = new WaitForFixedUpdate()); }
    }
    public static WaitForEndOfFrame EndOfFrame
    {
        get { return _EndOfFrame ?? (_EndOfFrame = new WaitForEndOfFrame()); }
    }




    // Start is called before the first frame update
    void Start()
    {
        uis = new CanvasGroup[] { newspaperUI, mainUI, pauseUI };
        foreach(CanvasGroup c in uis)
        {
            c.alpha = 0;
            c.gameObject.SetActive(false);
        }
        newspaperUI.gameObject.SetActive(true);
        newspaperUI.alpha = 1f;
        newspaperAnim.Play("NewspaperSpin", -1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        baitAmountText.text = "Bait: " + GameStateManager.GetBaitAmount().ToString();

        /*if(Input.GetKeyDown(KeyCode.Space))
        {
            newspaperUI.gameObject.SetActive(true);
            newspaperAnim.Play("NewspaperSpin", -1, 0);
            StartCoroutine(Transition(mainUI, newspaperUI));
        }*/

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseOrUnPause();
        }
    }

    private void PauseOrUnPause()
    {
        if (paused)
        {
            pauseUI.gameObject.SetActive(false);
            pauseUI.alpha = 0f;
            mainUI.gameObject.SetActive(true);
            mainUI.alpha = 1f;
            paused = false;
        }
        else if (mainUI.alpha == 1 && mainUI.gameObject.activeSelf)
        {
            pauseUI.gameObject.SetActive(true);
            pauseUI.alpha = 1f;
            mainUI.gameObject.SetActive(false);
            mainUI.alpha = 0f;
            paused = true;
        }
    }

    private IEnumerator Transition(CanvasGroup decrease, CanvasGroup increase)
    {

        increase.gameObject.SetActive(true);

        increase.alpha = 0f;
        while (increase.alpha < 1f)
        {
            increase.alpha += fadeSpeed * Time.unscaledDeltaTime;
            decrease.alpha -= fadeSpeed * Time.unscaledDeltaTime;
            yield return EndOfFrame;
        }
        increase.alpha = 1f;
        decrease.alpha = 0f;
        decrease.gameObject.SetActive(false);
    }



    public void SwitchtoMainUI()
    {
        StartCoroutine(Transition(newspaperUI, mainUI));
    }
    

    public void ResumeClick()
    {
        PauseOrUnPause();
    }


}
