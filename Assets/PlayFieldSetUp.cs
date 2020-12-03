using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFieldSetUp : MonoBehaviour
{
    [SerializeField] private int num_of_squares = 64;
    [SerializeField] private float xDist;
    [SerializeField] private float yDist;
    public GameObject squareField;
    [SerializeField] private Vector2 position;
    [SerializeField] private Vector2 basePosition;
    // Start is called before the first frame update
    void Awake()
    {
        position = basePosition;
        for (int i = 0; i < 8; i++)
        {
            if (i < 4)
            {
                for (int j = 0; j < 8; j++)
                {
                    GameObject newSquare = Instantiate(squareField, gameObject.transform);
                    if (j < 4)
                    {
                        newSquare.transform.localPosition = position;
                        position.x -= xDist;
                    }
                    if (j == 4)
                    {
                        position.x = basePosition.x + xDist;
                    }
                    if (j >= 4)
                    {
                        newSquare.transform.localPosition = position;
                        position.x += xDist;
                    }
                }
                position.x = basePosition.x;
                position.y += yDist;
            }
            if (i == 4)
            {
                position.y = basePosition.y - yDist;
            }
            if (i >= 4)
            {
                for (int j = 0; j < 8; j++)
                {
                    GameObject newSquare = Instantiate(squareField, gameObject.transform);
                    if (j < 4)
                    {
                        newSquare.transform.localPosition = position;
                        position.x -= xDist;
                    }
                    if (j == 4)
                    {
                        position.x = basePosition.x + xDist;
                    }
                    if (j >= 4)
                    {
                        newSquare.transform.localPosition = position;
                        position.x += xDist;
                    }
                }
                position.x = basePosition.x;
                position.y -= yDist;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
