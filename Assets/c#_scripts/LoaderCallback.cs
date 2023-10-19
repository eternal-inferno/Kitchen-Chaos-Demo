using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool isFirstLoaded = true;
    private void Update()
    {
        if (isFirstLoaded)
        {
            isFirstLoaded = false;

            Loader.LoaderCallback();
        }
    }
}
