using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class Question : MonoBehaviour
{
    public virtual UniTask Ask()
    {
        return UniTask.CompletedTask;
    }

    public abstract bool AnsweredCorrectly();

    public virtual UniTask GiveFeedback()
    {
        return UniTask.CompletedTask;
    }

    public virtual void Cleanup()
    {
        Destroy(gameObject);
    }
}