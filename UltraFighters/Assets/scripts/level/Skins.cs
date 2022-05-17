using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skin
{
    public string skinName;
    public string playerName;
    public Sprite downTexture;
    public AnimatorOverrideController movement;
    public AnimatorOverrideController shooting;
}
