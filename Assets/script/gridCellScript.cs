using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Windows;

public class gridCellScript : MonoBehaviour
{
    public int value = 0;
    public TextMeshPro valueText;
    public bool covered = true;

    public Color coveredColor;
    public Color uncoveredColor;

    public List<GameObject> gridCellArray = new List<GameObject> { };
    public Vector2 gridSize;

    public bool flagged = false;
    GameObject flag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        valueText = GetComponentInChildren<TextMeshPro>();

        Transform[] childrenTransform = GetComponentsInChildren<Transform>();
        GameObject[] children = { };

        for(int i = 0; i < childrenTransform.Length; i++)
        {
            children.Append(childrenTransform[i].gameObject);

            if (childrenTransform[i].gameObject.name == "flag")
            {
                flag = childrenTransform[i].gameObject;
            }
        }

        gridGenerator gridGenerator = GameObject.FindWithTag("gridGenerator").GetComponent<gridGenerator>();
        gridCellArray = gridGenerator.gridCellArray;
        gridSize = gridGenerator.gridSize;

        flag.SetActive(flagged);
    }

    // Update is called once per frame
    void Update()
    {
        changeCellDisplayState();
    }

    List<GameObject> getNearbyCells(int currentId)
    {
        List<GameObject> nearbyCells = new List<GameObject> { };
        List<int> nearbyCellIds = new List<int> { };

        // -1-W  -W  +1-W
        // -1    0   +1
        // -1+W  +W  +1+W

        // left column
        if (currentId % gridSize.x != 0)
        {
            nearbyCellIds.Add(currentId - 1 - (int)gridSize.x);
            nearbyCellIds.Add(currentId - 1);
            nearbyCellIds.Add(currentId - 1 + (int)gridSize.x);
        }

        // middle column
        nearbyCellIds.Add(currentId - (int)gridSize.x);
        nearbyCellIds.Add(currentId + (int)gridSize.x);

        // right column
        if (currentId % gridSize.x != gridSize.x - 1)
        {
            nearbyCellIds.Add(currentId + 1 - (int)gridSize.x);
            nearbyCellIds.Add(currentId + 1);
            nearbyCellIds.Add(currentId + 1 + (int)gridSize.x);
        }

        for (int i = 0; i < nearbyCellIds.Count; i++)
        {
            if (nearbyCellIds[i] > 0 && nearbyCellIds[i] < (gridCellArray.Count - 1))
            {
                nearbyCells.Add(gridCellArray[nearbyCellIds[i]]);
            }
        }

        return nearbyCells;
    }

    public void calculateValue()
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

    public void uncoverAllNearbyCells(int currentId)
    {
        gridCellScript currentScript = gridCellArray[currentId].GetComponent<gridCellScript>();

        if (currentScript.flagged)
        {
            return;
        }

        currentScript.covered = false;
        // currentScript.changeCellDisplayState();

        if (currentScript.value == 0)
        {
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

        if (currentScript.value == -1)
        {
            // lose?
        }
    }

    void OnMouseOver()
    {
        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            if (covered == true && !flagged)
            {
                int currentId = gridCellArray.IndexOf(gameObject);
                uncoverAllNearbyCells(currentId);
            }
        }
        else if (UnityEngine.Input.GetMouseButtonDown(1))
        {
            flagged = !flagged;
            // changeCellDisplayState();
        }
    }

    public void changeCellDisplayState()
    {
        if (!covered)
        {
            gameObject.GetComponent<SpriteRenderer>().color = uncoveredColor;

            string valueStr = value.ToString();

            if (value == -1)
            {
                valueStr = "B";
            }
            else if (value == 0)
            {
                valueStr = "";
            }

            valueText.text = valueStr;

            if (value == -1)
            {
                valueText.color = new Color(1, 0, 0);
            }

            flagged = false;
            flag.SetActive(false);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = coveredColor;
            valueText.text = "";

            flag.SetActive(flagged);
        }
    }
}
