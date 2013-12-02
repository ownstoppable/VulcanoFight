using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

    private float originalScale;
    public int scaleLvl;

	// Use this for initialization
	void Start () {
        originalScale = transform.localScale.x;
        scaleLvl = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.GetTimePassed() > GameOptions.Instance.nMins * 60 * 0.75f && scaleLvl == 2)
        {
            transform.localScale = new Vector3(0.25f * originalScale, 1, 0.25f * originalScale);
            scaleLvl = 3;
        }
        else if (GameManager.Instance.GetTimePassed() > GameOptions.Instance.nMins * 60 * 0.50f && scaleLvl == 1)
        {
            transform.localScale = new Vector3(0.50f * originalScale, 1, 0.50f * originalScale);
            scaleLvl = 2;
        }
        else if (GameManager.Instance.GetTimePassed() > GameOptions.Instance.nMins * 60 * 0.25f && scaleLvl == 0)
        {
            transform.localScale = new Vector3(0.75f * originalScale, 1, 0.75f * originalScale);
            scaleLvl = 1;
        }
	}
}
