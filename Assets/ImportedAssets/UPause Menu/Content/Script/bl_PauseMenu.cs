using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class bl_PauseMenu : MonoBehaviour {

    public static PauseState m_PauseState = PauseState.None;
    /// <summary>
    /// Global var for know is pause game
    /// </summary>
    public static bool m_Pause = false;
    [Header("Pause Main")]
    public bool useTimeScale = true;
    public bool LookCursor = true;
    public GameObject PauseUI = null;
    public string m_PauseShowAnim = "PauseMenuShow";
    public string m_PauseHideAnim = "PauseMenuHide";
    public string m_PauseMovedHideAnim = "PauseMenuMovedHide";
    public string m_PauseMoveAnim = "PauseMenuToLeft";
    public string m_PauseMoveReturnAnim = "PauseMenuToCenter";
    [Space(5)]
    [Header("Pause Options")]
    public GameObject OptionsUI = null;
    public string OptionsHideAnim = "OptionsHide";
    [Space(5)]
    [Header("Pause Credits")]
    public GameObject CreditsUI = null;
    public string CreditsHideAnim = "CreditsHide";
    [Space(5)]
    public CanvasGroup Overlay = null;
    [Range(0.0f,1.0f)]
    public float MaxAlpha = 0.75f;
    //private 
    private bool isMoved = false;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        if (PauseUI != null)
        {
            PauseUI.SetActive(false);
        }
        if (OptionsUI != null)
        {
            OptionsUI.SetActive(false);
        }
        if (CreditsUI != null)
        {
            CreditsUI.SetActive(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        if (LookCursor)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DoPause();
        }
        //Fade effect
        if (Overlay != null)
        {
            if (m_Pause && Overlay.alpha < MaxAlpha)
            {
                Overlay.alpha = Mathf.Lerp(Overlay.alpha, MaxAlpha, Time.unscaledDeltaTime * 5);
            }
            else if (Overlay.alpha > 0.0f)
            {
                Overlay.alpha = Mathf.Lerp(Overlay.alpha, 0.0f, Time.unscaledDeltaTime * 5);
            }
        }
        
    }
    /// <summary>
    /// 
    /// </summary>
    public void DoPause()
    {
        if (PauseUI != null)
        {
            //True or False
            m_Pause = !m_Pause;
            if (m_Pause)
            {
                //Active Pause UI with animation
                PauseUI.SetActive(true);
                PauseUI.GetComponent<Animator>().Play(m_PauseShowAnim,0,0);
                m_PauseState = PauseState.Main;
                if (LookCursor)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                if (useTimeScale)
                {
                    Time.timeScale = 0;
                }
            }
            else
            {
                if (useTimeScale)
                {
                    Time.timeScale = 1;
                }
                if (LookCursor)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                //This animation content a event for auto desactive
                //when animation finished
                if (isMoved)
                {
                    PauseUI.GetComponent<Animator>().Play(m_PauseMovedHideAnim, 0, 0);
                }
                else
                {
                    PauseUI.GetComponent<Animator>().Play(m_PauseHideAnim, 0, 0);
                }
                //If options active, then hide too
                if (OptionsUI.activeSelf)
                {
                    OptionsUI.GetComponent<Animator>().SetBool("show", false);
                }
                //If you do not want to disable animation for event
                //use this:
                //StartCoroutine(DesactiveInTime(PauseUI,2f);
                if (CreditsUI.activeSelf)
                {
                    CreditsUI.GetComponent<Animator>().SetBool("show", false);
                }

                m_PauseState = PauseState.None;
            }
        }else{
        
            Debug.LogError("Pause UI is Emty please add this in inspector");
        }
        isMoved = false;
    }
    /// <summary>
    /// 
    /// </summary>
    public void DoMain()
    {
        if (OptionsUI.activeSelf)
        {
            OptionsUI.GetComponent<Animator>().SetBool("show", false);
        }
        if (CreditsUI.activeSelf)
        {
            CreditsUI.GetComponent<Animator>().SetBool("show", false);
        }
        PauseUI.GetComponent<Animator>().Play(m_PauseMoveReturnAnim, 0, 0);
        isMoved = false;
        m_PauseState = PauseState.Main;

    }
    /// <summary>
    /// 
    /// </summary>
    public void DoOptions()
    {
        if (!OptionsUI.activeSelf )
        {
           
            if (CreditsUI.activeSelf)
            {
                CreditsUI.GetComponent<Animator>().SetBool("show", false);
            }
            //This animation have a event to call Options UI show
            if (!isMoved)
            {
                PauseUI.GetComponent<Animator>().Play(m_PauseMoveAnim, 0, 0);
            }
            else
            {
                OptionsUI.SetActive(true);
                OptionsUI.GetComponent<Animator>().SetBool("show", true);
            }
            isMoved = true;
            //If you do not want to disable animation for event
            //use this:
            m_PauseState = PauseState.Options;
        }
        else
        {
            OptionsUI.GetComponent<Animator>().SetBool("show", false);
            PauseUI.GetComponent<Animator>().Play(m_PauseMoveReturnAnim, 0, 0);
            isMoved = false;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void DoCredits()
    {
        if (!CreditsUI.activeSelf )
        {
            
            if (OptionsUI.activeSelf)
            {
                OptionsUI.GetComponent<Animator>().SetBool("show", false);
            }
            //This animation have a event to call Options UI show
            if (!isMoved)
            {
                PauseUI.GetComponent<Animator>().Play(m_PauseMoveAnim, 0, 0);
            }
            else
            {
                CreditsUI.SetActive(true);
                CreditsUI.GetComponent<Animator>().SetBool("show", true);
            }
            isMoved = true;
            m_PauseState = PauseState.Credits;
        }
        else
        {
            CreditsUI.GetComponent<Animator>().SetBool("show", false);
            PauseUI.GetComponent<Animator>().Play(m_PauseMoveReturnAnim, 0, 0);
            isMoved = false;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }
    /// <summary>
    /// Simple Restart Scene
    /// </summary>
    public void SimpleRestart()
    {
        m_Pause = false;
        m_PauseState = PauseState.None;
        Application.LoadLevel(Application.loadedLevelName);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator DesactiveInTime(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        go.SetActive(false);
    }
}