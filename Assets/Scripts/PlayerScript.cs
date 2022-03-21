using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public float jumpForce;
    public float jumpForce2;
    public float checkRadius;
    public float checkRadius2;
    public Text score;
    public Text lives;
    public Text endMess;
    public Text introText;
    private int scoreValue;
    private int livesValue;
    private bool facingRight = true;
    private bool isOnGround;
    private bool isOnSide;
    private bool hasWon = false;
    private bool canMove;
    public Transform groundCheck;
    public Transform sideCheck;
    public LayerMask allGround;
    public AudioClip music;
    public AudioClip sound;
    public AudioSource musicSource;
    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        scoreValue = 0;
        livesValue = 3;
        canMove = true;
        SetCountText();
        musicSource.clip = music;
        musicSource.Play();
        Invoke("DisableText", 10f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        if(canMove == true)
        { 
            rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
            isOnGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, allGround);
            isOnSide = Physics2D.OverlapCircle(sideCheck.position, checkRadius2, allGround);

        }
        
        if(facingRight == false && hozMovement > 0 && canMove)
        {
            Flip();
        }

        else if(facingRight == true && hozMovement < 0 && canMove)
        {
            Flip();
        }

        if(rd2d.velocity.magnitude > 0 && isOnGround == true && canMove)
        {
            anim.SetInteger("State", 1);
        }
        
        else if(rd2d.velocity.magnitude <= 0 && isOnGround == true && canMove)
        {
            anim.SetInteger("State", 0);
        }

        else if(vertMovement > 0 && isOnGround == false && canMove)
        {
            anim.SetInteger("State", 2);
        }

        if(hozMovement > 0 && isOnGround == true)
        {
            Debug.Log ("Facing Right");
        }

        if(hozMovement < 0 && isOnGround == true)
        {
            Debug.Log ("Facing Left");
        }

        
        if(vertMovement > 0 && isOnGround == false)
        {
            Debug.Log ("Jumping");
        }

        if(vertMovement == 0 && isOnGround == true)
        {
            Debug.Log ("Not Jumping");
        }

        if(Input.GetKey("escape")) 
        {
            Application.Quit();
        }
    }

    void DisableText()
    { 
      introText.enabled = false; 
    }  

    void SetCountText()
    {
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            Destroy(collision.collider.gameObject);
            scoreValue++;
            SetCountText();

            if(scoreValue == 4)
            {
                transform.position = new Vector2(200.0f, 0.0f);
                livesValue = 3;
                SetCountText();
            }
        }

        else if(collision.collider.tag == "Enemy")
        {
            Destroy(collision.collider.gameObject);
            livesValue--;
            SetCountText();
        }

        if(scoreValue == 8 && hasWon == false)
        {
            hasWon = true;
            endMess.text = "\n\n\n\n\n\n\n\n\nYou Won!\nGame by John Collera";
            musicSource.clip = sound;
            musicSource.Play();
            musicSource.loop = false;
            canMove = false;
        }

        if(livesValue <= 0)
        {
            endMess.text = "\n\n\n\n\n\n\n\n\nYou Lost!\nGame by John Collera";
            rd2d.gameObject.SetActive(false);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && isOnGround && canMove)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
        if(collision.collider.tag == "Ground" && isOnSide && canMove)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce2), ForceMode2D.Impulse); 
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}