
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.IO;

namespace ShipmentTrackerSMAPI {

    public class MainModClass : Mod {
        private String statsFilePath;
        private int lastDayRun = -1;
        private List<Item> itemsToShip;

        public override void Entry(params object[] objects) {

            statsFilePath = PathOnDisk + Path.DirectorySeparatorChar;

            //also maybe could use:
            //GameEvents.Events.TimeEvents.DayOfMonthChanged
            GameEvents.QuarterSecondTick += GameEvents_QuarterSecondTick;
            Log.Verbose("Shipment Tracker by Iceburg333 => Initialized");
        }

        private void GameEvents_QuarterSecondTick(object sender, EventArgs e) {
            if (!Game1.hasLoadedGame) { 
                return;
            }

            //every update, store what is in the bin. Then at night when they see the shipping results we have the items ready
            if (Game1.activeClickableMenu == null) {                
                itemsToShip = new List<Item>(Game1.getFarm().shippingBin);
                //if (itemsToShip.Count > 0) {
                //    Log.Verbose("Found " + itemsToShip.Count + " items");
                //}
            }


            if (Game1.dayOfMonth == lastDayRun) {
                //Don't run a second time
                return;
            }

            if (Game1.activeClickableMenu is ShippingMenu) {                
                Log.Verbose("End of day, saving shipping stats");
                ShippingTracker tracker = new ShippingTracker(itemsToShip, statsFilePath, Game1.player.name, Game1.player.farmName);
                tracker.parseAndSaveItems();

                //reset for next day
                lastDayRun = Game1.dayOfMonth;
                itemsToShip = null;

                Log.Verbose("Shipping Stats saved");
            }
        }

    }
}
