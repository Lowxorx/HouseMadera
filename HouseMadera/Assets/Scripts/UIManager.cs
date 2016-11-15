using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {

    public GameObject panelTexture;
    public GameObject panelModule;
    public GameObject texturePosition;
    public GameObject modulePosition;
    Vector3 textureInitialPosition;
    Vector3 modulePositionInitial;
    bool show = false;
    bool hide = true;
	// Use this for initialization
	void Start ()
    {
        textureInitialPosition = panelTexture.transform.position;
        modulePositionInitial = panelModule.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag != "Wall" && !EventSystem.current.IsPointerOverGameObject())
                {
                    GameObject.Find("Event").GetComponent<EditWall>().wallSelected = null;
                    show = false;
                    hide = true;
                }
            }
        }
        if (show)
        {
            if (panelTexture.transform.position.x < texturePosition.transform.position.x)
            {
                panelTexture.transform.Translate(Vector3.right * Time.deltaTime * 300);
            }

            if (panelModule.transform.position.x > modulePosition.transform.position.x)
            {
                panelModule.transform.Translate(Vector3.left * Time.deltaTime * 300);
            }
        }

        if (hide)
        {
            if (panelTexture.transform.position.x > textureInitialPosition.x)
            {
                panelTexture.transform.Translate(Vector3.left * Time.deltaTime * 300);
            }

            if (panelModule.transform.position.x < modulePositionInitial.x)
            {
                panelModule.transform.Translate(Vector3.right * Time.deltaTime * 300);
            }
        }
    }

    public void ShowPanels()
    {
        hide = false;
        show = true;
    }
}
