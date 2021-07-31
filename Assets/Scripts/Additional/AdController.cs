using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdController : MonoBehaviour, IUnityAdsListener
{
    public static AdController AdInstance;

    private string AppleStore_ID = "3749344";
    private string GoogleStore_ID = "3749345";

    private string video_ad = "video";
    private string rewarded_video_ad = "rewardedVideo";
    private string banner_ad = "banner";

    private bool TestMode = false;

    private void Awake()
    {
        if (AdInstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            AdInstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {

        if (Application.platform != RuntimePlatform.IPhonePlayer &&
            Application.platform != RuntimePlatform.OSXPlayer)
        {
            Advertisement.Initialize(GoogleStore_ID, TestMode); //Turn to false when not testing
        }
        else
        {
            Advertisement.Initialize(AppleStore_ID, TestMode); //Turn to false when not testing
        }

        Advertisement.AddListener(this);
    }

    public void ShowAd(string p)
    {
        if (Advertisement.IsReady(p)) Advertisement.Show(p);
    }

    public void ShowBannerAd(string p)
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Show(p);
    }

    public void OnUnityAdsReady(string placementId)
    {
        
    }

    public void OnUnityAdsDidError(string message)
    {
        
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
