using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameUIController : MonoBehaviour
{
    public Text playerName;
    public Transform target;
    public float heightOffset = 1;
    // Start is called before the first frame update
    void Start()
    {
        // worldPositionStays set to false makes the child keep its local
        // orientation rather than its global orientation.
        // https://docs.unity3d.com/ScriptReference/Transform.SetParent.html

        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        playerName = this.GetComponent<Text>();
    }

    private void LateUpdate()
    {
        // update object's position to that of the world position of the target (car) + a height offset
        this.transform.position = Camera.main.WorldToScreenPoint(target.position + new Vector3(0, heightOffset, 0));
    }
}
