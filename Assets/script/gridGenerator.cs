using System;
using System.Collections.Generic;
using UnityEngine;

public class gridGenerator : MonoBehaviour
{
    public GameObject gridCell;
    public GameObject grid;

    public Vector2 gridSize;

    public float spacing = 1;
    public float borderSpacingRelative = .25f;

    int numOfBombs = 0;
    public float bombRatio = .2f;

    public List<GameObject> gridCellArray = new List<GameObject> { };

    public bool generateCovered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numOfBombs = (int)(gridSize.x * gridSize.y * bombRatio);
        borderSpacingRelative *= spacing;
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateGrid()
    {
        // generate a grid
        for (int i = 0; i < gridSize.y; i++)
        {
            for (int j = 0; j < gridSize.x; j++)
            {
                GameObject currGridCell = Instantiate(gridCell, parent: grid.transform);

                currGridCell.transform.position = new Vector3(j * spacing, i * -spacing, 0);
                currGridCell.transform.localScale = new Vector3(spacing - borderSpacingRelative, spacing - borderSpacingRelative, 1);
                currGridCell.name = $"gridCell-{i}-{j}-{gridCellArray.Count}";

                gridCellScript script = currGridCell.GetComponent<gridCellScript>();

                script.value = 0;
                script.covered = generateCovered;

                gridCellArray.Add(currGridCell);

            }
        }


        // move cells to the center of grid
        float gridHeight = Mathf.Abs((gridSize.y - 1) * spacing);
        float gridWidth = Mathf.Abs((gridSize.x - 1) * spacing);

        foreach (Transform gridCell in grid.GetComponentInChildren<Transform>())
        {
            gridCell.transform.position += new Vector3(-(gridWidth / 2), gridHeight / 2, 0);
        }


        // generate bombs
        for (int i = 0; i < numOfBombs; i++)
        {
            int id = UnityEngine.Random.Range(0, gridCellArray.Count);
            gridCellScript script = gridCellArray[id].GetComponent<gridCellScript>();

            if (script.value == -1)
            {
                i--;
            }
            else
            {
                script.value = -1;
            }
        }

        // calculate all values and add lists
        for (int i = 0; i < gridCellArray.Count; i++)
        {
            gridCellScript script = gridCellArray[i].GetComponent<gridCellScript>();

            script.gridCellArray = gridCellArray;
            script.Start();

            if (script.value != -1)
            {
                script.calculateValue();
            }
        }

        // uncover a start spot
        bool searchingForStartSpot = true;

        while(searchingForStartSpot)
        {
            int id = UnityEngine.Random.Range(0, gridCellArray.Count);
            GameObject cell = gridCellArray[id];
            gridCellScript script = cell.GetComponent<gridCellScript>();

            if(script.value == 0)
            {
                script.uncoverAllNearbyCells(id);
                searchingForStartSpot = false;
            }
        }
    }

    public void DeleteGrid()
    {
        for(int i = 0; i < gridCellArray.Count; i++)
        {
            Destroy(gridCellArray[i].gameObject);
        }

        gridCellArray = new List<GameObject> { };
    }

    public void showAllBombs()
    {
        for (int i = 0; i < gridCellArray.Count; i++)
        {
            gridCellScript script = gridCellArray[i].GetComponent<gridCellScript>();
            if (script.value == -1)
            {
                script.covered = false;
                script.changeCellDisplayState();
            }
        }
    }
}