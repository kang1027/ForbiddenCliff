using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerControllor : MonoBehaviour
{
    private Transform myT = null;
    public  Rigidbody2D myR = null;
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

    public Image[] hp;
    public Image[] halpHp;

    public AudioClip walk;
    public AudioClip gem;
    public AudioClip chaging;
    public AudioClip hitSound;

    public AudioManager am;

    public Sprite onButton;

    private bool tryJump;
    SpriteRenderer playerSr;
    public GameObject Jump;

    // Start is called before the first frame update
    void Start()
    {


        myT = GetComponent<Transform>();
        myR = GetComponent<Rigidbody2D>();
        mySR = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();
        playerSr = GetComponent<SpriteRenderer>();

        sweat.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            Transform button = GameObject.Find("BackGround").transform.Find("NextButton").gameObject.transform.GetChild(i);
            button.gameObject.tag = "Button" + i.ToString();
        }
    }

    private void Awake()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.isPlay)
        {
            PlayerMove();
            PlayerJump();
            PlayerAnim();

            if (!isJump)
                Jump.GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            else
                Jump.GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
    }
    private void PlayerAnim()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) || ButtonControllor.isLeftButton || ButtonControllor.isRightButton && !isJump)
        {
            myAnim.SetBool("IsChaging", false);
            myAnim.SetBool("IsJump", false);
            myAnim.SetBool("IsDie1", false);
            myAnim.SetBool("IsWalk", true);
            if (Input.GetKey(KeyCode.Space) || ButtonControllor.isJumpButton)
            {
                myAnim.SetBool("IsDie1", false);
                myAnim.SetBool("IsWalk", false);
                myAnim.SetBool("IsJump", false);
                myAnim.SetBool("IsChaging", true);
            }
        }
        else if (Input.GetKey(KeyCode.Space) || ButtonControllor.isJumpButton)
        {
            myAnim.SetBool("IsDie1", false);
            myAnim.SetBool("IsWalk", false);
            myAnim.SetBool("IsJump", false);
            myAnim.SetBool("IsChaging", true);
        }
        else
        {

            if (!isJump)
            {
                myAnim.SetBool("IsDie1", false);
                myAnim.SetBool("IsWalk", false);
                myAnim.SetBool("IsChaging", false);
                myAnim.SetBool("IsJump", false);
            }
        }
        if (isJump)
        {
            myAnim.SetBool("IsDie1", false);
            myAnim.SetBool("IsWalk", false);
            myAnim.SetBool("IsChaging", false);
            myAnim.SetBool("IsJump", true);
        }
    }

    private void PlayerJump()
    {
        if (Input.GetKey(KeyCode.Space) || ButtonControllor.isJumpButton)
        {
            if (am.isJump)
                am.PlayAudio(chaging, 0, 0.1f);
        }
        if (!isJump)
        {
            if (Input.GetKey(KeyCode.Space) || ButtonControllor.isJumpButton)
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
        myR.velocity = Vector2.zero;
        myR.AddForce(Vector2.up * chargeJumpPower * 120);
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
            if (ButtonControllor.isLeftButton)
                _bXMove = -1;
            if (ButtonControllor.isRightButton)
                _bXMove = 1;
            myR.velocity = new Vector2(_bXMove * moveSpeed, myR.velocity.y);
            if (_bXMove != 0 && !isJump && !isJumpChaging)
            {
                am.PlayAudio(walk, 0.15f, 0.3f);
            }
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || ButtonControllor.isLeftButton)
        {
            mySR.flipX = true;
            Invoke("SweatPlay", 0.5f);
            sweat.transform.localPosition = new Vector3(20, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || ButtonControllor.isRightButton)
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
        if (!GameManager.isPlay)
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "notWall")
        {
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
            GameManager.isPlay = false;
            myAnim.SetBool("IsWalk", false);
            myAnim.SetBool("IsChaging", false);
            myAnim.SetBool("IsJump", false);
            myAnim.SetBool("IsDie1", true);
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
            isJump = false;
        Physics2D.gravity = new Vector2(0, -9.81f);

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isJump = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        string exitBlocks = "ExitBlock";

        if (collision.gameObject.tag == "Gem")
        {
            am.PlayAudio(gem, 0, 0.1f);
            GameManager.gem++;
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
            GameManager.isGameClear = true;

    }

}

