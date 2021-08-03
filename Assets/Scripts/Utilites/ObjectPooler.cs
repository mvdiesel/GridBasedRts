using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------

//This script is responsible for object pooling

//---------------------------------------------------

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool   //Defining pool class
    {
        public string tag;
        public GameObject prefab;
        public int size;

        public Pool(string poolTag, GameObject poolPrefab, int poolSize){
            tag = poolTag;
            prefab = poolPrefab;
            size = poolSize;
        }

        public void setTag(string tag){
            this.tag = tag;
        }

        public void setPrefab(GameObject go){
            this.prefab = go;
        }

        public void setSize(int size){
            this.size = size;
        }
    }

    public List<Pool> pools = new List<Pool>();
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public BuildingFactory bf;
    [SerializeField] public int poolCount;

    #region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Start is called before the first frame update             
    void Start()
    {
        createPools();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools){
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++){
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void createPools(){  //Create pools dynamically with the buildingTypes from "BuildingFactory"
        List<GameObject> buildingTypes = bf.GetComponent<BuildingFactory>().getBuildingTypes();
        
        foreach(GameObject obj in buildingTypes){
            Pool p = new Pool(obj.name, obj, poolCount);
            pools.Add(p);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)  //Activate object from pool at given location
    {
        if(!poolDictionary.ContainsKey(tag)){
            Debug.LogWarning(tag + " pool doesn't exist!");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn); 

        return objectToSpawn;
    }

    public GameObject getObjectFromPool(string tag){
        return poolDictionary[tag].Peek();
    }
}
