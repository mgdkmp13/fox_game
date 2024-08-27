using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    public float moveRange = 1.0f;
    private bool isMovingRight = false;
    private float startPositionX;


    void Awake()
    {
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
                isMovingRight = !isMovingRight;
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
                isMovingRight = !isMovingRight;
                moveRight();
            }
        }
    }

    void moveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    void moveLeft()
    {
        transform.Translate(-(moveSpeed * Time.deltaTime), 0.0f, 0.0f, Space.World);
    }


}
