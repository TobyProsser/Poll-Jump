using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConttroller : MonoBehaviour
{
    public float camSpeed;
    public Vector3 offset;
    Transform player;

    Vector3 originPos = new Vector3(-15, 10, 0);
    Vector3 leftPos = new Vector3(-15, 10, 5.5f);
    Vector3 rightPos = new Vector3(-15, 10, -5.5f);

    Vector3 moveToPos;

    PlayerMovementController playerMovement;

    int currentLane;

    void Start()
    {
        player = this.transform.parent;

        playerMovement = player.GetComponent<PlayerMovementController>();

        moveToPos = originPos;
    }

    void Update()
    {
        transform.LookAt(player.position + offset);

        int curLane = playerMovement.curLane;
        if(curLane != currentLane)
        {
            if (curLane == 0)
            {
                moveToPos = originPos;
                currentLane = 0;
            }
            else if (curLane == 1)
            {
                moveToPos = rightPos;
                currentLane = 1;
            }
            else if (curLane == -1)
            {
                moveToPos = leftPos;
                currentLane = -1;
            }
        }

        if(Mathf.Abs(Vector3.Distance(moveToPos, transform.localPosition)) > .1f) transform.localPosition += (moveToPos - transform.localPosition).normalized * camSpeed * Time.deltaTime;
    }
}
