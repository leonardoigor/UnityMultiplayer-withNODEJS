using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using SocketIO;
using UnityEngine;

public class Controller : MonoBehaviour
{
  private SocketIOComponent socket;
  public GameObject playerPrefab;
  public Transform spawn;
  public string CurrentPlayer;
  public Player playerComponent;
  public GameObject BulletPrefab;
  public string test;

  void Start()
  {
    GameObject go = GameObject.Find("SocketIO");
    socket = go.GetComponent<SocketIOComponent>();
    StartCoroutine("Connect");
    socket.On("Move", Onmove);
    socket.On("Start", OnStart);
    socket.On("LifeChange", OnLifeChange);
    socket.On("destroi", Ondestroi);
    socket.On("Current", OnCurrent);
    socket.On("SpawnBullet", OnSpawnBullet);
    playerComponent = this.GetComponent<Player>();
  }

  // Update is called once per frame
  void Update()
  {

  }
  private IEnumerator Connect()
  {
    yield return new WaitForSeconds(1);

  }
  public void EmitPlayer(string name)
  {
    Dictionary<string, string> data = new Dictionary<string, string>();
    data["name"] = name;
    data["life"] = "800";
    data["p_x"] = "0";
    data["p_y"] = "0";
    data["p_z"] = "0";
    data["l_x"] = "0";
    data["l_y"] = "0";
    data["l_z"] = "0";
    data["l_w"] = "0";
    socket.Emit("Start", new JSONObject(data));
  }
  public void OnStart(SocketIOEvent data)
  {
    string name = JsonToString("name", data);
    string life = JsonToString("life", data);
    string id = JsonToString("id", data);
    // POsition
    var p_x = JsonToString("p_x", data);
    var p_y = JsonToString("p_y", data);
    var p_z = JsonToString("p_z", data);
    Vector3 NewVP = new Vector3(float.Parse(p_x), float.Parse(p_y), float.Parse(p_z));
    // Rotation
    var l_x = JsonToString("l_x", data);
    var l_y = JsonToString("l_y", data);
    var l_z = JsonToString("l_z", data);
    var l_w = JsonToString("l_w", data);
    Quaternion NewVR = new Quaternion(0, 0, 0, 0);
    NewVR.x = float.Parse(l_x);
    NewVR.y = float.Parse(l_y);
    NewVR.z = float.Parse(l_z);
    NewVR.w = float.Parse(l_w);



    GameObject newPlayer = Instantiate(playerPrefab, spawn.position, Quaternion.identity);
    newPlayer.name = name;
    if (p_x != "0")
    {
      newPlayer.transform.position = NewVP;
      newPlayer.transform.rotation = NewVR;
    }
    PlayerController p = newPlayer.GetComponent<PlayerController>();
    p.nome = name;
    p.isCurrent = false;
    p.ID = id;
    p.setLife(float.Parse(life));
  }
  public void OnCurrent(SocketIOEvent data)
  {
    string name = JsonToString("name", data);
    string id = JsonToString("id", data);
    GameObject newPlayer = Instantiate(playerPrefab, spawn.position, Quaternion.identity);
    newPlayer.name = id;
    PlayerController p = newPlayer.GetComponent<PlayerController>();
    p.name = id;
    p.isCurrent = true;
  }
  public void EmitMove(Vector3 dir, Quaternion rotation, string name)
  {
    Dictionary<string, string> data = new Dictionary<string, string>();
    data["name"] = name;
    data["p_x"] = dir.x.ToString();
    data["p_y"] = dir.y.ToString();
    data["p_z"] = dir.z.ToString();
    data["l_x"] = rotation.x.ToString();
    data["l_y"] = rotation.y.ToString();
    data["l_z"] = rotation.z.ToString();
    data["l_w"] = rotation.w.ToString();

    socket.Emit("Move", new JSONObject(data));
  }

