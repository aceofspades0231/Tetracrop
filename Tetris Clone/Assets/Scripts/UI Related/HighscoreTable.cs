using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    private Transform scrollContainer;
    private Transform panelContainer;
    private Transform entryContainer;
    private Transform entryTemplate;

    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        entryContainer = transform.Find("Highscore Container");
        scrollContainer = entryContainer.Find("Scroll");
        panelContainer = scrollContainer.Find("Panel");
        entryTemplate = panelContainer.Find("Highscore Entry Template");

        entryTemplate.gameObject.SetActive(false);        

        string jsonString = PlayerPrefs.GetString("highScoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            highscores = new Highscores();
        }
        if (highscores.highscoreEntryList == null)
        {
            highscores.highscoreEntryList = new List<HighscoreEntry>();
        }

        //  Sort the Highest to Lowest scores
        highscores.highscoreEntryList = highscores.highscoreEntryList.OrderByDescending(entry => entry.score).Take(50).ToList();

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry entry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(entry, panelContainer, highscoreEntryTransformList);
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry entry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 65f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;

        switch (rank)
        {
            default:
                rankString = rank + "TH";
                break;

            case 1: 
                rankString = "1ST";
                entryTransform.Find("RankGold").gameObject.SetActive(true);
                break;
            case 2: rankString = "2ND";
                entryTransform.Find("RankSilver").gameObject.SetActive(true);
                break;
            case 3: rankString = "3RD";
                entryTransform.Find("RankBronze").gameObject.SetActive(true);
                break;
        }

        if (rank == 1)
        {
            entryTransform.Find("Position Text").GetComponent<TextMeshProUGUI>().color = ColorUtility.TryParseHtmlString("#FFD700", out Color gold) ? gold : Color.white;
            entryTransform.Find("Level Text").GetComponent<TextMeshProUGUI>().color = ColorUtility.TryParseHtmlString("#FFD700", out gold) ? gold : Color.white;
            entryTransform.Find("Score Text").GetComponent<TextMeshProUGUI>().color = ColorUtility.TryParseHtmlString("#FFD700", out gold) ? gold : Color.white;
            entryTransform.Find("Name Text").GetComponent<TextMeshProUGUI>().color = ColorUtility.TryParseHtmlString("#FFD700", out gold) ? gold : Color.white;
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

        if (highscores == null)
        {
            highscores = new Highscores();
        }
        if (highscores.highscoreEntryList == null)
        {
            highscores.highscoreEntryList = new List<HighscoreEntry>();
        }

        // Add score to the Table
        highscores.highscoreEntryList.Add(entry);

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
