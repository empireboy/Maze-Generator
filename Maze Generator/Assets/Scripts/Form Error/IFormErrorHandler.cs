using System;

public interface IFormErrorHandler
{
    Action<string> OnFormError { get; set; }
    Action OnFormSuccess { get; set; }
}