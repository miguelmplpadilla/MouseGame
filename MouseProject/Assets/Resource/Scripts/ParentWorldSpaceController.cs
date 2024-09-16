using UnityEngine;

public class ParentWorldSpaceController : MonoBehaviour
{
    public bool activeInStart = true;
    
    void Start()
    {
        float normalAsspectRatio = (float)1920 / 1080;
        float asspectRatio = (float)Screen.width / Screen.height;

        float newScale = asspectRatio / normalAsspectRatio;

        transform.localScale = new Vector3(newScale,newScale,newScale);
        
        gameObject.SetActive(activeInStart);
    }
}
