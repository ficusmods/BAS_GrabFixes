using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;

namespace GrabFixes
{
    class NoGrabMoveModule : MonoBehaviour
    {
        Creature creature;

        bool active = false;
        BrainModuleMove moveModule = null;

        void Awake()
        {
            creature = gameObject.GetComponent<Creature>();
            moveModule = creature.brain.instance.GetModule<BrainModuleMove>();
        }

        void FixedUpdate()
        {
            if (!Player.local || !Player.local.creature) return;
            if (creature.ragdoll.GetPart(RagdollPart.Type.LeftFoot) == null || creature.ragdoll.GetPart(RagdollPart.Type.RightFoot) == null) return;

            bool toActivate = false;
            if (Config.fixGrabMove)
            {
                Vector3 playerLeftFeetLoc = Player.local.creature.ragdoll.GetPart(RagdollPart.Type.LeftFoot).transform.position;
                Vector3 playerRightFeetLoc = Player.local.creature.ragdoll.GetPart(RagdollPart.Type.RightFoot).transform.position;
                Vector3 playerLoc = Vector3.Lerp(playerLeftFeetLoc, playerRightFeetLoc, 0.5f);

                Vector3 creatureLeftFeetLoc = creature.ragdoll.GetPart(RagdollPart.Type.LeftFoot).transform.position;
                Vector3 creatureRightFeetLoc = creature.ragdoll.GetPart(RagdollPart.Type.RightFoot).transform.position;
                Vector3 creatureLoc = Vector3.Lerp(creatureLeftFeetLoc, creatureRightFeetLoc, 0.5f);

                float dist = Vector3.Distance(creatureLoc, playerLoc);
                toActivate = (dist <= Config.grabMoveStopRadius) ? true : false;
            }

            if (creature.ragdoll.isGrabbed && toActivate)
            {
                if (moveModule != null)
                {
                    moveModule.StopMove();
                    moveModule.allowMove = false;
                    creature.locomotion.allowMove = false;
                }
                active = true;
            }
            else
            {
                if (active)
                {
                    if (moveModule != null)
                    {
                        moveModule.allowMove = true;
                        creature.locomotion.allowMove = true;
                    }
                }
                active = false;
            }

        }

    }
}
