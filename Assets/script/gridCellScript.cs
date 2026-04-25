using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

public class gridCellScript : MonoBehaviour
{
    public int value = 0;
    public TextMeshPro valueText;
    public bool covered = true;

    public Color coveredColor;
    public Color uncoveredColor;

    public List<GameObject> gridCellArray = new List<GameObject> { };
    public Vector2 gridSize;

    public bool cleared = false;
    bool calculated = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        valueText = GetComponentInChildren<TextMeshPro>();

        gridGenerator gridGenerator = GameObject.FindWithTag("gridGenerator").GetComponent<gridGenerator>();
        gridCellArray = gridGenerator.gridCellArray;
        gridSize = gridGenerator.gridSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (!calculated & gridCellArray.Count == gridSize.x * gridSize.y)
        {
            calculated = true;
            calculateValue();
        }

        if (!covered)
        { 
            gameObject.GetComponent<SpriteRenderer>().color = uncoveredColor;

            string valueStr = value.ToString();

            if (value == -1)
            {
                valueStr = "B";
            }else if(value == 0)
            {
                valueStr = "";
            }

            valueText.text = valueStr;

            if (value == -1)
            {
                valueText.color = new Color(1, 0, 0);
            }
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = coveredColor;
            valueText.text = "";
        }
    }

    List<GameObject> getNearbyCells(int currentId)
    {
        List<GameObject> nearbyCells = new List<GameObject> { };
        List<int> nearbyCellIds = new List<int> { };

        // -1-W  -W  +1-W
        // -1    0   +1
        // -1+W  +W  +1+W

        // left column
        nearbyCellIds.Add(currentId - 1 - (int)gridSize.x);
        nearbyCellIds.Add(currentId - 1);
        nearbyCellIds.Add(currentId - 1 + (int)gridSize.x);

        // middle column
        nearbyCellIds.Add(currentId - (int)gridSize.x);
        nearbyCellIds.Add(currentId + (int)gridSize.x);

        // right column
        nearbyCellIds.Add(currentId + 1 - (int)gridSize.x);
        nearbyCellIds.Add(currentId + 1);
        nearbyCellIds.Add(currentId + 1 + (int)gridSize.x);

        for (int i = 0; i < nearbyCellIds.Count; i++)
        {
            if (nearbyCellIds[i] > 0 && nearbyCellIds[i] < (gridCellArray.Count - 1))
            {
                nearbyCells.Add(gridCellArray[nearbyCellIds[i]]);
            }
        }

        return nearbyCells;
    }

    void calculateValue()
    {
        if (value == -1)
        {
            return;
        }

        value = 0;

        int currentId = gridCellArray.IndexOf(gameObject);
        List<GameObject> nearbyCells = getNearbyCells(currentId);

        for (int i = 0; i < nearbyCells.Count; i++)
        {
            if (nearbyCells[i].GetComponent<gridCellScript>().value == -1)
            {
                value++;
            }
        }
    }

    void uncoverAllNearbyCells(int currentId)
    {
        gridCellScript currentScript = gridCellArray[currentId].GetComponent<gridCellScript>();
        currentScript.covered = false;

        if (currentScript.value == 0 && currentScript.cleared == false)
        {
            currentScript.cleared = true;
            List<GameObject> nearbyCells = getNearbyCells(currentId);

            for (int i = 0; i < nearbyCells.Count; i++)
            {
                gridCellScript nearbyScript = nearbyCells[i].GetComponent<gridCellScript>();

                if (nearbyScript.covered)
                {
                    uncoverAllNearbyCells(gridCellArray.IndexOf(nearbyCells[i]));
                }
            }
        }
    }

    void OnMouseDown()
    {
        if (covered == true)
        {
            int currentId = gridCellArray.IndexOf(gameObject);
            uncoverAllNearbyCells(currentId);
        }
    }
}
