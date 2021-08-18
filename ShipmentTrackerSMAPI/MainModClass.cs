

using StardewModdingAPI;
using StardewValley;
using StardewValley.Network;
using Microsoft.Xna.Framework.Net;
using System;
using System.Collections.Generic;
using System.IO;
using StardewValley.Menus;
using StardewModdingAPI.Events;
using StardewValley.Objects;
using Netcode;


//https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Events
namespace ShipmentTrackerSMAPI {

    public class MainModClass : Mod {
        private static MainModClass instance;

      private String statsFilePath;
        private int lastDayRun = -1;
        private Dictionary<Chest, List<Item>> chestsToShip;
        private Dictionary<Farmer, List<Item>> farmerItems;


        public override void Entry(IModHelper helper) {
           instance = this;
            statsFilePath = helper.DirectoryPath + Path.DirectorySeparatorChar;
            helper.Events.GameLoop.Saved += AfterSaveEvent;
            helper.Events.Display.MenuChanged += OnShippingBinUpdate;
            helper.Events.World.ChestInventoryChanged+= OnChestChanged;

            chestsToShip = new Dictionary<Chest, List<Item>>();
            farmerItems = new Dictionary<Farmer, List<Item>>();


            //also maybe could use:
            //GameEvents.Events.TimeEvents.DayOfMonthChanged

            Log("Shipment Tracker by Iceburg333 => Initialized");
        }

        private void AfterSaveEvent(object sender, EventArgs e) {            
            Log("after save");
            saveStats();
        }

        private void OnShippingBinUpdate(object sender, EventArgs e) {
            if (!Game1.hasLoadedGame) { 
                return;
            }

            //every update, store what is in the bin. Then at night when they see the shipping results we have the items ready
            NetCollection<Item> previous = null;
            if (Game1.activeClickableMenu == null) {
                foreach (Farmer farmer in Game1.getAllFarmers())
                {
                    int itemCount = (farmerItems.ContainsKey(farmer)) ? farmerItems[farmer].Count : 0;
                    NetCollection<Item> current = Game1.getFarm().getShippingBin(farmer);
                    if (current != previous)
                    {
                        farmerItems[farmer] = new List<Item>(current);
                    } else
                    {
                        farmerItems[farmer] = new List<Item>();
                    }
                    previous = current;
                    

                    if (farmerItems[farmer].Count > itemCount)
                    {
                        Log("Added " + (farmerItems[farmer].Count - itemCount) + " items to ship for " + farmer.name);
                    }
                }
            }
        }

        private void OnChestChanged(object sender, EventArgs e)
        {
            ChestInventoryChangedEventArgs chestEvent = e as ChestInventoryChangedEventArgs;
            Chest chest = chestEvent.Chest;
            if (chest.specialChestType == 1){
                int itemCount = (chestsToShip.ContainsKey(chest)) ? chestsToShip[chest].Count : 0;
                chestsToShip[chestEvent.Chest] = new List<Item>(chestEvent.Chest.items);

                if (chestsToShip[chest].Count > itemCount)
                {
                    Log("Added " + (chestsToShip[chest].Count - itemCount) + " items to ship.");
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
            
            List<Item> itemsToShip = new List<Item>();
            foreach (List<Item> shipItems in farmerItems.Values)
            {
                itemsToShip.AddRange(shipItems);
            }

            foreach (List<Item> shipItems in chestsToShip.Values)
            {
                itemsToShip.AddRange(shipItems);
            }

            ShippingTracker tracker = new ShippingTracker(itemsToShip, statsFilePath, Game1.player.Name, Game1.getFarm().Name);            
            tracker.parseAndSaveItems();

            //reset for next day
            lastDayRun = Game1.dayOfMonth;
            farmerItems.Clear();
            chestsToShip.Clear();

            Log("Shipping Stats saved");
        }

    
        public static void Log(string message) {
            instance.Monitor.Log(message, LogLevel.Debug);
        }
    }
}
