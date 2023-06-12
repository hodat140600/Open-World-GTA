using UnityEngine;
using System.Collections.Generic;
using Extensions;
using Math = System.Math;

namespace GleyTrafficSystem
{
    /// <summary>
    /// This class is for testing purpose only
    /// It is the car controller provided by Unity:
    /// https://docs.unity3d.com/Manual/WheelColliderTutorial.html
    /// </summary>
    [System.Serializable] public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool          motor;
        public bool          steering;
    }

    public class PlayerCar : MonoBehaviour
    {
        public List<AxleInfo>  axleInfos;
        public Transform       centerOfMass;
        public float           maxMotorTorque;
        public float           maxSteeringAngle;
        VehicleLightsComponent lightsComponent;

        private bool _mainLights;
        private bool _brake;
        private bool _reverse;
        private bool _blinkLeft;
        private bool _blinkRight;

        Rigidbody rb;

        UIInput inputScript;

        private void Start()
        {
            GetComponent<Rigidbody>().centerOfMass = centerOfMass.localPosition;
            inputScript                            = gameObject.AddComponent<UIInput>();
            lightsComponent                        = gameObject.GetComponent<VehicleLightsComponent>();
            lightsComponent.Initialize();
            rb = GetComponent<Rigidbody>();
        }

        // finds the corresponding visual wheel
        // correctly applies the transform
        public void ApplyLocalPositionToVisuals(WheelCollider collider)
        {
            if (collider.transform.childCount == 0)
            {
                return;
            }

            Transform visualWheel = collider.transform.GetChild(0);

            Vector3    position;
            Quaternion rotation;
            collider.GetWorldPose(out position, out rotation);

            visualWheel.transform.position = position;
            visualWheel.transform.rotation = rotation;
        }

        private float _motor, _steering;

        public void FixedUpdate()
        {
            _motor    = maxMotorTorque * inputScript.GetVerticalInput();
            _steering = maxSteeringAngle * inputScript.GetHorizontalInput();

            float localVelocity = transform.InverseTransformDirection(rb.velocity).z + 0.1f;
            _reverse = false;
            _brake   = false;

            if (localVelocity < 0)
            {
                _reverse = true;
            }

            if (_motor < 0)
            {
                if (localVelocity > 0)
                {
                    _brake = true;
                }
            }
            else
            {
                if (_motor > 0)
                {
                    if (localVelocity < 0)
                    {
                        _brake = true;
                    }
                }
            }

            var front = axleInfos[0];
            var rear  = axleInfos[1];

            if (front.steering)
            {
                front.leftWheel.steerAngle  = _steering;
                front.rightWheel.steerAngle = _steering;
            }

            if (front.motor)
            {
                front.leftWheel.motorTorque  = _motor / 2f;
                front.rightWheel.motorTorque = _motor / 2f;
            }

            ApplyLocalPositionToVisuals(front.leftWheel);
            ApplyLocalPositionToVisuals(front.rightWheel);

            if (rear.steering)
            {
                rear.leftWheel.steerAngle  = _steering;
                rear.rightWheel.steerAngle = _steering;
            }

            if (rear.motor)
            {
                rear.leftWheel.motorTorque  = _motor;
                rear.rightWheel.motorTorque = _motor;
            }

            ApplyLocalPositionToVisuals(rear.leftWheel);
            ApplyLocalPositionToVisuals(rear.rightWheel);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _mainLights = !_mainLights;
                lightsComponent.SetMainLights(_mainLights);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                _blinkLeft = !_blinkLeft;
                if (_blinkLeft == true)
                {
                    _blinkRight = false;
                    lightsComponent.SetBlinker(BlinkType.BlinkLeft);
                }
                else
                {
                    lightsComponent.SetBlinker(BlinkType.Stop);
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _blinkRight = !_blinkRight;
                if (_blinkRight == true)
                {
                    _blinkLeft = false;
                    lightsComponent.SetBlinker(BlinkType.BlinkRight);
                }
                else
                {
                    lightsComponent.SetBlinker(BlinkType.Stop);
                }
            }

            lightsComponent.SetBrakeLights(_brake);
            lightsComponent.SetReverseLights(_reverse);
            lightsComponent.UpdateLights();
        }
    }
}