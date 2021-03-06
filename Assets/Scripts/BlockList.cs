﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block List", menuName = "Data/Block List")]
public class BlockList : ScriptableObject
{
    public List<BlockData> blockDatas = new List<BlockData>();
    public float percentage;
    public int startingScore;
}
