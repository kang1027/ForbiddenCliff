using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class MultiPlayerControllor : MonoBehaviourPun, IPunObservable
{
    private Transform myT = null;
    public Rigidbody2D myR = null;
    private Animator myAnim = null;
    public float jumpPower = 0f;
    public float moveSpeed = 1.0f;
    public static float playerHp = 6;

    private float chargeJumpPower = 0.0f;

    private bool isJumpChaging;
    public bool isJump;
    private bool isMove = true;
    private bool isSweat;

    private SpriteRenderer mySR;

    public GameObject sweat;

    private bool isDamaged;
    private bool isDamagedEffect;

    private Image[] hp = new Image[3];
    private Image[] halpHp = new Image[3];

    public AudioClip walk;
    public AudioClip gem;
    public AudioClip chaging;
    public AudioClip hitSound;

    private MultiAudioManager am;

    public Sprite onButton;

    private bool tryJump;
    SpriteRenderer playerSr;

    private GameObject playerName;
    private GameObject remotePlayerName;
    private GameObject Jump;
    bool endGame = false;
    GameObject winPanel;
    GameObject losePanel;
    void Start()
    {
            winPanel = GameObject.Find("InGameCanvas").transform.Find("WinPanel").gameObject;
            losePanel = GameObject.Find("InGameCanvas").transform.Find("LosePanel").gameObject;
        if (photonView.IsMine)
        {
            myScore = GameObject.Find("MyScore").GetComponent<Text>();
            GameObject.Find("MyName").GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName;
            GameObject.Find("GameManager").GetComponent<MultiGameManager>().player = gameObject;
            GameObject[] tempAMs = GameObject.FindGameObjectsWithTag("AudioManager");
            for(int i = 0; i < tempAMs.Length; i++)
            {
                MultiAudioManager tempAM = tempAMs[i].GetComponent<MultiAudioManager>();
                if (tempAM.isMine)
                    am = tempAM;
                else
                    return;
            }
            hp[0] = GameObject.Find("Hp1").GetComponent<Image>();
            hp[1] = GameObject.Find("Hp2").GetComponent<Image>();
            hp[2] = GameObject.Find("Hp3").GetComponent<Image>();
            halpHp[0] = GameObject.Find("HalpHp1").GetComponent<Image>();
            halpHp[1] = GameObject.Find("HalpHp2").GetComponent<Image>();
            halpHp[2] = GameObject.Find("HalpHp3").GetComponent<Image>();

            myT = GetComponent<Transform>();
            myR = GetComponent<Rigidbody2D>();
            playerSr = GetComponent<SpriteRenderer>();

            sweat.SetActive(false);
            for (int i = 0; i < 3; i++)
            {
                Transform button = GameObject.Find("BackGround").transform.Find("NextButton").gameObject.transform.GetChild(i);
                button.gameObject.tag = "Button" + i.ToString();
            }
            playerName = GameObject.Find("PlayerNameText");
            playerName.GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName;
            Jump = GameObject.Find("Jump");
        
            
        }
        else
        {
            GameObject.Find("RemoteName").GetComponent<Text>().text = photonView.Owner.NickName;
            remotePlayerName = GameObject.Find("RemotePlayerNameText");
            if (PhotonNetwork.LocalPlayer.NickName.Equals(PhotonNetwork.PlayerList[0].NickName))
                remotePlayerName.GetComponent<Text>().text = PhotonNetwork.PlayerList[1].NickName + "님";
            else
                remotePlayerName.GetComponent<Text>().text = PhotonNetwork.PlayerList[0].NickName + "님";

        }
        myAnim = GetComponent<Animator>();
        mySR = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (MultiGameManager.isPlay)
            {
                PlayerMove();
                PlayerJump();
                PlayerAnim();

                playerName.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 18f, 0));

                if (!isJump)
                    Jump.GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                else
                    Jump.GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                if (endGame)
                    winPanel.SetActive(true);
            }
        }
        else if ((transform.position - currPos).sqrMagnitude >= 100) transform.position = currPos;
        else{
            if(GameObject.Find("RemoteScore"))
                GameObject.Find("RemoteScore").GetComponent<Text>().text = currScore.ToString();
            mySR.flipX = currFlipX;
            myAnim.SetBool("IsChaging", currJumpChaging);
            myAnim.SetBool("IsJump", currJump);
            myAnim.SetBool("IsDie1", currDie);
            myAnim.SetBool("IsWalk", currMove);
            mySR.color = Color.yellow;
            transform.position = Vector3.Lerp(transform.position, currPos, Time.deltaTime * 10);
            remotePlayerName.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 18f, 0));
            if (currEndGame)
            {
                losePanel.SetActive(true);

            }

        }
    }
    bool animWalk;
    bool animChaging;
    bool animDie;
    int myScoreInt;
    int gemScore = 0;
    Text myScore;
    private void PlayerAnim()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) || MultiButtonControllor.isLeftButton || MultiButtonControllor.isRightButton && !isJump)
        {
            animWalk = true;
            animChaging = false;
            animDie = false;

            myAnim.SetBool("IsChaging", false);
            myAnim.SetBool("IsJump", false);
            myAnim.SetBool("IsDie1", false);
            myAnim.SetBool("IsWalk", true);
            if (Input.GetKey(KeyCode.Space) || MultiButtonControllor.isJumpButton)
            {
                animWalk = false;
                animChaging = true;
                animDie = false;

                myAnim.SetBool("IsDie1", false);
                myAnim.SetBool("IsWalk", false);
                myAnim.SetBool("IsJump", false);
                myAnim.SetBool("IsChaging", true);
            }
        }
        else if (Input.GetKey(KeyCode.Space) || MultiButtonControllor.isJumpButton)
        {
            animWalk = false;
            animChaging = true;
            animDie = false;
            myAnim.SetBool("IsDie1", false);
            myAnim.SetBool("IsWalk", false);
            myAnim.SetBool("IsJump", false);
            myAnim.SetBool("IsChaging", true);
        }
        else
        {

            if (!isJump)
            {
                animWalk = false;
                animChaging = false;
                animDie = false;

                myAnim.SetBool("IsDie1", false);
                myAnim.SetBool("IsWalk", false);
                myAnim.SetBool("IsChaging", false);
                myAnim.SetBool("IsJump", false);
            }
        }
        if (isJump)
        {
            animWalk = false;
            animChaging = false;
            animDie = false;

            myAnim.SetBool("IsDie1", false);
            myAnim.SetBool("IsWalk", false);
            myAnim.SetBool("IsChaging", false);
            myAnim.SetBool("IsJump", true);
        }
    }

    private void PlayerJump()
    {
        if (Input.GetKey(KeyCode.Space) || MultiButtonControllor.isJumpButton)
        {
            if (am.isJump)
                am.PlayAudio(chaging, 0, 0.1f);
        }
        if (!isJump)
        {

            if (Input.GetKey(KeyCode.Space) || MultiButtonControllor.isJumpButton)
            {
                myR.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                isMove = false;
                while (jumpPower < 125 && !isJumpChaging)
                {
                    StartCoroutine("JumpChaging");
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                myR.constraints = RigidbodyConstraints2D.FreezeRotation;
                JumpingPlayer();
            }
            else
            {
                // myR.AddForce(Vector2.down * chargeJumpPower * 100);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                DownJumpingPlayer();
            }
        }

    }
    public void DownJumpingPlayer()
    {
        Vector2 a = Physics2D.gravity;
        a = new Vector2(Physics2D.gravity.x, -30f);
        Physics2D.gravity = a;
    }
    public void JumpingPlayer()
    {
        if (jumpPower > 125)
            jumpPower = 125;
        //if(Input.GetKey(KeyCode.D))
        //    {
        //        chargeJumpPower = jumpPower;
        //        Vector2 aa = Vector2.up + Vector2.right;
        //        myR.AddForce(aa * chargeJumpPower * 100);
        //        jumpPower = 1.5f;

        //}
        //else if (Input.GetKey(KeyCode.A))
        //    {
        //        chargeJumpPower = jumpPower;
        //        Vector2 aa = Vector2.up + Vector2.left;
        //        myR.AddForce(aa * chargeJumpPower * 100);
        //        jumpPower = 1.5f;
        //}
        am.isJump = false;
        am.PlayAudio(walk, 0, 0); // <= 점프중일 때 소리 넣어야함.
        chargeJumpPower = jumpPower;
        myR.AddForce(Vector2.up * chargeJumpPower * 100);
        jumpPower = 40f;
        isMove = true;
    }
    private void PlayerMove()
    {
        isSweat = true;
        if (isMove)
        {
            float _xMove = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
            float _bXMove = 0;
            if (MultiButtonControllor.isLeftButton)
                _bXMove = -1;
            if (MultiButtonControllor.isRightButton)
                _bXMove = 1;
            myR.velocity = new Vector2(_bXMove * moveSpeed, myR.velocity.y);
            if (_bXMove != 0 && !isJump && !isJumpChaging)
            {
                am.PlayAudio(walk, 0.15f, 0.3f);
            }
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || MultiButtonControllor.isLeftButton)
        {
            mySR.flipX = true;
            Invoke("SweatPlay", 0.5f);
            sweat.transform.localPosition = new Vector3(20, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || MultiButtonControllor.isRightButton)
        {
            mySR.flipX = false;
            Invoke("SweatPlay", 0.5f);
            sweat.transform.localPosition = new Vector3(-20, 0, 0);
        }
        else
        {
            isSweat = false;
            sweat.SetActive(false);
        }

    }

    private void SweatPlay()
    {
        if (!MultiGameManager.isPlay)
            sweat.SetActive(false);
        else if (isSweat)
            sweat.SetActive(true);

    }

    IEnumerator JumpChaging()
    {
        jumpPower += 16f;
        isJumpChaging = true;
        yield return new WaitForSeconds(0.1f);
        isJumpChaging = false;
    }

    public void RestartUISetting()
    {
        hp[0].gameObject.SetActive(true);
        hp[1].gameObject.SetActive(true);
        hp[2].gameObject.SetActive(true);
        halpHp[0].gameObject.SetActive(true);
        halpHp[1].gameObject.SetActive(true);
        halpHp[2].gameObject.SetActive(true);
        animDie = false;

        myAnim.SetBool("IsDie1", false);
        myAnim.SetBool("IsDie2", false);
        myAnim.SetBool("IsWalk", false);
        myAnim.SetBool("IsChaging", false);
        myAnim.SetBool("IsJump", false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!photonView.IsMine)
            return;

        if (collision.gameObject.tag != "notWall")
        {
            myScoreInt = (int)transform.position.y + 74 + gemScore;
            myScore.text = myScoreInt.ToString();
            isJump = false;
            am.isJump = true;
        }
        else
            am.isJump = false;


        isMove = true;
        if (collision.gameObject.tag == "Enemy" && !isDamaged)
        {
            am.PlayAudio(hitSound, 0.2f, 0.15f);
            if (playerHp == 6)
                hp[2].gameObject.SetActive(false);
            else if (playerHp == 5)
                halpHp[2].gameObject.SetActive(false);
            else if (playerHp == 4)
                hp[1].gameObject.SetActive(false);
            else if (playerHp == 3)
                halpHp[1].gameObject.SetActive(false);
            else if (playerHp == 2)
                hp[0].gameObject.SetActive(false);
            else if (playerHp == 1)
                halpHp[0].gameObject.SetActive(false);
            isDamaged = true;
            isDamagedEffect = true;
            StartCoroutine("DamagedEffect");
            Invoke("DamagedDelay", 1f);
            playerHp--;
        }
        if (collision.gameObject.tag == "Enemy2" && !isDamaged)
        {
            am.PlayAudio(hitSound, 0.2f, 0.15f);
            if (playerHp == 6)
            {
                hp[2].gameObject.SetActive(false);
                halpHp[2].gameObject.SetActive(false);
            }
            else if (playerHp == 5)
            {
                halpHp[2].gameObject.SetActive(false);
                hp[1].gameObject.SetActive(false);
            }
            else if (playerHp == 4)
            {
                hp[1].gameObject.SetActive(false);
                halpHp[1].gameObject.SetActive(false);
            }
            else if (playerHp == 3)
            {
                halpHp[1].gameObject.SetActive(false);
                hp[0].gameObject.SetActive(false);
            }
            else if (playerHp == 2)
            {
                hp[0].gameObject.SetActive(false);
                halpHp[0].gameObject.SetActive(false);
            }
            else if (playerHp == 1)
                halpHp[0].gameObject.SetActive(false);
            isDamaged = true;
            isDamagedEffect = true;
            if (isDamagedEffect)
                Invoke("DamagedDelay", 1f);
            isDamagedEffect = true;
            StartCoroutine("DamagedEffect");
            playerHp -= 2;
        }
        if (playerHp <= 0)
        {
            MultiGameManager.isPlay = false;
            myAnim.SetBool("IsWalk", false);
            myAnim.SetBool("IsChaging", false);
            myAnim.SetBool("IsJump", false);
            myAnim.SetBool("IsDie1", true);
            animDie = true;
            Invoke("DieAnim2", 0.1f);
        }
        Physics2D.gravity = new Vector2(0, -9.81f);

    }

    IEnumerator DamagedEffect()
    {
        isDamagedEffect = false;
        Color c = playerSr.color;
        c.a = 0.3f;
        playerSr.color = c;
        yield return new WaitForSeconds(0.15f);
        c.a = 1;
        playerSr.color = c;
        yield return new WaitForSeconds(0.15f);
        c.a = 0.3f;
        playerSr.color = c;
        yield return new WaitForSeconds(0.15f);
        c.a = 1;
        playerSr.color = c;
        yield return new WaitForSeconds(0.15f);
        c.a = 0.3f;
        playerSr.color = c;
        yield return new WaitForSeconds(0.15f);
        c.a = 1;
        playerSr.color = c;
    }
    private void DieAnim2()
    {
        
        myAnim.SetBool("IsWalk", false);
        myAnim.SetBool("IsChaging", false);
        myAnim.SetBool("IsJump", false);
        myAnim.SetBool("IsDie2", true);
        myAnim.SetBool("IsDie1", true);
    }
    private void DamagedDelay()
    {
        isDamaged = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "notWall")
        {
            isJump = false;
            
        }
        Physics2D.gravity = new Vector2(0, -9.81f);

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isJump = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine)
            return;

        string exitBlocks = "ExitBlock";

        if (collision.gameObject.tag == "Gem")
        {
            gemScore += 100;
            myScoreInt = (int)transform.position.y + 74 + gemScore;
            myScore.text = myScoreInt.ToString();
            am.PlayAudio(gem, 0, 0.1f);
            MultiGameManager.gem++;
            Destroy(collision.gameObject, 0.1f);
        }
        for (int i = 0; i < 3; i++)
        {
            if (collision.gameObject.tag == "Button" + i.ToString())
            {
                collision.gameObject.GetComponent<SpriteRenderer>().sprite = onButton;

                GameObject[] exitBlock = GameObject.FindGameObjectsWithTag(exitBlocks + i);
                foreach (GameObject eb in exitBlock)
                {
                    eb.SetActive(false);
                }
            }
        }
        if (collision.gameObject.tag == "GameClear")
        {
            MultiGameManager.isGameClear = true;
            endGame = true;
        }

    }


    //private bool isJumpChaging;
    //private bool isJump;
    //private bool isMove = true;
    bool currFlipX;
    bool currJumpChaging;
    bool currJump;
    bool currMove;
    bool currDie;
    Vector3 currPos;
    int currScore;
    bool currEndGame;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(mySR.flipX);
            stream.SendNext(animChaging);
            stream.SendNext(isJump);
            stream.SendNext(animWalk);
            stream.SendNext(animDie);
            stream.SendNext(transform.position);
            stream.SendNext(myScoreInt);
            stream.SendNext(endGame);
        }
        else
        {
            currFlipX = (bool)stream.ReceiveNext();
            currJumpChaging = (bool)stream.ReceiveNext();
            currJump = (bool)stream.ReceiveNext();
            currMove = (bool)stream.ReceiveNext();
            currDie = (bool)stream.ReceiveNext();
            currPos = (Vector3)stream.ReceiveNext();
            currScore = (int)stream.ReceiveNext();
            currEndGame = (bool)stream.ReceiveNext();
        }
    }
}

