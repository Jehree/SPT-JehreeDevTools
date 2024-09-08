using Comfort.Common;
using EFT;
using UnityEngine;

namespace JehreeDevTools.Common
{
    internal class LookAtTarget : JDTComponentBase
    {
        public GameObject Target;
        public float DistanceCutoff;
        public bool InvertLook = false;

        public void Init(GameObject target, bool invertLook = false, float distanceCutoff = 15f)
        {
            Target = target;
            DistanceCutoff = distanceCutoff;
            InvertLook = invertLook;
        }

        private void Update()
        {
            if (Target == null || DistanceCutoff <= 0) return;
            if (Vector3.Distance(Target.transform.position, gameObject.transform.position) > DistanceCutoff) return;
            
            Vector3 direction = Target.transform.position - gameObject.transform.position;
            direction.y = 0;
            if (InvertLook)
            {
                direction = -direction;
            }

            if (direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            gameObject.transform.rotation = targetRotation;
        }
    }
}
