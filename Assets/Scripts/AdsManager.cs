using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static UnityAction successCallback;
    bool isSuccessCallback, isPlayFail = false;
    public static bool isCanPlayAd = false;

    [HideInInspector] public float aimInterstitialAdTime = 30;
    [HideInInspector] public float InterstitialAdTime = 0;


    [SerializeField] string _androidGameId = "5338635";
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;
    //插屏
    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOSAdUnitId = "Interstitial_iOS";
    //激励
    [SerializeField] string _androidAdUnitId_Rewarded = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId_Rewarded = "Rewarded_iOS";

    //Banner
    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;
    [SerializeField] string _androidAdUnitId_Banner = "Banner_Android";
    [SerializeField] string _iOSAdUnitId_Banner = "Banner_iOS";



    string _adUnitId;
    string _adUnitId_Rewarded;
    string _adUnitId_Banner;


    void Awake()
    {
        G.dc.Load();
        DontDestroyOnLoad(this.gameObject);
        // 获取当前平台的 Ad Unit ID（广告单元 ID）：
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
           ? _iOSAdUnitId
           : _androidAdUnitId;

        _adUnitId_Rewarded = (Application.platform == RuntimePlatform.IPhonePlayer)
           ? _iOSAdUnitId_Rewarded
           : _androidAdUnitId_Rewarded;

        _adUnitId_Banner = (Application.platform == RuntimePlatform.IPhonePlayer)
           ? _iOSAdUnitId_Banner
           : _androidAdUnitId_Banner;
    }
    void Start()
    {
        aimInterstitialAdTime = G.dc.gd.adsConfig.nextLevelSafeDuration;
        //InterstitialLevel = G.dc.gd.adsConfig.interstitialLevel;
        // 设置横幅广告位置：
        Advertisement.Banner.SetPosition(_bannerPosition);
    }

    private void Update()
    {
        if (isSuccessCallback)
        {
            isSuccessCallback = false;
            if (null != successCallback)
            {
                successCallback.Invoke();
                successCallback -= successCallback;
            }
        }
        if (InterstitialAdTime <= aimInterstitialAdTime)
        {
            InterstitialAdTime += Time.deltaTime;
        }
        if (isPlayFail)
        {
            isPlayFail = false;
        }
    }
    public void InitializeAds()
    {
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
#elif UNITY_EDITOR
        _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
        LoadAd();
        LoadAd_Rewarded();
        LoadBanner();
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    //==============================================================================
    //插屏广告和激励

    // 将内容加载到广告单元中：
    public void LoadAd()
    {
        //加载插屏
        // 重要！仅在初始化之后再加载内容（在此示例中，初始化在另一个脚本中处理）。
        Debug.Log("Loading_Interstitial_Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);

    }

    public void LoadAd_Rewarded()
    {

        //加载激励
        // 重要！仅在初始化之后再加载内容（在此示例中，初始化在另一个脚本中处理）。
        Debug.Log("Loading_Rewarded_Ad: " + _adUnitId_Rewarded);
        Advertisement.Load(_adUnitId_Rewarded, this);

    }


    // 展示广告单元中加载的内容：
    public void ShowAd()
    {
        //显示插屏
        // 请注意，如果未事先加载广告内容，此方法将失败
        Debug.Log("Showing Ad: " + _adUnitId);
        Advertisement.Show(_adUnitId, this);
    }
    public bool ShowInterstitialAd(bool isReckon = true)
    {
        if (isReckon)
        {
            if (InterstitialAdTime >= aimInterstitialAdTime)
            {
                this.ShowAd();
                InterstitialAdTime = 0;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            this.ShowAd();
            return true;
        }
        
    }

    public void ShowAd_Rewarded()
    {
        //显示激励
        // 请注意，如果未事先加载广告内容，此方法将失败
        Debug.Log("Showing Ad: " + _adUnitId_Rewarded);
        Advertisement.Show(_adUnitId_Rewarded, this);
    }

    public bool ShowSpecialHeartReward(bool isReckon = true)
    {
        if (isReckon)
        {
            this.ShowAd_Rewarded();
            return true;
        }
        else
        {
            isPlayFail = true;
            return false;
        }
    }

    // 实现 Load Listener 和 Show Listener 接口方法： 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        //（可选）如果广告单元成功加载内容，执行代码。
    }

    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
        //（可选）如果广告单元加载失败，执行代码（例如再次尝试）。
    }

    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        //（可选）如果广告单元展示失败，执行代码（例如加载另一个广告）。
    }

    public void OnUnityAdsShowStart(string _adUnitId) { }
    public void OnUnityAdsShowClick(string _adUnitId) { }
    public void OnUnityAdsShowComplete(string _adUnitI, UnityAdsShowCompletionState showCompletionState)
    {


        //插屏/激励显示完成
        if (_adUnitI.Equals(_adUnitI) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // 给予奖励。
            if (_adUnitI == _adUnitId)
            {
                isSuccessCallback = true;
                LoadAd();
               
            }
            if (_adUnitI == _adUnitId_Rewarded)
            {
                isSuccessCallback = true;
                LoadAd_Rewarded();
               
            }

        }
    }

    //==============================================================================
    //Banner广告
    // 实现一个在单击 Load Banner（加载横幅广告）按钮时调用的方法：
    public void LoadBanner()
    {
        // 设置选项以将加载事件告知 SDK：
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // 向广告单元加载横幅广告内容：
        Advertisement.Banner.Load(_adUnitId_Banner, options);
    }

    // 实现在 loadCallback 事件触发时执行的代码：
    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");

        // 配置 Show Banner（展示横幅广告）按钮在单击该按钮时调用 ShowBannerAd() 方法：
        //_showBannerButton.onClick.AddListener(ShowBannerAd);
        // 配置 Hide Banner（隐藏横幅广告）按钮在单击该按钮时调用 HideBannerAd() 方法：
        //_hideBannerButton.onClick.AddListener(HideBannerAd);

        // 启用这两个按钮：
        //_showBannerButton.interactable = true;
        //_hideBannerButton.interactable = true;
    }

    // 实现在 load errorCallback 事件触发时执行的代码：
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        //（可选）执行其他代码，例如尝试加载另一个广告。
    }

    // 实现一个在单击 Show Banner（展示横幅广告）按钮时调用的方法：
    public void ShowBannerAd()
    {
        // 设置选项以将显示事件告知 SDK：
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // 展示加载的横幅广告单元：
        Advertisement.Banner.Show(_adUnitId_Banner, options);
    }

    // 实现一个在单击 Hide Banner（隐藏横幅广告）按钮时调用的方法：
    void HideBannerAd()
    {
        // 隐藏横幅广告：
        Advertisement.Banner.Hide();
    }

    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }


}

