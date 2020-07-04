using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenuMusic : MonoBehaviour
{

    private static PlayMenuMusic _Instance;

    public static PlayMenuMusic Instance {
        get {
            if(_Instance == null) {
                _Instance = FindObjectOfType<PlayMenuMusic>();
                DontDestroyOnLoad(_Instance.gameObject);
            }

            return _Instance;
        }
    }

    private void Awake() {
        if (_Instance == null) {
            _Instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            if (this != _Instance) {
                Play();
                Destroy(this.gameObject);
            }
        }
    }

    public void Update() {
        if (this != _Instance) {
            _Instance = null;
        }
    }

    public void Play() {
        this.gameObject.GetComponent<AudioSource>().Play();
    }

}
