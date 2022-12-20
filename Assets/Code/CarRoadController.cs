using UnityEngine;

namespace Assets.Code
{
    public class CarRoadController : MonoBehaviour
    {
        public static CarRoadController LocalInstance { get; private set; }

        public bool Started { get; set; }

        public const int maxLaneIndex = 4;
        public const float laneGap = 5.18f;
        public const float leftMostLaneXTitlescreen = -2.38f;
        public const float leftMostLaneX = -10.6f;

        private int laneIndex = maxLaneIndex / 2;

        private float zSpeed;
        private float acceleration = 10.0f;

        private const float startMaxZSpeed = 40.0f;
        private float maxZSpeed = startMaxZSpeed;

        private float startDelay = 1.0f;

        private Rigidbody body;


        public void OnCollisionEnter(Collision collision)
        {
            bool isDestroyer = collision.gameObject.tag == "destroyer";
            if (isDestroyer)
            {
                Destroy(gameObject);

                CrashCar();
                SpawnFracturedCar(transform.position, true);

                Game.Game.OnScore();
            }
        }

        private void SpawnFracturedCar(Vector3 position, bool red)
        {
            Debug.Log("spawning fractured car!");
            GameObject prefab;
            if (red)
                prefab = PrefabManager.Instance.FracturedRedCar;
            else
                prefab = PrefabManager.Instance.FracturedBlueCar;
            Instantiate(prefab, position, transform.rotation).GetComponent<ComponentExplosiveForce>().force *= (1.0f + transform.position.z / 40.0f);

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

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    GoLeft();
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                    GoRight();

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

        public void Update()
        {
            if (Started)
            {
                DetectSwipes();
                AccelerateAndMove();
                DampenRotation();
                UpdateEngine();
            }
            else
            {
                if (startDelay > 0f)
                    startDelay -= Time.deltaTime;
                else Started = true;
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

            maxZSpeed = startMaxZSpeed + (int) progressMultiplier * 3.0f;
            if (maxZSpeed > 200f)
                maxZSpeed = 200f;

            body.velocity = new Vector3(0, body.velocity.y, zSpeed);

            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + zSpeed * Time.deltaTime);
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
                light.enabled = false;
        }

        public void StartEngine()
        {
            SoundManager.PlayEngineStart();
        }

        private void GoRight()
        {
            if (laneIndex < maxLaneIndex)
                laneIndex++;
        }

        private void GoLeft()
        {
            if (laneIndex > 0)
                laneIndex--;
        }
    }
}
