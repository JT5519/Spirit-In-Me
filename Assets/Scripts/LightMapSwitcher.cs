using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
/*script to switch lighting data in the scene when demon appears, taken from LightMapper.cs on the web*/
public class LightMapSwitcher : MonoBehaviour
{
    //lightmap 1 data
    public Texture2D[] phase1Dir;
    public Texture2D[] phase1Color;
    //lightmap 2 data
    public Texture2D[] phase2Dir;
    public Texture2D[] phase2Color;

    //lightmap data structures for switching
    private LightmapData[] phase1LightMaps;
    private LightmapData[] phase2LightMaps;

    /*assets to store light probe data between the switches, found on youtube
      https://www.youtube.com/watch?v=BRapbR6vPII&t=298s : Birdmask Studios.
      The video was a great help in figuring out how to switch light probes along 
      with the lightmaps. They had some tools that made capturing lightprobes data
      really easy!*/
    public AlternativeLightingData phase1;
    public AlternativeLightingData phase2;

    //light component game objects changes
    public GameObject firstFloorRealtime;
    public GameObject firstFloorBaked;

    public GameObject corridorRealtime;
    public GameObject corridorBaked;

    public static bool isPhase1;
    private void Awake()
    {
        isPhase1 = true;
    }
    void Start()
    {
        if ((phase1Dir.Length != phase1Color.Length) || (phase2Dir.Length != phase2Color.Length))
        {
            Debug.Log("In order for LightMapSwitcher to work, the Near and Far LightMap lists must be of equal length");
            return;
        }

        // Sort the arrays in numerical order, so you can just blindly drag and drop them into the inspector
        phase1Dir = phase1Dir.OrderBy(t2d => t2d.name, new NaturalSortComparer<string>()).ToArray();
        phase1Color = phase1Color.OrderBy(t2d => t2d.name, new NaturalSortComparer<string>()).ToArray();
        phase2Dir = phase2Dir.OrderBy(t2d => t2d.name, new NaturalSortComparer<string>()).ToArray();
        phase2Color = phase2Color.OrderBy(t2d => t2d.name, new NaturalSortComparer<string>()).ToArray();

        // Put above data into LightMapData structures
        phase1LightMaps = new LightmapData[phase1Dir.Length];
        for (int i = 0; i < phase1Dir.Length; i++)
        {
            phase1LightMaps[i] = new LightmapData();
            phase1LightMaps[i].lightmapDir = phase1Dir[i];
            phase1LightMaps[i].lightmapColor = phase1Color[i];
        }

        phase2LightMaps = new LightmapData[phase2Dir.Length];
        for (int i = 0; i < phase2Dir.Length; i++)
        {
            phase2LightMaps[i] = new LightmapData();
            phase2LightMaps[i].lightmapDir = phase2Dir[i];
            phase2LightMaps[i].lightmapColor = phase2Color[i];
        }
        SetToDay();
    }

    #region Publics
    public void SetToDay() //first set of lighting data
    {
        //switching lightmaps and probes
        LightmapSettings.lightmaps = phase1LightMaps;
        LightmapSettings.lightProbes.bakedProbes = phase1.lightProbesData;
        //changing light game objects, player is in a location where they cant see the switch being made
        corridorBaked.SetActive(false);
        firstFloorBaked.SetActive(false);
        corridorRealtime.SetActive(true);
        firstFloorRealtime.SetActive(true);
    }

    public void SetToNight() //second set of lighting data
    {
        LightmapSettings.lightmaps = phase2LightMaps;
        LightmapSettings.lightProbes.bakedProbes = phase2.lightProbesData;
        corridorRealtime.SetActive(false);
        firstFloorRealtime.SetActive(false);
        corridorBaked.SetActive(true);
        firstFloorBaked.SetActive(true);
    }
    #endregion
    //used during testing to check switching of lights
    /*private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (isPhase1)
                SetToNight();
            else
                SetToDay();
            isPhase1 = !isPhase1;
        }
    }*/

    #region Debug
    [ContextMenu("Set to Night")]
    void Debug00()
    {
        SetToNight();
    }

    [ContextMenu("Set to Day")]
    void Debug01()
    {
        SetToDay();
    }
    #endregion
}

//taken from LightMapper.cs on the web
public class NaturalSortComparer<T> : IComparer<string>, IDisposable
{
    private readonly bool isAscending;

    public NaturalSortComparer(bool inAscendingOrder = true)
    {
        this.isAscending = inAscendingOrder;
    }

    #region IComparer<string> Members
    public int Compare(string x, string y)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region IComparer<string> Members
    int IComparer<string>.Compare(string x, string y)
    {
        if (x == y)
            return 0;

        string[] x1, y1;

        if (!table.TryGetValue(x, out x1))
        {
            x1 = Regex.Split(x.Replace(" ", ""), "([0-9]+)");
            table.Add(x, x1);
        }

        if (!table.TryGetValue(y, out y1))
        {
            y1 = Regex.Split(y.Replace(" ", ""), "([0-9]+)");
            table.Add(y, y1);
        }

        int returnVal;

        for (int i = 0; i < x1.Length && i < y1.Length; i++)
        {
            if (x1[i] != y1[i])
            {
                returnVal = PartCompare(x1[i], y1[i]);
                return isAscending ? returnVal : -returnVal;
            }
        }

        if (y1.Length > x1.Length)
        {
            returnVal = 1;
        }
        else if (x1.Length > y1.Length)
        {
            returnVal = -1;
        }
        else
        {
            returnVal = 0;
        }

        return isAscending ? returnVal : -returnVal;
    }

    private static int PartCompare(string left, string right)
    {
        int x, y;
        if (!int.TryParse(left, out x))
            return left.CompareTo(right);

        if (!int.TryParse(right, out y))
            return left.CompareTo(right);

        return x.CompareTo(y);
    }
    #endregion

    private Dictionary<string, string[]> table = new Dictionary<string, string[]>();

    public void Dispose()
    {
        table.Clear();
        table = null;
    }
}
