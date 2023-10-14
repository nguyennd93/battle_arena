using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormStudio.Common.Native.iOS
{
    public class GameCenterAuthenListener : MonoBehaviour
    {
#if UNITY_IOS
        public event System.Action AuthenCallback;

        public void GameCenterAuthenDidFinish()
        {
            AuthenCallback?.Invoke();
        }
#endif
    }
}


