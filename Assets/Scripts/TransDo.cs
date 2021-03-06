﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*Script to swtich materials of objects in spirits path, to make them transparent. This is because
  the spirit can pass through walls and if every wall comes between the spirit and the third person camera
   then a sense of continuity is lost. So materials in a certain region around the spirit and camera will
   become transparent for the duration they are in that region, and then switch back to their original 
    materials*/

/*tried chaning the alpha values of the materials themselves but, on lightweight render pipeline, due to the
  different shader used from the standard one, dynamic transparency changing has alot of issues. After lots of surfing,
  and finding many bug reports and similar complaints, I just decided to make a duplicate pair of materials with a fixed 
  transparency*/
public class TransDo : MonoBehaviour
{
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

    private List<GameObject> objectList;
    // Update is called once per frame
    private void OnTriggerEnter(Collider other) //change to transparent
    {
        if (!StoryController.combatHasBegun)
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
            else if (other.gameObject.tag == "main door")
                other.gameObject.GetComponent<MeshRenderer>().material = maindoorT1;
            else if (other.gameObject.tag == "main door 2")
                other.gameObject.GetComponent<MeshRenderer>().material = maindoorT2;
            else if (other.gameObject.tag == "first floor")
                other.gameObject.GetComponent<MeshRenderer>().material = firstFloorT;
            else if (other.gameObject.tag == "shutter")
                other.gameObject.GetComponent<MeshRenderer>().material = shutterT;
        }
    }
    private void OnTriggerExit(Collider other) //change to original
    {
        if (!StoryController.combatHasBegun)
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
    private void Start()
    {
        if (StoryController.combatHasBegun)
        {
            objectList = new List<GameObject>(GameObject.FindGameObjectsWithTag("wall"));
            objectList.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("light")));
            objectList.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("floor")));
            objectList.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("stair")));
            objectList.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("door")));
            objectList.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("door 2")));
            objectList.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("main door")));
            objectList.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("main door 2")));
            objectList.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("first floor")));
            objectList.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("shutter")));
            foreach (GameObject obj in objectList)
            {
                if (obj.gameObject.tag == "wall")
                    obj.gameObject.GetComponent<MeshRenderer>().material = wallT;
                else if (obj.gameObject.tag == "light")
                    obj.gameObject.GetComponent<MeshRenderer>().material = lightT;
                else if (obj.gameObject.tag == "floor")
                    obj.gameObject.GetComponent<MeshRenderer>().material = floorT;
                else if (obj.gameObject.tag == "stair")
                    obj.gameObject.GetComponent<MeshRenderer>().material = stairT;
                else if (obj.gameObject.tag == "door")
                    obj.gameObject.GetComponent<MeshRenderer>().material = doorT1;
                else if (obj.gameObject.tag == "door 2")
                    obj.gameObject.GetComponent<MeshRenderer>().material = doorT2;
                else if (obj.gameObject.tag == "main door")
                    obj.gameObject.GetComponent<MeshRenderer>().material = maindoorT1;
                else if (obj.gameObject.tag == "main door 2")
                    obj.gameObject.GetComponent<MeshRenderer>().material = maindoorT2;
                else if (obj.gameObject.tag == "first floor")
                    obj.gameObject.GetComponent<MeshRenderer>().material = firstFloorT;
                else if (obj.gameObject.tag == "shutter")
                    obj.gameObject.GetComponent<MeshRenderer>().material = shutterT;
            }
        }
    }
    private void OnDestroy()
    {
        if(StoryController.combatHasBegun)
        {
            foreach (GameObject obj in objectList)
            {
                if (obj.gameObject.tag == "wall")
                    obj.gameObject.GetComponent<MeshRenderer>().material = wallOG;
                else if (obj.gameObject.tag == "light")
                    obj.gameObject.GetComponent<MeshRenderer>().material = lightOG;
                else if (obj.gameObject.tag == "floor")
                    obj.gameObject.GetComponent<MeshRenderer>().material = floorOG;
                else if (obj.gameObject.tag == "stair")
                    obj.gameObject.GetComponent<MeshRenderer>().material = stairOG;
                else if (obj.gameObject.tag == "door")
                    obj.gameObject.GetComponent<MeshRenderer>().material = doorOG1;
                else if (obj.gameObject.tag == "door 2")
                    obj.gameObject.GetComponent<MeshRenderer>().material = doorOG2;
                else if (obj.gameObject.tag == "main door")
                    obj.gameObject.GetComponent<MeshRenderer>().material = maindoorOG1;
                else if (obj.gameObject.tag == "main door 2")
                    obj.gameObject.GetComponent<MeshRenderer>().material = maindoorOG2;
                else if (obj.gameObject.tag == "first floor")
                    obj.gameObject.GetComponent<MeshRenderer>().material = firstFloorOG;
                else if (obj.gameObject.tag == "shutter")
                    obj.gameObject.GetComponent<MeshRenderer>().material = shutterOG;
            }
        }
    }
}
