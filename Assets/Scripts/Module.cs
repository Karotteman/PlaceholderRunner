using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    public int dangerIndex;

    List<Transform> collectiblesTab;
    Quaternion axis;
    Vector3 path;
    
    public Vector3 Path { get { return path; } }
    public Quaternion Axis { get { return axis; } }

    // Start is called before the first frame update
    void Awake()
    {
        path = transform.GetChild(2).position - transform.GetChild(0).position;
        axis = transform.GetChild(2).rotation;
    }
    void Start()
    {
        collectiblesTab = new List<Transform>();
        Transform currentChild;

        for (int i = 0; i < transform.childCount; i++)
        {
            currentChild = transform.GetChild(i);

            if (currentChild.tag == "Collectible")
            {
                currentChild.rotation = Quaternion.identity;
                collectiblesTab.Add(currentChild);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Transform collectible in collectiblesTab)
        {
            collectible.Rotate(Vector3.up, 2, Space.World);
        }
    }
}
