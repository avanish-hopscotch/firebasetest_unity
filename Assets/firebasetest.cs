using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

{
    "allLevelData": {"0": {"1": 4, "2": 3, "3": 4, "4": 4, "5": 4, "6": 4}},
    "brokenBuilding": {"0": {"1": 0, "2": 0, "3": 0, "4": 0, "5": 0, "6": 0}},
    "coin": 8224,
    "playerName": "John",
    "playerId": "playerData",
    "areaLevel": 0,
    "charge": 6,
    "shield": 3,
    "energy": 2,
    "star": 0,
    "PPURL": "",
    "menu": {
        "revenge": [
            {"playerName": "james", "playerId": "friendData2.json", "attackTime": 637212517415407822},
            {"playerName": "Aaron", "playerId": "friendData.json", "attackTime": 637212517415000000},
            {"playerName": "Aaron", "playerId": "friendData.json", "attackTime": 637212517415000030}],
        "inbox": {
            "unreadCount": 1,
            "unreadMessages": [{"message": "You got a reward from the Breakfast quest!", "link": "questScreen"}],
        }
    }
};

//[Serializable]

public class playerData
{
	public Dictionary<int, Dictionary<int, int>> allLevelData = new Dictionary<int, Dictionary<int, int>>();
	public double coin = 500;
	public string playerName;
	public string playerId;
	public int areaLevel;
	public int charge = 1;
	public int shield;
	public int energy;
	public int star;
	public string PPURL;
	public string password;
	public int pendingSubmissions1, pendingSubmissions2, pendingSubmissions3;
	public bool isQuestGift1, isQuestGift2, isQuestGift3;
	public Dictionary<int, Dictionary<int, int>> brokenBuilding = new Dictionary<int, Dictionary<int, int>>();
	public List<string> friend = new List<string>();
	//public List<AttackerData> attacker = new List<AttackerData>();

}

public class firebasetest : MonoBehaviour
{
	protected bool isFirebaseInitialized = false;
	DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
	//playerData PlayerData = new playerData();
	ArrayList PlayerData = new ArrayList();
	private int coin = 100;
	private const int MaxScores = 5;
	private string playerName = "test";
	private string logText = "";
	const int kMaxLogSize = 16382;
	private Vector2 scrollViewVector = Vector2.zero;


	//// Start is called before the first frame update
	//void Start()
	//{
	//	// FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://health-game-c22a1.firebaseio.com/");
	//	// mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
	//	// Debug.Log("Write initilise");
	//	// Debug.Log(mDatabaseRef);
	//	if (isFirebaseInitialized != true){
	//		this.InitializeFirebase();
	//	}
	//	this.writeNewUser();
	//}

