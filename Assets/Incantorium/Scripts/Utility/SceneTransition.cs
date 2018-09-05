using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneName;
    public LoadSceneMode loadType;

    private void Start()
    {
        SceneManager.LoadScene( sceneName, loadType );
        Destroy( gameObject );
    }
}
