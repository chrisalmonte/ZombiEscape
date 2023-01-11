using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ZombieScape.AI
{
    public class AIStateView : MonoBehaviour
    {
        private Color[] _colorStateArray = { new Color(0.04158059f, 0.6886792f, 0.4994586f, 1), 
                                             new Color(0.6901961f, 0.04313726f, 0.129425f, 1) };

        [SerializeField] private Test _test;
        [SerializeField] private Image _stateImage;
        [SerializeField] private TextMeshProUGUI _stateName;
        private void Start()
        {
            _test.OnStateChange += _test_OnStateChange;
        }

        private void _test_OnStateChange(string stateName, int stateIndex)
        {
            _stateName.text = stateName;
            _stateImage.color= _colorStateArray[stateIndex];
        }
    }
}
