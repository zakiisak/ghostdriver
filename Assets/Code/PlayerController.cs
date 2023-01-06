using Assets.Code.Game;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController LocalInstance { get; private set; }

        public bool Started { get; set; }

        public const int maxLaneIndex = 4;
        public const float laneGap = 5.18f;
        public const float leftMostLaneXTitlescreen = -2.38f;
        public const float leftMostLaneX = -10.6f;

        private int laneIndex = maxLaneIndex / 2;

        private float zSpeed;
        private float acceleration = 30.0f;

        private const float startMaxZSpeed = 80.0f;
        private float maxZSpeed = startMaxZSpeed;
        private const float overallMaxZSpeed = 400f;

        private float startDelay = 1.0f;

        private Rigidbody body;

        private bool pressInputMode;


        public void OnCollisionEnter(Collision collision)
        {
            bool isDestroyer = collision.gameObject.tag == "destroyer";
            bool hasTruck = GetComponent<ComponentTruck>() != null;
            if (isDestroyer && hasTruck == false)
            {
                Crash();
            }

            if(isDestroyer && hasTruck)
            {
                body.velocity += collision.relativeVelocity;
            }
        }

        private void Crash()
        {
            Destroy(gameObject);
            CrashCar();
            SpawnFracturedCar(transform.position, true);
            Game.Game.OnScore();
        }

        private void SpawnFracturedCar(Vector3 position, bool red)
        {
            Debug.Log("spawning fractured car!");
            GameObject prefab;
            if (red)
                prefab = PrefabManager.Instance.FracturedRedCar;
            else
                prefab = PrefabManager.Instance.FracturedBlueCar;
            Instantiate(prefab, position, transform.rotation).GetComponent<ComponentExplosiveForce>().force *= (1.0f + transform.position.z / 1000.0f);

            Instantiate(PrefabManager.Instance.Explosion, position, Quaternion.identity);

        }

        private void CrashCar()
        {
            SoundManager.PlayCrash();
            SoundManager.StopPlayingEngine();
        }


        public void Awake()
        {
            LocalInstance = this;
            body = GetComponent<Rigidbody>();
            pressInputMode = PlayerPrefs.GetInt("InputMode", 0) == 1;
        }

        public void Start()
        {
            StartEngine();
            SoundManager.BeginPlayingEngine();
        }

        public void OnDestroy()
        {
            SoundManager.StopPlayingHorns();
            SoundManager.StopPlayingTire();
            SoundManager.StopPlayingBuild();
        }

        private Vector2 swipeStartPosition;
        private Vector2 swipeEndPosition;

        private bool swipeBegin;
        private bool mouseUsed;

        private void DetectSwipes()
        {
            if (swipeBegin == false)
            {
                foreach(Touch touch in Input.touches)
                {
                    if(touch.phase == TouchPhase.Began)
                    {
                        swipeStartPosition = touch.position;
                        swipeBegin = true;
                        break;
                    }
                }
                if (Input.GetMouseButtonDown(0))
                {
                    swipeStartPosition = Input.mousePosition;
                    mouseUsed = true;
                    swipeBegin = true;
                }
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    foreach (Touch touch in Input.touches)
                    {
                        if (touch.phase == TouchPhase.Moved)
                        {
                            swipeEndPosition = touch.position;
                            ResetSwipe();
                            break;
                        }
                        else if (touch.phase == TouchPhase.Ended)
                        {
                            swipeEndPosition = touch.position;
                            ResetSwipe();
                            break;
                        }
                    }

                    
                }
                else if (mouseUsed)
                {
                    Vector2 diff = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - swipeStartPosition;
                    if (diff.magnitude > 8.0f)
                    {
                        swipeEndPosition = Input.mousePosition;
                        ResetSwipe();
                    }
                    if (Input.GetMouseButton(0) == false)
                    {
                        swipeEndPosition = Input.mousePosition;
                        ResetSwipe();
                    }
                }


            }
        }


        private void ResetSwipe()
        {
            swipeBegin = false;
            mouseUsed = false;

            Vector2 diff = swipeEndPosition - swipeStartPosition;
            if (diff.x > 0)
            {
                //moving finger left - going right
                GoRight();
            }
            else if (diff.x < 0)
            {
                //moving finger right - going left
                GoLeft();

            }
        }

        private void DetectPresses()
        {
            /*
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Debug.Log("touch position: " + touch.position);
                    if (touch.position.x < Screen.width / 2)
                        GoLeft();
                    else GoRight();
                    break;
                }
            }
            */

            if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                if (Input.mousePosition.x < Screen.width / 2)
                    GoLeft();
                else GoRight();
            }
        }

        private void HandleInput()
        {
            Debug.Log("Press Input Mode " + pressInputMode);
            if (pressInputMode)
            {
                DetectPresses();
            }
            else DetectSwipes();

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                GoLeft();
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                GoRight();
        }

        public void Update()
        {
            if (Started)
            {
                HandleInput();

                AccelerateAndMove();
                DampenRotation();
                UpdateEngine();
            }
            else
            {
                if (startDelay > 0f)
                    startDelay -= Time.deltaTime;
                else
                {
                    Started = true;
                }
            }
            UpdateLanePosition();
        }

        private void UpdateEngine()
        {
            float percentage = Mathf.Max(transform.position.z / 2000f, 0.5f);

            SoundManager.UpdateEnginePitch(Mathf.Min(percentage, 1), Mathf.Min(percentage, 4));
        }

        private void DampenRotation()
        {
            Quaternion dest = Quaternion.Euler(0, 0, 0);

            transform.rotation = Quaternion.Lerp(transform.rotation, dest, Time.deltaTime * 4.0f);

            body.angularVelocity += (Vector3.zero - body.angularVelocity) * (Time.deltaTime * 4.0f);
        }

        private void AccelerateAndMove()
        {
            zSpeed += acceleration * Time.deltaTime;
            if (zSpeed > maxZSpeed)
                zSpeed = maxZSpeed;


            float progressMultiplier = transform.position.z / 100.0f;

            maxZSpeed = startMaxZSpeed + (int) progressMultiplier * 6.0f;
            if (maxZSpeed > overallMaxZSpeed)
                maxZSpeed = overallMaxZSpeed;

            body.velocity = new Vector3(0, body.velocity.y, zSpeed);
            PreventFromFallingThroughGround();

            if(GetComponent<ComponentTruck>() != null)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + zSpeed * Time.deltaTime);
        }


        private void PreventFromFallingThroughGround()
        {
            if(transform.position.y < 1.58f)
            {
                transform.position = new Vector3(transform.position.x, 1.58f, transform.position.z);
            }
        }

        private void UpdateLanePosition()
        {
            float destX = leftMostLaneX + laneIndex * laneGap;

            transform.position = transform.position + new Vector3((destX - transform.position.x) * 7.0f * Time.deltaTime, 0, 0);
        }

        private void PlayTireSounds()
        {
            SoundManager.PlayTire();
            SoundManager.PlayHorn();
        }

        public void TurnOffLights()
        {
            Light[] lights = GetComponentsInChildren<Light>();
            foreach (Light light in lights)
                light.enabled = false;
        }

        public void TurnOnLights()
        {
            Light[] lights = GetComponentsInChildren<Light>();
            foreach (Light light in lights)
                light.enabled = true;
        }

        public void StartEngine()
        {
            SoundManager.PlayEngineStart();
        }

        private void GoRight()
        {
            if (laneIndex < maxLaneIndex)
            {
                laneIndex++;
            }
        }


        private void GoLeft()
        {
            if (laneIndex > 0)
            {
                laneIndex--;
            }
        }
    }
}
