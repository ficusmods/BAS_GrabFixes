using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;

namespace GrabFixes
{
    class NoChokeRotationModule : MonoBehaviour
    {
        Creature creature;
        float oriDestabilizedSpringRotationMultiplier = 0.0f;
        bool active = false;

        public static HashSet<Creature.FallState> watchedFallStates = new HashSet<Creature.FallState> { Creature.FallState.Falling, Creature.FallState.Stabilizing, Creature.FallState.NearGround, Creature.FallState.StabilizedOnGround };

        void Awake()
        {
            creature = gameObject.GetComponent<Creature>();
            oriDestabilizedSpringRotationMultiplier = creature.ragdoll.destabilizedSpringRotationMultiplier;
        }


        void FixedUpdate()
        {
            if (Config.fixLiftRotation && watchedFallStates.Contains(creature.fallState) && (creature.ragdoll.isGrabbed || creature.ragdoll.isTkGrabbed))
            {
                if (!active)
                {
                    creature.ragdoll.destabilizedSpringRotationMultiplier = creature.ragdoll.destabilizedGroundSpringRotationMultiplier;
                    creature.UpdateFall();
                    active = true;
                }
            }
            else
            {
                if (active)
                {
                    creature.ragdoll.destabilizedSpringRotationMultiplier = oriDestabilizedSpringRotationMultiplier;
                    creature.UpdateFall();
                }
                active = false;
            }
        }

    }
}
