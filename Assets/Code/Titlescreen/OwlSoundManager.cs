using UnityEngine;

namespace Assets.Code.Titlescreen
{
    public class OwlSoundManager : MonoBehaviour
    {
        public AudioSource owlSource;

        private float nextTimeToPlayOwlSound;

        public void Awake()
        {
            nextTimeToPlayOwlSound = Time.time + Random.Range(1.0f, 10.0f);
        }

        public void Update()
        {
            if(nextTimeToPlayOwlSound <= Time.time)
            {
                owlSource.Play();
                nextTimeToPlayOwlSound = Time.time + 20.0f + Random.Range(0, 20.0f);
            }
        }

    }
}
