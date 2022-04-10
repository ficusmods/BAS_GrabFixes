﻿using System;
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
        float oriTurnSpeed = 0.0f;
        float oriHipsHeight = 0.0f;

        bool alterHipHeight = false;
        bool active = false;

        public static HashSet<Creature.FallState> watchedFallStates = new HashSet<Creature.FallState> { Creature.FallState.Falling, Creature.FallState.Stabilizing, Creature.FallState.NearGround, Creature.FallState.StabilizedOnGround };

        void Awake()
        {
            creature = gameObject.GetComponent<Creature>();
            oriTurnSpeed = creature.turnSpeed;
            oriHipsHeight = creature.morphology.hipsHeight;

            creature.OnFallEvent -= Creature_OnFallEvent;
            creature.OnFallEvent += Creature_OnFallEvent;
        }

        private void Creature_OnFallEvent(Creature.FallState state)
        {
            if(active)
            {
                if (state == Creature.FallState.Falling)
                {
                    alterHipHeight = true;
                }
            }
        }

        public void RefreshFallstate(Creature.FallState fallstate)
        {
            var method = creature.GetType().GetMethod("RefreshFallState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method.Invoke(creature, new object[] { fallstate });
        }

        void Update()
        {
            if (Config.fixLiftRotation && watchedFallStates.Contains(creature.fallState) && (creature.ragdoll.isGrabbed || creature.ragdoll.isTkGrabbed))
            {
                if (!active)
                {
                    RagdollPart neckPart = creature.ragdoll.GetPart(RagdollPart.Type.Neck);
                    foreach (HandleRagdoll handle in neckPart.handles)
                    {
                        handle.handleRagdollData.bodyTurnDirection = HandleRagdollData.BodyTurnDirection.GrabberPosition;
                    }
                    oriTurnSpeed = creature.turnSpeed;
                    oriHipsHeight = creature.morphology.hipsHeight;
                }
                if(alterHipHeight)
                {
                    creature.morphology.hipsHeight = creature.ragdoll.GetPart(RagdollPart.Type.Head).transform.position.y;
                }
                creature.UpdateFall();
                creature.turnSpeed = 0.0f;
                active = true;
            }
            else
            {
                if (active)
                {
                    RagdollPart neckPart = creature.ragdoll.GetPart(RagdollPart.Type.Neck);
                    foreach (HandleRagdoll handle in neckPart.handles)
                    {
                        handle.handleRagdollData.bodyTurnDirection = HandleRagdollData.BodyTurnDirection.PartDirection;
                    }
                    creature.turnSpeed = oriTurnSpeed;
                    creature.morphology.hipsHeight = oriHipsHeight;
                    alterHipHeight = false;
                }
                active = false;
            }
        }

    }
}
