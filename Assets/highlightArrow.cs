using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highlightArrow : MonoBehaviour
{
    public Vector3 startingPosition;
    public float amountGoing;
    public float maxHeight;
    public bool goingUp;
    // Start is called before the first frame update
    //option 1: don't face towards camera and include shadow lloyd liked this one too
    //option 2: face towards camera and don't include shadow
    public void Start()
    {
        startingPosition = transform.localPosition;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (goingUp) {
            amountGoing += Time.deltaTime;
            transform.localPosition = startingPosition + amountGoing * Vector3.up;
            if (amountGoing >= maxHeight)
                goingUp = false;
        }
        else if (!goingUp)
        {
            amountGoing -= Time.deltaTime;
            transform.localPosition = startingPosition + amountGoing * Vector3.up;
            if (transform.localPosition.y <= startingPosition.y)
                goingUp = true;
        }
    }

}
