using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float walkSpeed;
    public float pollJumpVelocity = 3;
    public float addedJumpForce = 3;
    float vel;

    Rigidbody rb;

    public GameObject poll;
    float pollRot;
    public float rotateSpeed;

    bool turning;

    public int curLane = 0;

    CapsuleCollider pollCollider;

    public bool collectedExtension;

    PlayerController playerController;

    public bool canMove;
    public float boost = 0;

    bool done = false;

    [HideInInspector]
    public bool decreasing;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        pollCollider = poll.GetComponent<CapsuleCollider>();
        playerController = this.GetComponent<PlayerController>();

        canMove = false;
        pollCollider.enabled = false;
        rb.useGravity = false;
    }

    public void StartPlaying()
    {
        canMove = true;
        done = false;
        rb.useGravity = true;
        StartCoroutine(IncreaseSpeed());
    }

    void Update()
    {
        if (poll != null && canMove)
        {
            pollRot = poll.transform.rotation.eulerAngles.z;

            if (Input.GetMouseButton(0) && turning)
            {
                if (pollRot > 330 && turning || pollRot < 91 && turning)
                {
                    poll.transform.Rotate(new Vector3(0, 0, -rotateSpeed * Time.deltaTime));

                    vel += Time.deltaTime * 50/walkSpeed;
                }
                else
                {
                    turning = false;
                    StartCoroutine(ReturnPoll());

                    pollCollider.enabled = false;
                }
            }
            else if (Input.GetMouseButtonUp(0) && turning)
            {
                pollCollider.enabled = false;

                turning = false;
                StartCoroutine(ReturnPoll());

                playerController.jumped = true;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                MoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                MoveRight();
            }
        }
        else if (!canMove) rb.velocity = Vector3.zero;

        if (playerController.finished)
        {
            if (rb.velocity == Vector3.zero && !done)
            {
                done = true;
                playerController.CheckScore();
            }
        }
    }

    void FixedUpdate()
    {
        if(canMove) rb.velocity = new Vector3(walkSpeed + vel + boost, rb.velocity.y, rb.velocity.z);
    }

    IEnumerator IncreaseSpeed()
    {
        while (walkSpeed < 10)
        {
            yield return new WaitForSeconds(.5f);
            walkSpeed += .06f;

            if (walkSpeed >= 10) break;
        }
    }

    public void FinishBoost()
    {
        StartCoroutine(IncreaseBoost(2));
    }

    IEnumerator IncreaseBoost(float time)
    {
        for (float t = 0.0f; t < 2.0f; t += Time.deltaTime / time)
        {
            boost = Mathf.Lerp(0, 5, t);
            yield return null;
        }
    }

    IEnumerator DecreaseAddedVel()
    {
        while (vel > 0)
        {
            vel -= Time.deltaTime * 12f;

            if (vel <= 0) break;

            yield return null;
        }

        vel = 0;
    }

    IEnumerator ReturnPoll()
    {
        decreasing = true;
        collectedExtension = false;
        StartCoroutine(DecreaseAddedVel());

        pollCollider.enabled = false;

        while (true)
        {
            poll.transform.rotation = Quaternion.RotateTowards(poll.transform.rotation, Quaternion.Euler(0, 0, -90), rotateSpeed * Time.deltaTime);

            if (poll.transform.localScale.y > 1 && !collectedExtension) poll.transform.localScale -= new Vector3(0, 15 * Time.deltaTime, 0);
            else if (poll.transform.rotation == Quaternion.Euler(0, 0, -90)) break;

            yield return null;
        }

        poll.transform.rotation = Quaternion.Euler(0, 0, 90);
        pollCollider.enabled = true;
        collectedExtension = false;

        decreasing = false;

        if (poll.transform.localScale.y < 1) poll.transform.localScale = new Vector3(poll.transform.localScale.x, 1, poll.transform.localScale.z);
    }

    public void MoveLeft()
    {
        if (curLane != -1)
        {
            this.transform.position += new Vector3(.5f, 0, 2.5f);
            curLane--;

            playerController.PlaySparks();
        }
    }

    public void MoveRight()
    {
        if (curLane != 1)
        {
            this.transform.position += new Vector3(.5f, 0, -2.5f);
            curLane++;

            playerController.PlaySparks();
        }
    }

    public void StartJump()
    {
        vel = 0;
        turning = true;

        if(poll != null) pollCollider.enabled = true;

        AudioManager.instance.Play("Jump");
    }
}
