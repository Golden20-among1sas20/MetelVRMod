using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Statistic : MonoBehaviour
{
    public static Statistic Instance = null;

    public SocketIOComponent socket;

    public string ID { get; private set; } = "";

    float Waiting = 0f;

    private string characters = "0123456789abcdefghijklmnopqrstuvwxABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private void Awake ()
    {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogError ("More than one Statistic!");

            Destroy (gameObject);

            return;
        }
    }

    void Start ()
    {
        DontDestroyOnLoad (gameObject);
        
        if (PlayerPrefs.GetInt ("Statistic", 0) == 1) {
            socket.Connect ();
        }

        socket.On ("disconnect", OnDisconnect);
        socket.On ("connect", OnConnect);
    }

    private void Update ()
    {
        if (!socket.IsConnected) {
            Waiting += Time.deltaTime;
            if (Waiting >= 1f) {
                Waiting = 0f;
                if (PlayerPrefs.GetInt ("Statistic", 0) == 1) {
                    socket.Connect ();
                }
            }
        }
    }

    private void OnDisconnect (SocketIOEvent e)
    {
        //Debug.LogError ("The connection was lost!");
    }

    private void OnConnect (SocketIOEvent e)
    {
#if UNITY_EDITOR
        return;
#endif
        JSONObject data = new JSONObject (JSONObject.Type.OBJECT);

        JSONObject NewParam = new JSONObject (JSONObject.Type.OBJECT);

        data.AddField ("module", "statistic");
        data.AddField ("act", "setDeviceID");
        string device_id = "";
#if UNITY_ANDROID
        AndroidJavaClass clsUnity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = clsUnity.GetStatic<AndroidJavaObject> ("currentActivity");
        AndroidJavaObject objResolver = objActivity.Call<AndroidJavaObject> ("getContentResolver");
        AndroidJavaClass clsSecure = new AndroidJavaClass ("android.provider.Settings$Secure");

        string android_id = clsSecure.CallStatic<string> ("getString", objResolver, "android_id");

        // Get bytes of Android ID
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding ();
        byte[] bytes = ue.GetBytes (android_id);

        // Encrypt bytes with md5
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider ();
        byte[] hashBytes = md5.ComputeHash (bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++) {
            hashString += System.Convert.ToString (hashBytes[i], 16).PadLeft (2, '0');
        }

        device_id = hashString.PadLeft (32, '0');
        ID = device_id;
#elif UNITY_STANDALONE
        if (PlayerPrefs.GetString ("DeviceID", "") == "") {
            ID = GenerateCode ();
            PlayerPrefs.SetString ("DeviceID", ID);
        }
        else ID = PlayerPrefs.GetString ("DeviceID", "");
        device_id = ID;
#endif

        data.AddField ("param", NewParam);
        NewParam.AddField ("gameID", device_id);

        socket.Emit ("onMessage", data);
    }

    public void SendStatistic (string Trigger)
    {
        return;
#if UNITY_EDITOR
        return;
#endif
        if (!socket.IsConnected || PlayerPrefs.GetInt ("Statistic", 0) != 1) return;

        JSONObject data = new JSONObject (JSONObject.Type.OBJECT);
        data.AddField ("module", "statistic");
        data.AddField ("act", "setStat");

        JSONObject NewParam = new JSONObject (JSONObject.Type.OBJECT);

        NewParam.AddField ("trigger", Trigger);
        data.AddField ("param", NewParam);

        socket.Emit ("onMessage", data);
    }

    string GenerateCode ()
    {
        string code = "";

        for (int i = 0; i < 20; i++) {
            int a = Random.Range (0, characters.Length);
            code = code + characters[a];
        }

        return code;
    }
}
