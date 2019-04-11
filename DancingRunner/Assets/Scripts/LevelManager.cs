using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static List<GameObject> Players;
    public static List<Color> CurrentPossibleColor;

    public GameObject PlateformHighlighterModel;
    public GameObject PlayerModel;

    private List<List<GameObject>> plateformsPerStage;
    private List<GameObject> highlighters;
    private GameObject level;
    private float highlighterElapsedTime;
    private float highlighterInterval;
    private int currentStageId;
    private GameObject highlightersParent;

    // Start is called before the first frame update
    private void Start()
    {
        Players = new List<GameObject>();
        plateformsPerStage = new List<List<GameObject>>();
        highlighters = new List<GameObject>();
        level = GetComponent<Transform>().gameObject;
        highlighterInterval = 1;
        currentStageId = 0;
        highlightersParent = new GameObject("Highlighters");

        CreateLevelStructure();
        CreatePlayers(1);

        //PrintAllLevelPlateforms();
    }

    // Update is called once per frame
    private void Update()
    {
        HighlighterGenerator();
        UpdatePossibleColors();
    }

    /// <summary>
    /// Update the current possible colors
    /// </summary>
    private void UpdatePossibleColors()
    {
        List<Color> colors = new List<Color>();
        Vector3 fusionnedColor = new Vector3();

        // The color of each player
        foreach (GameObject player in Players)
        {
            Color currentColor = player.GetComponent<ColorChanger>().GetColor();
            colors.Add(currentColor);
            fusionnedColor += new Vector3(currentColor.r, currentColor.g, currentColor.b);
        }

        // The fusionned color based on the color of each players
        fusionnedColor /= Players.Count;
        colors.Add(new Color(fusionnedColor.x, fusionnedColor.y, fusionnedColor.z));

        CurrentPossibleColor = colors;
    }

    /// <summary>
    /// Create all the players needed in the game
    /// </summary>
    /// <param name="numberOfPlayer"></param>
    private void CreatePlayers(int numberOfPlayer)
    {
        for (int i = 0; i < numberOfPlayer; i++)
        {
            GameObject player = Instantiate(PlayerModel);
            player.transform.position = plateformsPerStage[currentStageId][0].transform.position +
                Vector3.up * player.GetComponent<CharacterController>().height * (i + 2) * 1.5f;

            Players.Add(player);
        }
    }

    /// <summary>
    /// Generate a new highlighter when it's needed
    /// </summary>
    private void HighlighterGenerator()
    {
        highlighterElapsedTime += Time.deltaTime;

        if (highlighterElapsedTime > highlighterInterval)
        {
            highlighterElapsedTime = 0;

            Vector3 currentStageStart = plateformsPerStage[currentStageId][0].transform.position;

            GameObject highlighter = Instantiate(PlateformHighlighterModel);
            highlighter.transform.parent = highlightersParent.transform;
            highlighter.transform.position = currentStageStart;

            highlighters.Add(highlighter);
        }
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
            if (levelChild.tag == "Stage")
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

        if (plateformsPerStage.Count <= 0)
        {
            Debug.Log("Error during level creation! All level should have a number of stage > 0.");
        }
    }

    /// <summary>
    /// Print all level plateforms in the console.
    /// </summary>
    private void PrintAllLevelPlateforms()
    {
        string result = "";
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
