using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsScript : MonoBehaviour {
  public Transform player;
  public GameObject asteroidPrefab;

  public int sectorCount = 5;
  public int sectorSize = 50;
  public int sectorRange = 3;

  Vector3Int currentSector;
  Dictionary<Vector3Int, GameObject> sectors;

  void Update() {
    Vector3Int newCurrentSector = new Vector3Int(
      (int)Mathf.Floor(player.position.x / sectorSize),
      (int)Mathf.Floor(player.position.y / sectorSize),
      (int)Mathf.Floor(player.position.z / sectorSize));

    if (sectors == null || newCurrentSector != currentSector) {
      if (sectors == null) {
        sectors = new Dictionary<Vector3Int, GameObject>();
      }

      currentSector = newCurrentSector;

      Bounds bounds = new Bounds(currentSector, Vector3.one * sectorRange * 2);

      List<Vector3Int> keys = new List<Vector3Int>(sectors.Keys);
      foreach (Vector3Int sectorIndex in keys) {
        if (!bounds.Contains(sectorIndex)) {
          Destroy(sectors[sectorIndex]);
          sectors.Remove(sectorIndex);
        }
      }

      for (int x = (int)bounds.min.x; x <= bounds.max.x; x++) {
        for (int y = (int)bounds.min.y; y <= bounds.max.y; y++) {
          for (int z = (int)bounds.min.z; z <= bounds.max.z; z++) {
            Vector3Int sectorIndex = new Vector3Int(x, y, z);
            if (!sectors.ContainsKey(sectorIndex)) {
              Vector3 sectorPos = sectorIndex * sectorSize;

              sectors[sectorIndex] = new GameObject("Sector " + sectorIndex);
              sectors[sectorIndex].transform.position = sectorPos;
              sectors[sectorIndex].transform.parent = transform;

              for (int i = 0; i < sectorCount; i++) {
                Instantiate(asteroidPrefab,
                  new Vector3(
                    Random.Range(sectorPos.x, sectorPos.x + sectorSize),
                    Random.Range(sectorPos.y, sectorPos.y + sectorSize),
                    Random.Range(sectorPos.z, sectorPos.z + sectorSize)),
                  Quaternion.identity, sectors[sectorIndex].transform);
              }
            }
          }
        }
      }
    }
  }

  // void SpawnAsteroidBlock()
}
