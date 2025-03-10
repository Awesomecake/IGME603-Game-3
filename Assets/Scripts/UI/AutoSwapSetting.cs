using System;
using UnityEngine;
using UnityEngine.UI;

public class AutoSwapSetting : MonoBehaviour
{
    [SerializeField] private SettingsData settingsData;
    [SerializeField] private Toggle toggle;

    private void Start()
    {
        toggle.isOn = settingsData.isAutoSwapEnabled;
        toggle.onValueChanged.AddListener(Toggle);
    }

    private void OnDestroy()
    {
        toggle?.onValueChanged?.RemoveListener(Toggle);
    }

    private void Toggle(bool isEnabled)
    {
        settingsData.isAutoSwapEnabled = isEnabled;
    }
}