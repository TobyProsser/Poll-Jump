using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = false;

    bool isSwiping;
    bool finger;

    public float SWIPE_THRESHOLD = 45f;
    public float holderThreshold = .05f;
    float timer;

    PlayerMovementController playerMovement;

    private void Awake()
    {
        playerMovement = this.GetComponent<PlayerMovementController>();
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount > 1)
            {
                Touch touch1 = Input.GetTouch(1);

                if (touch1.phase == TouchPhase.Began)
                {
                    finger = true;
                }

                if (finger)
                {
                    playerMovement.StartJump();
                    finger = false;
                }
            }
            else
            {

                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    fingerUp = touch.position;
                    fingerDown = touch.position;

                    isSwiping = false;
                    timer = 0;
                    finger = true;
                }

                //Detects Swipe while finger is still moving
                if (touch.phase == TouchPhase.Moved)
                {
                    if (!detectSwipeOnlyAfterRelease)
                    {
                        fingerDown = touch.position;
                        checkSwipe();
                    }
                }
                else if (touch.phase == TouchPhase.Stationary && !isSwiping)
                {
                    timer += Time.deltaTime;

                    if (timer >= holderThreshold && finger)
                    {
                        if (!isSwiping) playerMovement.StartJump();

                        finger = false;
                    }
                }

                //Detects swipe after finger is released
                if (touch.phase == TouchPhase.Ended)
                {
                    fingerDown = touch.position;
                    //checkSwipe();

                    finger = false;
                    timer = 0;
                }
            }
        }
    }

    void checkSwipe()
    {
        if (!isSwiping)
        {
            //Check if Horizontal swipe
            if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
            {
                print("Swipe");
                isSwiping = true;
                //Debug.Log("Horizontal");
                if (fingerDown.x - fingerUp.x > 0)//Right swipe
                {
                    OnSwipeRight();
                }
                else if (fingerDown.x - fingerUp.x < 0)//Left swipe
                {
                    OnSwipeLeft();
                }
                fingerUp = fingerDown;
            }
            //No Movement at-all
            else
            {
                //Debug.Log("No Swipe!");
            }
        }
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////
    void OnSwipeUp()
    {
        //Debug.Log("Swipe UP");
    }

    void OnSwipeDown()
    {
        //Debug.Log("Swipe Down");
    }

    void OnSwipeLeft()
    {
        playerMovement.MoveLeft();
    }

    void OnSwipeRight()
    {
        playerMovement.MoveRight();
    }
}
