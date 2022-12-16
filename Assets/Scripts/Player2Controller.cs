using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    private playerState curstate = playerState.idle;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (collision != null)
        {
            playerState other = player.getCurState();
            Debug.Log(other);
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
}
