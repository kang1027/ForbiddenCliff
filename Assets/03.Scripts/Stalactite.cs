using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour
{
    public float fallingSpeed = 1.0f;
    // Start is called before the first frame update'

    private bool isFalling = false;
    // 현재 구현중
    private void Update()
    {
        fallingObject();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(this.gameObject, 0.7f);
            isFalling = true;
        }
    }

    private void fallingObject()
    {
        if(isFalling)
        transform.position += Vector3.down * fallingSpeed * Time.deltaTime;
    }
}
