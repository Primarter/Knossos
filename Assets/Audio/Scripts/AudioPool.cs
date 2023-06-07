using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool
{
    const int maxPoolSize = 32;
    Stack<GameObject> audioSourcePool;
    List<GameObject> audioSourcePlaying;

    public int poolCount => audioSourcePool.Count;
    public int playingCount => audioSourcePlaying.Count;

    public void Initialize()
    {
        audioSourcePool = new Stack<GameObject>(maxPoolSize);
        audioSourcePlaying = new List<GameObject>();

        for (int i = 0 ; i < maxPoolSize ; ++i)
        {
            GameObject obj = new GameObject();

            AudioSource audioSource = obj.AddComponent<AudioSource>();

            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 1.0f;
            audioSource.minDistance = 1.0f;
            audioSource.maxDistance = 30f; // default max Distance

            audioSourcePool.Push(obj);
        }
    }

    public void updatePool()
    {
        // remove from audioSourcePlaying and add to audioSourcePool if isn't playing
        audioSourcePlaying.RemoveAll(obj => {

            if (obj == null) return true; // if object has been destroyed by scene change, remove it

            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (!audioSource.isPlaying)
            {
                obj.transform.SetParent(null, false);
                audioSourcePool.Push(obj);
                return true;
            }
            return false;
        });
    }

    public GameObject getAvailablePoolObject()
    {
        if (audioSourcePool.Count == 0)
        {
            Debug.LogError("Not enough element in audioSourcePool! " + audioSourcePlaying.Count + " are currently being used.");
            return null;
        }

        GameObject obj = audioSourcePool.Pop();
        audioSourcePlaying.Add(obj);

        return obj;
    }

    public void StopAll()
    {
        audioSourcePlaying.ForEach(obj => obj.GetComponent<AudioSource>().Stop());
        updatePool();
    }
}