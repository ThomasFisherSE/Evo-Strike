using UnityEngine;
using UnityEngine.UI;

public class UPM_Test : MonoBehaviour {

    public Text Info = null;
	
	// Update is called once per frame
	void Update () {
        if (bl_PauseMenu.m_Pause)
        {
            Info.gameObject.SetActive(false);
        }
        else
        {
            Info.gameObject.SetActive(true);
        }
	}
}
