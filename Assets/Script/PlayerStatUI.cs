using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatUI : MonoBehaviour
{
    [SerializeField]
    private StatText[] _texts;
    internal void UpdateView(GameSystem.Player p)
    {
        foreach (var t in _texts)
        {
            object val=null;
            switch (t._pillar)
            {
                case CardData.Pillar.Economic:
                    val = p.Money;
                    break;
                case CardData.Pillar.Social:
                    val = p.PeopleSatistfaction;
                    break;
                case CardData.Pillar.Ecologic:
                    val = p.Temperature;
                    break;
                default:
                    break;
            }
            t.UpdateValue(val);
        }
    }

    internal void UpdateView(Vector3 prmStats)
    {
        foreach (var t in _texts)
        {
            object val = null;
            switch (t._pillar)
            {
                case CardData.Pillar.Economic:
                    if (prmStats.x < 0) 
                    { 
                        val = prmStats.x; 
                    }
                    else
                    {
                        val = "+" + prmStats.x;
                    }
                    
                    break;
                case CardData.Pillar.Social:
                    if (prmStats.z < 0)
                    {
                        val = prmStats.z;
                    }
                    else
                    {
                        val = "+" + prmStats.z;
                    }
                    break;
                case CardData.Pillar.Ecologic:
                    if (prmStats.y < 0)
                    {
                        val = prmStats.y;
                    }
                    else
                    {
                        val = "+" + prmStats.y;
                    }
                    break;
                default:
                    break;
            }
            t.UpdateValue(val);
        }
    }

    [System.Serializable]
    private class StatText
    {
        public CardData.Pillar _pillar;
        [SerializeField]
        private string _prefix;
        [SerializeField]
        private string _suffix;
        [SerializeField]
        private TMP_Text _text;
        public void UpdateValue(object value) => _text.text = _prefix + value+ _suffix;
    }
}
