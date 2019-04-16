using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static List<GameObject> Players;
    public static Color CurrentFusionnedColor;
    public static float PlateformFadingTime;
    public static int CurrentStageId;

    public GameObject PlateformHighlighterModel;
    public GameObject PlayerModel;
    public float HighlighterInterval = 1;
    public float MusicDelay;
    public AudioClip PlayedMusic;
    public AudioSource MainAudioSource;
    public ParticleSystem AudiowaveParticleSystem;
    public new Transform camera;
    public float LavaHeight;

    private List<List<GameObject>> plateformsPerStage;
    private List<GameObject> highlighters;
    private GameObject level;
    private float highlighterElapsedTime;
    private GameObject highlightersParent;
    private GameObject charactersParent;
    private CameraBehavior cameraBehavior;

    /// <summary>
    /// Initialize level and audio
    /// </summary>
    private void Start()
    {
        PlateformFadingTime = HighlighterInterval + HighlighterInterval * 0.1f;

        var audiowaveEmission = AudiowaveParticleSystem.emission;
        audiowaveEmission.rateOverTime = 1f / HighlighterInterval;

        Players = new List<GameObject>();
        plateformsPerStage = new List<List<GameObject>>();
        highlighters = new List<GameObject>();
        level = GetComponent<Transform>().gameObject;
        CurrentStageId = 0;
        highlightersParent = new GameObject("Highlighters");
        charactersParent = new GameObject("Characters");

        CreateLevelStructure();
        CreatePlayers(2);
        ReplaceParticleSystem();

        cameraBehavior = new CameraBehavior(camera, level, Players[0].transform, Players[1].transform, 0.3f);

        MainAudioSource.clip = PlayedMusic;
        MainAudioSource.PlayDelayed(MusicDelay);

        //PrintAllLevelPlateforms();
    }

    /// <summary>
    /// Call update methods every frame
    /// </summary>
    private void Update()
    {
        AudioManager();
        UpdateFusionnedColor();
        HighlighterGenerator();
        UpdateExistingHighlighters();
        RespawnFallingPlayers();

        cameraBehavior.UpdateCamera();
        int stageId = cameraBehavior.GetCameraIndex();
        if (stageId >= 0)
        {
            CurrentStageId = cameraBehavior.GetCameraIndex();
        }
        ReplaceParticleSystem();
    }

    /// <summary>
    /// Check when the audio source finished playing the clip.
    /// Resync music with objects.
    /// </summary>
    private void AudioManager()
    {
        if (!MainAudioSource.isPlaying)
        {
            highlighterElapsedTime = 0;
            AudiowaveParticleSystem.Play();

            MainAudioSource.PlayDelayed(MusicDelay);
        }
    }

    /// <summary>
    /// Remove all destroyed highlighters from the list
    /// </summary>
    private void UpdateExistingHighlighters()
    {
        for (int i = highlighters.Count - 1; i >= 0; i--)
        {
            if (highlighters[i] == null)
            {
                highlighters.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Update the current possible colors
    /// </summary>
    private void UpdateFusionnedColor()
    {
        Vector3 fusionnedColor = new Vector3();

        // The color of each player
        foreach (GameObject player in Players)
        {
            Color currentColor = player.GetComponent<ColorChanger>().GetColor();
            fusionnedColor += new Vector3(currentColor.r, currentColor.g, currentColor.b);
        }

        if(fusionnedColor.x > 0)
        {
            fusionnedColor.x = 1;
        }
        if(fusionnedColor.y > 0)
        {
            fusionnedColor.y = 1;
        }
        if(fusionnedColor.z > 0)
        {
            fusionnedColor.z = 1;
        }
        
        CurrentFusionnedColor = new Color(fusionnedColor.x, fusionnedColor.y, fusionnedColor.z);

        ParticleSystem.MainModule settings = AudiowaveParticleSystem.main;
        settings.startColor = new ParticleSystem.MinMaxGradient(CurrentFusionnedColor);
    }

    /// <summary>
    /// Generate a new highlighter when it's needed
    /// </summary>
    private void HighlighterGenerator()
    {
        highlighterElapsedTime += Time.deltaTime;

        if (highlighterElapsedTime > HighlighterInterval)
        {
            highlighterElapsedTime = highlighterElapsedTime - HighlighterInterval;

            Vector3 currentStageStart = plateformsPerStage[CurrentStageId][0].transform.position;

            GameObject highlighter = Instantiate(PlateformHighlighterModel);
            highlighter.transform.parent = highlightersParent.transform;
            highlighter.transform.position = currentStageStart;
            highlighter.GetComponent<Highlighter>().color = CurrentFusionnedColor;

            highlighters.Add(highlighter);
        }
    }
    
    /// <summary>
    /// Replace the particle system to the current start plateform
    /// </summary>
    private void ReplaceParticleSystem()
    {
        AudiowaveParticleSystem.transform.position = plateformsPerStage[CurrentStageId][0].transform.position;
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
            player.transform.parent = charactersParent.transform;
            player.transform.position = plateformsPerStage[CurrentStageId][0].transform.position +
                Vector3.up * 1.5f + Vector3.right * (2 * i - 1);
            player.name = "Player" + (i + 1);

            player.GetComponentInChildren<SkinnedMeshRenderer>().material = new Material(PlayerModel.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial);
            player.GetComponent<Character>().PlayerId = i + 1;
            Players.Add(player);
        }
    }

    /// <summary>
    /// Check when a player is falling and needs to be respawned
    /// </summary>
    private void RespawnFallingPlayers()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].transform.position.y <= LavaHeight)
            {
                Players[i].GetComponent<Character>().WarpToPosition(plateformsPerStage[CurrentStageId][0].transform.position +
                    Vector3.up * 1.5f + Vector3.right * (2 * i - 1));
            }
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
                    List<GameObject> tmpStagePlateforms = new List<GameObject>
                    {
                        startPlateform
                    };
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
