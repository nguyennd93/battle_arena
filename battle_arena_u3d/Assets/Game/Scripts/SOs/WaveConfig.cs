using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "WaveConfig", menuName = "Games/Wave Config", order = 1)]
    public class WaveConfig : ScriptableObject                                                                              
    {
        public int ID;
        public int EnemyPerStep;
        public float TimeStep;
        public List<EnemyConfig> Enemies;
    }
}