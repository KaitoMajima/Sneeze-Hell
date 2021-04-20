using UnityEngine;

namespace KaitoCo
{
    public interface IDetectionBehaviour
    {
        void Detect(ref MovementInput input);
    }
}
