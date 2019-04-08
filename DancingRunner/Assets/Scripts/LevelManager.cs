using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private List<List<GameObject>> plateformsPerStage;
    private GameObject level;
    private GameObject plateformHighlighter;

    // Start is called before the first frame update
    private void Start()
    {
        plateformsPerStage = new List<List<GameObject>>();
        level = GetComponent<Transform>().gameObject;
        plateformHighlighter = null;

        CreateLevelStructure();

        //PrintAllLevelPlateforms();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    /// <summary>
    /// Create the level structure
    /// </summary>
    private void CreateLevelStructure()
    {
        GameObject startPlateform = null;
        List<GameObject> stagePlateforms = new List<GameObject>();

        foreach (Transform levelChild in level.transform)
        {
            if (levelChild.tag == "PlateformHighlighter")
            {
                plateformHighlighter = levelChild.gameObject;
            }
            else
            {
                foreach (Transform stageChild in levelChild)
                {
                    if (stageChild.tag == "Start")
                    {
                        startPlateform = stageChild.gameObject;
                    }
                    else if (stageChild.tag == "Plateform")
                    {
                        stagePlateforms.Add(stageChild.gameObject);
                    }
                }

                if (startPlateform != null && stagePlateforms.Count > 0)
                {
                    List<GameObject> tmpStagePlateforms = new List<GameObject>();
                    tmpStagePlateforms.Add(startPlateform);
                    tmpStagePlateforms.AddRange(stagePlateforms);

                    plateformsPerStage.Add(tmpStagePlateforms);
                }
                else
                {
                    Debug.Log("Error during level creation! All stage should have a starting plateform and a number of plateforms > 0.");
                }

                startPlateform = null;
                stagePlateforms.Clear();
            }
        }

        if (plateformHighlighter == null || plateformsPerStage.Count <= 0)
        {
            Debug.Log("Error during level creation! All level should have a highlighter plateform and a number of stage > 0.");
        }
    }

    private void MovePlateformHighlighter()
    {

    }

    /// <summary>
    /// Print all level plateforms in the console.
    /// </summary>
    private void PrintAllLevelPlateforms()
    {
        string result = "";
        result += plateformHighlighter.name + "\n";

        int counter = 0;
        foreach (List<GameObject> item in plateformsPerStage)
        {
            counter++;
            result += "Stage " + counter + "\n";
            foreach (GameObject plateform in item)
            {
                result += plateform.name + "\n";
            }
        }

        Debug.Log(result);
    }
}
