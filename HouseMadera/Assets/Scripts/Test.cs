using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        //if (collider.name.Contains("Collider"))
        //{
        //    if (collider.name.Contains("Vertical"))
        //    {
        //        if (collider.transform.parent.GetChild(2).gameObject.activeInHierarchy)
        //        {
        //            collider.transform.parent.GetChild(2).gameObject.SetActive(false);
        //        }
        //    }
        //    if (collider.name.Contains("Horizontal"))
        //    {
        //        if (collider.transform.parent.GetChild(3).gameObject.activeInHierarchy)
        //        {
        //            collider.transform.parent.GetChild(3).gameObject.SetActive(false);
        //        }
        //    }
        //    collider.GetComponent<CloisonManager>().canBeActivate = false;
        //}
        //Debug.Log("collision madafaka" + collider.gameObject.transform.parent.name);
    }
}
