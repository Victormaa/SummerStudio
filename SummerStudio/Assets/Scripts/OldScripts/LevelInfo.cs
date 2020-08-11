using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int level;
    public int shotChance;
    public string color;
    public float time;
    public int targetnums;
}

public class LevelInfo : MonoBehaviour
{

    private void Awake()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("levelInfo");

        // from here you can get a array string which got a one more line of your data
        string[] data = textAsset.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            Level levelinfo = new Level();

            int.TryParse(row[0], out levelinfo.level);
            int.TryParse(row[1], out levelinfo.shotChance);
            levelinfo.color = row[2];
            float.TryParse(row[3], out levelinfo.time);
            int.TryParse(row[4], out levelinfo.targetnums);

            GameManager.Instance.levels.Add(levelinfo);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
