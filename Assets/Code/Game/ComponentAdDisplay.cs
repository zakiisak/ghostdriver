using GoogleMobileAds.Api;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

namespace Assets.Code.Game
{
    public class ComponentAdDisplay : MonoBehaviour, IUnityAdsInitializationListener
    {
        private BannerView bannerView;
        private string unityIosGameId = "5107372";
        private string unityIosPlacementId = "ios_banner";

        public void Start()
        {
#if UNITY_ANDROID
            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(initStatus =>
            {
            });

            this.RequestBannerAndroid();


#elif UNITY_IPHONE
            Debug.Log("Initializing ads for ios");
            Advertisement.Initialize(unityIosGameId, true, this);   
#endif
            SceneManager.sceneLoaded += OnSceneChanged;
            Object.DontDestroyOnLoad(gameObject);
        }

        private void OnSceneChanged(Scene arg0, LoadSceneMode arg1)
        {
            DisplayNewAd();
        }

        private void RequestBannerAndroid()
        {
            string adUnitId = "ca-app-pub-2657917745077989/5695597112";

            if (string.IsNullOrEmpty(adUnitId) == false)
            {
                // Create a 320x50 banner at the top of the screen.
                this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
                DisplayNewAd();
            }
        }

        private void OnDestroy()
        {
            if (this.bannerView != null)
                bannerView.Destroy();
        }

        private void DisplayNewAd()
        {
#if UNITY_ANDROID

            if (this.bannerView != null)
            {
                // Create an empty ad request.
                AdRequest request = new AdRequest.Builder().Build();

                // Load the banner with the request.
                this.bannerView.LoadAd(request);
            }
#elif UNITY_IPHONE
            Advertisement.Banner.Load(unityIosPlacementId, new BannerLoadOptions()
            {
                loadCallback = () =>
                {
                    Advertisement.Banner.Show(unityIosPlacementId);
                }
            });
#endif


        }
        public void OnInitializationComplete()
        {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            DisplayNewAd();   
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
        }
    }
}
