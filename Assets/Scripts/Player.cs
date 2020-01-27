using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float runSpeed;
    public float wallJumpForce;
    Rigidbody2D rb;
    private SpriteRenderer spr;
    private Animator anm;
    private bool Grounded;
    private bool WallSlide;
    public float playerX;
    public float wallX;
    public float wallFall;
    public float wallSlideVel;
    private GameObject Wall;
    public GameObject Camera;
    public GameObject hook;
    public static bool Forward;

    // Start is called before the first frame update
    void Start()
    {
        Camera.transform.position = this.transform.position;
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anm = GetComponent<Animator>();
        Grounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && WallSlide == false)
        {
            Instantiate(hook, this.transform.position, Quaternion.identity, this.gameObject.transform);
        }

        Camera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        playerX = transform.position.x;
        float velX = rb.velocity.x;
        float spdX = Mathf.Abs(velX);

        if (velX > 0)
        {
            spr.flipX = false;
            Forward = true;
        }
        if (velX < 0)
        {
            spr.flipX = true;
            Forward = false;
        }

        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && Grounded)
            anm.Play("Walk");
        if ((Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.A) == false) && Grounded)
            anm.Play("Stand");

            if (WallSlide == false && Grounded)
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * runSpeed, rb.velocity.y);

            if (Input.GetKey(KeyCode.W))
            {
                anm.Play("Jump");
                rb.velocity = new Vector2(rb.velocity.x, 10);
                Grounded = false;
            }
        }

        if (Grounded)
            WallSlide = false;

        if (WallSlide)
        {
            //Inputs for cancelling a wallslide based on wall's position relative to player
            anm.Play("Wallslide");

            if (wallX > playerX && Input.GetKeyDown(KeyCode.A))
            {
                WallSlide = false;
                transform.position = new Vector2(this.transform.position.x - 0.15f, this.transform.position.y);
            }
            if (wallX < playerX && Input.GetKeyDown(KeyCode.D))
            {
                WallSlide = false;
                transform.position = new Vector2(this.transform.position.x + 0.15f, this.transform.position.y);
            }

            if (Input.GetKeyDown(KeyCode.W) && wallX < playerX)
            {
                WallSlide = false;
                Grounded = false;
                transform.position = new Vector2(transform.position.x + 0.15f, transform.position.y + 0.15f);
                rb.velocity = new Vector2(0.75f, 0.5f) * wallJumpForce;
            }
            if (Input.GetKeyDown(KeyCode.W) && wallX > playerX)
            {
                WallSlide = false;
                Grounded = false;
                transform.position = new Vector2(transform.position.x - 0.15f, transform.position.y + 0.15f);
                rb.velocity = new Vector2(-0.75f, 0.5f) * wallJumpForce;
            }
            if (Input.GetKey(KeyCode.S))
                rb.velocity = new Vector2(rb.velocity.x, wallFall);
            else if (Input.GetKey(KeyCode.S) == false && WallSlide)
                rb.velocity = new Vector2(rb.velocity.x, wallSlideVel);
            if (wallX > playerX)
                spr.flipX = false;
            else if (wallX < playerX)
                spr.flipX = true;
        }

        if (WallSlide == false && Grounded == false)
        {
            rb.velocity = new Vector2(rb.velocity.x + (Input.GetAxis("Horizontal") * 0.25f), rb.velocity.y - 0.05f);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            Grounded = true;
            WallSlide = false;
        }

        if (col.gameObject.tag == "Wall" && Grounded == false)
        {
            WallSlide = true;
            Wall = col.gameObject;
            wallX = Wall.transform.position.x;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
            Grounded = true;
        if (col.gameObject.tag == "Wall")
            WallSlide = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Grounded = false;
        WallSlide = false;
        anm.Play("Fall");
    }
}
