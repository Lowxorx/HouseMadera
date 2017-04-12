using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {

    public List<GameObject> collideList = new List<GameObject>();
	void Start () {
	
	}
	
	
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {

        collideList.Add(collider.gameObject);
        if (collider.gameObject.name.Contains("Wall"))
        {
            if (name.Contains("Vertical"))
            {
                transform.parent.GetChild(0).GetComponent<CloisonManager>().touchWallVertical = true;
                transform.parent.GetChild(1).GetComponent<CloisonManager>().touchWallVertical = true;
            }

            if (name.Contains("Horizontal"))
            {
                transform.parent.GetChild(0).GetComponent<CloisonManager>().touchWallHorizontal = true;
                transform.parent.GetChild(1).GetComponent<CloisonManager>().touchWallHorizontal = true;
            }
        }
    }
}
