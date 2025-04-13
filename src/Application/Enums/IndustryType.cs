using System.Runtime.Serialization;

namespace portfolium.Application.Enums;

public enum IndustryType {
    [EnumMember(Value = "Technology")] Technology = 0,
    [EnumMember(Value = "Finance")] Finance = 1,
    [EnumMember(Value = "Healthcare")] Healthcare = 2,
    [EnumMember(Value = "Retail")] Retail = 3,
    [EnumMember(Value = "Energy")] Energy = 4,
    [EnumMember(Value = "RealEstate")] RealEstate = 5,
    [EnumMember(Value = "Manufacturing")] Manufacturing = 6,

    [EnumMember(Value = "Telecommunications")]
    Telecommunications = 7,
    [EnumMember(Value = "ConsumerGoods")] ConsumerGoods = 8,
    [EnumMember(Value = "Automotive")] Automotive = 9,

    [EnumMember(Value = "FoodAndBeverage")]
    FoodAndBeverage = 10,

    [EnumMember(Value = "Pharmaceuticals")]
    Pharmaceuticals = 11,
    [EnumMember(Value = "Utilities")] Utilities = 12,
    [EnumMember(Value = "Education")] Education = 13,
    [EnumMember(Value = "Entertainment")] Entertainment = 14,
    [EnumMember(Value = "Media")] Media = 15,
    [EnumMember(Value = "Transportation")] Transportation = 16,
    [EnumMember(Value = "Aerospace")] Aerospace = 17,
    [EnumMember(Value = "Agriculture")] Agriculture = 18,
    [EnumMember(Value = "Biotechnology")] Biotechnology = 19,
    [EnumMember(Value = "Insurance")] Insurance = 20,
    [EnumMember(Value = "Construction")] Construction = 21,
    [EnumMember(Value = "Hospitality")] Hospitality = 22,
    [EnumMember(Value = "Government")] Government = 23,
    [EnumMember(Value = "NonProfit")] NonProfit = 24,
    [EnumMember(Value = "Logistics")] Logistics = 25,

    [EnumMember(Value = "ProfessionalServices")]
    ProfessionalServices = 26,
    [EnumMember(Value = "Chemicals")] Chemicals = 27,
    [EnumMember(Value = "Mining")] Mining = 28,
    [EnumMember(Value = "Textiles")] Textiles = 29,
    [EnumMember(Value = "Fashion")] Fashion = 30,
    [EnumMember(Value = "Sports")] Sports = 31,
    [EnumMember(Value = "Gaming")] Gaming = 32,
    [EnumMember(Value = "ECommerce")] ECommerce = 33,

    [EnumMember(Value = "RenewableEnergy")]
    RenewableEnergy = 34,

    [EnumMember(Value = "MetalsAndMining")]
    MetalsAndMining = 35
}