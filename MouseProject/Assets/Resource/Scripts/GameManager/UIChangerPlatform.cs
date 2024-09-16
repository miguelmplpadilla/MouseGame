using System;
using UnityEngine;
using UnityEngine.UI;

public class UIChangerPlatform : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Image image;

    public Sprite spriteKeyboard;
    public Sprite spriteGamepad;
    public Sprite spriteMobile;

    private void Update()
    {
        if (Input.GetJoystickNames().Length > 0)
        {
            if (spriteRenderer != null) spriteRenderer.sprite = spriteGamepad;
            if (image != null) image.sprite = spriteGamepad;
        }
        else
        {
            if (spriteRenderer != null) spriteRenderer.sprite = spriteKeyboard;
            if (image != null) image.sprite = spriteKeyboard;
        }

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (spriteRenderer != null) spriteRenderer.sprite = spriteMobile;
            if (image != null) image.sprite = spriteMobile;
        }
    }
}
