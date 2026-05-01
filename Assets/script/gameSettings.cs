using UnityEngine;
using UnityEngine.SceneManagement;

public class gameSettings : MonoBehaviour
{
    public Vector2 gridSize = new Vector2();
    public float spacing;
    public float bombRatio;

    public int difficulty = 0;

    gridGenerator script = null;

    bool generated = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        script = GameObject.FindWithTag("gridGenerator").GetComponent<gridGenerator>();

        if (SceneManager.GetActiveScene().buildIndex == 1 && difficulty > 0)
        {
            SetDifficulty(difficulty);
            script = GameObject.FindWithTag("gridGenerator").GetComponent<gridGenerator>();
            SetSettings();
            script.GenerateGrid();
            generated = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && !generated)
        {
            SetDifficulty(difficulty);
            script = GameObject.FindWithTag("gridGenerator").GetComponent<gridGenerator>();
            SetSettings();
            script.GenerateGrid();
            generated = true;
        }else if (SceneManager.GetActiveScene().buildIndex != 1 && !generated)
        {
            generated = true;
        }
    }

    public void SetDifficulty(int difficultyTemp)
    {
        difficulty = difficultyTemp;

        if (difficultyTemp == 1)
        {
            gridSize = new Vector2(64 / 2, 36 / 2);
            spacing = .25f / 2;
            bombRatio = .175f;
        }
        else if (difficultyTemp == 2)
        {
            gridSize = new Vector2(64, 36);
            spacing = .25f;
            bombRatio = .2f;

        }
        else if (difficultyTemp == 3)
        {
            gridSize = new Vector2(64 * 2, 36 * 2);
            spacing = .25f * 2;
            bombRatio = .225f;
        }
    }

    public void SetSettings()
    {
        if(script == null || SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }

        script.gridSize = gridSize;
        script.spacing = spacing;
        script.bombRatio = bombRatio;
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
