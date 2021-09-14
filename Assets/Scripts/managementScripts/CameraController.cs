using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameManager gameManager;
    // Update is called once per frame
    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.mainCamera = this;
    }
    void Update()
    {
        if (!gameManager)
        {
            gameManager = FindObjectOfType<GameManager>();
            return;
        }
        else {
            if (gameManager.isPaused || !gameManager.playerTurn.acceptInstructions) {
                return;
            }
        }
        Vector3 cameraMovement = Vector3.zero;
        cameraMovement.x = Input.GetAxis("Horizontal");
        cameraMovement.z = Input.GetAxis("Vertical");
        cameraMovement.Normalize();
        if(gameManager.playerSettings)
            this.gameObject.transform.position = this.gameObject.transform.position + (cameraMovement*gameManager.playerSettings.CameraSpeed)/10;
    }
}
