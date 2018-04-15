using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class bl_PauseOptions : MonoBehaviour {

    public Transform ResolutionPanel = null;
    public GameObject ResolutionButtons = null;
    [Space(5)]
    public bool ShowFramesPerSecond = true;
    public Text FPSFrames = null;
    public float UpdateInterval = 0.5F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval
    /// <summary>
    /// Get this sensitivity from your mouse loook
    /// </summary>
    public static float Sensitivity = 5.0f;
    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        PostResolutions();
        if (FPSFrames != null) { FPSFrames.gameObject.SetActive(ShowFramesPerSecond); }
        timeleft = UpdateInterval;

        //Developer Util info
        /*Debug.Log(SystemInfo.deviceModel);
        Debug.Log(SystemInfo.deviceName);
        Debug.Log(SystemInfo.deviceType);
        Debug.Log(SystemInfo.deviceUniqueIdentifier);
        Debug.Log(SystemInfo.graphicsDeviceID);
        Debug.Log(SystemInfo.graphicsDeviceName);
        Debug.Log(SystemInfo.graphicsDeviceVendor);
        Debug.Log(SystemInfo.operatingSystem);
        Debug.Log(SystemInfo.processorCount);
        Debug.Log(SystemInfo.processorType);
        Debug.Log(SystemInfo.systemMemorySize);
        Debug.Log(SystemInfo.supportsShadows);*/
    }

    /// <summary>
    /// 
    /// </summary>
    void PostResolutions()
    {
        if (ResolutionPanel == null)
            return;
        if (ResolutionButtons == null)
            return;

        int n = -1;
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            GameObject b = Instantiate(ResolutionButtons) as GameObject;
            b.GetComponentInChildren<Text>().text = Screen.resolutions[i].width + " x " + Screen.resolutions[i].height;
            b.transform.SetParent(ResolutionPanel,false);
            b.GetComponent<Button>().onClick.AddListener(() => { ChangeResolution(n); });
            n++;
        }


    }
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (ShowFramesPerSecond && FPSFrames != null)
        {
            FramesPerSecond();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    void FramesPerSecond()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;
        
        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            float fps = accum / frames;
            string format = System.String.Format("{0:F2} FPS", fps);
            FPSFrames.text = format;

            if (fps < 30)
            {
                FPSFrames.color = Color.yellow;
            }
            else
            {
                if (fps < 10)
                {
                    FPSFrames.color = Color.red;
                }
                else
                {
                    FPSFrames.color = Color.green;
                }
            }
            timeleft = UpdateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }
    /// <summary>
    /// Change Show / Hide Frames Per Second UI.
    /// </summary>
    /// <param name="b"></param>
    public void ChangeFPSFrames(bool b)
    {
        ShowFramesPerSecond = b;
        FPSFrames.gameObject.SetActive(b);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="r"></param>
    public void ChangeResolution(int r)
    {
        Screen.SetResolution(Screen.resolutions[r].width, Screen.resolutions[r].height, true);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="q"></param>
    public void ChangeQuality(int q)
    {
        QualitySettings.SetQualityLevel(q,true);
    }

    /// <summary>
    /// Update Volumen of game
    /// </summary>
    /// <param name="v"></param>
    public void UpdateVolumen(float v)
    {
        //Apply volumen
         AudioListener.volume = v;
    }

    /// <summary>
    /// Update Sensitivity of game
    /// </summary>
    /// <param name="v"></param>
    public void UpdateSensitivity(float s)
    {
        Sensitivity = s;
    }

    public void AntiAliasing(int a)
    {
        QualitySettings.antiAliasing = a;
    }

    /// <summary>
    /// Update Texture Quality
    /// </summary>
    /// <param name="tq"></param>
    public void TextureQuality(int tq)
    {
        QualitySettings.masterTextureLimit = tq;
    }

    /// <summary>
    /// Update Anisotropic Texture
    /// </summary>
    /// <param name="a"></param>
    public void UpdateAnisotropic(int a)
    {
        switch (a)
        {
            case 0:
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            break;
            case 1:
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
            break;
            case 2:
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
            break;
        }
    }
    /// <summary>
    /// Update VSync Option
    /// </summary>
    /// <param name="vs"></param>
    public void VSync(int vs)
    {
        QualitySettings.vSyncCount = vs;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    public void ShadowsCascades(int s)
    {
        QualitySettings.shadowCascades = s;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bw"></param>
    public void BlendWight(int bw)
    {
        switch (bw)
        {
            case 0 :
                QualitySettings.blendWeights = BlendWeights.OneBone;
                break;
            case 1:
                QualitySettings.blendWeights = BlendWeights.TwoBones;
                break;
            case 2:
                QualitySettings.blendWeights = BlendWeights.FourBones;
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="b"></param>
    public void SoftVegetation(bool b)
    {
        QualitySettings.softVegetation = b;
    }
}
