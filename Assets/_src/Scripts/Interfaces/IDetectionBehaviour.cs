using UnityEngine;

namespace KaitoCo
{
    public interface IDetectionBehaviour
    {
        void Detect(Transform detectionTransform, ref MovementInput input);
    }
}
