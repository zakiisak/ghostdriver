using System.Collections.Generic;
using UnityEngine;

namespace Assets2.Code
{
    public class PrefabManager2 : MonoBehaviour
    {
        public static PrefabManager2 Instance;

        public GameObject FracturedBlueCar;
        public GameObject FracturedRedCar;

        public GameObject RedCar;
        public GameObject BlueCar;

        public GameObject Explosion;


        public void Awake()
        {
            Instance = this;
        }


    }
}