  public void Onmove(SocketIOEvent data)
  {
    var nome = JsonToString("id", data);
    // Debug.Log(nome);
    // POsition
    var p_x = JsonToString("p_x", data);
    var p_y = JsonToString("p_y", data);
    var p_z = JsonToString("p_z", data);
    Vector3 NewVP = new Vector3(float.Parse(p_x), float.Parse(p_y), float.Parse(p_z));
    // Rotation
    var l_x = JsonToString("l_x", data);
    var l_y = JsonToString("l_y", data);
    var l_z = JsonToString("l_z", data);
    var l_w = JsonToString("l_w", data);
    Quaternion NewVR = new Quaternion(0, 0, 0, 0);
    NewVR.x = float.Parse(l_x);
    NewVR.y = float.Parse(l_y);
    NewVR.z = float.Parse(l_z);
    NewVR.w = float.Parse(l_w);
    GameObject moveP = GameObject.Find(nome) as GameObject;
    moveP.transform.position = NewVP;
    moveP.transform.rotation = NewVR;

  }
  public void EmitLifeChange(float currentLife, string id)
  {
    Dictionary<string, string> data = new Dictionary<string, string>();
    data["life"] = currentLife.ToString();
    data["id"] = id;
    socket.Emit("LifeChange", new JSONObject(data));
  }
  public void Ondestroi(SocketIOEvent data)
  {
    var id = JsonToString("id", data);
    GameObject deleteP = GameObject.Find(id);
    Destroy(deleteP);
  }
  public void OnLifeChange(SocketIOEvent data)
  {
    var id = JsonToString("id", data);
    var life = JsonToString("life", data);

    GameObject ChangelifePlaye = GameObject.Find(id) as GameObject;
    PlayerController contP = ChangelifePlaye.GetComponent<PlayerController>();
    contP.setLife(float.Parse(life));
  }
  public void SpawnBullet(Vector3 position, Vector3 shooter, string id)
  {
    Dictionary<string, string> data = new Dictionary<string, string>();
    data["id"] = id;
    data["p_x"] = position.x.ToString();
    data["p_y"] = position.y.ToString();
    data["p_z"] = position.z.ToString();
    data["shooter_x"] = shooter.x.ToString();
    data["shooter_y"] = shooter.y.ToString();
    data["shooter_z"] = shooter.z.ToString();
    socket.Emit("SpawnBullet", new JSONObject(data));
  }
  public void OnBulletMove(Vector3 position, string id)
  {
    Dictionary<string, string> data = new Dictionary<string, string>();
    data["id"] = id;
    data["p_x"] = position.x.ToString();
    data["p_y"] = position.y.ToString();
    data["p_z"] = position.z.ToString();
    socket.Emit("MovenBullet", new JSONObject(data));
  }
  public void OnBullerDie(string id)
  {
    Dictionary<string, string> data = new Dictionary<string, string>();
    data["id"] = id;
    socket.Emit("destroyBullet", new JSONObject(data));
  }

  public void OnSpawnBullet(SocketIOEvent evt)
  {
    string idB = JsonToString("id", evt);
    string p_x = JsonToString("p_x", evt);
    string p_y = JsonToString("p_y", evt);
    string p_z = JsonToString("p_z", evt);
    string shooter_x = JsonToString("shooter_x", evt);
    string shooter_y = JsonToString("shooter_y", evt);
    string shooter_z = JsonToString("shooter_z", evt);

    Vector3 shootPosition = new Vector3(float.Parse(shooter_x), float.Parse(shooter_y), float.Parse(shooter_z));
    Vector3 BulletVect = new Vector3(float.Parse(p_x), float.Parse(p_y), float.Parse(p_z));

    BulletPrefab.transform.position = BulletVect;

    GameObject BulletSpawl = Instantiate(BulletPrefab, shootPosition, Quaternion.identity);
    BulletSpawl.name = idB + "_bullet";
    BulletController bulletControl = BulletSpawl.GetComponent<BulletController>();
    Transform targetnew = GameObject.Find(idB).GetComponent<Transform>();
    bulletControl.bulletMove(targetnew);

  }



  string JsonToString(string target, SocketIOEvent data)
  {
    string so = data.data.GetField(target).ToString();
    string[] newString = Regex.Split(so, "\"");
    return newString[1];
  }
  string JsonToStringNormal(string target, string s)
  {
    string[] newString = Regex.Split(target, s);
    return newString[1];
  }
  Vector3 jsonToVector3(string target)
  {

    Vector3 newVector;
    string[] newString = Regex.Split(target, ",");
    newVector = new Vector3(float.Parse(newString[0]), float.Parse(newString[1]), float.Parse(newString[2]));
    return newVector;
  }
  public void setCurrenPlayer(string name)
  {
    CurrentPlayer = name;
  }

}