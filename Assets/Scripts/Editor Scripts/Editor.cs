using System.Collections;
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

        //Initialize some static values
        Player.customPlayerCollisions = new int[8];
        for (int i = 0; i < 8; i++) Player.customPlayerCollisions[i] = -1;

        Enemy.customEnemyCollisions = new int[8];
        for (int i = 0; i < 8; i++) Enemy.customEnemyCollisions[i] = -1;

        Coin.customCoinCollisions = new int[8];
        for (int i = 0; i < 8; i++) Coin.customCoinCollisions[i] = -1;

        Spring.customSpringCollisions = new int[8];
        for (int i = 0; i < 8; i++) Spring.customSpringCollisions[i] = -1;

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


        //Save all scene objects
        Save("testingScene");
        


        getAllSceneObjects();

        foreach (GameObject obj in sceneObjects)
        {

            BaseObject baseObject = obj.GetComponent<BaseObject>();
            baseObject.inEditor = false;

            //Make sure static values get loaded in. Scene objects won't call onStart() when playtesting, so this is a workaround------
            if (baseObject.prefabID == 4) baseObject.loadCustomValues(Enemy.customEnemyCollisions);
            if (baseObject.prefabID == 6) baseObject.loadCustomValues(Coin.customCoinCollisions);
            if (baseObject.prefabID == 5) baseObject.loadCustomValues(Spring.customSpringCollisions);


            if (baseObject.prefabID == 3)
            {

                baseObject.loadCustomValues(Player.customPlayerCollisions);

                //Attach camera to player
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
        Load("testingScene");
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



        foreach (GameObject currentGameObject in sceneObjects)
        {

            

        data.posX.Add(currentGameObject.transform.position.x);
        data.posY.Add(currentGameObject.transform.position.y);

        data.prefabID.Add(currentGameObject.GetComponent<BaseObject>().prefabID);

        data.numOfObjects++;
        }

        data.customCollisions = new List<int[]>();

        //Custom collision arrays to be saved
        data.customCollisions.Add(Player.customPlayerCollisions);
        data.customCollisions.Add(Enemy.customEnemyCollisions);
        data.customCollisions.Add(Coin.customCoinCollisions);
        data.customCollisions.Add(Spring.customSpringCollisions);



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

        public List<int[]> customCollisions;
        
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

            //Load custom collision static variables here!
            if (data.customCollisions != null)
            Player.customPlayerCollisions = data.customCollisions[0];

            if (data.customCollisions != null)
            Enemy.customEnemyCollisions = data.customCollisions[1];

            if (data.customCollisions != null)
            Coin.customCoinCollisions = data.customCollisions[2];

            if (data.customCollisions != null)
            Spring.customSpringCollisions = data.customCollisions[3];

            //Start recreating objects as they were 
            for (int i = 0; i < data.numOfObjects; i++)
            {
                Instantiate(prefabs[data.prefabID[i]], new Vector3(data.posX[i], data.posY[i], 0.0f), new Quaternion());
            }
        }
    }

}
   