﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Menus.Compatability
{
    class CompatabilityManager
    {
        public static bool characterCustomizer;
        public static bool loadMenu;
        public static bool aboutMenu;
        public static bool doUpdate;
        public static Menus.Compatability.CompatInterface compatabilityMenu;

        public static void doUpdateSet(bool f)
        {
           doUpdate = f;
        }

        public static void doUpdateGet()
        {

        }
    }
}
