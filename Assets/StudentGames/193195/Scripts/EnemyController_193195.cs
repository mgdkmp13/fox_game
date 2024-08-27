using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static Unity.VisualScripting.Member;

public class EnemyContoller : MonoBehaviour
{
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private Animator animator;
    private float startPositionX;
    public float moveRange = 1.0f;
    private bool isMovingRight = false;
    private bool isFacingRight = false;
    public bool isDead = false;


    void Awake()
    {
        //rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPositionX = this.transform.position.x;      
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingRight)
        {
            if (this.transform.position.x < startPositionX + moveRange)
            {
                moveRight();
            }
            else
            {
                Flip();
                moveLeft();
            }
        }
        else
        {
            if (this.transform.position.x > startPositionX - moveRange)
            {
                moveLeft();
            }
            else
            {
                Flip();
                moveRight();
            }
        }
    }

    private void Flip()
    {
        Vector3 theScale;
        theScale = transform.localScale;
        isFacingRight = !isFacingRight;
        isMovingRight = !isMovingRight;
        theScale.x = theScale.x * -1;
        transform.localScale = theScale;
    }

    void moveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        if (!isFacingRight)
        {
            Flip();
        }
    }

    void moveLeft()
    {
        transform.Translate(-(moveSpeed * Time.deltaTime), 0.0f, 0.0f, Space.World);
        if (isFacingRight)
        {
            Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //zapobiega ponownej kolizji z obiektem,
        //jeœli ten nie zd¹zy³ jeszcze zniknac z sceny
        if (isDead) return;
      if (other.CompareTag("Player"))
        {
            if (other.gameObject.transform.position.y > this.transform.position.y || other.CompareTag("Bullet"))
            {
                KillEnemy();
            }
            else
            {
                PlayerController.instance.Death();
            }

        }
      if (other.CompareTag("Bullet"))
        {
            KillEnemy();
        }
    }
    private void KillEnemy()
    {
        isDead = true;
        GameManager.instance.addKill();
        var eSound = PlayerController.instance.enemySound;
        PlayerController.instance.source.PlayOneShot(eSound, AudioListener.volume);
        animator.SetBool("isDead", true);
        StartCoroutine(FadeAndDestroy(gameObject));
    }


    private IEnumerator FadeAndDestroy(GameObject obj)
    {
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();

        Color startColor = renderer.color;

        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        float elapsedTime = 0f;
        float fadeDuration = 1.0f;

        while (elapsedTime < fadeDuration)
        {
            renderer.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);

            elapsedTime += Time.deltaTime;

            // pozwala na plynne dzialanie gry
            //nie zawiesza sie na petli while
            yield return null;
        }

        obj.SetActive(false);
        Destroy(obj);
    }
}
