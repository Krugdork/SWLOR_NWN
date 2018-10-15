﻿using System.Linq;
using NWN;
using SWLOR.Game.Server.Event;
using SWLOR.Game.Server.GameObject;

namespace SWLOR.Game.Server.AreaInstance
{
    public class ExitAreaInstance: IRegisteredEvent
    {
        private readonly INWScript _;

        public ExitAreaInstance(INWScript script)
        {
            _ = script;
        }

        public bool Run(params object[] args)
        {
            NWObject door = Object.OBJECT_SELF;

            if (!door.Area.IsInstance) return false;

            NWObject target = _.GetTransitionTarget(door);
            NWPlayer player = _.GetClickingObject();
            
            _.DelayCommand(6.0f, () =>
            {
                int playerCount = NWModule.Get().Players.Count(x => !Equals(x, player) && Equals(x.Area, door.Area));
                if (playerCount <= 0)
                {
                    _.DestroyArea(door.Area);
                }
            });

            player.AssignCommand(() =>
            {
                _.ActionJumpToLocation(target.Location);
            });

            return true;
        }
    }
}
