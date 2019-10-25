using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    Animator anim;
    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    public Text winText;
    public Text livesText;
    private int scoreValue;
    private int lives;
    private bool facingRight = true;

    public AudioSource musicSource;
    public AudioClip bgm;
    public AudioClip winMusic;
    public AudioClip loseMusic;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        scoreValue = 0;
        lives = 3;
        winText.text = "";
        SetScore();
        SetLivesText();
        musicSource.clip = bgm;
        musicSource.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        //flip player when switch moving direction
        float moveHorizontal = moveHorizontal = Input.GetAxis("Horizontal");
        if (facingRight == false && moveHorizontal > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveHorizontal < 0)
        {
            Flip();
        }
        //close game when escape key pressed
        if (Input.GetKey("escape"))
            Application.Quit();
    }
    //player gets 1 point when collide with "coin"
    //player lose 1 live when collide with "enemy"
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            Destroy(collision.collider.gameObject);
            scoreValue += 1;
            SetScore();
        }
        else if (collision.collider.tag == "Enemy")
        {
            Destroy(collision.collider.gameObject);
            lives--;
            SetLivesText();
        }
    }
    //player jumps when pressing w key
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            anim.SetInteger("State", 0);
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                anim.SetInteger("State", 2);//play jump animation
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                anim.SetInteger("State", 1);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                anim.SetInteger("State", 1);
            }
        }
    }
    //update game sccore and transform to level 2
    //check for win game and play win music
    void SetScore()
    {
        score.text = "Score: " + scoreValue.ToString();
        if (scoreValue == 4)
        {
            transform.position = new Vector2(70.0f,0.0f); //moving to level 2
            lives = 3;
            SetLivesText();
        }
        //display win msg and play win music
        if (scoreValue >= 8)
        {
            winText.text = "You Win!! Game created by Yi Chen";
            musicSource.clip = winMusic;
            musicSource.Play();
            Destroy(this);
        }
    }
    //update player lives and check for end game and play lose game music
    void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives == 0)
        {
            winText.text = "You lose...";
            musicSource.clip = loseMusic;
            musicSource.Play();
            Destroy(this);
        }
    }
    //function flips player object
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}