using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

    // Use this for initialization
    public GameObject target;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey("left"))
        {
            transform.RotateAround(target.transform.position, Vector3.up, 50 * Time.deltaTime);
        }

        if (Input.GetKey("right"))
        {
            transform.RotateAround(target.transform.position, Vector3.down, 50 * Time.deltaTime);
        }

    }
}
