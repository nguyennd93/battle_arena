// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using StormStudio.GameOps;
// #if UNITY_IOS && !UNITY_EDITOR
// using System.Runtime.InteropServices;
// #endif
// #if UNITY_ANDROID
// using UnityEngine.SocialPlatforms;
// #endif

// namespace StormStudio.Common.Native
// {
//     public static class GameCenterHelper
//     {
// #if UNITY_IOS && !UNITY_EDITOR
//         [DllImport("__Internal")]
//         private static extern bool _isAuthenticated();
        
//         [DllImport("__Internal")]
//         private static extern void _authenticatePlayer(string objectCallbackName);

//         [DllImport("__Internal")] 
//         private static extern bool _isAuthenticating();

//         [DllImport("__Internal")] 
//         private static extern string _getPlayerId();

//         [DllImport("__Internal")] 
//         private static extern void _reportScore(int score, string leaderboardId);

//         [DllImport("__Internal")] 
//         private static extern void _loadPlayerScore(string leaderboardId);

//         [DllImport("__Internal")] 
//         private static extern int _getPlayerScore(string leaderboardId);

//         [DllImport("__Internal")] 
//         private static extern void _showLeaderboard(string leaderboardId);
// #endif
//         public static bool IsAuthenticated()
//         {
// #if UNITY_IOS && !UNITY_EDITOR
//             return _isAuthenticated();
// #else
//             return false;
// #endif
//         }

//         public static void AuthenticatePlayer(string objectCallbackName)
//         {
// #if UNITY_IOS && !UNITY_EDITOR
//             _authenticatePlayer(objectCallbackName);
// #endif
//         }

//         public static bool IsAuthenticating()
//         {
// #if UNITY_IOS && !UNITY_EDITOR
//             return _isAuthenticating();
// #else
//             return false;
// #endif
//         }

//         public static void ReportScore(int score, string leaderboardId)
//         {
// #if UNITY_IOS && !UNITY_EDITOR
//             _reportScore(score, leaderboardId);
// #endif
//         }

//         public static void LoadPlayerScore(string leaderboardId)
//         {
// #if UNITY_IOS && !UNITY_EDITOR
//             _loadPlayerScore(leaderboardId);
// #endif
//         }

//         public static int GetPlayerScore(string leaderboardId)
//         {
// #if UNITY_IOS && !UNITY_EDITOR
//             return _getPlayerScore(leaderboardId);
// #else
//             return 0;
// #endif
//         }

//         public static void ShowLeaderboard(string leaderboardId)
//         {
// #if UNITY_IOS && !UNITY_EDITOR
//             _showLeaderboard(leaderboardId);
// #endif
//         }

//         public static string GetPlayerId()
//         {
// #if UNITY_IOS && !UNITY_EDITOR
//             return _getPlayerId();
// #else
//             return "";
// #endif
//         }
//     }

//     public interface IHighScoreListener
//     {
//         void RefreshLeaderboardScore(string leaderboardId, int score);
//     }

//     public class NativeLeaderboardManager
//     {
//         const string KEY_LOCAL_SOCIAL_USER = "SOCIAL_USER";
//         const string KEY_BESTSCORE = "BEST_SCORE";

//         List<IHighScoreListener> _highscoreListeners = new List<IHighScoreListener>();

//         Dictionary<string, int> _syncedHighscores = new Dictionary<string, int>();
//         List<string> _leaderboardIds;
//         bool _shouldSyncHighScore;

// #if UNITY_ANDROID
//         bool _isGPGSDebug;
// #endif

//         IMainAppService _appService;

//         #region Public
//         public int GetLocalHighScore(string leaderboardId)
//         {
//             var localKeyScore = string.Format("{0}_{1}", KEY_BESTSCORE, leaderboardId);
//             return PlayerPrefs.GetInt(localKeyScore, 0);
//         }

//         public NativeLeaderboardManager(string[] leaderboardIds, IMainAppService appService
// #if UNITY_ANDROID
//         , bool debug
// #endif
//         )
//         {
// #if UNITY_ANDROID
//             _isGPGSDebug = debug;
// #endif
//             _appService = appService;

//             foreach (var leaderboardId in leaderboardIds)
//                 _syncedHighscores[leaderboardId] = -1;

//             _leaderboardIds = new List<string>(leaderboardIds);
//         }

//         // Authenticate player and sync all high scores
//         public void Authenticate(string objectCallbackName, bool silent = true)
//         {
// #if (UNITY_IOS || UNITY_ANDROID)
// #if UNITY_ANDROID
//             // Activate the Google Play Games platform
//             var config = new GooglePlayGames.BasicApi.PlayGamesClientConfiguration.Builder().EnableHidePopups().Build();
//             GooglePlayGames.PlayGamesPlatform.InitializeInstance(config);

