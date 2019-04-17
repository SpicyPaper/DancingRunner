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
    private Dictionary<int, List<GameObject>> ghosties;

    private const float SPEED_FACTOR = 100;

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
        ghosties = new Dictionary<int, List<GameObject>>();

        CreateLevelStructure();
        CreatePlayers(2);
        ReplaceParticleSystem();

        cameraBehavior = new CameraBehavior(Camera.main.transform, level, Players[0].transform, Players[1].transform, 0.3f);

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
        MoveGhosties();

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
        if (plateformsPerStage[CurrentStageId][0].GetComponent<PlayerOnActivatorDetector>().MustShowAllPlatforms())
        {
            CurrentFusionnedColor = Color.white;
        }
        else
        {
            Vector3 fusionnedColor = new Vector3();

            // The color of each player
            foreach (GameObject player in Players)
            {
                Color currentColor = player.GetComponent<ColorChanger>().GetColor();
                fusionnedColor += new Vector3(currentColor.r, currentColor.g, currentColor.b);
            }

            if (fusionnedColor.x > 0)
            {
                fusionnedColor.x = 1;
            }
            if (fusionnedColor.y > 0)
            {
                fusionnedColor.y = 1;
            }
            if (fusionnedColor.z > 0)
            {
                fusionnedColor.z = 1;
            }

            CurrentFusionnedColor = new Color(fusionnedColor.x, fusionnedColor.y, fusionnedColor.z);
        }

        GameObject.Find("Helmet").GetComponent<MeshRenderer>().sharedMaterial.SetColor("_EmissionColor", CurrentFusionnedColor);
        GameObject.Find("Helmet").GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Color", CurrentFusionnedColor);
        GameObject.Find("Note").GetComponent<ParticleSystemRenderer>().sharedMaterial.SetColor("_Color", CurrentFusionnedColor);


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
    /// Move all Ghosties present in the active stage
    /// </summary>
    private void MoveGhosties()
    {
        List<GameObject> activeGhosties = ghosties[CurrentStageId];

        foreach (GameObject ghosty in activeGhosties)
        {
            Ghosty ghostyScript = ghosty.GetComponent<Ghosty>();

            if (CurrentFusionnedColor == ghosty.GetComponent<Ghosty>().color)
            {
                Vector3 posPlayer1 = Players[0].transform.position;
                Vector3 posPlayer2 = Players[1].transform.position;
                Vector3 posGhosty = ghosty.transform.position;

                Vector3 headingPlayer1 = posPlayer1 - posGhosty;
                Vector3 headingPlayer2 = posPlayer2 - posGhosty;

                float distPlayer1 = headingPlayer1.magnitude;
                float distPlayer2 = headingPlayer2.magnitude;

                if (distPlayer1 < ghosty.transform.localScale.x / 2 + 0.5f)
                {
                    Players[0].GetComponent<Character>().WarpToPosition(plateformsPerStage[CurrentStageId][0].transform.position +
                        Vector3.up * 1.5f - Vector3.right);
                    ghosty.GetComponent<Ghosty>().Respawn();
                }

                if (distPlayer2 < ghosty.transform.localScale.x / 2 + 0.5f)
                {
                    Players[1].GetComponent<Character>().WarpToPosition(plateformsPerStage[CurrentStageId][0].transform.position +
                        Vector3.up * 1.5f + Vector3.right);
                    ghosty.GetComponent<Ghosty>().Respawn();
                }

                Vector3 direction = distPlayer1 < distPlayer2
                    ? headingPlayer1 / distPlayer1
                    : headingPlayer2 / distPlayer2;

                Vector3 floatHorizontal = Quaternion.Euler(0, 90, 0) * direction;
                Vector3 floatVertical = Quaternion.Euler(90, 0, 0) * direction;

                direction *= ghostyScript.speed / SPEED_FACTOR;
                floatHorizontal *= ghostyScript.horizontalFloatingAmplitude * Mathf.Sin(Time.time * ghostyScript.horizontalFloatingSpeed + ghostyScript.GetFloatingShift()) / 50;
                floatVertical *= ghostyScript.verticalFloatingAmplitude * Mathf.Cos(Time.time * ghostyScript.verticalFloatingSpeed + ghostyScript.GetFloatingShift()) / 50;

                ghosty.transform.position += direction + floatHorizontal + floatVertical;

                Renderer rend = ghosty.GetComponent<Renderer>();
                rend.material.SetColor("_Color", ghostyScript.color);
            }
            else
            {
                Renderer rend = ghosty.GetComponent<Renderer>();
                Color color = ghostyScript.color;
                rend.material.SetColor("_Color", new Color(0.6f, 0.6f, 0.6f));
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
        List<GameObject> ghostiesPerStage;

        int stageId = 0;
        foreach (Transform levelChild in level.transform)
        {
            if (levelChild.tag == "Stage")
            {
                ghostiesPerStage = new List<GameObject>();

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
                    else if (stageChild.tag == "Ghosty")
                    {
                        ghostiesPerStage.Add(stageChild.gameObject);
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
                    ghosties.Add(stageId, ghostiesPerStage);
                }
                else
                {
                    Debug.Log("Error during level creation! All stage should have a starting plateform and a number of plateforms > 0.");
                }

                startPlateform = null;
                stagePlateforms.Clear();
                stageId++;
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
