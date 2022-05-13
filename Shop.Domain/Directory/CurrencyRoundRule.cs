namespace Shop.Domain.Directory
{
    public enum CurrencyRoundRule
    {
        RoundMidPointDown = 0, //9.225 -> 9.20
        RoundMidPointUp = 1, //9.225 -> 9.25
        RoundDown = 2, //9.24 -> 9.20
        RoundUp = 3, //9.26 -> 9.30
    }
}
