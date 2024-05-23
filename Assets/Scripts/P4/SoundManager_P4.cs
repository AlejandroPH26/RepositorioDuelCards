using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager_P4 : MonoBehaviour
{
    static public SoundManager_P4 instance;
    [SerializeField]
    private AudioSource buttonSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void PlaySFX(AudioClip clip)
    {
        buttonSource.PlayOneShot(clip);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
