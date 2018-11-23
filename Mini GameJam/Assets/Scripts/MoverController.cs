using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverController : MonoBehaviour {

    //global instance
    public static MoverController Instance;

    //objects reserved by movers, no other movers can grab reserved objects
    public List<GameObject> reservedObjects = new List<GameObject>();

    //objects placed by the player. Can be reserved by movers
    public List<GameObject> placedObjects;

    //teddy bear object
    public GameObject teddyBear;

    public List<Mover> movers = new List<Mover>();

    void Awake()
    {
        Instance = this;
    }
    
    /// <summary>
    /// subscribe an object for the movers
    /// </summary>
    /// <param name="obj"></param>
    public void SubscribeObject(GameObject obj)
    {
        reservedObjects.Add(obj);
    }

    /// <summary>
    /// UNsubscribe an object for the movers
    /// </summary>
    /// <param name="obj"></param>
    public void UnsubscribeObject(GameObject obj)
    {
        reservedObjects.Remove(obj);
    }

    /// <summary>
    /// check if an object is reserved
    /// </summary>
    /// <param name="obj"></param>
    public bool IsReserved(GameObject obj)
    {
        return reservedObjects.Contains(obj);
    }
}
