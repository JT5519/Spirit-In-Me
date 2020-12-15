using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransDo : MonoBehaviour
{
    // Start is called before the first frame update
    public Material wallOG;
    public Material wallT;

    public Material doorOG1;
    public Material doorT1;

    public Material doorOG2;
    public Material doorT2;

    public Material stairOG;
    public Material stairT;

    public Material lightOG;
    public Material lightT;

    public Material floorOG;
    public Material floorT;

    public Material firstFloorOG;
    public Material firstFloorT;

    public Material maindoorOG1;
    public Material maindoorOG2;
    public Material maindoorT1;
    public Material maindoorT2;

    public Material shutterOG;
    public Material shutterT;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "wall")
            other.gameObject.GetComponent<MeshRenderer>().material = wallT;
        else if (other.gameObject.tag == "light")
            other.gameObject.GetComponent<MeshRenderer>().material = lightT;
        else if (other.gameObject.tag == "floor")
            other.gameObject.GetComponent<MeshRenderer>().material = floorT;
        else if (other.gameObject.tag == "stair")
            other.gameObject.GetComponent<MeshRenderer>().material = stairT;
        else if (other.gameObject.tag == "door")
            other.gameObject.GetComponent<MeshRenderer>().material = doorT1;
        else if (other.gameObject.tag == "door 2")
            other.gameObject.GetComponent<MeshRenderer>().material = doorT2;
        else if(other.gameObject.tag == "main door")
            other.gameObject.GetComponent<MeshRenderer>().material = maindoorT1;
        else if (other.gameObject.tag == "main door 2")
            other.gameObject.GetComponent<MeshRenderer>().material = maindoorT2;
        else if (other.gameObject.tag == "first floor")
            other.gameObject.GetComponent<MeshRenderer>().material = firstFloorT;
        else if (other.gameObject.tag == "shutter")
            other.gameObject.GetComponent<MeshRenderer>().material = shutterT;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "wall")
            other.gameObject.GetComponent<MeshRenderer>().material = wallOG;
        else if (other.gameObject.tag == "light")
            other.gameObject.GetComponent<MeshRenderer>().material = lightOG;
        else if (other.gameObject.tag == "floor")
            other.gameObject.GetComponent<MeshRenderer>().material = floorOG;
        else if (other.gameObject.tag == "stair")
            other.gameObject.GetComponent<MeshRenderer>().material = stairOG;
        else if (other.gameObject.tag == "door")
            other.gameObject.GetComponent<MeshRenderer>().material = doorOG1;
        else if (other.gameObject.tag == "door 2")
            other.gameObject.GetComponent<MeshRenderer>().material = doorOG2;
        else if (other.gameObject.tag == "main door")
            other.gameObject.GetComponent<MeshRenderer>().material = maindoorOG1;
        else if (other.gameObject.tag == "main door 2")
            other.gameObject.GetComponent<MeshRenderer>().material = maindoorOG2;
        else if (other.gameObject.tag == "first floor")
            other.gameObject.GetComponent<MeshRenderer>().material = firstFloorOG;
        else if (other.gameObject.tag == "shutter")
            other.gameObject.GetComponent<MeshRenderer>().material = shutterOG;
    }
}
