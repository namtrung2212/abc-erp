using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using ABCBusinessEntities;

namespace ABCProvider
{
    public class CostProvider
    {
        public static void CalculateCostGroup ( Guid costGroupID )
        {
        }
        public static void CalculateCostAllocate ( Guid costAllocateRegisterID )
        {
        }
        public static void CalculateFixedAssetDepreciate ( Guid fixedAssetID )
        {
        }
        public static void CalculateEquipmentDepreciate ( Guid equipmentID )
        {
        }

        public static List<COAllocatesInfo> GetCostAllocates ( )
        {
            return new List<COAllocatesInfo>();
        }
        public static List<COFixedAssetDepreciatesInfo> GetFixedAssetDepreciates ( )
        {
            return new List<COFixedAssetDepreciatesInfo>();
        }
        public static List<COEquipmentDepreciatesInfo> GetEquipmentDepreciates ( )
        {
            return new List<COEquipmentDepreciatesInfo>();
        }

        public static bool CanViewCostGroup ( Guid userID , Guid costGroupID )
        {
            return true;
        }
        public static bool CanViewCostAccount ( Guid userID , Guid costAccountID )
        {
            return true;
        }
    }
}
