using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScene : MonoBehaviour
{
    public static int currentScene = 0;
    public static int currentEventScene = 0;
    public BoxCollider2D player;
    public new Transform camera;


    public GameObject[] changeBGMPos;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (player.transform.position.y > 100 && player.transform.position.y < 320)
            currentScene = 1;
        else if (player.transform.position.y > 320 && player.transform.position.y < 500)
            currentScene = 2;
        else if (player.transform.position.y > 500 && player.transform.position.y < 730)
            currentScene = 3;
        else if (player.transform.position.y > 730 && player.transform.position.y < 915)
        {
            currentScene = 4;
            if (player.transform.position.x > 230)
            {
                Debug.Log("??");
                currentEventScene = 2;
            }
            else
            {
                Debug.Log("?????");
                currentEventScene = 0;
            }
        }
        else if (player.transform.position.y > 915 && player.transform.position.y < 1112)
            currentScene = 5;
        else if (player.transform.position.y > 1112 && player.transform.position.y < 1312)
            currentScene = 6;

        else if (player.transform.position.y > 1312 && player.transform.position.y < 1510)
            currentScene = 7;
        else if (player.transform.position.y > 1510 && player.transform.position.y < 1540)
            currentScene = 8;
        else
            currentScene = 0;
    }


}
