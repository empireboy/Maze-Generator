using UnityEngine;

public class CloseApplicationOnKey : MonoBehaviour
{
    public KeyCode key;

    private void Update()
    {
        if (Input.GetKeyDown(key))
            Application.Quit();
    }
}
