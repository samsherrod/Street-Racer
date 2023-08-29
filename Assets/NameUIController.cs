using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameUIController : MonoBehaviour
{
    public Text playerName;
    public Transform target;
    public float heightOffset = 1;

    CanvasGroup canvasGroup;
    public Renderer carRend;

    // Start is called before the first frame update
    void Start()
    {
        // worldPositionStays set to false makes the child keep its local
        // orientation rather than its global orientation.
        // https://docs.unity3d.com/ScriptReference/Transform.SetParent.html

        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        playerName = this.GetComponent<Text>();
        canvasGroup = this.GetComponent<CanvasGroup>();
    }

    private void LateUpdate()
    {
        if (carRend == null) return;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        bool carInView = GeometryUtility.TestPlanesAABB(planes, carRend.bounds);
        canvasGroup.alpha = carInView ? 1 : 0;

        // update object's position to that of the world position of the target (car) + a height offset
        this.transform.position = Camera.main.WorldToScreenPoint(target.position + new Vector3(0, heightOffset, 0));
    }
}
