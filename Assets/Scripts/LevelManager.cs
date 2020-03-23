using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public float maxDanger;
    public GameObject playerRef;
    public GameObject[] moduleList;

    [Header("Modules Combos Ratio")]
    [Range(0, 1)]
    public float turnRatio;
    [Range(0, 1)]
    public float rollRatio;

    List<GameObject> addedModules;
    GameObject nextModule;
    int dangerMeter = 0;

    // Start is called before the first frame update
    void Start()
    {
        addedModules = new List<GameObject>();
        nextModule = Instantiate(new GameObject(),Vector3.zero,Quaternion.identity,transform);

        AddModule(moduleList[0]);

        for (int i = 0; i < 15; i++)
        {
            AddModule();
        }
    }

    public void CompareModule(GameObject module)
    {
        if (module == addedModules[8])
        {
            AddModule();
            RemoveFirstModule();
        }
    }

    void RandomRotation()
    {
        nextModule.transform.Rotate(nextModule.transform.right, Random.Range(-4, 5) * 36, Space.World);
    }

    void RemoveFirstModule()
    {
        GameObject firstModule = addedModules[0];
        addedModules.Remove(firstModule);
        Destroy(firstModule);
    }

    void AddModule([Optional]GameObject module)
    {
        GameObject tilePrefab;
        if (module != null)
        {
            tilePrefab = module;
        }
        else
        {
            tilePrefab = RandomModule();
        }

        GameObject currentTile = Instantiate(tilePrefab);

        Vector3 currentPath = nextModule.transform.rotation * currentTile.GetComponent<Module>().Path;
        nextModule.transform.position += currentPath / 2;

        currentTile.transform.position = nextModule.transform.position;
        currentTile.transform.rotation = nextModule.transform.rotation;
        currentTile.transform.parent = transform;

        nextModule.transform.position += currentPath / 2;
        nextModule.transform.rotation *= currentTile.GetComponent<Module>().Axis;

        addedModules.Add(currentTile);
    }

    GameObject RandomModule()
    {
        GameObject currentModule;
        string lastModulename = addedModules[addedModules.Count - 1].name;

        bool rotationCombo = Random.value <= turnRatio;
        bool rollingCombo = Random.value <= rollRatio;


        if (lastModulename == moduleList[1].name + "(Clone)" && rotationCombo)
        {
            currentModule = moduleList[1];
        }
        else if (dangerMeter >= maxDanger)
        {
            currentModule = moduleList[Random.Range(0, 2)];
            RandomRotation();
        }
        else if (lastModulename == moduleList[3].name + "(Clone)" && rollingCombo)
        {
            currentModule = moduleList[3];
        }
        else
        {
            currentModule = moduleList[Random.Range(0, moduleList.Length)];
            RandomRotation();
        }

        dangerMeter += currentModule.GetComponent<Module>().dangerIndex;

        return currentModule;
    }

}