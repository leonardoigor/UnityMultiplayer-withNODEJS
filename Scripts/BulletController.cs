using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
  // Start is called before the first frame update
  public Transform targett;
  public Controller controller;
  public Vector3 currentPosition, Posposition;
  public Collider collider;
  void Start()
  {
    controller = GameObject.Find("Network Controller").GetComponent<Controller>();

    currentPosition = this.transform.position;
    Posposition = this.transform.position;
  }

  // Update is called once per frame
  void Update()
  {
    bulletMove(targett);
    currentPosition = this.transform.position;
    if (currentPosition != Posposition)
    {
      controller.OnBulletMove(currentPosition, this.transform.name);
      Posposition = currentPosition;
    }
  }
  public void bulletMove(Transform target)
  {
    targett = target;
    if (targett != null)
    {
      this.transform.Translate(0, 0, 5 * Time.deltaTime);
      this.transform.LookAt(target);
    }
  }
  void OnCollisionEnter(Collision other)
  {
    Debug.Log("colision");
    collider = other.collider;
    if (collider)
    {

      controller.OnBullerDie(this.transform.name);
      dieBullet();
    }
  }
  private void OnCollisionExit(Collision other)
  {
    collider = null;
  }
  public void dieBullet()
  {
    Destroy(this.gameObject);
  }
}
