/**************************************************************************************/
/*!
    \file Singleton.cs

    \author Unity Community, Paul Schwarzwalder

    \brief Provides a Singleton class which gives subclasses 
            Singleton behavior.

*/
/**************************************************************************************/
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    // ReSharper disable once InconsistentNaming
    // ReSharper disable once StaticMemberInGenericType
    private static readonly object _lock = new object();

    public static T Instance
    {
        get
        {
            //return null if application is quitting
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (instance == null)
                {
                    //find a copy of the singleton in the scene
                    instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                        return instance;
                    }

                    //if no copy of the singleton has already been made, create it
                    if (instance == null)
                    {
                        GameObject singletonObj = new GameObject();
                        instance = singletonObj.AddComponent<T>();
                        singletonObj.name = "(singleton) " + typeof(T);

                        //If your instance is a singleton<T> (which should normally be the case
                        //run OnInstantiation on it
                        Singleton<T> singleton = instance as Singleton<T>;
                        if (singleton != null)
                        {
                            singleton.OnInstantiation();
                        }

                        DontDestroyOnLoad(singletonObj);

                        Debug.Log("[Singleton] An instance of " + typeof(T) +
                            " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " +
                            instance.gameObject.name);
                    }
                }

                return instance;
            }
        }
    }

    protected virtual void OnInstantiation() {}

    // ReSharper disable once StaticMemberInGenericType
    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}