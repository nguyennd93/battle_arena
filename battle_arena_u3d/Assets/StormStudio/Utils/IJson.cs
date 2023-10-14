using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public interface IFromJson
    {
        void FromJson(object jsonDict);
    }

    public interface IToJson
    {
        string ToJson();
    }
}
