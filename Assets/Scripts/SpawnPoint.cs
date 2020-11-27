using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Spawn Point")]
public class SpawnPoint : ScriptableObject
{
    public List<Vector2> spawnPoints = new List<Vector2>();
}
