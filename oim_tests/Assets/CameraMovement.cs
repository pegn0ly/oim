using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 Translation = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            Translation.y = 1;
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            Translation.y = -1;
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            Translation.x = -1;
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            Translation.x = 1;
        }
        else if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            Translation.y = 0;
        }
        else if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            Translation.x = 0;
        }
        Translation.z = Input.mouseScrollDelta.y;
        transform.Translate(Translation);

        //Translation = new Vector3(0, 0, 0);
    }
}
