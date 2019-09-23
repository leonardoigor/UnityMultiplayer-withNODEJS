using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

  public string nome = "test";
  public string ID;
  public float speed;
  public Camera camera;
  public Transform LookCamera;
  public LayerMask Layer;
  public Slider lifeBar;
  public float Life;
  public NavMeshAgent agent;
  public Vector3 currentPosition;
  public Vector3 POSPosition;
  public Quaternion currentRotation;
  public Quaternion POSRotation;
  public Controller controller;
  public bool isCurrent;
  void Start()
  {
    currentPosition = this.transform.position;
    POSPosition = currentPosition;
    currentRotation = this.transform.rotation;
    POSRotation = currentRotation;
    agent = GetComponent<NavMeshAgent>();
    camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    controller = GameObject.Find("Network Controller").GetComponent<Controller>();
    speed = 3;
    controller.setCurrenPlayer(this.name);
    StartCoroutine("changeToID");
    lifeBar = this.GetComponentInChildren<Slider>();
    lifeBar.maxValue = Life;
    LookCamera = GameObject.Find("Look at camera").transform;

  }

  // Update is called once per frame
  void Update()
  {
    POSPosition = this.transform.position;
    POSRotation = this.transform.rotation;
    lifeBar.transform.LookAt(LookCamera);
  }
  public void MOVE()
  {
    agent.speed = speed;
    RaycastHit hit;
    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out hit, 100, Layer))
    {
      if (Input.GetMouseButton(0))
      {
        setDir(hit.point);
      }

    }
    if (currentPosition != POSPosition)
    {
      controller.EmitMove(POSPosition, POSRotation, this.name);
      currentPosition = POSPosition;
      currentRotation = POSRotation;
    }
  }
  public void setDir(Vector3 target)
  {

    currentPosition = target;

    agent.SetDestination(target);

  }
  IEnumerator changeToID()
  {
    yield return new WaitForSeconds(1);
    this.name = ID;
  }
  public void setLife(float lifeInit)
  {
    Life = lifeInit;
    lifeBar.value = lifeInit;
  }
}
