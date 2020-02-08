package com.sg.ssimsapp.Model;

public class Item {

    private String ID;
    private String Category;
    private String Description;
    private String UnitOfMeasure;
    private String ImageURL;

    public Item() {
    }

    public Item(String ID, String category, String description, String unitOfMeasure, String imageURL) {
        this.ID = ID;
        Category = category;
        Description = description;
        UnitOfMeasure = unitOfMeasure;
        ImageURL = imageURL;
    }

    public String getID() {
        return ID;
    }

    public void setID(String ID) {
        this.ID = ID;
    }

    public String getCategory() {
        return Category;
    }

    public void setCategory(String category) {
        Category = category;
    }

    public String getDescription() {
        return Description;
    }

    public void setDescription(String description) {
        Description = description;
    }

    public String getUnitOfMeasure() {
        return UnitOfMeasure;
    }

    public void setUnitOfMeasure(String unitOfMeasure) {
        UnitOfMeasure = unitOfMeasure;
    }

    public String getImageURL() {
        return ImageURL;
    }

    public void setImageURL(String imageURL) {
        ImageURL = imageURL;
    }
}
