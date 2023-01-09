using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  public int attacker {get; set;}

  public GameObject gameOver;
  public GameObject player1Win;
  public GameObject player2Win;

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
        
  }

  public void GameOver(int loser) {
    gameOver.SetActive(true);

    if (loser == 1) {
      player2Win.SetActive(true);
    }
    else {
      player1Win.SetActive(true);
    }

    Time.timeScale = 0;
  }

  public void Restart() {
    Time.timeScale = 1;
    SceneManager.LoadScene(0);
  }
}
