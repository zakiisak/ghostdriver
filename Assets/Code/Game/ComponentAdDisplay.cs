using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

namespace Assets.Code.Game
{
    public class ComponentAdDisplay : MonoBehaviour
    {
        private BannerView bannerView;

        public void Start()
        {
            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(initStatus => 
            {
            
            
            });

            this.RequestBanner();

            SceneManager.sceneLoaded += OnSceneChanged;

            Object.DontDestroyOnLoad(gameObject);
        }

        private void OnSceneChanged(Scene arg0, LoadSceneMode arg1)
        {
            DisplayNewAd();
        }

        private void RequestBanner()
        {
#if UNITY_ANDROID
            string adUnitId = "ca-app-pub-2657917745077989/5695597112";
#elif UNITY_IPHONE
                string adUnitId = "ca-app-pub-2657917745077989/8206457804";
#else
                string adUnitId = null;
#endif


            if (string.IsNullOrEmpty(adUnitId) == false)
            {
                // Create a 320x50 banner at the top of the screen.
                this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
                DisplayNewAd();
            }
        }

        private void OnDestroy()
        {
            if(this.bannerView != null)
                bannerView.Destroy();
        }

        private void DisplayNewAd()
        {
            if(this.bannerView != null)
            {
                // Create an empty ad request.
                AdRequest request = new AdRequest.Builder().Build();

                // Load the banner with the request.
                this.bannerView.LoadAd(request);
            }
        }

    }
}
