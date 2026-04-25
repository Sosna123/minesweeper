using System.Collections.Generic;
using UnityEngine;

public class minesweeperManager : MonoBehaviour
{
    public GameObject gridGenerator;
    public gridGenerator gridGeneratorScript;

    public List<GameObject> gridCellArray = new List<GameObject> { };

    public GameObject gameOverMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridGenerator = GameObject.FindWithTag("gridGenerator");
        gridGeneratorScript = gridGenerator.GetComponent<gridGenerator>();
        gridCellArray = gridGeneratorScript.gridCellArray;

        //gridGeneratorScript.GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            gridGeneratorScript.DeleteGrid();
            gridGeneratorScript.GenerateGrid();
            gameOverMenu.SetActive(false);
        }
    }

    void GameOver()
    {
        gameOverMenu.SetActive(true);
    }
}
