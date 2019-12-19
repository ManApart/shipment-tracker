

using StardewModdingAPI;
using StardewValley;
using StardewValley.Network;
using Microsoft.Xna.Framework.Net;
using System;
using System.Collections.Generic;
using System.IO;
using StardewValley.Menus;

namespace ShipmentTrackerSMAPI {

    public class MainModClass : Mod {
        private static MainModClass instance;

      private String statsFilePath;
        private int lastDayRun = -1;
        private List<Item> itemsToShip;


        public override void Entry(IModHelper helper) {
           instance = this;
            statsFilePath = helper.DirectoryPath + Path.DirectorySeparatorChar;
            helper.Events.GameLoop.Saved += AfterSaveEvent;
            helper.Events.GameLoop.OneSecondUpdateTicking += GameEvents_OneSecondTick;

            //also maybe could use:
            //GameEvents.Events.TimeEvents.DayOfMonthChanged

            Log("Shipment Tracker by Iceburg333 => Initialized");
        }

        private void AfterSaveEvent(object sender, EventArgs e) {            
            Log("after save");
            saveStats();
        }

        private void GameEvents_OneSecondTick(object sender, EventArgs e) {
            if (!Game1.hasLoadedGame) { 
                return;
            }

            //every update, store what is in the bin. Then at night when they see the shipping results we have the items ready
            if (Game1.activeClickableMenu == null) {
                int itemCount = (itemsToShip !=null ) ? itemsToShip.Count : 0;

                itemsToShip = new List<Item>(Game1.getFarm().getShippingBin(Game1.player));
                //itemsToShip = new List<Item>(Game1.getFarm().getShippingBin(Game1.player));
                if (itemsToShip.Count > itemCount)
                {
                    Log("Added " + (itemsToShip.Count - itemCount) + " items to ship");
                }
            }
        }

        private void saveStats() {
            if (Game1.dayOfMonth == lastDayRun) {
                //Don't run a second time
                Log("Day is last day run, aborting");
                return;
            }

            Log("End of day, saving shipping stats");
            ShippingTracker tracker = new ShippingTracker(itemsToShip, statsFilePath, Game1.player.Name, Game1.getFarm().Name);            
            tracker.parseAndSaveItems();

            //reset for next day
            lastDayRun = Game1.dayOfMonth;
            itemsToShip = null;

            Log("Shipping Stats saved");
        }

    
        public static void Log(string message) {
            instance.Monitor.Log(message, LogLevel.Debug);
        }
    }
}
