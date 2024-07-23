using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    private const string CoroutineRunnerName = "CoroutineRunner";

    public static CoroutineRunner Instance
    {
        get
        {
            if (!_instance)
            {
                GameObject coroutineRunnerObject = new();

                _instance = coroutineRunnerObject.AddComponent<CoroutineRunner>();
                coroutineRunnerObject.name = CoroutineRunnerName;
            }

            return _instance;
        }
    }

    private static CoroutineRunner _instance = null;
}
