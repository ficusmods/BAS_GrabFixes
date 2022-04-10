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

        void Update()
        {
            bool toActivate = false;
            if (Config.fixGrabMove)
            {
                Vector3 playerLoc = Player.local.creature.ragdoll.headPart.transform.position;
                float dist = float.MaxValue;
                foreach (var handler in creature.ragdoll.handlers)
                {
                    float currDist = Vector3.Distance(handler.grabbedHandle.transform.position, playerLoc);
                    dist = Math.Min(currDist, dist);
                }
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
