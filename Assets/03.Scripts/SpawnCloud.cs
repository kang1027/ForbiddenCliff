using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCloud : MonoBehaviour
{
    public GameObject[] cloud;
    public Transform[] spawnPoint;

    private bool isSpawn;
    public float spawnDelay = 0.0f;
    // Update is called once per frame
    void Start()
    {

    }
    void Update()
    {
        if(!isSpawn)
        {
            Instantiate(cloud[Random.Range(0, cloud.Length)], spawnPoint[0].position, Quaternion.identity);
            Instantiate(cloud[Random.Range(0, cloud.Length)], spawnPoint[1].position, Quaternion.identity);
            Instantiate(cloud[Random.Range(0, cloud.Length)], spawnPoint[2].position, Quaternion.identity);
            isSpawn = true;
            Invoke("Spawn", spawnDelay);
        }
    }

    private void Spawn()
    {
        isSpawn = false;
    }


}
