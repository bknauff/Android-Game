using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stacking : MonoBehaviour {

    private const float BOUND_SIZE = 3.5f;
    private GameObject[] Stack;
    private int stackIndex = 0;
    private int scoreCount = 0;
    private float itemTransition = 0.0f;
    private float itemSpeed = 2.5f;

	private void Start () {
        Stack = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Stack[i] = transform.GetChild(i).gameObject;
        }
        stackIndex = transform.childCount - 1;
    }
	
	private void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if (PlaceItem())
            {
                SpawnItem();
                scoreCount++;
            }
            else
            {
                EndGame();
            }
        }
        MoveItem();
    }

    private void MoveItem()
    {
        itemTransition += Time.deltaTime * itemSpeed;
        Stack[stackIndex].transform.localPosition = new Vector3(Mathf.Sin(itemTransition * BOUND_SIZE), scoreCount, 0);
    }

    private void SpawnItem()
    {
        stackIndex--;
        if(stackIndex < 0)
        {
            stackIndex = transform.childCount - 1;
        }
        Stack[stackIndex].transform.localPosition = new Vector3(0, scoreCount, 0);
    }

    private bool PlaceItem()
    {
        return true;
    }

    private void EndGame()
    {

    }
}
