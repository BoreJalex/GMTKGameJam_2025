using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicScript : MonoBehaviour
{
    // Start is called before the first frame update
    private static musicScript instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
