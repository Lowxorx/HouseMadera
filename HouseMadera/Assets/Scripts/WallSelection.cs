using UnityEngine;


public class WallSelection : MonoBehaviour {

    private GameObject childrenInside;
    private GameObject childrenOutside;
    public bool wallPlaced = false;
    private bool wallSelected = false;
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0) && wallSelected && !wallPlaced)
        {
            wallPlaced = true;
            childrenOutside.GetComponent<Renderer>().material.color = Color.white;
            childrenInside.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    void OnMouseEnter()
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

    void OnMouseExit()
    {
        if(childrenOutside != null && !wallPlaced)
        {
            wallSelected = false;
            childrenOutside.SetActive(false);
            childrenInside.SetActive(false);
        }
    }
}
