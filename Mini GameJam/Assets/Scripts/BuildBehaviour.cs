using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildBehaviour : MonoBehaviour {

    public List<GameObject> buildableObjects;
    public List<GameObject> buildableObjectsUI;
    public List<AudioClip> buildableObjectsSounds;

    [HideInInspector]
    public GameObject prevGO;
    [HideInInspector]
    public GameObject prevGOUI;
    [HideInInspector]
    public AudioClip prevGOSound;
    [HideInInspector]
    public bool objectVisible;

    public GameObject objectFolder;

    public List<GameObject> builtObjects;

    MoverController moverController;

    PlayerChar pc;
    public GameManager gm;
    AudioSource audioSource;
	
    void Start()
    {
        moverController = MoverController.Instance;

        pc = GetComponent<PlayerChar>();
        audioSource = GetComponent<AudioSource>();
    }

	void Update ()
    {
        if(objectVisible && prevGO != null)
        {
            if (prevGO.GetComponent<Collectible>().canBuild)
                prevGO.GetComponentInChildren<SpriteRenderer>().color = Color.green;
            else if (prevGO.GetComponent<Collectible>().canBuild == false)
                prevGO.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }

        //Change different building blueprints
        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            if(prevGO != null)
                if(prevGO != buildableObjects[selectObject() - 1])
                {
                    prevGO.SetActive(false);
                    prevGOUI.GetComponent<Image>().color = Color.white;
                }

            if (buildableObjects[selectObject()-1].gameObject.activeInHierarchy)
            {
                buildableObjects[selectObject() - 1].SetActive(false);
                objectVisible = false;
                buildableObjectsUI[selectObject() - 1].gameObject.GetComponent<Image>().color = Color.white;
            }
            else
            {
                buildableObjects[selectObject() - 1].SetActive(true);
                objectVisible = true;
                buildableObjectsUI[selectObject() - 1].gameObject.GetComponent<Image>().color = Color.green;
                if (pc.gunEquipped)
                {
                    pc.gun.SetActive(false);
                    pc.gunEquipped = false;
                }
            }
                

            prevGO = buildableObjects[selectObject() - 1];
            prevGOUI = buildableObjectsUI[selectObject() - 1];
            prevGOSound = buildableObjectsSounds[selectObject() - 1];
        }

        //If an object is visible and left mouse button is being pressed, place object.
        if(Input.GetMouseButtonDown(0) && objectVisible)
        {
            //Check prevGO.cost
            if(gm.points - prevGO.GetComponent<Collectible>().cost >= 0 && prevGO.GetComponent<Collectible>().canBuild)
            {
                GameObject go = Instantiate(prevGO);
                go.transform.localPosition = new Vector3(prevGO.transform.position.x, 1, prevGO.transform.position.z);
                go.transform.rotation = Quaternion.Euler(90, 0, 0);
                go.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255, 255);
                moverController.placedObjects.Add(go);

                gm.ChangePoints(-prevGO.GetComponent<Collectible>().cost);
                audioSource.PlayOneShot(prevGOSound);
                prevGO.GetComponent<Collectible>().canBuild = false;
            }
            else
            {
                print("Hoi Niels. Je hebt te weinig geld.........kankerboef of je kunt niet bouwen");
            }
        }

        //Rotate buildable object towards mouse position
        if(prevGO != null)
        {
            var mouse = Input.mousePosition;
            var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
            var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            objectFolder.transform.rotation = Quaternion.Euler(90, 0, angle);
            prevGO.transform.rotation = Quaternion.Euler(90, 0, 0);
        }

    }

    int selectObject()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            return 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            return 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            return 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            return 4;
        }

        return 0;
    }
}
