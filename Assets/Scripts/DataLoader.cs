using UnityEngine;

public class DataLoader : MonoBehaviour
{
    [SerializeField] private Data data;

    private void Start()
    {
        data.Load();

        foreach (var item in data.Values.Components) Debug.Log(item);

        foreach (var item in data.Values.Crafted) Debug.Log(item);
    }
}