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

    public int[] customGeneralCollisions;

    // Use this for initialization
    void Start()
    {
        cam = transform.GetChild(0).gameObject;
        selectedObject = null;
        canMoveCamera = true;

        //Initialize some collision values
        customGeneralCollisions = new int[8];
        for (int i = 0; i < 8; i++) customGeneralCollisions[i] = -1; //General custom collisions are for unspecified ones like ground or the goal

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
            baseObject.becomeGameState();

            //Make sure static values get loaded in. Scene objects won't call onStart() when playtesting, so this is a workaround. Could alternatively overwrite the become gamestate function...------
            if (baseObject.prefabID == 4) baseObject.loadCustomValues(Enemy.customEnemyCollisions);
            else if (baseObject.prefabID == 6) baseObject.loadCustomValues(Coin.customCoinCollisions);
            else if (baseObject.prefabID == 5) baseObject.loadCustomValues(Spring.customSpringCollisions);
           else if (baseObject.prefabID == 3)
            {

                baseObject.loadCustomValues(Player.customPlayerCollisions);

                //Attach camera to player
                cam.transform.parent = obj.transform;
                cam.transform.localPosition = new Vector3(0.0f, 0.0f, cam.transform.localPosition.z);
            }
            else
            { 
            baseObject.loadCustomValues(customGeneralCollisions);
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


    //The following save and load functions are for saving entire levels, as binary data. These functions were taken and modified from the following unity tutorial: https://unity3d.com/learn/tutorials/topics/scripting/persistence-saving-and-loading-data

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

        data.mass = new List<float>();
        data.drag = new List<float>();
        data.gravity = new List<float>();
        data.rotate = new List<bool>();

        data.customVelX = new List<float>();
        data.customVelY = new List<float>();

        data.numOfObjects = 0;


        //Save everything for every scene object
        foreach (GameObject currentGameObject in sceneObjects)
        {

            BaseObject baseObject = currentGameObject.GetComponent<BaseObject>();

            data.posX.Add(currentGameObject.transform.position.x);
            data.posY.Add(currentGameObject.transform.position.y);

            data.prefabID.Add(baseObject.prefabID);

            data.mass.Add(baseObject.mass);
            data.drag.Add(baseObject.linearDrag);
            data.gravity.Add(baseObject.gravityScale);
            data.rotate.Add(baseObject.rotate);

            data.customVelX.Add(baseObject.overwriteVelocity.x);
            data.customVelY.Add(baseObject.overwriteVelocity.y);

            data.numOfObjects++;
        }

        data.customCollisions = new List<int[]>();

        //Custom collision arrays to be saved
        data.customCollisions.Add(Player.customPlayerCollisions);
        data.customCollisions.Add(Enemy.customEnemyCollisions);
        data.customCollisions.Add(Coin.customCoinCollisions);
        data.customCollisions.Add(Spring.customSpringCollisions);
        data.customCollisions.Add(customGeneralCollisions);



        binaryFormatter.Serialize(file, data);
        file.Close();
    }

    //Class that will save object positions, prefab ids and any other editor values
    [System.Serializable]
    class Data : System.Object
    {
        //This data class uses lists. Each element is for one object

        public List<float> posX;
        public List<float> posY;


        //PrefabID is the most important variable, that lets the game know what the object originally was.
        public List<int> prefabID;

        //Custom rigid body stuff
        public List<float> mass;
        public List<float> gravity;
        public List<float> drag;
        public List<bool> rotate;

        //Custom movement (Vectors are a unity thing so floats have to be used)
        public List<float> customVelX;
        public List<float> customVelY;

        //Arrays of custom collision events
        public List<int[]> customCollisions;



        //Number of objects saved (for a for loop in the loading later)
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

            //Load custom collision static variables here!
            if (data.customCollisions != null)
            { 
            Player.customPlayerCollisions = data.customCollisions[0];
                
            Enemy.customEnemyCollisions = data.customCollisions[1];

            Coin.customCoinCollisions = data.customCollisions[2];
            
            Spring.customSpringCollisions = data.customCollisions[3];

                customGeneralCollisions = data.customCollisions[4];
            }

            //Start recreating objects as they were when saved
            for (int i = 0; i < data.numOfObjects; i++)
            {
               GameObject newObject = Instantiate(prefabs[data.prefabID[i]], new Vector3(data.posX[i], data.posY[i], 0.0f), new Quaternion());

                //Load in specific custom values here
                BaseObject baseObject = newObject.GetComponent<BaseObject>();

                if (baseObject)
                {
                    Debug.Log("mass: " + data.mass[i]);
                baseObject.mass = data.mass[i];
                baseObject.gravityScale = data.gravity[i];
                baseObject.linearDrag = data.drag[i];
                baseObject.rotate = data.rotate[i];

                baseObject.overwriteVelocity.x = data.customVelX[i];
                baseObject.overwriteVelocity.y = data.customVelY[i];
                baseObject.makeDefaults = false;
                }
            }
        }
    }

}
   