using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class Question : MonoBehaviour
{
    public virtual UniTask<bool> Ask()
    {
        return UniTask.FromResult(true);
    }

    public virtual UniTask GiveFeedback()
    {
        return UniTask.CompletedTask;
    }

    public virtual void Cleanup()
    {
        Destroy(gameObject);
    }
}