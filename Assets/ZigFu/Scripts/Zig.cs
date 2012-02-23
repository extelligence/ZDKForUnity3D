using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zig : MonoBehaviour {
    public ZigInputType inputType = ZigInputType.OpenNI;
    public bool UpdateDepthmapImage = false;
    public bool UpdateImagemapImage = false;
    public List<GameObject> listeners = new List<GameObject>();
    public bool Verbose = true;

	void Start () {
        #if UNITY_WEBPLAYER
        #if UNITY_EDITOR
        Debug.LogError("Depth camera input will not work in editor when target platform is Webplayer. Please change target platform to PC/Mac standalone.");
        return;
        #endif
        #endif

        ZigInput.InputType = inputType;
        ZigInput.UpdateDepth = UpdateDepthmapImage;
        ZigInput.UpdateImage = UpdateImagemapImage;
        ZigInput.Instance.AddListener(gameObject);
	}

    void notifyListeners(string msgname, object arg) {
        for (int i = 0; i < listeners.Count; ) {
            GameObject go = listeners[i];
            if (go) {
                go.SendMessage(msgname, arg, SendMessageOptions.DontRequireReceiver);
                i++;
            }
            else {
                listeners.RemoveAt(i);
            }
        }
    }

    void Zig_UserFound(ZigTrackedUser user) {
        if (Verbose) Debug.Log("Zig: Found user  " + user.Id);
        notifyListeners("Zig_UserFound", user);
    }

    void Zig_UserLost(ZigTrackedUser user) {
        if (Verbose) Debug.Log("Zig: Lost user " + user.Id);
        notifyListeners("Zig_UserLost", user);
    }

    void Zig_Update(ZigInput zig) {
        notifyListeners("Zig_Update", zig);
    }
}
