using System.Collections;
using System.Collections.Generic;
using StormStudio.Common.UI;
using TMPro;
using UnityEngine;

public class PlayUI : UIController
{
    [SerializeField] UIProfile _profile;
    [SerializeField] TMP_Text _textMelee;
    [SerializeField] TMP_Text _textRange;

    System.Action _onAttack;
    System.Action _onSkill;

    public void Setup(string userName, int MaximumHP, System.Action onAttack, System.Action onSkill)
    {
        _profile.SetName(userName);
        _profile.SetEnemiesKill(0);
        _profile.SetHP(MaximumHP, MaximumHP);

        UpdateAmountEnemies(0, 0);
    }

    public void UpdateAmountEnemies(int countMelee, int countRange)
    {
        _textMelee.text = countMelee.ToString();
        _textRange.text = countRange.ToString();
    }

    public void TouchedAttack()
    {
        _onAttack?.Invoke();
    }

    public void TouchedSkill()
    {
        _onSkill?.Invoke();
    }
}
