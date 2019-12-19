
using ShipmentTrackerSMAPI;
using StardewValley.Objects;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Locations;
using StardewValley.TerrainFeatures;
using System.IO;
using System.Xml.Serialization;
using xTile.Dimensions;


namespace StardewValley.Menus {
    public class ShippingTracker {
        private List<Item> items;
        private string statsFolderPath, playerName, farmName;

        public ShippingTracker(List<Item> items, string statsFilePath, string playerName, string farmName) {
            this.items = items;
            this.statsFolderPath = statsFilePath;
            this.playerName = playerName;
            this.farmName = farmName;
            MainModClass.Log("Found " + items.Count + " items to track");
        }

        public void parseAndSaveItems() {
            List<ItemTrack> trackedItems = parseItemsForTracker(items);
            MainModClass.Log("Collected " + trackedItems.Count + " shipped items to track");
            int season = getCurrentSeason();
            ShipTrackerParser.saveShippedItems(statsFolderPath, playerName, farmName, trackedItems, Game1.year, season, Game1.dayOfMonth-1);
        }

        private List<ItemTrack> parseItemsForTracker(List<Item> items) {
            Utility.consolidateStacks(items);
            List<ItemTrack> trackedItems = new List<ItemTrack>();

            foreach (Item current in items) {
                if (current is StardewValley.Object) {
                    Object @object = current as Object;                   

                    ItemTrack item = new ItemTrack(@object.name, @object.Stack, @object.sellToStorePrice(), getCategoryIndex(@object), @object.ParentSheetIndex);
                    trackedItems.Add(item);
                    //testPrintItem(current);
                }
            }
            return trackedItems;
          
        }

        private void testPrintItem(Item item) {
            Object @object = item as Object;
            string debugString = item.Name + ", " + item.getCategoryName();

            debugString += ", parentsheetindex: " + @object.ParentSheetIndex + ", category: " + @object.Category + ", categoryName: " + @object.getCategoryName();
            MainModClass.Log(debugString);
        }
        

        private int getCurrentSeason() {
            if (Game1.IsSpring) {
                return 0;
            } else if (Game1.IsSummer) {
                return 1;
            } else if (Game1.IsFall) {
                return 2;
            } else {
                return 3;
            }

        }

        public int getCategoryIndex(StardewValley.Object @object) {
            if (@object is Furniture) {
                return 1;
                //return "Furniture";
            }
            if (@object.Type != null && @object.Type.Equals("Arch")) {
                return 2;
                //return "Artifact";
            }
            int category = @object.Category;
            switch (category) {
                case -81:
                    //return "Forage";
                    return 3;
                case -80:
                    //return "Flower";
                    return 4;
                case -79:
                    //return "Fruit";
                    return 5;
                case -78:
                case -77:
                case -76:
                    break;
                case -75:
                    //return "Vegetable";
                    return 6;
                case -74:
                    //return "Seed";
                    return 7;
                default:
                    switch (category) {
                        case -28:
                            //return "Monster Loot";
                            return 8;
                        case -27:
                        case -26:
                            //return "Artisan Goods";
                            return 9;
                        case -25:
                            //return "Cooking";
                            return 10;
                        case -24:
                            //return "Decor";
                            return 11;
                        case -22:
                            //return "Fishing Tackle";
                            return 12;
                        case -21:
                            //return "Bait";
                            return 13;
                        case -20:
                            //return "Trash";
                            return 14;
                        case -19:
                            //return "Fertilizer";
                            return 15;
                        case -18:
                        case -14:
                        case -5:
                            //return "Animal Product";
                            return 16;
                        case -16:
                        case -15:
                            //return "Resource";
                            return 17;
                        case -12:
                        case -2:
                            //return "Mineral";
                            return 18;
                        case -8:
                            //return "Crafting";
                            return 19;
                        case -7:
                            //return "Cooking";
                            return 10;
                        case -6:
                            //return "Animal Product";
                            return 16;
                        case -4:
                            //return "Fish";
                            return 20;
                    }
                    break;
            }
            //return "";
            return 0;
        }

        //public override Color getCategoryColor() {
        //    if (this is Furniture) {
        //        return new Color(100, 25, 190);
        //    }
        //    if (this.type != null && this.type.Equals("Arch")) {
        //        return new Color(110, 0, 90);
        //    }
        //    int category = this.category;
        //    switch (category) {
        //        case -81:
        //            return new Color(10, 130, 50);
        //        case -80:
        //            return new Color(219, 54, 211);
        //        case -79:
        //            return Color.DeepPink;
        //        case -78:
        //        case -77:
        //        case -76:
        //            break;
        //        case -75:
        //            return Color.Green;
        //        case -74:
        //            return Color.Brown;
        //        default:
        //            switch (category) {
        //                case -28:
        //                    return new Color(50, 10, 70);
        //                case -27:
        //                case -26:
        //                    return new Color(0, 155, 111);
        //                case -24:
        //                    return Color.Plum;
        //                case -22:
        //                    return Color.DarkCyan;
        //                case -21:
        //                    return Color.DarkRed;
        //                case -20:
        //                    return Color.DarkGray;
        //                case -19:
        //                    return Color.SlateGray;
        //                case -18:
        //                case -14:
        //                case -6:
        //                case -5:
        //                    return new Color(255, 0, 100);
        //                case -16:
        //                case -15:
        //                    return new Color(64, 102, 114);
        //                case -12:
        //                case -2:
        //                    return new Color(110, 0, 90);
        //                case -8:
        //                    return new Color(148, 61, 40);
        //                case -7:
        //                    return new Color(220, 60, 0);
        //                case -4:
        //                    return Color.DarkBlue;
        //            }
        //            break;
        //    }
        //    return Color.Black;
        //}

        

    }
}
