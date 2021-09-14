using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class effectText : MonoBehaviour
{
    public float duration;
    public float speed;
    public float heightToLevitate;
    public float heightToLevitateTo;
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        heightToLevitateTo =
            heightToLevitate + this.gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < heightToLevitateTo)
        {
            transform.position += speed * Vector3.up*Time.deltaTime;
        }
        else {
            Destroy(this.gameObject);
        }
    }
}
