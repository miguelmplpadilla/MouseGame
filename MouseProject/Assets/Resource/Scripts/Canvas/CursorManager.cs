using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{

    public Texture2D cursor_OnButton;
     Vector2 normalCursorSpot;

    public Texture2D cursor_Normal;
     Vector2 onButtonCursorHotSpot;

    void Start()
    {
        onButtonCursorHotSpot = new Vector2(1, 0);
    }

    void Update()
    {
        
    }

    public void OnButtonCursorEnter()
    {
        Cursor.SetCursor(cursor_OnButton, onButtonCursorHotSpot, CursorMode.Auto);
    }

    public void OnButtonCursorExit()
    {
        Cursor.SetCursor(cursor_Normal, normalCursorSpot, CursorMode.Auto);
    }
}
