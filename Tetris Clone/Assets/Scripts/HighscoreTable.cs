using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HighscoreTable : MonoBehaviour
{ 
    private Transform entryContainer;
    private Transform entryTemplate;

    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        entryContainer = transform.Find("Highscore Container");
        entryTemplate = entryContainer.Find("Highscore Entry Template");

        entryTemplate.gameObject.SetActive(false);        

        string jsonString = PlayerPrefs.GetString("highScoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        // Sort the Highest to Lowest scores
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for(int j = i; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    // Swap
                    HighscoreEntry temp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = temp;
                }
            }
        }

        if (highscores.highscoreEntryList.Count > 5)
        {
            for (int h = highscores.highscoreEntryList.Count; h > 5; h--)
            {
                highscores.highscoreEntryList.RemoveAt(5);
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry entry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(entry, entryContainer, highscoreEntryTransformList);
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry entry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 50f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;

        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }
        entryTransform.Find("Position Text").GetComponent<TextMeshProUGUI>().text = rankString;

        int level = entry.level;
        entryTransform.Find("Level Text").GetComponent<TextMeshProUGUI>().text = level.ToString();

        int score = entry.score;
        entryTransform.Find("Score Text").GetComponent<TextMeshProUGUI>().text = score.ToString();

        string name = entry.name;
        entryTransform.Find("Name Text").GetComponent<TextMeshProUGUI>().text = name;

        transformList.Add(entryTransform);
    }

    public void AddHighscoreEntry(int level, int score, string name)
    {
        // Set the HighscoreEntry
        HighscoreEntry entry = new HighscoreEntry { level = level, score = score, name = name };

        // Load Saved scores
        string jsonString = PlayerPrefs.GetString("highScoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        // Add score to the Table
        highscores.highscoreEntryList.Add(entry);

        if (highscores.highscoreEntryList.Count > 5)
        {
            for (int h = highscores.highscoreEntryList.Count; h > 5; h--)
            {
                highscores.highscoreEntryList.RemoveAt(5);
            }
        }

        // Save the table
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highScoreTable", json);
        PlayerPrefs.Save();
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int level;
        public int score;
        public string name;
    }
}
