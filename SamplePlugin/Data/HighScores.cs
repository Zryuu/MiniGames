using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SamplePlugin.Windows;

public class HighScore
{
    public string PlayerName { get; set; }
    public int Score { get; set; }

    public HighScore(string playerName, int score)
    {
        PlayerName = playerName;
        Score = score;
    }
}

public class HighScoreManager
{
    private readonly string filePath;
    private readonly Dictionary<EGame, List<HighScore>> highScores;

    public HighScoreManager(string filePath)
    {
        this.filePath = filePath;
        highScores = new Dictionary<EGame, List<HighScore>>();
        LoadHighScores();
    }

    public List<HighScore> GetHighScores(EGame game)
    {
        return highScores.TryGetValue(game, out var scores) ? scores : new List<HighScore>();
    }

    public void AddHighScore(EGame game, string playerName, int score)
    {
        if (!highScores.ContainsKey(game))
        {
            highScores[game] = new List<HighScore>();
        }

        highScores[game].Add(new HighScore(playerName, score));
        highScores[game].Sort((a, b) => b.Score.CompareTo(a.Score)); // Sort by score descending
        SaveHighScores();
    }

    private void LoadHighScores()
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            var loadedScores = JsonConvert.DeserializeObject<Dictionary<EGame, List<HighScore>>>(json);
            if (loadedScores != null)
            {
                foreach (var entry in loadedScores)
                {
                    highScores[entry.Key] = entry.Value;
                }
            }
        }
    }

    private void SaveHighScores()
    {
        var json = JsonConvert.SerializeObject(highScores, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }
}
