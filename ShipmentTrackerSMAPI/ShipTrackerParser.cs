using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShipmentTrackerSMAPI {
    class ShipTrackerParser {
        

        public static void saveShippedItems(string statsFolderPath, string playerName, string farmName, List<ItemTrack> trackedItems, int year, int month, int day) {
            //adjust for month transition
            if (day == 0) {
                day = 28;
                month--;
                if (month == -1) {
                    month = 3;
                    year--;
                }
            }
            String statsFilePath = statsFolderPath + playerName + " Track Stats.csv";

            Log.Verbose("Saving stats for " + trackedItems.Count + " items for day " + day + " of the " + month + " season of year " + year + " to the file " + statsFilePath);
            StringBuilder csv = new StringBuilder();

            if (!File.Exists(statsFilePath)) {
                addHeaderLine(csv, playerName, farmName);
            }

            csv.Append(year + "," + month + "," + day);
            foreach (ItemTrack item in trackedItems) {
                csv.Append("," + item.getID() + "," + item.getCount() + "," + item.getPrice() + "," + item.getCategory() + "," + item.getParentSheetIndex());
            }
            csv.AppendLine();
            File.AppendAllText(statsFilePath, csv.ToString());
        }

        private static void addHeaderLine(StringBuilder csv, string playerName, string farmName) {
            csv.AppendLine(playerName + "," + farmName + "," +  DateTime.Now.ToString("yyyyMMddhmmss"));
        }
    }
}
