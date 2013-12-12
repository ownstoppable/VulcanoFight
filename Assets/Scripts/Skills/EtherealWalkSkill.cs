using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EtherealWalkSkill : Skill {
    private float speed;
    private float duration;
    private List<Material> origMaterials;
    private Material hatOrig;
    private Material staffOrig;

    public EtherealWalkSkill(float cd, float spd, float dur)
    {
        cooldown = cd;
        lastUsed = Time.time - cd;
        speed = spd;
        duration = dur;
    }

    public IEnumerator Launch(GameObject character)
    {
        BaseCharacter bc = character.GetComponent<BaseCharacter>();
        bc.GetBaseStat(StatName.Speed).ChangeCurTotal(speed);
        origMaterials = new List<Material>(character.transform.FindChild("skeleton").renderer.materials);
        hatOrig = bc.hatGO.renderer.material;
        staffOrig = bc.staffGO.renderer.material;
        List<Material> newMaterials = new List<Material>();
        for (int i = 0; i < origMaterials.Count; i++)
        {
            Material m = new Material(origMaterials[i]);
            m.shader = Shader.Find("Transparent/Diffuse");
            m.SetColor("_Color", new Color(0.05f, 1, 0.1f, 0.7f));
            newMaterials.Add(m);
        }
        Material hatm = new Material(hatOrig);
        hatm.shader = Shader.Find("Transparent/Diffuse");
        hatm.SetColor("_Color", new Color(0.05f, 1, 0.1f, 0.7f));
        bc.hatGO.renderer.material = hatm;

        Material staffm = new Material(staffOrig);
        staffm.shader = Shader.Find("Transparent/Diffuse");
        staffm.SetColor("_Color", new Color(0.05f, 1, 0.1f, 0.7f));
        bc.staffGO.renderer.material = staffm;

        character.transform.FindChild("skeleton").renderer.materials = newMaterials.ToArray();
        bc.IsHollow = true;

        yield return new WaitForSeconds(duration);
        bc.GetBaseStat(StatName.Speed).ChangeCurTotal(-speed);
        character.transform.FindChild("skeleton").renderer.materials = origMaterials.ToArray();
        bc.hatGO.renderer.material = hatOrig;
        bc.staffGO.renderer.material = staffOrig;
        bc.IsHollow = false;
    }
}
