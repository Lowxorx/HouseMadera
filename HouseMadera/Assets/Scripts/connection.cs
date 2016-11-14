using UnityEngine;
using UnityEngine.SceneManagement;


public class connection : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void LoadScene()
    {
        SceneManager.LoadScene("POC");
    }
}
