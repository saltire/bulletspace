using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveScript : MonoBehaviour {
  public float minSpeed = 1f;
  public float maxSpeed = 4f;
  public float accelerateSpeed = 1f;
  public float brakeSpeed = 1f;
  float speed;
  float accelerate;
  float brake;

  public float pitchSpeed = 90f;
  public float rollSpeed = 90f;
  public float yawSpeed = 90f;
  float pitch;
  float roll;
  float yaw;

  public Transform cameraRig;
  float cameraVelocity;
  public float cameraSmoothTime = .1f;

  void Start() {
    speed = minSpeed;
  }

  void Update() {
    // Move

    speed = Mathf.Clamp(
      speed + ((accelerate * accelerateSpeed) - (brake * brakeSpeed)) * Time.deltaTime,
      minSpeed, maxSpeed);

    transform.position += transform.rotation * Vector3.forward * speed * Time.deltaTime;

    // Rotate

    Quaternion prevRotation = cameraRig.rotation;

    if (pitch != 0) {
      transform.Rotate(Vector3.right, pitch * pitchSpeed * Time.deltaTime);
    }
    if (roll != 0) {
      transform.Rotate(Vector3.back, roll * rollSpeed * Time.deltaTime);
    }
    if (yaw != 0) {
      transform.Rotate(Vector3.up, yaw * yawSpeed * Time.deltaTime);
    }

    cameraRig.rotation = SmoothDampRotation(prevRotation, transform.rotation);
  }

  Quaternion SmoothDampRotation(Quaternion current, Quaternion target) {
    float delta = Quaternion.Angle(current, target);
    if (delta == 0) return current;

    float frameDelta = Mathf.SmoothDampAngle(delta, 0f, ref cameraVelocity, cameraSmoothTime);
    return Quaternion.Slerp(current, target, 1f - (frameDelta / delta));
  }

  void OnPitchRoll(InputValue value) {
    Vector2 pitchRoll = value.Get<Vector2>();
    pitch = pitchRoll.y;
    yaw = pitchRoll.x;
  }

  void OnYaw(InputValue value) {
    roll = value.Get<float>();
  }

  void OnAccelerate(InputValue value) {
    accelerate = value.Get<float>();
  }

  void OnBrake(InputValue value) {
    brake = value.Get<float>();
  }
}
