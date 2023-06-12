using UnityEngine;
using UnityEngine.SceneManagement;

namespace GleyTrafficSystem
{
    public class UIInput : MonoBehaviour
    {
        //Events used for UI buttons only on mobile device

        public delegate void ButtonDown(string button);

        public static event ButtonDown onButtonDown;

        public static void TriggerButtonDownEvent(string button)
        {
            if (onButtonDown != null)
            {
                onButtonDown(button);
            }
        }

        public delegate void ButtonUp(string button);

        public static event ButtonUp onButtonUp;

        public static void TriggerButtonUpEvent(string button)
        {
            if (onButtonUp != null)
            {
                onButtonUp(button);
            }
        }

        bool left, right, up, down;

        float horizontalInput;
        float verticalInput;

        public bool useAxis = true;

        /// <summary>
        /// Initializes the input system based on platform used
        /// </summary>
        /// <returns></returns>
        public void Awake()
        {
            onButtonDown += PointerDown;
            onButtonUp   += PointerUp;
#if UNITY_EDITOR
            GameObject steeringUI = GameObject.Find("SteeringUI");
            if (steeringUI)
            {
                steeringUI.SetActive(false);
            }
#endif
            //return this;
        }


        /// <summary>
        /// Get the steer input
        /// </summary>
        /// <returns></returns>
        public float GetHorizontalInput()
        {
            return horizontalInput;
        }


        /// <summary>
        /// Get acceleration input
        /// </summary>
        /// <returns></returns>
        public float GetVerticalInput()
        {
            return verticalInput;
        }

        /// <summary>
        /// Read input
        /// </summary>
        private void Update()
        {
#if UNITY_EDITOR
            up    = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
            down  = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
            left  = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
            right = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
#endif

            if (left)
            {
                // horizontalInput -= Time.deltaTime;
                //
                // if (horizontalInput > -.5f)
                //     horizontalInput = -.5f;
                horizontalInput = Mathf.Min(-.95f, horizontalInput - Time.deltaTime);
            }
            else
            {
                if (right)
                {
                    // horizontalInput += Time.deltaTime;
                    //
                    // if (horizontalInput < .5f)
                    //     horizontalInput = .5f;
                    horizontalInput = Mathf.Max(.95f, horizontalInput + Time.deltaTime);
                }
                else
                {
                    horizontalInput = Mathf.MoveTowards(horizontalInput, 0, 5 * Time.deltaTime);
                }
            }

            horizontalInput = Mathf.Clamp(horizontalInput, -1, 1);

            if (up)
            {
                // verticalInput += Time.deltaTime;
                verticalInput = Mathf.Max(.95f, verticalInput + Time.deltaTime);
            }
            else
            {
                if (down)
                {
                    // verticalInput -= Time.deltaTime;
                    verticalInput = Mathf.Min(-.95f, verticalInput - Time.deltaTime);
                }
                else
                {
                    verticalInput = 0;
                }
            }

            verticalInput = Mathf.Clamp(verticalInput, -.3f, 1);


            // Debug.Log("H:" + horizontalInput);
            // Debug.Log("V:" + verticalInput);
        }

        //Click event handlers for mobile devices
        public void PointerDown(string name)
        {
            if (name == "Restart")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (name == "Left")
            {
                left  = true;
                right = false;
            }

            if (name == "Right")
            {
                right = true;
                left  = false;
            }

            if (name == "Up")
            {
                up   = true;
                down = false;
            }

            if (name == "Down")
            {
                down = true;
                up   = false;
            }
        }

        public void PointerUp(string name)
        {
            if (name == "Left")
            {
                left = false;
            }

            if (name == "Right")
            {
                right = false;
            }

            if (name == "Up")
            {
                up = false;
            }

            if (name == "Down")
            {
                down = false;
            }
        }

        private void OnDestroy()
        {
            onButtonDown -= PointerDown;
            onButtonUp   -= PointerUp;
        }
    }
}