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
    public float rollTrapRatio;
    [Range(0, 1)]
    public float centralTrapRatio;

    List<GameObject> addedModules;
    GameObject nextModule;
    int dangerMeter;
    bool rest = true;

    // Start is called before the first frame update
    void Start()
    {
        addedModules = new List<GameObject>();
        nextModule = Instantiate(new GameObject(),Vector3.zero,Quaternion.identity,transform);

        AddModule(moduleList[1]);

        for (int i = 0; i < 10; i++)
        {
            AddModule();
        }
        SwitchMaterial();
        dangerMeter = 0;
    }

    void Update()
    {
        if(dangerMeter >= maxDanger)
        {
            rest = true;
        }
        else if(dangerMeter <= 0)
        {
            rest = false;
        }

        if(addedModules.Count < 20)
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
        GameObject firstModule;
        do
        {
            firstModule = addedModules[0];
            addedModules.Remove(firstModule);
            Destroy(firstModule);
        }
        while (firstModule.name == moduleList[0].name + "(Clone)");
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
            tilePrefab = ModuleGenerator();
        }

        GameObject currentModule = Instantiate(tilePrefab);
        Module currentModuleScript = currentModule.GetComponent<Module>();

        Vector3 currentPath = nextModule.transform.rotation * currentModuleScript.Path;
        nextModule.transform.position += currentPath / 2;

        currentModule.transform.position = nextModule.transform.position;
        currentModule.transform.rotation = nextModule.transform.rotation;
        currentModule.transform.parent = transform;

        nextModule.transform.position += currentPath / 2;
        nextModule.transform.rotation *= currentModuleScript.Axis;

        dangerMeter += currentModuleScript.dangerIndex;
        print("DANGER METER : " + dangerMeter);

        addedModules.Add(currentModule);
    }

    GameObject ModuleGenerator()
    {
        GameObject currentModule;
        string lastModulename = addedModules[addedModules.Count - 1].name;

        bool rotationCombo = Random.value <= turnRatio;
        bool rollingCombo = Random.value <= rollTrapRatio;
        bool centralCombo = Random.value <= centralTrapRatio;


        if (lastModulename == moduleList[2].name + "(Clone)" && rotationCombo)
        {
            currentModule = moduleList[2];
        }
        else if (rest)
        {
            currentModule = moduleList[Random.Range(1, 3)];
            RandomRotation();
        }
        else
        {
            if (lastModulename == moduleList[9].name + "(Clone)" && rollingCombo)
            {
                currentModule = moduleList[9];
            }
            else if (lastModulename == moduleList[4].name + "(Clone)" && centralCombo)
            {
                AddModule(moduleList[0]);
                currentModule = moduleList[4];
                RandomRotation();
            }
            else
            {
                currentModule = RandomModule();
            }
        }
        
        return currentModule;
    }

    GameObject RandomModule()
    {
        GameObject currentModule;
        print("MAX DANGER : "+Mathf.RoundToInt(maxDanger));

        for (int i = 25; i > Mathf.RoundToInt(maxDanger); i -= 5)
        {
            AddModule(moduleList[0]);
        }

        int Range;
        if(maxDanger <= 10)
        {
            Range = moduleList.Length  - (2 + (int)maxDanger /5) ;
        }
        else
        {
            Range = moduleList.Length;
        }
        print(Range);

        currentModule = moduleList[Random.Range(3, Range)];
        RandomRotation();

        return currentModule;
    }

    public void SwitchMaterial()
    {
        //foreach(GameObject module in moduleList)
        //{

        //}
        GameObject test = (GameObject)Instantiate(Resources.Load("Assets/Prefabs/NeutralPipe/DarkNeutralPipe", typeof(GameObject)));
        print(Resources.Load("Assets/Prefabs/NeutralPipe/DarkNeutralPipe"));
        moduleList[1] = test;
    }
}