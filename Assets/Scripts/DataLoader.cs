using UnityEngine;

public class DataLoader : MonoBehaviour
{
    [SerializeField] private Data data;

    private void Start()
    {
        data.Load();
    }
}