using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDisplay : MonoBehaviour
{
    public PieceData data;
    public SpriteRenderer sR;
    // Start is called before the first frame update
    private void OnEnable()
    {
        //Debug.Log(transform.position);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData(PieceData data)
    {
        if (data == null) return;
        sR.sprite = data.sprite;
        this.data = data;
    }
}
