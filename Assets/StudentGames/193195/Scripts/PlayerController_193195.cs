using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Movement parameters")]
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    [Space(10)]
    [Range(0.01f, 20.0f)][SerializeField] private float jumpForce = 6.0f;
    private Rigidbody2D rigidBody;
    public LayerMask groundLayer;
    private const float rayLength = 1.5f;
    [SerializeField] private Animator animator;
    private bool isWalking = false;
    private bool isFacingRight = true;
    private bool isClimbing = false;
    private bool isLadder = false;
    private float vertical = 0.0f;
    Vector2 startPosition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip keySound;
    public AudioClip enemySound;
    [SerializeField] private AudioClip exitSound;
    [SerializeField] private AudioClip liveSound;
    [SerializeField] private AudioClip crankSound;
    [SerializeField] private AudioClip shootSound;
    public AudioSource source;
    GameObject bullet;

    public static PlayerController instance;



    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }
        if (other.CompareTag("Ladder"))
        {
            Debug.Log("Opuszczono drabine");
            isLadder = false;
            isClimbing = false;
        }
    }

    public void Shoot()
    {
        if (shootSound != null)
        {
            source.PlayOneShot(liveSound, AudioListener.volume);
        }
        bullet = Instantiate(bulletPrefab,transform.position,Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (isFacingRight)
        {
            bulletRb.AddForce(transform.right * 20.0f,ForceMode2D.Impulse);
        } else
        {
            bulletRb.AddForce(-1 * transform.right * 20.0f,ForceMode2D.Impulse);
        }

        Destroy(bullet, 4.0f);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bonus"))
        {
            source.PlayOneShot(coinSound, AudioListener.volume);
            GameManager.instance.AddPoints(10);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("EXIT"))
        {
            if(GameManager.instance.keysFound == GameManager.keysNumber)
            {
                source.PlayOneShot(exitSound, AudioListener.volume);
                GameManager.instance.AddPoints(GameManager.instance.lives * 100);
                GameManager.instance.LevelCompleted();

            }
                //GameFreeze(3.0f);
        }
        else if (other.CompareTag("crank"))
        {
            source.PlayOneShot(crankSound, AudioListener.volume);
            FindObjectOfType<GeneratedPlatforms>().swthc(other);

        }
        else if (other.CompareTag("Key"))
        {
            source.PlayOneShot(keySound, AudioListener.volume);
            if (other.name == "klucz1")
            {
                GameManager.instance.addKeys(0);
            }
            else if (other.name == "klucz2")
            {
                GameManager.instance.addKeys(1);
            }
            else if (other.name == "klucz3")
            {
                GameManager.instance.addKeys(2);
            }
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Heart"))
        {
            source.PlayOneShot(liveSound, AudioListener.volume);
            GameManager.instance.changeLives(1);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("FallLevel"))
        {
            Death();
        }
        else if(other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(other.transform);
        }
        else if (other.CompareTag("Ladder"))
        {
            Debug.Log("Wejscie na drabine");

            isLadder = true;
        }
    }

    private bool isGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    private void Flip()
    {
        Vector3 theScale;
        theScale = transform.localScale;
        isFacingRight = !isFacingRight;
        theScale.x = theScale.x * -1;
        transform.localScale = theScale;
    }



    private void Jump()
    {
        if (isGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = this.transform.position;
    }
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (isClimbing)
        {
            rigidBody.gravityScale = 0;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, vertical * moveSpeed);
        }
        else
        {
            rigidBody.gravityScale = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        isWalking = false;
        vertical = Input.GetAxis("Vertical");
        if (isLadder && vertical > 0.0f)
        {
            isClimbing = true;
        }


        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {


            if (Input.GetKey(KeyCode.RightArrow) )
            {
                transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                if (isGrounded()) isWalking = true;
                if (!isFacingRight)
                {
                    Flip();
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(-(moveSpeed * Time.deltaTime), 0.0f, 0.0f, Space.World);
                if (isGrounded()) isWalking = true;
                if (isFacingRight)
                {
                    Flip();
                }

            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shoot();
            }

            animator.SetBool("isJumping", !isGrounded());
            animator.SetBool("isGrounded", isGrounded());
            animator.SetBool("isWalking", isWalking);


        }
    }
    public void Death()
    {
        GameManager.instance.changeLives(-1);
        if (GameManager.instance.lives==0)
        {
            GameManager.instance.GameOver();
        }
        else
        {
            this.rigidBody.velocity = Vector3.zero;
            this.transform.position = startPosition;            
        }
    }
}

