using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevelManager : MonoBehaviour
{
    public GameObject LevelModel;
    public uint levelNumber;

    private const int START_POS_X = -750;
    private const int START_POS_Y = 125;
    private const int END_POS_Y = -100;
    private const int NUMBER_OF_LEVEL_PER_LINE = 5;
    private const int NUMBER_OF_LINE = 3;
    private const int BUTTON_SIZE = 100;

    // Start is called before the first frame update
    void Start()
    {
        int spaceX = (-START_POS_X * 2 - BUTTON_SIZE * (NUMBER_OF_LEVEL_PER_LINE - 1)) / (NUMBER_OF_LEVEL_PER_LINE - 1);
        int spaceY = ((START_POS_Y - END_POS_Y) - BUTTON_SIZE * (NUMBER_OF_LINE - 1)) / (NUMBER_OF_LINE - 1);
        Debug.Log(spaceY);
        float currentPosX = START_POS_X;
        float currentPosY = START_POS_Y;
        int currentNumberOfLevel = 0;

        for (int i = 0; i < levelNumber; i++)
        {
            if (currentNumberOfLevel < NUMBER_OF_LEVEL_PER_LINE * NUMBER_OF_LINE)
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

                if (currentNumberOfLevel % NUMBER_OF_LEVEL_PER_LINE == 0)
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
