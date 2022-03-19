using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public float jumpForce;

    public Text score;

    private int scoreValue = 0;
    
    private bool facingRight = true;

    private bool isOnGround;

    public Transform groundcheck;

    public float checkRadius;

    public LayerMask allGround;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if(facingRight == false && hozMovement > 0)
        {
            Flip();
        }

        else if(facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if(hozMovement > 0 && facingRight == true)
        {
            Debug.Log ("Facing Right");
        }

        if(hozMovement < 0 && facingRight == false)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && isOnGround)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); //the 3 in this line of code is the player's "jumpforce," and you change that number to get different jump behaviors.  You can also create a public variable for it and then edit it in the inspector.
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1
    }
}