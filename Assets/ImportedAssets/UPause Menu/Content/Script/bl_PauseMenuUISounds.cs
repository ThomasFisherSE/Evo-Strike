using UnityEngine;
using UnityEngine.EventSystems;

public class bl_PauseMenuUISounds : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private AudioClip Sound;

    public void OnPointerEnter(PointerEventData e)
    {
        Play();
    }

    public void Play()
    {
        bool ispause = (Time.timeScale == 0);
        Time.timeScale = 1;
        if (Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(Sound, Camera.main.transform.position);
        }
        else if (Camera.current != null)
        {
            AudioSource.PlayClipAtPoint(Sound, Camera.current.transform.position);
        }
        else
        {
            AudioSource.PlayClipAtPoint(Sound, transform.position);
        }
        if (ispause)
        {
            Time.timeScale = 0;
        }
    }
}