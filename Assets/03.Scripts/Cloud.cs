using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    int speed;
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        speed = Random.Range(10, 100);
    }
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DestroyCloud")
            Destroy(gameObject, 0);
    }
}
