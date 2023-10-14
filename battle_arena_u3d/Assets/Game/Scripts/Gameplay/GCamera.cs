using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class GCamera : MonoBehaviour
    {
        public static GCamera Instance;
        public Transform TargetCamara;

        void Awake()
        {
            Instance = this;
        }
    }
}
