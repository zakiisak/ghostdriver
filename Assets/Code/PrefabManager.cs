using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class PrefabManager : MonoBehaviour
    {
        public static PrefabManager Instance;

        public GameObject FracturedBlueCar;
        public GameObject FracturedRedCar;

        public GameObject RedCar;
        public GameObject BlueCar;
        public GameObject Tree;
        public GameObject Road;
        public GameObject Wall;
        public GameObject Ramp;

        public GameObject Explosion;


        public List<AudioClip> Sound_Engine;
        public List<AudioClip> Sound_Start;
        public List<AudioClip> Sound_Build;
        public List<AudioClip> Sound_Crash;
        public List<AudioClip> Sound_Horn;
        public List<AudioClip> Sound_Tire;


        public void Awake()
        {
            Instance = this;
        }


    }
}

