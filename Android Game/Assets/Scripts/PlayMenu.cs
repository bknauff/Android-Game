using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenu : MonoBehaviour {

    void OnMouseDown()
    {
        Application.LoadLevel("GameLevel");
    }
}
