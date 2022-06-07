namespace Shop.Domain.Catalog
{
    public enum AttributeControlType
    {
        DropdownList = 1,
        RadioList = 2,
        Checkboxes = 3,
        Textbox = 4,
        MultipleLineTextBox = 5,
        FileUpload = 6,
        ColorSquareRGB = 7,
        ImageSquare = 8,
        ReadOnlyCheckboxes = 9,
    }

    public enum ProductVariantAttributeValueType
    {
        Simple = 0,
        ProductLink = 1
    }
}
