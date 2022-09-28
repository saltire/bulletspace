using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsScript : MonoBehaviour {
  public int count = 100;
  public float radius = 50f;
  public GameObject asteroidPrefab;

  void Start() {
    for (int i = 0; i < count; i++) {
      Instantiate(asteroidPrefab,
        Random.insideUnitSphere * radius, Quaternion.identity, transform);
    }
  }
}
