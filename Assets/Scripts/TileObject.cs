using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    public PieceDisplay model;
    protected PieceData _data;
    public Vector2 position;

    public void addPieceData(PieceData data)
    {
        model.LoadData(data);
        model.gameObject.SetActive(data != null);
        _data = data;
    }

    public void Hover(PieceData data)
    {
        model.LoadData(data);
        model.gameObject.SetActive(data != null);
        Color tmp = model.gameObject.GetComponent<SpriteRenderer>().color;
        tmp.a = 0.6f;
        model.gameObject.GetComponent<SpriteRenderer>().color = tmp;
    }

    public bool isEmpty()
    {
        if (_data == null)
        {
            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
