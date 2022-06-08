using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using OrderReturn.Consts;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace OrderReturn.EntityFrameworkCore
{
    public static class OrderReturnDbContextModelCreatingExtensions
    {
        public static void ConfigureOrderReturn(
            [NotNull] this ModelBuilder builder,
            [CanBeNull] Action<OrderReturnModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new OrderReturnModelBuilderConfigurationOptions(
                OrderReturnDbProperties.DbTablePrefix,
                OrderReturnDbProperties.DbSchema);
   
            optionsAction?.Invoke(options);

            builder.Entity<OrderReturnHistory>(b =>
            {
                b.ToTable(OrderReturnDbProperties.DbTablePrefix + "OrderReturnHistories", OrderReturnDbProperties.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.OrderNumber).IsRequired(false).HasMaxLength(OrderReturnConsts.MaxOrderNumberLength);
                b.Property(x => x.ParamJson).IsRequired(false).HasMaxLength(OrderReturnConsts.MaxParamJsonLength);
                b.Property(x => x.FileName).IsRequired(false).HasMaxLength(OrderReturnConsts.MaxFileNameLength);
                b.Property(x => x.TrackingId).HasMaxLength(OrderReturnConsts.MaxTrackingIdLength);
                b.Property(x => x.OSId).HasMaxLength(OrderReturnConsts.MaxOSIdLength);
                b.Property(x => x.ErrorMessage).HasColumnType("text");


                //b.Property(x => x.SenderName).HasMaxLength(OrderReturnConsts.MaxSenderNameLength);
                //b.Property(x => x.Email).HasMaxLength(OrderReturnConsts.MaxEmailLength);
                //b.Property(x => x.Phone).HasMaxLength(OrderReturnConsts.MaxPhoneLength);
                //b.Property(x => x.ReturnReason).HasMaxLength(OrderReturnConsts.MaxReturnReasonLength);

                //b.OwnsOne(x => x.Address, a =>
                //{
                //    a.Property(x => x.Country).HasMaxLength(AddressConsts.MaxCountryLength);
                //    a.Property(x => x.CountryCode).HasMaxLength(AddressConsts.MaxCountryCodeLength);
                //    a.Property(x => x.City).HasMaxLength(AddressConsts.MaxCityLength);
                //    a.Property(x => x.Street).HasMaxLength(AddressConsts.MaxStreetLength);
                //    a.Property(x => x.HouseNumber).HasMaxLength(AddressConsts.MaxHouseNumberLength);
                //    a.Property(x => x.PostCode).HasMaxLength(AddressConsts.MaxPostCodeLength);
                //});
            });

            builder.Entity<DHLConfig>(b =>
            {
                b.ToTable(OrderReturnDbProperties.DbTablePrefix + "DHLConfigs", OrderReturnDbProperties.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.Id).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
                b.Property(x => x.Alias).IsRequired(false).HasMaxLength(DHLConfigConsts.MaxAliasLength);
                b.Property(x => x.UserName).IsRequired(false).HasMaxLength(DHLConfigConsts.MaxUserNameLength);
                b.Property(x => x.Password).IsRequired(false).HasMaxLength(DHLConfigConsts.MaxPasswordLength);
            });
        }
    }
}
