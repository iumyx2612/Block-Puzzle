using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceScale : MonoBehaviour
{
    public SpriteRenderer sR;
    public float desiredX = 1f;
    public float desiredY = 1f;
    public float XscaleFactor;
    public float YscaleFactor;
    // Start is called before the first frame update
    void Start()
    {
        XscaleFactor = desiredX / sR.sprite.bounds.size.x;
        YscaleFactor = desiredY / sR.sprite.bounds.size.y;
        transform.localScale = new Vector2(XscaleFactor, YscaleFactor);
    }
}
