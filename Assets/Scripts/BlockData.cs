using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block", menuName = "Data/Block")]
public class BlockData : ScriptableObject
{
    public List<Vector2> points = new List<Vector2>();
    public Vector2 offset;
    public int possibleRotation;
}
