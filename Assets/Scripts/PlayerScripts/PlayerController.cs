using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject poll;
    public GameObject camera;

    public bool finished;
    bool scored;
    int curScore = -1;

    PlayerMovementController playerMovement;

    public ParticleSystem[] sparkParts = new ParticleSystem[2];
    public bool jumped;

    public List<GameObject> playerParts = new List<GameObject>();

    bool died;

    public CanvasController canvasController;

    public ColorWave curColorWave;

    void Awake()
    {
        playerMovement = this.GetComponent<PlayerMovementController>();
        finished = false;
        scored = false;
        curScore = -1;
    }

    void FixedUpdate()
    {
        if (this.transform.position.y < .2f && !died && !finished) OnDeath();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Extension")
        {
            if (poll.transform.localScale.y < 7)
            {
                if(playerMovement.decreasing) poll.transform.localScale = new Vector3(poll.transform.localScale.x, 2, poll.transform.localScale.z);
                else poll.transform.localScale += new Vector3(0, 2f, 0);
                playerMovement.collectedExtension = true;
            }
            
            Destroy(other.gameObject);
        }

        if (other.tag == "FinishLine")
        {
            finished = true;
            playerMovement.FinishBoost();
            PlaySparks();
        }

        if (finished)
        {
            try
            {
                string tempString = other.tag;
                curScore = Int32.Parse(tempString);
            }
            catch (Exception e)
            {
                print("Couldnt turn tag to num");
            }
        }

        if (other.tag == "Bridge")
        {
            print("Play sound");
            AudioManager.instance.Play("RailSound");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Bridge") AudioManager.instance.Stop("RailSound");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (jumped)
        {
            PlaySparks();
            jumped = false;
        }

        if (this.transform.position.y < -.2f && !died && !finished) OnDeath();
        else if (this.transform.position.y < -.2f && !died && finished) playerMovement.canMove = false;

        if (collision.transform.tag == "Cone" && !died)
        {
            OnDeath();
        }
    }

    public void PlaySparks()
    {
        AudioManager.instance.Play("RailSparks");
        for (int i = 0; i < sparkParts.Length; i++)
        {
            sparkParts[i].Play();
        }
    }

    void OnDeath()
    {
        AudioManager.instance.Play("Break");
        died = true;
        print("dead");

        camera.transform.parent = null;
        camera.GetComponent<CameraConttroller>().camSpeed = 2;
        this.GetComponent<Rigidbody>().useGravity = false;
        playerMovement.canMove = false;

        for (int i = 0; i < playerParts.Count; i++)
        {
            GameObject curPart = playerParts[i];
            curPart.AddComponent<Rigidbody>();
            curPart.AddComponent<BoxCollider>();

            curPart.GetComponent<Rigidbody>().AddExplosionForce(10, curPart.transform.position, 6, 4);

            Destroy(curPart, 3);
        }

        CanvasController.timesPlayed++;
        canvasController.OpenBasePanel();
    }

    public void CheckScore()
    {
        AudioManager.instance.Play("BeatLevel");
        CanvasController.timesPlayed++;

        SavedData.savedData.coins += curScore;
        SavedData.savedData.level++;
        canvasController.OpenBasePanel();
    }
}
