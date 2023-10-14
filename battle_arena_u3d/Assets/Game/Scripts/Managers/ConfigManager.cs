using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class ConfigManager : MonoBehaviour
    {
        public static ConfigManager Instance;

        [Header("Configs")]
        [SerializeField] List<WaveConfig> _waveConfigs;

        Dictionary<int, WaveConfig> _waves;

        void Awake()
        {
            Instance = this;
        }

        public void LoadConfig()
        {
            _waves = new Dictionary<int, WaveConfig>();
            foreach (var wave in _waveConfigs)
                _waves.Add(wave.ID, wave);
        }

        public WaveConfig GetWaveConfig(int id)
        {
            return _waves[id];
        }
    }
}