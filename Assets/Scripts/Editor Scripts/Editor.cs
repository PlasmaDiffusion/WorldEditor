﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class Editor : MonoBehaviour
{


    public GameObject selectedObject;
    public GameObject cam;
    public bool holdingObject;
    private float waitTime;

    private bool canMoveCamera;

    private GameObject[] editorExclusiveObjects;
    private GameObject[] sceneObjects;

    public GameObject[] prefabs;

    // Use this for initialization
    void Start()
    {
        cam = transform.GetChild(0).gameObject;
        selectedObject = null;
        canMoveCamera = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (canMoveCamera) moveCamera();

        Vector2 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        if (selectedObject) selectedObject.transform.position = newPos;


        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            return;
        }

        if (Input.GetMouseButtonDown(0) && holdingObject)
        {
            //Snap selected object onto position in grid
            if (selectedObject)
            {

                newPos.x = Mathf.Round((newPos.x / 32) * 32);
                newPos.y = Mathf.Round((newPos.y / 32) * 32);

                selectedObject.transform.position = newPos;
                selectedObject = null;
                holdingObject = false;
            }
        }
    }

    void moveCamera()
    {
        float speed = 6.0f;

        if (Input.GetKey(KeyCode.A))
        {
            cam.transform.position += new Vector3(-speed * Time.deltaTime, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            cam.transform.position += new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.W))
        {
            cam.transform.position += new Vector3(0.0f, speed * Time.deltaTime, 0.0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            cam.transform.position += new Vector3(0.0f, -speed * Time.deltaTime, 0.0f);
        }
    }

    void OnMouseDown()
    {

    }

    public void attachObject(GameObject obj)
    {
        if (holdingObject) return;

        selectedObject = obj;
        waitTime = 0.5f;
        holdingObject = true;
    }


    public void play()
    {
        canMoveCamera = false;

        getAllSceneObjects();

        //Save all scene objects
        Save("test");

        foreach (GameObject obj in sceneObjects)
        {
            
            obj.GetComponent<BaseObject>().inEditor = false;


            //Attach camera to player
            if (obj.GetComponent<BaseObject>().prefabID == 3)
            {
                cam.transform.parent = obj.transform;
                cam.transform.localPosition = new Vector3(0.0f, 0.0f, cam.transform.localPosition.z);
            }

        }
        

    

        editorExclusiveObjects = GameObject.FindGameObjectsWithTag("EditorOnly");



        foreach (GameObject obj in editorExclusiveObjects)
        {
            obj.SetActive(false);


        }
    }

    public void endPlay()
    {
        canMoveCamera = true;


        //Attach camera to this object
        cam.transform.parent = transform;

        destroyAllSceneObjects();



        foreach (GameObject obj in editorExclusiveObjects)
        {
            obj.SetActive(true);
        }

        //Now load the scene objects back
        Load("test");
    }

    public void destroyAllSceneObjects()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Editor and Game"))
        {

            Destroy(obj);

        }
    }

    void getAllSceneObjects()
    {
        sceneObjects = GameObject.FindGameObjectsWithTag("Editor and Game");
    }


    //The following save and load functions are for saving entire levels. These functions were taken and modified from the following tutorial: https://unity3d.com/learn/tutorials/topics/scripting/persistence-saving-and-loading-data

    public void Save(string name)
    {
        //Make sure the list of scene objects is up to date
        getAllSceneObjects();

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + name + ".dat");

        
        //Initialize new data class of the scene
        Data data = new Data();

        data.posX = new List<float>();
        data.posY = new List<float>();
        data.prefabID = new List<int>();
        data.numOfObjects = 0;

        Debug.Log(sceneObjects.Length);

        foreach (GameObject currentGameObject in sceneObjects)
        {

            

        data.posX.Add(currentGameObject.transform.position.x);
        data.posY.Add(currentGameObject.transform.position.y);

        data.prefabID.Add(currentGameObject.GetComponent<BaseObject>().prefabID);

        data.numOfObjects++;
        }

        Debug.Log(data.numOfObjects);

        binaryFormatter.Serialize(file, data);
        file.Close();
    }

    //Class that will save object positions, prefab ids and any other editor values
    [System.Serializable]
    class Data : System.Object
    {

        public List<float> posX;
        public List<float> posY;

        public List<int> prefabID;

        public int numOfObjects;

    }

    public void Load(string name)
    {

        if (File.Exists(Application.persistentDataPath + "/" + name + ".dat"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + name + ".dat", FileMode.Open);

            //Cast to player data
            Data data = (Data)binaryFormatter.Deserialize(file);
            file.Close();
            
            //Start recreating objects as they were 
            for (int i = 0; i < data.numOfObjects; i++)
            {
                Instantiate(prefabs[data.prefabID[i]], new Vector3(data.posX[i], data.posY[i], 0.0f), new Quaternion());
            }
        }
    }

}
   