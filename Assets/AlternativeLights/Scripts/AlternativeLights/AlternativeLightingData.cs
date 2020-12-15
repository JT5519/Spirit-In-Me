using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


[CreateAssetMenu(fileName = "AlternativeLightData", menuName = "Swapping Lightmaps/AlternativeLightData", order = 1)]

public class AlternativeLightingData : ScriptableObject
{
    [Header ("Stored Light Maps")]
    [SerializeField]
    public Texture2D[] l_Light = new Texture2D[1];
    [SerializeField]
    public Texture2D[] l_Dir = new Texture2D[1];

    [Header("Stored Light Probes Settings")]
    [SerializeField]
    public SphericalHarmonicsL2[] lightProbesData;

   // [SerializeField]
   // public LightmapData refLightmapData;

#if UNITY_EDITOR
    //The EditorWindow_GetAlternativeLightingData script uses these value to temporary store the found textures
    [HideInInspector]
    public List<Texture2D> l_LightTemp;
    [HideInInspector]
    public List<Texture2D> l_DirTemp;
#endif
}