//             if (_isGPGSDebug)
//                 GooglePlayGames.PlayGamesPlatform.DebugLogEnabled = true;

//             GooglePlayGames.PlayGamesPlatform.Activate();

//             if (!Social.localUser.authenticated)
//             {
//                 GooglePlayGames.PlayGamesPlatform.Instance.Authenticate((success, error) =>
//                 {
//                     Debug.Log("[LeaderboardManager] Social silent sign in result: " + success + ", error: " + error);
//                     if (success)
//                     {
//                         SyncHighScores(Social.localUser.id);
//                     }
//                     else if (GooglePlayGames.PlayGamesPlatform.Instance.GetSignInStatusCode() == 4)
//                     {
//                         GooglePlayGames.PlayGamesPlatform.Instance.Authenticate((success1, error1) =>
//                         {
//                             Debug.Log("[LeaderboardManager] Social authenticated result: " + success1 + ", error: " + error1);
//                             if (success)
//                             {
//                                 SyncHighScores(Social.localUser.id);
//                             }
//                         }, false);
//                     }
//                 }, silent);
//             }
//             else
//             {
//                 SyncHighScores(Social.localUser.id);
//     }
// #elif UNITY_IOS
//             if (!GameCenterHelper.IsAuthenticated())
//             {
//                 SocialAuthenticate(objectCallbackName);
//             }
//             else
//             {
//                 SyncHighScores(Social.localUser.id);
//             }
// #endif
// #endif
// }

//         public void OnGameCenterAuthenDidFinish()
//         {
//             _appService.StartCoroutine(SyncHighScoreDelay(GameCenterHelper.GetPlayerId(), 1.0f));
//         }

//         // Show a native leaderboard by id
//         public void ShowLeaderboard(string leaderboardId)
//         {
// #if UNITY_IOS
//             GameCenterHelper.ShowLeaderboard(leaderboardId);
//             if (GameCenterHelper.IsAuthenticated() && _shouldSyncHighScore)
//             {
//                 SyncHighScores(GameCenterHelper.GetPlayerId());
//             }
// #elif UNITY_ANDROID
//             //if (Social.localUser.authenticated)
//             //{
//             //    GooglePlayGames.PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardId);
//             //    if (Social.localUser.authenticated && _shouldSyncHighScore)
//             //        SyncHighScores(Social.localUser.id);
//             //}
//             //else
//             //{
//             //    GooglePlayGames.PlayGamesPlatform.Instance.Authenticate((success, error) =>
//             //    {
//             //        Debug.Log("[LeaderboardManager] Social authenticated result: " + success + ", error: " + error);
//             //        if (success)
//             //        {
//             //            SyncHighScores(Social.localUser.id);
//             //        }
//             //    }, false);
//             //}
// #endif
//         }

//         // Report score for a leaderboard
//         // This will make sure to sync score to leaderboard and local highscore
//         public void ReportPlayerScore(int score, string leaderboardId)
//         {
//             var localHighScore = GetLocalHighScore(leaderboardId);
//             if (localHighScore < score)
//             {
//                 SetLocalHighScore(leaderboardId, score);
//             }

// #if UNITY_ANDROID
//             if (Social.localUser.authenticated)
//             {
//                 Social.ReportScore(score, leaderboardId, (result) =>
//                 {
//                     Debug.Log("[LeaderboardManager] ReportUserScore success: " + result);
//                 });
//             }
// #elif UNITY_IOS
//             if (GameCenterHelper.IsAuthenticated())
//             {
//                 GameCenterHelper.ReportScore(score, leaderboardId);
//             }
// #endif
//         }

//         // Register a high score listener
//         public void RegisterHighScoreListener(IHighScoreListener listener)
//         {
//             _highscoreListeners.Add(listener);
//         }

//         // Unregister a high score listener
//         public void RemoveHighScoreListener(IHighScoreListener listener)
//         {
//             _highscoreListeners.Remove(listener);
//         }
//         #endregion

//         #region Private implementations
//         string LocalUser
//         {
//             get
//             {
//                 return PlayerPrefs.GetString(KEY_LOCAL_SOCIAL_USER, "");
//             }

//             set
//             {
//                 PlayerPrefs.SetString(KEY_LOCAL_SOCIAL_USER, value);
//                 PlayerPrefs.Save();
//             }
//         }

