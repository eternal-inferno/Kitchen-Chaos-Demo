using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> platesVisualGameObjectList;

    private void Awake()
    {
        platesVisualGameObjectList = new List<GameObject>();
    }
    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateDestroyed += PlatesCounter_OnPlateDestroyed;
    }

    private void PlatesCounter_OnPlateDestroyed(object sender, System.EventArgs e)
    {
        GameObject plateGameObject = platesVisualGameObjectList[platesVisualGameObjectList.Count - 1];
        platesVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {   
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        float plateOffsetY = .1f; 
        plateVisualTransform.transform.localPosition = new Vector3(0, plateOffsetY * platesVisualGameObjectList.Count, 0);

        platesVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}
