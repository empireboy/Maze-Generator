using System.Globalization;
using System.Threading;
using UnityEngine;

public class InitializeAsEnglish : MonoBehaviour
{
    private void Awake()
    {
        // Ensure that input fields use English formatting
        CultureInfo cultureInfo = new("en-US");
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
    }
}
