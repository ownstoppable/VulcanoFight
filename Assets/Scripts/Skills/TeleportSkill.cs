using UnityEngine;
using System.Collections;

public class TeleportSkill : Skill {

    private float maxDistance;

	// Use this for initialization
    public TeleportSkill(float cd, float dist)
    {
        cooldown = cd;
        lastUsed = Time.time - cd;
        maxDistance = dist;
    }

    public void Launch(GameObject character, Vector3 dest)
    {
        character.transform.rotation = Quaternion.LookRotation(new Vector3(dest.x, character.transform.position.y, dest.z) - character.transform.position);
		GameObject telept = GameObject.Instantiate(Resources.Load("Skills/Teleport"),  character.transform.position, Quaternion.identity) as GameObject;
		GameObject.Destroy(telept, 1);
        if (Vector3.Distance(character.transform.position, dest) > maxDistance)
        {
            character.transform.Translate(new Vector3(0, 0, maxDistance));
        }
        else character.transform.position = new Vector3(dest.x, character.transform.position.y, dest.z);

    }
}
