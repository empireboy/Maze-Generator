using TMPro;
using UnityEngine;

[RequireComponent(typeof(IFormErrorHandler))]
public class FormError : MonoBehaviour
{
    [SerializeField]
    TMP_Text errorText;

    private IFormErrorHandler _formErrorHandler;

    private void Awake()
    {
        // Add the formErrorHandler if it found it
        if (TryGetComponent(out _formErrorHandler))
        {
            _formErrorHandler.OnFormError += OnFormError;
            _formErrorHandler.OnFormSuccess += OnFormSuccess;
        }

        errorText.text = string.Empty;
    }

    private void OnDestroy()
    {
        // Make sure that this object is not destroyed already
        if (_formErrorHandler != null)
        {
            _formErrorHandler.OnFormError -= OnFormError;
            _formErrorHandler.OnFormSuccess -= OnFormSuccess;
        }
    }

    private void OnFormError(string errorMessage)
    {
        // Display the error message
        errorText.text = errorMessage;
    }

    private void OnFormSuccess()
    {
        // Remove the error message
        errorText.text = string.Empty;
    }
}