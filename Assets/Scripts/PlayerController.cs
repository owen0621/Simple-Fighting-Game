using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum playerState {
  idle = 0,
  move = 1,
  kick = 2,
  ukick = 3,
  defence = 4,
  hurt = 5
}

public class PlayerController : MonoBehaviour {
  public GameManager gameManager;
  public int health;
  public Image healthImg;
  public int playerId;
  private playerState curstate = playerState.idle;
  public float speed = 1f;
  public Vector3 attackPos = new Vector3(-0.3f, -0.3f, 0);
  public Vector3 forward = new Vector3(1f, 0f, 0f);
  
  [Header("KeyBinds")]
  public KeyCode moveRight = KeyCode.D;
  public KeyCode moveLeft = KeyCode.A;
  public KeyCode downKick = KeyCode.F;
  public KeyCode upKick = KeyCode.G;
  public KeyCode defense = KeyCode.R;

  public int distanceState;
  private BodySourceView _bsv;

  private Animator animator;
  public float delay;
  
  // Start is called before the first frame update
  void Start() {
    health = 100;
    delay = 0;
    animator = gameObject.GetComponent<Animator>();
    _bsv = GameObject.Find("GameManager").GetComponent<BodySourceView>();
  }

  // Update is called once per frame
  void Update() {
    SetHealth();
    distanceState = (playerId == 1) ? _bsv.user1 : _bsv.user2;

    if (delay >= 0) {
      delay -= Time.deltaTime;
    }
    else {
      if (curstate != playerState.idle) {
        if (delay >= 0) {
          delay -= Time.deltaTime;
        }
        else {
          curstate = playerState.idle;
          animator.SetInteger("state", 0);
        }
      }
      else {
        if (distanceState == 2 && this.transform.position != attackPos) {
          curstate = playerState.move;
          this.transform.position += forward * speed;
        }
        else if (distanceState == 3 && this.transform.position == attackPos) {
          curstate = playerState.move;
          this.transform.position += -forward * speed;
        }
        else if (Input.GetKeyDown(downKick)) {
          curstate = playerState.kick;
          animator.SetInteger("state", 1);
          delay = 0.4f;
        }
        else if (Input.GetKeyDown(upKick)) {
          curstate = playerState.ukick;
          animator.SetInteger("state", 2);
          delay = 0.3f;
        }
        else if (Input.GetKey(defense)) {
          curstate = playerState.defence;
          animator.SetInteger("state", 3);
          delay = 0.4f;
        }
        else if (Input.GetKeyDown(KeyCode.T)) {
          curstate = playerState.hurt;
          animator.SetInteger("state", 4);
        }
      }
    }
  }

  public void GetHurt(float delayPre) {
    curstate = playerState.hurt;
    animator.SetTrigger("Hurt-Trigger");
    delay += delayPre;
    health -= 10;
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.gameObject.tag == "Player") {
      PlayerController other = collision.GetComponent<PlayerController>();
      playerState otherState = other.getCurState();
      if (curstate == playerState.kick || curstate == playerState.ukick) {
        if (otherState != playerState.defence) {
          other.GetHurt(0.4f);
        }
        else {
          delay = 1f;
          curstate = playerState.idle;
          animator.SetInteger("state", 0);
        }
      }
      else if (curstate == playerState.defence){}
      else {
        // other.GetHurt(0.4f);
      }

      // if (curstate == playerState.defence) {
      //   if (otherState == playerState.kick || otherState == playerState.ukick) {
      //     other.delay = 10000f;
      //   }
      // }
      // else if (curstate == playerState.kick || curstate == playerState.ukick) {
      //   if (otherState != playerState.defence) {
      //     Debug.Log(otherState);
      //     other.GetHurt(0.4f);
      //   }
      // }

      // Debug.Log(other.getCurState());
    }
  }

  public playerState getCurState() {
    return curstate;
  }

  public void setCurState(playerState newState) {
    curstate = newState;
  }


  private void SetHealth() {
    if (health <= 0) {
      GameObject.Find("GameManager").GetComponent<GameManager>().GameOver(playerId);
    }
    healthImg.fillAmount = (float)health / 100f;
  }
}
