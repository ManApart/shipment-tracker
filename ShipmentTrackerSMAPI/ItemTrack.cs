using System;

public class ItemTrack{

    private string itemID;
    private int count, price, category, parentSheetIndex;


	public ItemTrack(string name, int count, int price, int category, int parentSheetIndex){
        this.itemID = name;
        this.count = count;
        this.price = price;
        this.category = category;
        this.parentSheetIndex = parentSheetIndex;
    }

    public string getID() {
        return itemID;
    }

    public int getCount() {
        return count;
    }
    public int getPrice() {
        return price;
    }
    public int getCategory() {
        return category;
    }
    public int getParentSheetIndex() {
        return parentSheetIndex;
    }

}
