using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Application.Infrastructure.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Round = table.Column<int>(type: "int", nullable: false),
                    CurrencyRound = table.Column<int>(type: "int", nullable: false),
                    CreateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Picture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true),
                    Height = table.Column<int>(type: "int", nullable: true),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picture", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttribute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttribute", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFreeShipping = table.Column<bool>(type: "bit", nullable: false),
                    IsTaxExempt = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsSystemRole = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecificationAttributeGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificationAttributeGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WareHouse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouse", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRtl = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Culture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Language_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsInlcudeTopMenu = table.Column<bool>(type: "bit", nullable: false),
                    IsShowOnHomePage = table.Column<bool>(type: "bit", nullable: false),
                    PictureId = table.Column<int>(type: "int", nullable: true),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    CreateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaKeywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Category_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Category_Picture_PictureId",
                        column: x => x.PictureId,
                        principalTable: "Picture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PictureId = table.Column<int>(type: "int", nullable: true),
                    CreateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaKeywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manufacturer_Picture_PictureId",
                        column: x => x.PictureId,
                        principalTable: "Picture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpecificationAttribute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsShowOnHomePage = table.Column<bool>(type: "bit", nullable: false),
                    IsAllowFilter = table.Column<bool>(type: "bit", nullable: false),
                    SpecificationAttributeGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificationAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecificationAttribute_SpecificationAttributeGroup_SpecificationAttributeGroupId",
                        column: x => x.SpecificationAttributeGroupId,
                        principalTable: "SpecificationAttributeGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Slug",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    EntityGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slug", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Slug_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TranslationEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    EntityGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslationEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranslationEntity_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TranslationResource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslationResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranslationResource_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: true),
                    TimezoneId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailedLoginAttempt = table.Column<int>(type: "int", nullable: false),
                    CannotLoginUntilUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastActivityUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gtin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SKu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsShowOnHomePage = table.Column<bool>(type: "bit", nullable: false),
                    IsTaxExempt = table.Column<bool>(type: "bit", nullable: false),
                    TaxId = table.Column<int>(type: "int", nullable: true),
                    IsFreeShipping = table.Column<bool>(type: "bit", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    IsMarkAsNew = table.Column<bool>(type: "bit", nullable: false),
                    IsAvaliableForOnline = table.Column<bool>(type: "bit", nullable: false),
                    MarkAsNewStartDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MarkAsNewEndDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StockQuality = table.Column<int>(type: "int", nullable: false),
                    DisplayStockAvaliablity = table.Column<bool>(type: "bit", nullable: false),
                    DisplayStockQuality = table.Column<bool>(type: "bit", nullable: false),
                    OrderMaxmimumQuality = table.Column<int>(type: "int", nullable: false),
                    OrderMinimumQuality = table.Column<int>(type: "int", nullable: false),
                    IsAvaliableForOrder = table.Column<bool>(type: "bit", nullable: false),
                    IsDisableBuyButton = table.Column<bool>(type: "bit", nullable: false),
                    IsDisableWishlistButton = table.Column<bool>(type: "bit", nullable: false),
                    IsCallForPrice = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsAllowForReview = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedRatingSum = table.Column<int>(type: "int", nullable: false),
                    ApprovedTotalReviews = table.Column<int>(type: "int", nullable: false),
                    NotApprovedRatingSum = table.Column<int>(type: "int", nullable: false),
                    NotApprovedTotalReviews = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceOnline = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Width = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ManufacturerId = table.Column<int>(type: "int", nullable: true),
                    AvaliableStartDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvaliableEndDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaKeywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Manufacturer_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpecificationAttributeOption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecificationAttributeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PictureId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificationAttributeOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecificationAttributeOption_Picture_PictureId",
                        column: x => x.PictureId,
                        principalTable: "Picture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SpecificationAttributeOption_SpecificationAttribute_SpecificationAttributeId",
                        column: x => x.SpecificationAttributeId,
                        principalTable: "SpecificationAttribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserFied",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Field = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFied", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFied_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserPassword",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPassword", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPassword_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCategory_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductPicture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    PictureId = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPicture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPicture_Picture_PictureId",
                        column: x => x.PictureId,
                        principalTable: "Picture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPicture_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantAttrbiute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductAttributeId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    TextPrompt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControlTypeId = table.Column<int>(type: "int", nullable: false),
                    ControlType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantAttrbiute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantAttrbiute_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductVariantAttrbiute_ProductAttribute_ProductAttributeId",
                        column: x => x.ProductAttributeId,
                        principalTable: "ProductAttribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RelatedProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Product1Id = table.Column<int>(type: "int", nullable: false),
                    Product2Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatedProduct_Product_Product1Id",
                        column: x => x.Product1Id,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RelatedProduct_Product_Product2Id",
                        column: x => x.Product2Id,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockQuantityHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    WareHouseId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjumentQuality = table.Column<int>(type: "int", nullable: false),
                    CreateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockQuantityHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockQuantityHistory_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockQuantityHistory_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockQuantityHistory_WareHouse_WareHouseId",
                        column: x => x.WareHouseId,
                        principalTable: "WareHouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductSpecificationAttribute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SpecificationAttributeOptionId = table.Column<int>(type: "int", nullable: false),
                    IsShowOnProductPage = table.Column<bool>(type: "bit", nullable: false),
                    IsAllowFiltering = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpecificationAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSpecificationAttribute_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSpecificationAttribute_SpecificationAttributeOption_SpecificationAttributeOptionId",
                        column: x => x.SpecificationAttributeOptionId,
                        principalTable: "SpecificationAttributeOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantAttributeValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductVariantAttributeId = table.Column<int>(type: "int", nullable: false),
                    ProductVariantAttrbiuteId = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quality = table.Column<int>(type: "int", nullable: false),
                    IsPreSelected = table.Column<bool>(type: "bit", nullable: false),
                    PriceAdjustment = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceAdjustmentUsePercentage = table.Column<bool>(type: "bit", nullable: false),
                    WeightAdjustment = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    HeightAdjustment = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    WidthAdjustment = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    PictureId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    VariantTypeId = table.Column<int>(type: "int", nullable: false),
                    VariantType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantAttributeValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantAttributeValue_Picture_PictureId",
                        column: x => x.PictureId,
                        principalTable: "Picture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductVariantAttributeValue_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductVariantAttributeValue_ProductVariantAttrbiute_ProductVariantAttrbiuteId",
                        column: x => x.ProductVariantAttrbiuteId,
                        principalTable: "ProductVariantAttrbiute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Language",
                columns: new[] { "Id", "Code", "Culture", "CurrencyId", "DisplayOrder", "IsActive", "IsRtl", "Name" },
                values: new object[] { 1, "en", "en-US", null, 0, true, false, "English" });

            migrationBuilder.InsertData(
                table: "Language",
                columns: new[] { "Id", "Code", "Culture", "CurrencyId", "DisplayOrder", "IsActive", "IsRtl", "Name" },
                values: new object[] { 2, "vi", "vi-VN", null, 0, true, false, "VietNamese" });

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentCategoryId",
                table: "Category",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_PictureId",
                table: "Category",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_Language_CurrencyId",
                table: "Language",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturer_PictureId",
                table: "Manufacturer",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ManufacturerId",
                table: "Product",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_CategoryId",
                table: "ProductCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_ProductId",
                table: "ProductCategory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPicture_PictureId",
                table: "ProductPicture",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPicture_ProductId",
                table: "ProductPicture",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecificationAttribute_ProductId",
                table: "ProductSpecificationAttribute",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecificationAttribute_SpecificationAttributeOptionId",
                table: "ProductSpecificationAttribute",
                column: "SpecificationAttributeOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttrbiute_ProductAttributeId",
                table: "ProductVariantAttrbiute",
                column: "ProductAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttrbiute_ProductId",
                table: "ProductVariantAttrbiute",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttributeValue_PictureId",
                table: "ProductVariantAttributeValue",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttributeValue_ProductId",
                table: "ProductVariantAttributeValue",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttributeValue_ProductVariantAttrbiuteId",
                table: "ProductVariantAttributeValue",
                column: "ProductVariantAttrbiuteId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedProduct_Product1Id",
                table: "RelatedProduct",
                column: "Product1Id");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedProduct_Product2Id",
                table: "RelatedProduct",
                column: "Product2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Slug_LanguageId",
                table: "Slug",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificationAttribute_SpecificationAttributeGroupId",
                table: "SpecificationAttribute",
                column: "SpecificationAttributeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificationAttributeOption_PictureId",
                table: "SpecificationAttributeOption",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificationAttributeOption_SpecificationAttributeId",
                table: "SpecificationAttributeOption",
                column: "SpecificationAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_StockQuantityHistory_ProductId",
                table: "StockQuantityHistory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockQuantityHistory_UserId",
                table: "StockQuantityHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StockQuantityHistory_WareHouseId",
                table: "StockQuantityHistory",
                column: "WareHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TranslationEntity_LanguageId",
                table: "TranslationEntity",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_TranslationResource_LanguageId",
                table: "TranslationResource",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_User_LanguageId",
                table: "User",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFied_UserId",
                table: "UserFied",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPassword_UserId",
                table: "UserPassword",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "ProductPicture");

            migrationBuilder.DropTable(
                name: "ProductSpecificationAttribute");

            migrationBuilder.DropTable(
                name: "ProductVariantAttributeValue");

            migrationBuilder.DropTable(
                name: "RelatedProduct");

            migrationBuilder.DropTable(
                name: "Setting");

            migrationBuilder.DropTable(
                name: "Slug");

            migrationBuilder.DropTable(
                name: "StockQuantityHistory");

            migrationBuilder.DropTable(
                name: "TranslationEntity");

            migrationBuilder.DropTable(
                name: "TranslationResource");

            migrationBuilder.DropTable(
                name: "UserFied");

            migrationBuilder.DropTable(
                name: "UserPassword");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "SpecificationAttributeOption");

            migrationBuilder.DropTable(
                name: "ProductVariantAttrbiute");

            migrationBuilder.DropTable(
                name: "WareHouse");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "SpecificationAttribute");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "ProductAttribute");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "SpecificationAttributeGroup");

            migrationBuilder.DropTable(
                name: "Manufacturer");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "Picture");
        }
    }
}