	protected virtual void Start()
	{
		PlayerData.Clear();
		PlayerData.Add("Firebase Top " + coin.ToString() + " Scores");
		Debug.Log("start");

		FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
		{
			dependencyStatus = task.Result;
			if (dependencyStatus == DependencyStatus.Available)
			{
				InitializeFirebase();
				Debug.Log("test initilise");
			}
			else
			{
				Debug.LogError(
				  "Could not resolve all Firebase dependencies: " + dependencyStatus);
			}
		});
	}

	//// Initialize the Firebase database:
	//protected virtual void InitializeFirebase() {
	//	FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://health-game-c22a1.firebaseio.com/");
	//	mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
	//  // FirebaseApp app = FirebaseApp.DefaultInstance;
	//  // StartListener();
	//  isFirebaseInitialized = true;
	//}
	// Initialize the Firebase database:
	protected virtual void InitializeFirebase()
	{
		FirebaseApp app = FirebaseApp.DefaultInstance;
		StartListener();
		isFirebaseInitialized = true;
		Debug.Log("Initialise");
	}

	protected void StartListener()
	{
		FirebaseDatabase.DefaultInstance
		  .GetReference("playerData").OrderByChild("coin")
		  .ValueChanged += (object sender2, ValueChangedEventArgs e2) =>
		  {
			  if (e2.DatabaseError != null)
			  {
				  Debug.LogError(e2.DatabaseError.Message);
				  return;
			  }
			  Debug.Log("Received values for player.");
			  string title = PlayerData[0].ToString();
			  PlayerData.Clear();
			  PlayerData.Add(title);
			  if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
			  {
				  foreach (var childSnapshot in e2.Snapshot.Children)
				  {
					  if (childSnapshot.Child("coin") == null
					|| childSnapshot.Child("coin").Value == null)
					  {
						  Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
						  break;
					  }
					  else
					  {
						  Debug.Log("playerdata entry : " +
						childSnapshot.Child("playerName").Value.ToString() + " - " +
						childSnapshot.Child("coin").Value.ToString());
						  PlayerData.Insert(1, childSnapshot.Child("coin").Value.ToString()
						+ "  " + childSnapshot.Child("playerName").Value.ToString());
					  }
				  }
			  }
		  };
	}

	// Exit if escape (or back, on mobile) is pressed.
	protected virtual void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	//private void writeNewUser()
	//{
	//	string name = "abc";
	//	string email = "test@test.com";
	//	string userId = "jdc";
	//	Usertest Usertest = new Usertest(name, email);
	//	string json = JsonUtility.ToJson(Usertest);
	//	Debug.Log(json);

	//	mDatabaseRef.Child("users").Child(userId).SetRawJsonValueAsync(json);
	//	Debug.Log("Write success");
	//	this.readUser();
	//}

	// public void writePlayerData(string userId, string sdata) 
	// {
	//     Debug.Log("Write testdata start");
	//     // Usertest Usertest = new Usertest(name, email);
	//     string json = JsonUtility.ToJson(sdata);
	//     Debug.Log(mDatabaseRef);

	//     mDatabaseRef.Child("testdata").Child(userId).SetRawJsonValueAsync(json);
	//     Debug.Log("Write testdata success");
	// }
	//private void readUser()
	//{
	//	Firebase.Database.FirebaseDatabase getref = Firebase.Database.FirebaseDatabase.DefaultInstance;
	//	getref.GetReference("users").GetValueAsync().ContinueWith(task =>
	//	{
	//		if (task.IsFaulted)
	//		{
	//			// Handle the error...
	//		}
	//		else if (task.IsCompleted)
	//		{
	//			DataSnapshot snapshot = task.Result;
	//			foreach (DataSnapshot user in snapshot.Children)
	//			{
	//				IDictionary dictUser = (IDictionary)user.Value;
	//				Debug.Log("" + dictUser["email"] + " - " + dictUser["username"]);
	//			}
	//		}
	//	});

	//}

	// Output text to the debug log text field, as well as the console.
	public void DebugLog(string s)
	{
		Debug.Log(s);
		logText += s + "\n";

		while (logText.Length > kMaxLogSize)
		{
			int index = logText.IndexOf("\n");
			logText = logText.Substring(index + 1);
		}

		scrollViewVector.y = int.MaxValue;
	}

	// A realtime database transaction receives MutableData which can be modified
	// and returns a TransactionResult which is either TransactionResult.Success(data) with
	// modified data or TransactionResult.Abort() which stops the transaction with no changes.
	TransactionResult AddScoreTransaction(MutableData mutableData)
	{
		List<object> player = mutableData.Value as List<object>;

		if (player == null)
		{
			player = new List<object>();
		}
		else if (mutableData.ChildrenCount >= MaxScores)
		{
			// If the current list of scores is greater or equal to our maximum allowed number,
			// we see if the new score should be added and remove the lowest existing score.
			long minScore = long.MaxValue;
			object minVal = null;
			foreach (var child in player)
			{
				if (!(child is Dictionary<string, object>))
					continue;
				long childScore = (long)((Dictionary<string, object>)child)["coin"];
				if (childScore < minScore)
				{
					minScore = childScore;
					minVal = child;
				}
			}
			// If the new score is lower than the current minimum, we abort.
			if (minScore > coin)
			{
				return TransactionResult.Abort();
			}
			// Otherwise, we remove the current lowest to be replaced with the new score.
			player.Remove(minVal);
		}

		// Now we add the new score as a new entry that contains the email address and score.
		Dictionary<string, object> newScoreMap = new Dictionary<string, object>();
		newScoreMap["coin"] = coin;
		newScoreMap["playerName"] = playerName;
		player.Add(newScoreMap);

		// You must set the Value to indicate data at that location has changed.
		mutableData.Value = player;
		return TransactionResult.Success(mutableData);
	}

	public void AddScore()
	{
		if (coin == 0 || string.IsNullOrEmpty(playerName))
		{
			DebugLog("invalid score or playerName.");
			return;
		}
		DebugLog(String.Format("Attempting to add score {0} {1}",
		  playerName, coin.ToString()));

		DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("Playerdata");

		DebugLog("Running Transaction...");
		// Use a transaction to ensure that we do not encounter issues with
		// simultaneous updates that otherwise might create more than MaxScores top scores.
		reference.RunTransaction(AddScoreTransaction)
		  .ContinueWithOnMainThread(task => {
			  if (task.Exception != null)
			  {
				  DebugLog(task.Exception.ToString());
			  }
			  else if (task.IsCompleted)
			  {
				  DebugLog("Transaction complete.");
			  }
		  });
	}

}



public class Usertest
{
	public string username;
	public string email;

	public Usertest()
	{
	}

	public Usertest(string username, string email)
	{
		this.username = username;
		this.email = email;
	}
}
