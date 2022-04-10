using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ThunderRoad;
using UnityEngine;

namespace GrabFixes
{
    public class LoadModule : LevelModule
    {

        public string mod_version = "0.0";
        public string mod_name = "UnnamedMod";
        public string logger_level = "Basic";
        public bool FixLiftRotation
        {
            get => Config.fixLiftRotation;
            set => Config.fixLiftRotation = value;
        }

        public bool FixGrabMove
        {
            get => Config.fixGrabMove;
            set => Config.fixGrabMove = value;
        }
        public float GrabMoveStopRadius
        {
            get => Config.grabMoveStopRadius;
            set => Config.grabMoveStopRadius = value;
        }

        public override IEnumerator OnLoadCoroutine()
        {
            Logger.init(mod_name, mod_version, logger_level);
            Logger.Basic("Loading " + mod_name);

            EventManager.onCreatureSpawn += EventManager_onCreatureSpawn;

            return base.OnLoadCoroutine();
        }

        private void EventManager_onCreatureSpawn(Creature creature)
        {
            if (!creature.isPlayer)
            {
                if (!creature.gameObject.TryGetComponent<NoChokeRotationModule>(out NoChokeRotationModule _))
                {
                    Logger.Detailed(String.Format("Adding NoChokeRotationModule component to {0} ({1}, {2})", creature.name, creature.creatureId, creature.GetInstanceID()));
                    creature.gameObject.AddComponent<NoChokeRotationModule>();
                }

                if (!creature.gameObject.TryGetComponent<NoGrabMoveModule>(out NoGrabMoveModule _))
                {
                    Logger.Detailed(String.Format("Adding NoGrabMoveModule component to {0} ({1}, {2})", creature.name, creature.creatureId, creature.GetInstanceID()));
                    creature.gameObject.AddComponent<NoGrabMoveModule>();
                }
            }
        }

    }
}
