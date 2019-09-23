using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


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
  public LayerMask layer;

  public float AttackSpeed;
  public float delay;
  public float Damage;
  public Camera camera;
  public Transform TargetEnemie;
  public PlayerController enemieController;
  public GameObject bullet;
  public Transform shooter;


  public void statGame()
  {
    Canvas.SetActive(false);
    if (inputField.text != "")
    {
      controller.EmitPlayer(inputField.text);
      Nome = inputField.text;

      StartCoroutine("FindPlayer");
      camera = GameObject.Find("Main Camera").GetComponent<Camera>();

    }
    else
    {
      Canvas.SetActive(true);
    }

  }
  IEnumerator FindPlayer()
  {
    yield return new WaitForSeconds(1);
    GameObject P = GameObject.Find(Nome) as GameObject;
    playerCurrent = P;

    {

      ControlePlayer = playerCurrent.GetComponent<PlayerController>();
      SetCurrentAtributs(ControlePlayer.ID);
      CurrentLife = ControlePlayer.Life;
      shooter = ControlePlayer.shooter;
    }


  }

  public void SetCurrentAtributs(string id)
  {
    ID = id;
    playerCurrent.name = id;
  }
  void Update()
  {
    if (delay > 0)
    {
      delay -= Time.deltaTime;
    }
    {

    }
    if (delay <= 0)
    {
      delay = 0;
    }
    if (playerCurrent != null)
    {
      ControlePlayer.MOVE();
      EmitLife();
      SelectTarget();
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

  public void SelectTarget()
  {
    RaycastHit hit;
    Ray ray = camera.ScreenPointToRay(Input.mousePosition);

    if (Physics.Raycast(ray, out hit, 100, layer))
    {
      if (Input.GetMouseButton(1))
      {
        // Debug.Log("clicou");
        // Debug.Log(hit.transform.name);
        TargetEnemie = hit.transform;
        enemieController = TargetEnemie.GetComponent<PlayerController>();
        if (delay == 0)
        {
          playerCurrent.transform.LookAt(TargetEnemie);

          Shoot();
          delay = AttackSpeed;
        }

      }
    }


  }
  public void Shoot()
  {
    if (TargetEnemie != null && delay == 0)
    {
      GameObject bulle = Instantiate(bullet, shooter.position, Quaternion.identity);
      BulletController bulletControl = bulle.GetComponent<BulletController>();
      // Debug.Log("Bulletada");
      bulletControl.bulletMove(TargetEnemie);
      controller.SpawnBullet(this.transform.position, shooter.position, TargetEnemie.name);

    }
  }
}
