using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Advertisements;
using UnityEngine.UIElements;
using UnityEngine.Analytics;

public class Ads : MonoBehaviour//, IUnityAdsListener
{
    string gameId_Appodeal = "ac748966391bf6fc6007dcadba4b4af197f0634271ecaf30";
    string gameId_Applovin = "TcJO_TENJs-Lns9-V_xhJ_rBg33ylLATi1aZ9RDOXPJ5M7oZTutZlJoqp2VpVp5amW_6PlG38v9ac7V4uovJdf";
    string gameId = "3792327";
    bool testMode = true;
    const string myPlacementId = "rewardedVideo";
    int LastAdTime = 0;

    [SerializeField] ManiacSkin ButcherSkin;
    [SerializeField] ManiacMask PsychopathMask;

    static Ads _instance;
    public static Ads instance { get { return _instance; } }

    private void Awake ()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad (this);
        }
        else {
            Destroy (this);
        }
    }

    void Start ()
    {
        //InitializingAppodeal ();
        /*Advertisement.AddListener (this);
        Advertisement.Initialize (gameId, testMode);*/
        InitApplovin ();
        switch (PlayerPrefs.GetInt ("Privacy", 0)) {
            case 1:
                SwitchOnAnalytics ();
                break;
            case 2:
                SwitchOffAnalytics ();
                break;
        }
    }

    void InitializingAppodeal ()
    {
        /*Appodeal.setTesting (testMode);
        Appodeal.muteVideosIfCallsMuted (true);
        Appodeal.initialize (gameId_Appodeal, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO, PlayerPrefs.GetInt ("Privacy", 0) == 1);*/
    }

    void InitApplovin ()
    {
        MaxSdk.SetSdkKey (gameId_Applovin);
        MaxSdk.InitializeSdk ();

        MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.OnInterstitialHiddenEvent += OnInterstitialDismissedEvent;

        // Load the first interstitial
        MaxSdk.LoadInterstitial ("019e98d7f105b5d0");

        // Attach callback
        MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        // Load the first rewarded ad
        MaxSdk.LoadRewardedAd ("6eb24de8621a9259");
        MaxSdk.LoadRewardedAd ("a69592dd1428c1eb");
        MaxSdk.LoadRewardedAd ("1ee4cc098869550d");
        MaxSdk.LoadRewardedAd ("2ea10c9cd380cdaa");
        MaxSdk.LoadRewardedAd ("62139d0ed05538d7");
    }

    private void OnInterstitialLoadedEvent (string adUnitId)
    {
        // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(adUnitId) will now return 'true'

        // Reset retry attempt
        //retryAttempt = 0;
    }

    private void OnInterstitialFailedEvent (string adUnitId, int errorCode)
    {
        // Interstitial ad failed to load 
        // We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds)

        /*retryAttempt++;
        double retryDelay = Math.Pow (2, Math.Min (6, retryAttempt));

        Invoke ("LoadInterstitial", (float)retryDelay);*/
        MaxSdk.LoadInterstitial ("019e98d7f105b5d0");
    }

    private void InterstitialFailedToDisplayEvent (string adUnitId, int errorCode)
    {
        // Interstitial ad failed to display. We recommend loading the next ad
        //LoadInterstitial ();
        MaxSdk.LoadInterstitial ("019e98d7f105b5d0");
    }

    private void OnInterstitialDismissedEvent (string adUnitId)
    {
        // Interstitial ad is hidden. Pre-load the next ad
        //LoadInterstitial ();
        MaxSdk.LoadInterstitial ("019e98d7f105b5d0");
    }

    private void OnRewardedAdLoadedEvent (string adUnitId)
    {
        // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(adUnitId) will now return 'true'

        // Reset retry attempt
        //retryAttempt = 0;
    }

    private void OnRewardedAdFailedEvent (string adUnitId, int errorCode)
    {
        //DebugText.text += "ERROR: " + errorCode + " FROM: " + adUnitId;
        // Rewarded ad failed to load 
        // We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds)

        //retryAttempt++;
        //double retryDelay = Math.Pow (2, Math.Min (6, retryAttempt));

        //Invoke ("LoadRewardedAd", (float)retryDelay);
        MaxSdk.LoadRewardedAd (adUnitId);
    }

    private void OnRewardedAdFailedToDisplayEvent (string adUnitId, int errorCode)
    {
        // Rewarded ad failed to display. We recommend loading the next ad
        //LoadRewardedAd ();
        MaxSdk.LoadRewardedAd (adUnitId);
    }

    private void OnRewardedAdDisplayedEvent (string adUnitId) { }

    private void OnRewardedAdClickedEvent (string adUnitId) { }

    private void OnRewardedAdDismissedEvent (string adUnitId)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        //LoadRewardedAd ();
        MaxSdk.LoadRewardedAd (adUnitId);
    }

    private void OnRewardedAdReceivedRewardEvent (string adUnitId, MaxSdk.Reward reward)
    {
        // Rewarded ad was displayed and user should receive the reward
        onRewardedVideoFinished (0, adUnitId);
        MaxSdk.LoadRewardedAd (adUnitId);
    }

    public bool ShowAd ()
    {
        
        if (Application.internetReachability != NetworkReachability.NotReachable) {
            if (/*Advertisement.IsReady ()*//*Appodeal.isLoaded (Appodeal.INTERSTITIAL)*/MaxSdk.IsInterstitialReady ("019e98d7f105b5d0")) {
                if ((int)(DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1))).TotalSeconds - LastAdTime > 120) {
                    if (PlayerPrefs.GetInt ("NoAds", 0) == 0) {
                        MaxSdk.ShowInterstitial ("019e98d7f105b5d0");
                    }//Appodeal.show (Appodeal.INTERSTITIAL);//Advertisement.Show ();
                    LastAdTime = (int)(DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1))).TotalSeconds;
                    return true;
                }
                else {
                    Debug.Log ("Less than 30 sec after last ad");
                    return false;
                }
            }
            else {
                Debug.Log ("Interstitial ad not ready at the moment! Please try again later!");
                return false;
            }
        }
        return false;
    }

    public void ShowRewardedVideoHelp ()
    {
        if (PlayerPrefs.GetInt ("NoAds", 0) == 1) {
            Helper.instance.ContinueActiveHelp ();
            return;
        }

        if (Application.internetReachability != NetworkReachability.NotReachable) {
            if (/*Advertisement.IsReady (myPlacementId)*//*Appodeal.isLoaded (Appodeal.REWARDED_VIDEO)*/MaxSdk.IsRewardedAdReady ("6eb24de8621a9259")) {
                /*var options = new ShowOptions { resultCallback = HandleShowResultHelp };
                Advertisement.Show (myPlacementId, options);*/
                //Appodeal.show (Appodeal.REWARDED_VIDEO, "Help");
                MaxSdk.ShowRewardedAd ("6eb24de8621a9259");
            }
            else {
                Debug.Log ("Rewarded video is not ready at the moment! Please try again later!");
            }
        }
        else {
            Subtitles.instance.NewAdvice (Settings.instance.GetTranslatedPhrase ("Нет подключения к интернету"));
        }
    }

    public void ShowRewardedVideoSkip ()
    {
        if (PlayerPrefs.GetInt ("NoAds", 0) == 1) {
            Terminal.instance.ContinueSkipPassword ();
            return;
        }

        if (Application.internetReachability != NetworkReachability.NotReachable) {
            if (/*Advertisement.IsReady (myPlacementId)*//*Appodeal.isLoaded (Appodeal.REWARDED_VIDEO)*/MaxSdk.IsRewardedAdReady ("a69592dd1428c1eb")) {
                /*var options = new ShowOptions { resultCallback = HandleShowResultSkip };
                Advertisement.Show (myPlacementId, options);*/
                //Appodeal.show (Appodeal.REWARDED_VIDEO, "Skip");
                MaxSdk.ShowRewardedAd ("a69592dd1428c1eb");
            }
            else {
                Debug.Log ("Rewarded video is not ready at the moment! Please try again later!");
            }
        }
        else {
            Subtitles.instance.NewAdvice (Settings.instance.GetTranslatedPhrase ("Нет подключения к интернету"));
        }
    }

    /*void HandleShowResultSkip (ShowResult result)
    {
        switch (result) {
            case ShowResult.Finished:
                Terminal.instance.ContinueSkipPassword ();
                break;
            case ShowResult.Skipped:
                Debug.Log ("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError ("The ad failed to be shown.");
                break;
        }
    }*/

    public void ShowRewardedVideoSkipLockBox ()
    {
        if (PlayerPrefs.GetInt ("NoAds", 0) == 1) {
            LockBox.instance.ContinueSkipPassword ();
            return;
        }

        if (Application.internetReachability != NetworkReachability.NotReachable) {
            if (/*Advertisement.IsReady (myPlacementId)*//*Appodeal.isLoaded (Appodeal.REWARDED_VIDEO)*/MaxSdk.IsRewardedAdReady ("1ee4cc098869550d")) {
                /*var options = new ShowOptions { resultCallback = HandleShowResultSkipLockBox };
                Advertisement.Show (myPlacementId, options);*/
                //Appodeal.show (Appodeal.REWARDED_VIDEO, "SkipLockBox");
                MaxSdk.ShowRewardedAd ("1ee4cc098869550d");
            }
            else {
                Debug.Log ("Rewarded video is not ready at the moment! Please try again later!");
            }
        }
        else {
            Subtitles.instance.NewAdvice (Settings.instance.GetTranslatedPhrase ("Нет подключения к интернету"));
        }
    }

    public void ShowRewardedVideoForMaskOrSkin (string Name)
    {
        /*if (PlayerPrefs.GetInt ("NoAds", 0) == 1) {
            LockBox.instance.ContinueSkipPassword ();
            return;
        }*/
        if (Application.internetReachability != NetworkReachability.NotReachable) {
            if (/*Appodeal.isLoaded (Appodeal.REWARDED_VIDEO)*/MaxSdk.IsRewardedAdReady (GetUnitId (Name))) {
                MaxSdk.ShowRewardedAd (GetUnitId (Name));
                //Appodeal.show (Appodeal.REWARDED_VIDEO, Name);
            }
            else {
                Debug.Log ("Rewarded video is not ready at the moment! Please try again later!");
            }
        }
        else {
            Subtitles.instance.NewAdvice (Settings.instance.GetTranslatedPhrase ("Нет подключения к интернету"));
        }
    }

    string GetUnitId (string Name)
    {
        switch (Name) {
            case "ButcherSkin":
                return "2ea10c9cd380cdaa";
                break;
            case "PsychopathMask":
                return "62139d0ed05538d7";
                break;
        }

        return Name;
    }

    /*void HandleShowResultSkipLockBox (ShowResult result)
    {
        switch (result) {
            case ShowResult.Finished:
                LockBox.instance.ContinueSkipPassword ();
                break;
            case ShowResult.Skipped:
                Debug.Log ("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError ("The ad failed to be shown.");
                break;
        }
    }*/

    /*void HandleShowResultHelp (ShowResult result)
    {
        switch (result) {
            case ShowResult.Finished:
                Helper.instance.ContinueActiveHelp ();
                break;
            case ShowResult.Skipped:
                Debug.Log ("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError ("The ad failed to be shown.");
                break;
        }
    }*/

    /*public void OnUnityAdsDidFinish (string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished) {
            switch (placementId) {
                /*case myPlacementId:
                    Helper.instance.ContinueActiveHelp ();
                    break;*/
    /*}
}
else if (showResult == ShowResult.Skipped) {
    // Do not reward the user for skipping the ad.
}
else if (showResult == ShowResult.Failed) {
    Debug.LogWarning ("The ad did not finish due to an error.");
}
}*/

    /*public void OnUnityAdsReady (string placementId)
    {*/
    /*// If the ready Placement is rewarded, show the ad:
    if (placementId == myPlacementId) {
        // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
    }*/
    //}

    public void OnUnityAdsDidError (string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart (string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy ()
    {
        //Advertisement.RemoveListener (this);
    }

    public void SwitchOnAnalytics ()
    {
        Analytics.enabled = true;
        PlayerPrefs.SetInt ("Privacy", 1);
        MaxSdk.SetHasUserConsent (true);
    }

    public void SwitchOffAnalytics ()
    {
        Analytics.enabled = false;
        PlayerPrefs.SetInt ("Privacy", 2);
        MaxSdk.SetHasUserConsent (false);
    }

    public void OpenMoreInfo ()
    {
        string url = "https://www.applovin.com/privacy/";
        Application.OpenURL (url);
    }

    public void onRewardedVideoLoaded (bool precache)
    {
        //throw new NotImplementedException ();
    }

    public void onRewardedVideoFailedToLoad ()
    {
        //throw new NotImplementedException ();
    }

    public void onRewardedVideoShowFailed ()
    {
        //throw new NotImplementedException ();
    }

    public void onRewardedVideoShown ()
    {
        //throw new NotImplementedException ();
    }

    public void onRewardedVideoFinished (double amount, string name)
    {
        switch (name) {
            case "6eb24de8621a9259":
                Helper.instance.ContinueActiveHelp ();
                break;
            case "a69592dd1428c1eb":
                Terminal.instance.ContinueSkipPassword ();
                break;
            case "1ee4cc098869550d":
                LockBox.instance.ContinueSkipPassword ();
                break;
            case "2ea10c9cd380cdaa":
                ButcherSkin.IncAdQuantity ();
                break;
            case "62139d0ed05538d7":
                PsychopathMask.IncAdQuantity ();
                break;
        }
        MaxSdk.LoadRewardedAd (name);
    }

    public void onRewardedVideoClosed (bool finished)
    {
        //throw new NotImplementedException ();
    }

    public void onRewardedVideoExpired ()
    {
        //throw new NotImplementedException ();
    }

    public void onRewardedVideoClicked ()
    {
        //throw new NotImplementedException ();
    }
}
