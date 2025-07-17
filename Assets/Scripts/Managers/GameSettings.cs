using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{

    [SerializeField]
    private string _gameVersion = "0.0.1";
    public string GameVersion {
        get { return _gameVersion; }
    }

    [SerializeField]
    private string _nickName = "CTC";
    public string NickName
    {
        get {
            int value = Random.Range(0, 9999);
            return _nickName + value;
        }
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
