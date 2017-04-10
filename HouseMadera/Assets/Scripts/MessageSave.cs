using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageSave : MonoBehaviour {

    bool modification = false;
    float targetTime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (modification)
        {
            targetTime -= Time.deltaTime;
            if (targetTime <= 0.0f)
            {
                modification = false;
                this.gameObject.SetActive(false);
            }
        }
	}

    public void Message(string text, Color color)
    {
        this.gameObject.SetActive(true);
        this.GetComponent<Image>().color = color;
        this.transform.GetChild(0).GetComponent<Text>().text = text;
        targetTime = 5.0f;
        modification = true;
    }
}