using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Stacking : MonoBehaviour {

    public Text score;
    private const float BOUND_SIZE = 3.5f;
    private GameObject[] Stack;
    private int stackIndex = 0;
    private int scoreCount = 0;
    private float itemTransition = 0.0f;
    private float itemSpeed = 2.5f;
    private bool isMovingX = true;
    private float secPosition;
    private Vector3 Iposition;
    private const float SPEED = 5.0f;
    private Vector3 lastPosition;
    private const float ERR_MARGIN = 0.1f;
    private int combo;
    private Vector2 bounds = new Vector2(BOUND_SIZE, BOUND_SIZE);
    private bool gameOver = false;
    private const float BOUND_GAIN = .15f;
    private const float START_GAIN = 5;
    public GameObject uiPanel;


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
        //if(Input.touchCount == 1)
        {
            if (PlaceItem())
            {
                SpawnItem();
                scoreCount++;
                score.text = scoreCount.ToString();
            }
            else
            {
                EndGame();
            }
        }
        MoveItem();

        //Moving the stack
        transform.position = Vector3.Lerp(transform.position, Iposition, SPEED - Time.deltaTime);
    }

    private void MoveItem()
    {
        if (gameOver)
        {
            return;
        }
        itemTransition += Time.deltaTime * itemSpeed;
        if (isMovingX)
        {
            Stack[stackIndex].transform.localPosition = new Vector3(Mathf.Sin(itemTransition) * BOUND_SIZE, scoreCount, secPosition);
        }
        else
        {
            Stack[stackIndex].transform.localPosition = new Vector3(secPosition, scoreCount, Mathf.Sin(itemTransition) * BOUND_SIZE);
        }
    }

    private void SpawnItem()
    {
        lastPosition = Stack[stackIndex].transform.localPosition;
        stackIndex--;
        if(stackIndex < 0)
        {
            stackIndex = transform.childCount - 1;
        }
        Iposition = (Vector3.down) * scoreCount;
        Stack[stackIndex].transform.localPosition = new Vector3(0, scoreCount, 0);
        Stack[stackIndex].transform.localScale = new Vector3(bounds.x, 1, bounds.y);
    }

    private bool PlaceItem()
    {
        Transform t = Stack[stackIndex].transform;

        if (isMovingX)
        {
            float deltaX = lastPosition.x - t.position.x;
            if(Mathf.Abs(deltaX) > ERR_MARGIN)
            {
                //Get rid of part of item that is not in line with item below it
                combo = 0;
                bounds.x -= Mathf.Abs(deltaX);
                if(bounds.x <= 0)
                {
                    return false;
                }
                float middle = lastPosition.x + t.localPosition.x / 2;
                t.localScale = new Vector3(bounds.x, 1, bounds.y);
                t.localPosition = new Vector3(middle - (lastPosition.x / 2), scoreCount, lastPosition.z);
            }
            else
            {
                if(combo > START_GAIN)
                {
                    bounds.x += BOUND_GAIN;
                    if(bounds.x > BOUND_SIZE)
                    {
                        bounds.x = BOUND_SIZE;
                    }
                    float middle = lastPosition.x + t.localPosition.x / 2;
                    t.localScale = new Vector3(bounds.x, 1, bounds.y);
                    t.localPosition = new Vector3(middle - (lastPosition.x / 2), scoreCount, lastPosition.z);
                }
                combo++;
                t.localPosition = new Vector3(lastPosition.x, scoreCount, lastPosition.z);
            }
        }
        else
        {
            float deltaZ = lastPosition.z - t.position.z;
            if (Mathf.Abs(deltaZ) > ERR_MARGIN)
            {
                //Get rid of part of item that is not in line with item below it
                combo = 0;
                bounds.y -= Mathf.Abs(deltaZ);
                if (bounds.y <= 0)
                {
                    return false;
                }
                float middle = lastPosition.z + t.localPosition.z / 2;
                t.localScale = new Vector3(bounds.x, 1, bounds.y);
                t.localPosition = new Vector3(lastPosition.x, scoreCount,middle - (lastPosition.z/2));
            }
            else
            {
                if (combo > START_GAIN)
                {
                    if (bounds.y > BOUND_SIZE)
                    {
                        bounds.y = BOUND_SIZE;
                    }
                    bounds.y += BOUND_GAIN;
                    float middle = lastPosition.z + t.localPosition.z / 2;
                    t.localScale = new Vector3(bounds.x, 1, bounds.y);
                    t.localPosition = new Vector3(lastPosition.x, scoreCount, middle - (lastPosition.z/2));
                }
                combo++;
                t.localPosition = new Vector3(lastPosition.x, scoreCount, lastPosition.z);
            }
        }
        secPosition = (isMovingX)
            ? t.localPosition.x
            : t.localPosition.z;
        isMovingX = !isMovingX;
        return true;
    }

    private void EndGame()
    {
        if(PlayerPrefs.GetInt("score") < scoreCount)
        {
            PlayerPrefs.SetInt("score", scoreCount);
        }
        gameOver = true;
        uiPanel.SetActive(true);
        Stack[stackIndex].AddComponent<Rigidbody>();
    }

    public void NewGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

