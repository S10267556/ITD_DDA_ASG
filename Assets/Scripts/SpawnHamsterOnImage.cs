using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class SpawnHamsterOnImage : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject hamsterPrefab;

    private Dictionary<string, GameObject> spawnedHamsters = new Dictionary<string, GameObject>();

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnChanged;
    }

    void OnChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            SpawnHamster(trackedImage);
        }

        foreach (var trackedImage in args.updated)
        {
            UpdateHamsterPosition(trackedImage);
        }
    }

    void SpawnHamster(ARTrackedImage trackedImage)
    {
        GameObject hamster = Instantiate(hamsterPrefab, trackedImage.transform);

        hamster.transform.localPosition = new Vector3(0, 0, -0.1f);

        spawnedHamsters.Add(trackedImage.referenceImage.name, hamster);
    }

    void UpdateHamsterPosition(ARTrackedImage trackedImage)
    {
        if (spawnedHamsters.ContainsKey(trackedImage.referenceImage.name))
        {
            GameObject hamster = spawnedHamsters[trackedImage.referenceImage.name];
            hamster.transform.localPosition = new Vector3(0, 0, -0.1f);
        }
    }
}
