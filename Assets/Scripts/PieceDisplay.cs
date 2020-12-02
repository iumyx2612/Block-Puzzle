using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDisplay : MonoBehaviour
{
    public PieceData data;
    public SpriteRenderer sR;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData(PieceData data)
    {
        sR.sprite = data.sprite;
        //gameObject.AddComponent<BoxCollider2D>();
    }
}
