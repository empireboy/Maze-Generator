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
        _formErrorHandler = GetComponent<IFormErrorHandler>();

        _formErrorHandler.OnFormError += OnFormError;
        _formErrorHandler.OnFormSuccess += OnFormSuccess;

        errorText.text = string.Empty;
    }

    private void OnDestroy()
    {
        _formErrorHandler.OnFormError -= OnFormError;
        _formErrorHandler.OnFormSuccess -= OnFormSuccess;
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