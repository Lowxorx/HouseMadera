using UnityEngine;


public class ModelSelected : MonoBehaviour {

    public GameObject modelA;
    public GameObject modelB;
    public GameObject modelC;
    public GameObject selectionModel;
    public GameObject floor;
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void ModelASelected()
    {
        floor.SetActive(true);
        selectionModel.SetActive(false);
        GameObject house = Instantiate(modelA);
        house.name = "House";
    }

    public void ModelBSelected()
    {

    }

    public void ModelCSelected()
    {

    }
}
