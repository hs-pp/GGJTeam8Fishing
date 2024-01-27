using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [SerializeField] float fadeSpeed = 1f;

    //General Idea
    //Should be a UI on screen at all times
    //Whether its the newspaper screen, the main UI, pause, or whatever
    //Use StartCoroutine(Transtion(from, to)) to swap between them


    [Header("Newspaper Screen")]
    [SerializeField] CanvasGroup newspaperUI;
    [SerializeField] Animator newspaperAnim;

    [Header("Main UI")]
    [SerializeField] CanvasGroup mainUI;


    [Header("Pause UI")]
    [SerializeField] CanvasGroup pauseUI;




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
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            newspaperUI.gameObject.SetActive(true);
            newspaperAnim.Play("NewspaperSpin", -1, 0);
            StartCoroutine(Transition(mainUI, newspaperUI));
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
    

}
