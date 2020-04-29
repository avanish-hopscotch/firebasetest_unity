using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class firebasetest : MonoBehaviour
{
	DatabaseReference mDatabaseRef;
	protected bool isFirebaseInitialized = false;

    // Start is called before the first frame update
    void Start()
    {
    	// FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://health-game-c22a1.firebaseio.com/");
    	// mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    	// Debug.Log("Write initilise");
    	// Debug.Log(mDatabaseRef);
    	if (isFirebaseInitialized != true){
    		this.InitializeFirebase();
    	}
    	this.writeNewUser();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Initialize the Firebase database:
    protected virtual void InitializeFirebase() {
    	FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://health-game-c22a1.firebaseio.com/");
    	mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
      // FirebaseApp app = FirebaseApp.DefaultInstance;
      // StartListener();
      isFirebaseInitialized = true;
    }

    private void writeNewUser() 
    {
    	string name = "abc";
    	string email = "test@test.com";
    	string userId = "jdc";
    	Usertest Usertest = new Usertest(name, email);
    	string json = JsonUtility.ToJson(Usertest);
    	Debug.Log(json);

    	mDatabaseRef.Child("users").Child(userId).SetRawJsonValueAsync(json);
    	Debug.Log("Write success");
    	this.readUser();
	}

    // public void writePlayerData(string userId, string sdata) 
    // {
    //     Debug.Log("Write testdata start");
    //     // Usertest Usertest = new Usertest(name, email);
    //     string json = JsonUtility.ToJson(sdata);
    //     Debug.Log(mDatabaseRef);

    //     mDatabaseRef.Child("testdata").Child(userId).SetRawJsonValueAsync(json);
    //     Debug.Log("Write testdata success");
    // }
    private void readUser()
    {
    	Firebase.Database.FirebaseDatabase getref = Firebase.Database.FirebaseDatabase.DefaultInstance;
    	getref.GetReference("users").GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
                        // Handle the error...
                    }
                    else if (task.IsCompleted) {
                      DataSnapshot snapshot = task.Result;
                      foreach ( DataSnapshot user in snapshot.Children){
                        IDictionary dictUser = (IDictionary)user.Value;
                        Debug.Log ("" + dictUser["email"] + " - " + dictUser["username"]);
                      }
                    }
          });

    }
}



public class Usertest {
    public string username;
    public string email;

    public Usertest() {
    }

    public Usertest(string username, string email) {
        this.username = username;
        this.email = email;
    }
}
