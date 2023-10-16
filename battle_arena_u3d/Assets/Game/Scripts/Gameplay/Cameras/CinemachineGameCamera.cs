using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CinemachineGameCamera : MonoBehaviour
{
    [SerializeField] public Transform Target;

    public static CinemachineGameCamera Instance;

    void Awake()
    {
        Instance = this;
    }
}
