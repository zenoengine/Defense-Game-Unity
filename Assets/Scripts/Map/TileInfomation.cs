using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfomation : MonoBehaviour {
    public TILESTYLE currentTileStyle;
    public Material[] tileMaterials;

    public void UpdateMaterial()
    {
        GetComponent<Renderer>().material = tileMaterials[(int)currentTileStyle];
    }
}
