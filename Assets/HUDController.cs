using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    CanvasGroup canvasGroup;
    float HUDSetting = 0;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
