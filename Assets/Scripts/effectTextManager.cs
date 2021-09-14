using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectTextManager : MonoBehaviour
{
    public GameObject effectTextPrefab;
    public float damageTime;
    // Start is called before the first frame update
    void Start()
    {
        PrimaryCharacter.onTakeDamage += PrimaryCharacter_onTakeDamage;
    }

    private void PrimaryCharacter_onTakeDamage(Unit dealtTo, float dealt, Unit Dealer)
    {
        doText(dealtTo.unitName + " took " + dealt, damageTime, dealtTo.transform.position);
    }

    public void OnDestroy()
    {

        PrimaryCharacter.onTakeDamage -= PrimaryCharacter_onTakeDamage;
    }

    public void doText(string stringToShow, float time, Vector3 position)
    {
        effectText newText =
            Instantiate(effectTextPrefab, 
            position, Quaternion.identity).GetComponent<effectText>();
        newText.text.text = stringToShow;
        newText.duration = time;
        newText.speed = newText.duration / newText.heightToLevitate;

    }
}