//         void SetLocalHighScore(string leaderboardId, int highscore)
//         {
//             var localKeyScore = string.Format("{0}_{1}", KEY_BESTSCORE, leaderboardId);
//             PlayerPrefs.SetInt(localKeyScore, highscore);
//         }

//         void SyncHighScores(string userId)
//         {
//             if (string.IsNullOrEmpty(userId))
//                 return;

//             _appService.StartCoroutine(SyncLeaderboards(userId, (success) =>
//             {
//                 _shouldSyncHighScore = !success;
//             }));
//         }

//         IEnumerator SyncLeaderboards(string userId, System.Action<bool> cb)
//         {
//             int count = _leaderboardIds.Count;
//             var syncResult = true;
//             foreach (var key in _leaderboardIds)
//             {
//                 _appService.StartCoroutine(SyncHighScore(userId, key, (success) =>
//                 {
//                     count--;
//                     syncResult &= success;
//                 }));
//             }

//             while (count > 0)
//                 yield return null;

//             cb(syncResult);
//         }

//         IEnumerator SyncHighScore(string userId, string leaderboardId, System.Action<bool> cb)
//         {
//             // 1. Sync high score from GameCenter/GooglePlay Games
// #if UNITY_ANDROID
//             var leaderboard = GooglePlayGames.PlayGamesPlatform.Instance.CreateLeaderboard();
//             leaderboard.id = leaderboardId;
//             leaderboard.timeScope = TimeScope.AllTime;
//             leaderboard.SetUserFilter(new string[] { Social.localUser.id });

//             bool loading = true;
//             bool ok = false;
//             leaderboard.LoadScores((success) =>
//             {
//                 loading = false;
//                 ok = success;
//             });

//             while (loading)
//                 yield return null;

//             // sync failed
//             if (!ok)
//             {
//                 cb(false);
//                 yield break;
//             }

//             //var syncedUserScore = 0;
//             var syncedUserScore = leaderboard.localUserScore == null ? 0 : (int)leaderboard.localUserScore.value;
//             _syncedHighscores[leaderboardId] = syncedUserScore;
// #else
//             GameCenterHelper.LoadPlayerScore(leaderboardId);
//             while (GameCenterHelper.GetPlayerScore(leaderboardId) == -1)
//                 yield return null;

//             // sync failed
//             var syncedUserScore = GameCenterHelper.GetPlayerScore(leaderboardId);
//             if (syncedUserScore < 0)
//             {
//                 cb(false);
//                 yield break;
//             }
//             _syncedHighscores[leaderboardId] = syncedUserScore;
// #endif
//             var localUser = LocalUser;
//             if (string.IsNullOrEmpty(localUser) || localUser.Equals(userId))
//             {
//                 if (string.IsNullOrEmpty(localUser))
//                     LocalUser = userId;

//                 var localBestScore = GetLocalHighScore(leaderboardId);
//                 var best = Mathf.Max(syncedUserScore, localBestScore);
//                 // get a better previous high score
//                 if (best > localBestScore)
//                 {
//                     SetLocalHighScore(leaderboardId, best);
//                     BroadcastHighScore(leaderboardId, best);
//                 }

//                 // report local highscore
//                 if (best > syncedUserScore)
//                     _appService.StartCoroutine(ReportPlayerScoreDelay(best, leaderboardId, 1.0f));
//             }
//             // It's possible to lose progress of current local user
//             else
//             {
//                 // TODO: backup current local user data
//                 //...

//                 // Override
//                 LocalUser = userId;
//                 SetLocalHighScore(leaderboardId, syncedUserScore);
//                 BroadcastHighScore(leaderboardId, syncedUserScore);
//             }
//             cb(true);
//         }

// #if UNITY_IOS
//         void SocialAuthenticate(string objectCallbackName)
//         {
//             _shouldSyncHighScore = true;
//             GameCenterHelper.AuthenticatePlayer(objectCallbackName);
//         }
// #endif

//         IEnumerator SyncHighScoreDelay(string userId, float delay)
//         {
//             yield return new WaitForSeconds(delay);
//             SyncHighScores(userId);
//         }

//         IEnumerator ReportPlayerScoreDelay(int score, string leaderboardId, float delay)
//         {
//             yield return new WaitForSeconds(delay);

//             ReportPlayerScore(score, leaderboardId);
//         }

//         void BroadcastHighScore(string leaderboardId, int score)
//         {
//             foreach (var listener in _highscoreListeners)
//                 listener.RefreshLeaderboardScore(leaderboardId, score);
//         }
//         #endregion
//     }
// }