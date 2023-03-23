using System;
using System.Collections.Generic;
using System.Linq;

namespace Contaquest.Metaverse.Data
{
    public class RobotPartCatalogueCategory
    {
        public EquipSlot equipSlot;
        public Dictionary<int, RobotPart> robotParts = new Dictionary<int, RobotPart>();

        public RobotPart GetFirst()
        {
            KeyValuePair<int, RobotPart> data = robotParts.FirstOrDefault();
            return data.Value;
        }
    }
}
