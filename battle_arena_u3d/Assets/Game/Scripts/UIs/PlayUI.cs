using System.Collections;
using System.Collections.Generic;
using StormStudio.Common.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : UIController
{
    [SerializeField] UIProfile _profile;
    [SerializeField] TMP_Text _textMelee;
    [SerializeField] TMP_Text _textRange;

    [Header("Button Skill")]
    [SerializeField] Button _btnSkill;
    [SerializeField] Image _skillMask;

    public System.Action OnSetting;

    public void Setup(string userName, int MaximumHP)
    {
        _profile.SetName(userName);
        _profile.SetHP(MaximumHP, MaximumHP);
    }

    public void UpdateGameInfo(GameInfo info)
    {
        _textMelee.text = Mathf.Max(info.MeleeSpawn - info.MeleeDead).ToString();
        _textRange.text = Mathf.Max(info.RangeSpawn - info.RangeDead).ToString();
        _profile.UpdateInfo(info);
    }

    public void UpdateHP(int hp, int maxHP)
    {
        _profile.SetHP(hp, maxHP);
    }

    public void UpdateSkillReload(float percent)
    {
        _btnSkill.interactable = percent >= 0.99f;
        _skillMask.fillAmount = 1f - percent;
    }

    public void TouchedSetting()
    {
        OnSetting?.Invoke();
    }
}
