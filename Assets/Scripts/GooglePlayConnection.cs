using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GooglePlayConnection : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        SignIn();
    }

    void SignIn()
    {
        Social.localUser.Authenticate(success => { });
    }


    public static void AddScoreToLeaderboard(long score)
    {
        Social.ReportScore(score, GPGSIds.leaderboard_polywing, success => { });
    }

    public static void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }
}
