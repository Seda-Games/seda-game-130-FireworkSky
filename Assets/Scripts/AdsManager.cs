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
    //����
    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOSAdUnitId = "Interstitial_iOS";
    //����
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
        // ��ȡ��ǰƽ̨�� Ad Unit ID����浥Ԫ ID����
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
        // ���ú�����λ�ã�
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
    //�������ͼ���

    // �����ݼ��ص���浥Ԫ�У�
    public void LoadAd()
    {
        //���ز���
        // ��Ҫ�����ڳ�ʼ��֮���ټ������ݣ��ڴ�ʾ���У���ʼ������һ���ű��д�����
        Debug.Log("Loading_Interstitial_Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);

    }

    public void LoadAd_Rewarded()
    {

        //���ؼ���
        // ��Ҫ�����ڳ�ʼ��֮���ټ������ݣ��ڴ�ʾ���У���ʼ������һ���ű��д�����
        Debug.Log("Loading_Rewarded_Ad: " + _adUnitId_Rewarded);
        Advertisement.Load(_adUnitId_Rewarded, this);

    }


    // չʾ��浥Ԫ�м��ص����ݣ�
    public void ShowAd()
    {
        //��ʾ����
        // ��ע�⣬���δ���ȼ��ع�����ݣ��˷�����ʧ��
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
        //��ʾ����
        // ��ע�⣬���δ���ȼ��ع�����ݣ��˷�����ʧ��
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

    // ʵ�� Load Listener �� Show Listener �ӿڷ����� 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        //����ѡ�������浥Ԫ�ɹ��������ݣ�ִ�д��롣
    }

    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
        //����ѡ�������浥Ԫ����ʧ�ܣ�ִ�д��루�����ٴγ��ԣ���
    }

    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        //����ѡ�������浥Ԫչʾʧ�ܣ�ִ�д��루���������һ����棩��
    }

    public void OnUnityAdsShowStart(string _adUnitId) { }
    public void OnUnityAdsShowClick(string _adUnitId) { }
    public void OnUnityAdsShowComplete(string _adUnitI, UnityAdsShowCompletionState showCompletionState)
    {


        //����/������ʾ���
        if (_adUnitI.Equals(_adUnitI) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // ���轱����
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
    //Banner���
    // ʵ��һ���ڵ��� Load Banner�����غ����棩��ťʱ���õķ�����
    public void LoadBanner()
    {
        // ����ѡ���Խ������¼���֪ SDK��
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // ���浥Ԫ���غ��������ݣ�
        Advertisement.Banner.Load(_adUnitId_Banner, options);
    }

    // ʵ���� loadCallback �¼�����ʱִ�еĴ��룺
    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");

        // ���� Show Banner��չʾ�����棩��ť�ڵ����ð�ťʱ���� ShowBannerAd() ������
        //_showBannerButton.onClick.AddListener(ShowBannerAd);
        // ���� Hide Banner�����غ����棩��ť�ڵ����ð�ťʱ���� HideBannerAd() ������
        //_hideBannerButton.onClick.AddListener(HideBannerAd);

        // ������������ť��
        //_showBannerButton.interactable = true;
        //_hideBannerButton.interactable = true;
    }

    // ʵ���� load errorCallback �¼�����ʱִ�еĴ��룺
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        //����ѡ��ִ���������룬���糢�Լ�����һ����档
    }

    // ʵ��һ���ڵ��� Show Banner��չʾ�����棩��ťʱ���õķ�����
    public void ShowBannerAd()
    {
        // ����ѡ���Խ���ʾ�¼���֪ SDK��
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // չʾ���صĺ����浥Ԫ��
        Advertisement.Banner.Show(_adUnitId_Banner, options);
    }

    // ʵ��һ���ڵ��� Hide Banner�����غ����棩��ťʱ���õķ�����
    void HideBannerAd()
    {
        // ���غ����棺
        Advertisement.Banner.Hide();
    }

    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }


}

