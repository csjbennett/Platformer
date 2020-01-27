using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookThrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anm;
    private SpriteRenderer spr;
    private bool fading;
    private float fadeNum;
    // Start is called before the first frame update
    private void Start()
    {
        fadeNum = 1.5f;
        fading = false;
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        if (Player.Forward == true)
        {
            rb.velocity = new Vector2(15, 5);
        }
        if (Player.Forward == false)
        {
            rb.velocity = new Vector2(-15, 5);
            anm.Play("ThrowLeft");
        }
    }

    private void Update()
    {
        float velX = rb.velocity.x;
        if (velX > 0)
            anm.Play("ThrowRight");
        if (velX < 0)
            anm.Play("ThrowLeft");
        if (fading)
        {
            fadeNum = fadeNum - 0.05f;
            spr.color = new Color(255, 255, 255, fadeNum);
            if (fadeNum < 0)
                Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject)
        {
            rb.velocity = Vector2.zero;
            fading = true;
            anm.Play("Stop");
            Debug.Log("fading");
        }
    }
}
