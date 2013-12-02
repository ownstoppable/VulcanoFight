using UnityEngine;
using System.Collections;

public class Aim : MonoBehaviour {
    Transform myTransform;


	// Use this for initialization
	void Start () {
        myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
        Plane playerPlane = new Plane(Vector3.up, myTransform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitdist = 0.0f;

        if (playerPlane.Raycast(ray, out hitdist))
        {
            myTransform.position = ray.GetPoint(hitdist);
        }
	}

    public void ShowUI() {
        gameObject.SetActive(true);
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitdist = 0.0f;

        if (playerPlane.Raycast(ray, out hitdist))
        {
            transform.position = ray.GetPoint(hitdist);
        }
    }
}
