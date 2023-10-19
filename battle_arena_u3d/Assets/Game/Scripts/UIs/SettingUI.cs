using System.Collections;
using System.Collections.Generic;
using StormStudio.Common.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UIController
{
    [SerializeField] TMP_Text _textCount;
    [SerializeField] TMP_Text _textInterval;

    [Header("Slide")]
    [SerializeField] Slider _sliderCount;
    [SerializeField] Slider _sliderInterval;

    System.Action<int> _onSpawnChanged;
    System.Action<int> _onIntervalChanged;

    public System.Action OnClosed;
    public System.Action OnReset;

    public void Setup(int enemyPerTurn, int interval, System.Action<int> onSpawnChanged, System.Action<int> onIntervalChanged)
    {
        _onSpawnChanged = onSpawnChanged;
        _onIntervalChanged = onIntervalChanged;

        _sliderCount.maxValue = 1000;
        _sliderCount.minValue = 10;

        _sliderInterval.maxValue = 100;
        _sliderInterval.minValue = 1;

        _sliderInterval.value = interval;
        _sliderCount.value = enemyPerTurn;
        OnSpawnChanged();
        OnIntervalChanged();
    }

    public void OnSpawnChanged()
    {
        int count = Mathf.RoundToInt(_sliderCount.value);
        _textCount.text = $"{count} per turn";
        _onSpawnChanged?.Invoke(count);
    }

    public void OnIntervalChanged()
    {
        int count = Mathf.RoundToInt(_sliderInterval.value);
        _textInterval.text = $"Spawn in {count}s";
        _onIntervalChanged?.Invoke(count);
    }

    public void TouchedClose()
    {
        UIManager.Instance.ReleaseUI(this, true);
        OnClosed?.Invoke();
    }

    public void TouchedReset()
    {
        UIManager.Instance.ReleaseUI(this, true);
        OnReset?.Invoke();
    }
}
