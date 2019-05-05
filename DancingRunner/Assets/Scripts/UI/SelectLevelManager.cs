using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Place buttons to load level with the asked starting pos, size and space.
/// </summary>
public class SelectLevelManager : MonoBehaviour
{
    public GameObject LevelModel;
    public uint levelNumber;
    public int numberOfLevelPerLine = 5;
    public int NumberOfLine = 3;

    private const int START_POS_X = -750;
    private const int START_POS_Y = 125;
    private const int END_POS_Y = -100;
    private const int BUTTON_SIZE = 100;
    
    /// <summary>
    /// Place buttons to load level with the asked starting pos, size and space.
    /// </summary>
    void Start()
    {
        int spaceX = (-START_POS_X * 2 - BUTTON_SIZE * (numberOfLevelPerLine - 1)) / (numberOfLevelPerLine - 1);
        int spaceY = ((START_POS_Y - END_POS_Y) - BUTTON_SIZE * (NumberOfLine - 1)) / (NumberOfLine - 1);
        float currentPosX = START_POS_X;
        float currentPosY = START_POS_Y;
        int currentNumberOfLevel = 0;

        for (int i = 0; i < levelNumber; i++)
        {
            if (currentNumberOfLevel < numberOfLevelPerLine * NumberOfLine)
            {
                GameObject levelSelector = Instantiate(LevelModel);
                RectTransform rectLevelSelector = (RectTransform)levelSelector.transform;

                levelSelector.transform.SetParent(transform);
                levelSelector.SetActive(true);
                levelSelector.name = (i + 1).ToString();
                levelSelector.transform.localScale = new Vector3(1, 1, 1);
                levelSelector.transform.localPosition = new Vector3(currentPosX, currentPosY, 0);
                levelSelector.GetComponentInChildren<Text>().text = (i + 1).ToString();
                levelSelector.GetComponent<Button>().onClick.AddListener(delegate { LoadLevel(int.Parse(levelSelector.name)); });
                rectLevelSelector.sizeDelta = new Vector2(BUTTON_SIZE, BUTTON_SIZE);

                currentPosX += BUTTON_SIZE + spaceX;
                currentNumberOfLevel++;

                if (currentNumberOfLevel % numberOfLevelPerLine == 0)
                {
                    currentPosX = START_POS_X;
                    currentPosY -= BUTTON_SIZE + spaceY;
                }
            }
            else
            {
                Debug.Log("Too much level to display, reduce level size, increase number of level per line, and number of line!");
                break;
            }
        }
    }

    private void LoadLevel(int levelID)
    {
        Debug.Log(levelID);
        SceneManager.LoadScene("Level" + levelID);
    }
}
