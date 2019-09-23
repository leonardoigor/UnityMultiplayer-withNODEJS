using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
  public InputField inputField;
  public GameObject Canvas;
  public Controller controller;
  public PlayerController ControlePlayer;
  public GameObject playerCurrent;
  public string Nome, ID;
  public float CurrentLife;
  public float PosLife;

  public void statGame()
  {
    if (inputField.text != "")
    {
      controller.EmitPlayer(inputField.text);
      Nome = inputField.text;

      StartCoroutine("FindPlayer");
    }

  }
  IEnumerator FindPlayer()
  {
    yield return new WaitForSeconds(1);
    GameObject P = GameObject.Find(Nome) as GameObject;
    playerCurrent = P;
    ControlePlayer = playerCurrent.GetComponent<PlayerController>();
    SetCurrentAtributs(ControlePlayer.ID);
    Canvas.SetActive(false);
    CurrentLife = ControlePlayer.Life;
  }

  public void SetCurrentAtributs(string id)
  {
    ID = id;
    playerCurrent.name = id;
  }
  void Update()
  {
    if (playerCurrent != null)
    {
      ControlePlayer.MOVE();
      EmitLife();

    }
  }
  public void EmitLife()
  {
    if (CurrentLife != PosLife)
    {
      controller.EmitLifeChange(CurrentLife, ID);
      PosLife = CurrentLife;
    }
  }
}
