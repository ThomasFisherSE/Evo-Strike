using UnityEngine;
using System.Collections;

public class bl_AnimationEventHelper : MonoBehaviour{

    public GameObject OptionsUI = null;
    public GameObject CreditsUI = null;
    /// <summary>
    ///     Call this when animation or tweent is finished
    /// </summary>
    public void Desactive()
    {
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// 
    /// </summary>
    public void Active()
    {
        if (bl_PauseMenu.m_PauseState == PauseState.Options)
        {
            if (OptionsUI != null)
            {
                OptionsUI.SetActive(true);
            }
            else
            {
                Debug.LogError("Options UI is Emty");
            }
        }
        else if (bl_PauseMenu.m_PauseState == PauseState.Credits)
        {
            if (CreditsUI != null)
            {
                CreditsUI.SetActive(true);
            }
            else
            {
                Debug.LogError("Credits UI is Emty");
            }
        }
    }
}
