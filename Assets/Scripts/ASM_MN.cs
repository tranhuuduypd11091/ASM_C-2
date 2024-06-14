using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Threading;
using System;

public class ASM_MN : Singleton<ASM_MN>
{
    public List<Region> listRegion = new List<Region>();
    public List<Players> listPlayer = new List<Players>();

    private void Start()
    {
        createRegion();
    }

    public void createRegion()
    {
        listRegion.Add(new Region(0, "VN"));
        listRegion.Add(new Region(1, "VN1"));
        listRegion.Add(new Region(2, "VN2"));
        listRegion.Add(new Region(3, "JS"));
        listRegion.Add(new Region(4, "VS"));
    }

    public string calculate_rank(int score)
    {
        if (score < 100)
            return "Đồng";
        else if (score < 500)
            return "Bạc";
        else if (score < 1000)
            return "Vàng";
        else
            return "Kim cương";
    }

    public void YC1()
    {
        string name = ScoreKeeper.Instance.GetUserName();
        int id = ScoreKeeper.Instance.GetID();
        int idR = ScoreKeeper.Instance.GetIDregion();
        int score = ScoreKeeper.Instance.GetScore();

        string regionName = listRegion.FirstOrDefault(r => r.ID == idR)?.Name ?? "Unknown";

        Region playerRegion1 = new Region(idR, regionName);
        Players player3 = new Players(id, name, score, playerRegion1);
        listPlayer.Add(player3);

        Players player1 = new Players(id, "Player1", 50, new Region(2, "VN2"));
        listPlayer.Add(player1);
        Players player2 = new Players(id, "Player2", 100000, new Region(3, "JS"));
        listPlayer.Add(player2);
        Players player4 = new Players(id, "Player3", 500, new Region(4, "VN"));
        listPlayer.Add(player4);
        Players player5 = new Players(id, "Player4", 700, new Region(5, "VS"));
        listPlayer.Add(player5);
    }

    public void YC2()
    {
        foreach (Players player in listPlayer)
        {
            string rank = calculate_rank(player.Score);
            Debug.Log("Player Name: " + player.Name + " - Score: " + player.Score + " - Region: " + player.PlayerRegion.Name + "- Rank: " + rank);
        }
    }

    public void YC3()
    {
        if (listPlayer.Count == 0)
        {
            return;
        }

        int currentPlayerScore = listPlayer[0].Score;
        var less = listPlayer.Where(Pr => Pr.Score < currentPlayerScore);

        if (!less.Any())
        {
            return;
        }

        Debug.Log("thông tin các Player có score bé hơn score hiện tại của người chơi:");
        foreach (var player in less)
        {
            string rank = calculate_rank(player.Score);
            Debug.Log("Name: " + player.Name + " - Score: " + player.Score + " - Rank: " + rank);
        }
    }

    public void YC4()
    {
        int currentId = ScoreKeeper.Instance.GetID();
        var currentPlayer = listPlayer.FirstOrDefault(p => p.Id == currentId);
        if (currentPlayer != null)
        {
            string rank = calculate_rank(currentPlayer.Score);
            Debug.Log("Nguời chơi hiện tại: - Name: " + currentPlayer.Name + " - Score: " + currentPlayer.Score + " - Region: " + currentPlayer.PlayerRegion.Name + " - Rank: " + rank);
        }
    }

    public void YC5()
    {
        var sortedPlayers = listPlayer.OrderByDescending(p => p.Score).ToList();

        Debug.Log("thông tin các Player trong listPlayer theo thứ tự score giảm dần:");
        foreach (var player in sortedPlayers)
        {
            string rank = calculate_rank(player.Score);
            Debug.Log("Name: " + player.Name + " - Score: " + player.Score + " - Region: " + player.PlayerRegion.Name + " - Rank: " + rank);
        }
    }

    public void YC6()
    {
        var lowestScores = listPlayer.OrderBy(p => p.Score).Take(5).ToList();

        Debug.Log("thông tin 5 player có score thấp nhất theo thứ tự tăng dần:");
        foreach (var player in lowestScores)
        {
            string rank = calculate_rank(player.Score);
            Debug.Log("Name: " + player.Name + " - Score: " + player.Score + " - Region: " + player.PlayerRegion.Name + " - Rank: " + rank);
        }
    }

    public void YC7()
    {
        Thread bxhThread = new Thread(CalculateAndSaveAverageScoreByRegion);
        bxhThread.Name = "BXH";
        bxhThread.Start();
    }

    void CalculateAndSaveAverageScoreByRegion()
    {
        var regionGroups = listPlayer.GroupBy(p => p.PlayerRegion.Name)
                                     .Select(g => new
                                     {
                                         RegionName = g.Key,
                                         AverageScore = g.Average(p => p.Score)
                                     });

        string path = Path.Combine(Application.dataPath, "bxhRegion.txt");

        using (StreamWriter writer = new StreamWriter(path, false))
        {
            foreach (var region in regionGroups)
            {
                writer.WriteLine($"Region: {region.RegionName} - Average Score: {region.AverageScore}");
            }
        }

        Debug.Log(" Đã thực hiện tính score trung bình dựa trên Regionn mà Player đó đã đăng ký và lưu nó vào tập tin bxhReigon.txt");
    }
}

[SerializeField]
public class Region
{
    public int ID;
    public string Name;

    public Region(int ID, string Name)
    {
        this.ID = ID;
        this.Name = Name;
    }
}

[SerializeField]
public class Players
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Score { get; set; }
    public Region PlayerRegion { get; set; }

    public Players(int id, string name, int score, Region region)
    {
        Id = id;
        Name = name;
        Score = score;
        PlayerRegion = region;
    }
}
