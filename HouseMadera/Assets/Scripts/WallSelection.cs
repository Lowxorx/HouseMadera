using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallSelection : MonoBehaviour {

    private GameObject childrenInside;
    private GameObject childrenOutside;
    public List<GameObject> listModule;
    public bool wallPlaced = false;
    private bool wallSelected = false;
    public bool canBeActivate = true;
    public GameObject _switch;
    public int gamme;
	void Start ()
    {
        _switch = GameObject.Find("Switch");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonUp(0) && wallSelected && !wallPlaced)
        {
            wallPlaced = true;
            childrenOutside.GetComponent<Renderer>().material.color = Color.white;
            childrenInside.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    void OnMouseEnter()
    {
        if (canBeActivate)
        {
            childrenOutside = this.gameObject.transform.GetChild(0).gameObject;
            childrenInside = this.gameObject.transform.GetChild(1).gameObject;
            childrenOutside.SetActive(true);
            childrenInside.SetActive(true);
            wallSelected = true;
            if (!wallPlaced)
            {
                childrenOutside.GetComponent<Renderer>().material.color = Color.red;
                childrenInside.GetComponent<Renderer>().material.color = Color.red;
            }
        }
            
    }

    void OnMouseExit()
    {
        if (canBeActivate)
        {
            if (childrenOutside != null && !wallPlaced)
            {
                wallSelected = false;
                childrenOutside.SetActive(false);
                childrenInside.SetActive(false);
            }
        }
        
    }
}
