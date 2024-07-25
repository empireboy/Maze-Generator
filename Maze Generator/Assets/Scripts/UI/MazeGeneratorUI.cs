using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MazeGeneration
{
    public class MazeGeneratorUI : MonoBehaviour, IFormErrorHandler
    {
        private const string FormErrorFormat = "The {0} must be between {1} and {2}";

        public Action<string> OnFormError { get; set; }
        public Action OnFormSuccess { get; set; }

        [SerializeField]
        private MazeGenerator _mazeGenerator;

        [SerializeField]
        private TMP_InputField _widthInput;

        [SerializeField]
        private TMP_InputField _heightInput;

        [SerializeField]
        private TMP_InputField _searchDelayInput;

        [SerializeField]
        private Button _generateButton;

        private void Awake()
        {
            _generateButton.onClick.AddListener(OnGenerateMaze);
        }

        private void OnDestroy()
        {
            _generateButton.onClick.RemoveListener(OnGenerateMaze);
        }

        private void OnGenerateMaze()
        {
            int width = int.Parse(_widthInput.text);
            int height = int.Parse(_heightInput.text);
            float searchDelay = float.Parse(_searchDelayInput.text);

            // Do not generate the maze if any input field gives an error
            if (!ValidateInputs(width, height, searchDelay))
                return;

            // Make sure there are no error messages displayed
            OnFormSuccess?.Invoke();

            _mazeGenerator.Generate(
                width,
                height,
                searchDelay,
                _mazeGenerator.MazeRootTransform
            );
        }

        private bool ValidateInputs(int width, int height, float searchDelay)
        {
            // Make sure the width stays within range
            if (
                width < MazeGenerator.MinWidth ||
                width > MazeGenerator.MaxWidth
            )
            {
                FormError("Width", MazeGenerator.MinWidth, MazeGenerator.MaxWidth);

                return false;
            }

            // Make sure the height stays within range
            if (
                height < MazeGenerator.MinHeight ||
                height > MazeGenerator.MaxHeight
            )
            {
                FormError("Height", MazeGenerator.MinHeight, MazeGenerator.MaxHeight);

                return false;
            }

            // Make sure the search delay stays within range
            if (
                searchDelay < MazeGenerator.MinSearchDelay ||
                searchDelay > MazeGenerator.MaxSearchDelay
            )
            {
                FormError("Search Delay", MazeGenerator.MinSearchDelay, MazeGenerator.MaxSearchDelay);

                return false;
            }

            return true;
        }

        private void FormError(string inputName, float minValue, float maxValue)
        {
            OnFormError?.Invoke(
                string.Format(FormErrorFormat, inputName, minValue, maxValue)
            );
        }
    }
}
