using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    private playerState curstate = playerState.idle;
    public float speed = 1f;
    Vector3 left = new Vector3(0.3f, -0.3f, 0);
    private Animator animator;
    private int count = 0;
    private int delay = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("state", 0);
        if (curstate != playerState.idle)
        {
            if (count < delay)
            {
                ++count;
            }
            else
            {
                curstate = playerState.idle;
                count = 0;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.L) && this.transform.position == left)
            {
                curstate = playerState.move;
                this.transform.position += new Vector3(speed, 0f, 0f);
                delay = 100;
            }
            else if (Input.GetKeyDown(KeyCode.J) && this.transform.position != left)
            {
                curstate = playerState.move;
                this.transform.position += new Vector3(-speed, 0f, 0f);
                delay = 100;
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                curstate = playerState.kick;
                animator.SetInteger("state", 1);
                delay = 300;
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                curstate = playerState.ukick;
                animator.SetInteger("state", 2);
                delay = 250;
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                curstate = playerState.defence;
                animator.SetInteger("state", 3);
                delay = 250;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController other = collision.GetComponent<PlayerController>();
        playerState otherState = other.getCurState();
        Debug.Log(otherState);
        if (curstate == playerState.kick || curstate == playerState.ukick)
        {
            if (otherState == playerState.kick || otherState == playerState.ukick)
            {
                curstate = playerState.hurt;
                animator.SetInteger("state", 4);
                delay = 400;
            }

        }
        else if (curstate == playerState.defence)
        {
        }
        else
        {
            curstate = playerState.hurt;
            animator.SetInteger("state", 4);
            delay = 400;
        }
    }

    public playerState getCurState()
    {
        return curstate;
    }

    public void setCurState(playerState newState)
    {
        curstate = newState;
    }

    //private playerState curstate = playerState.idle;
    //public float speed = 0.1f;
    //Vector3 right = new Vector3(-0.3f, -0.3f, 0);
    //private Animator animator;
    //private int count = 0;
    //private int delay = 0;
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Player2Controller other = collision.GetComponent<Player2Controller>();
    //    playerState otherState = other.getCurState();
    //    Debug.Log(otherState);
    //    if (curstate == playerState.kick || curstate == playerState.ukick)
    //    {
    //        if (otherState == playerState.kick || otherState == playerState.ukick)
    //        {
    //            curstate = playerState.hurt;
    //            animator.SetInteger("state", 3);
    //            delay = 400;
    //        }

    //    }
    //    else if (curstate == playerState.defence)
    //    {
    //    }
    //    else
    //    {
    //        curstate = playerState.hurt;
    //        animator.SetInteger("state", 3);
    //        delay = 400;
    //    }
    //}

    //public playerState getCurState()
    //{
    //    return curstate;
    //}

    //public void setCurState(playerState newState)
    //{
    //    curstate = newState;
    //}
}
