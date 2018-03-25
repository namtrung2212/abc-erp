using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using ABCBusinessEntities;

namespace ABCProvider
{
    public class CompanyUnitProvider
    {

        public static GECompanyUnitsInfo GetCompanyUnit ( BusinessObject realCompanyUnit )
        {
            if ( realCompanyUnit==null )
                return null;

            String strQuery=String.Format( "SELECT * FROM GECompanyUnits WHERE ABCStatus ='Alive'  AND No='{0}' AND FK_GECompanyUnitTypeID IN (SELECT GECompanyUnitTypeID FROM GECompanyUnitTypes  WHERE ABCStatus ='Alive' AND TableName='{1}')" , realCompanyUnit.GetNoValue() , realCompanyUnit.AATableName );
            return new GECompanyUnitsController().GetObject( strQuery ) as GECompanyUnitsInfo;
        }
        public static GECompanyUnitsInfo GetCompanyUnit ( String strRealTableName , Guid realCompanyUnitID )
        {
            if ( DataStructureProvider.IsExistedTable( strRealTableName ) )
            {
                BusinessObject obj=BusinessControllerFactory.GetBusinessController( strRealTableName ).GetObjectByID( realCompanyUnitID );
                return GetCompanyUnit( obj );
            }
            return null;
        }
        public static Guid GetCompanyUnitID ( String strRealTableName , Guid realCompanyUnitID )
        {
            GECompanyUnitsInfo comUnit=GetCompanyUnit( strRealTableName , realCompanyUnitID );
            if ( comUnit!=null )
                return comUnit.GECompanyUnitID;
            return Guid.Empty;
        }
        public static List<GECompanyUnitsInfo> GetCompanyUnitsByUnitType ( Guid companyUnitTypeID )
        {
            return new GECompanyUnitsController().GetListByForeignKey( "FK_GECompanyUnitTypeID" , companyUnitTypeID ).Cast<GECompanyUnitsInfo>().ToList();
        }
        public static List<GECompanyUnitsInfo> GetInventoryCompanyUnits ( )
        {
            return new GECompanyUnitsController().GetListAllObjects().Cast<GECompanyUnitsInfo>().ToList();
        }

        public static BusinessObject GetRealCompanyUnit ( Guid companyUnitID )
        {
            GECompanyUnitsInfo comUnit=new GECompanyUnitsController().GetObjectByID( companyUnitID ) as GECompanyUnitsInfo;
            if ( comUnit==null )
                return null;

            if ( !comUnit.FK_GECompanyUnitTypeID.HasValue )
                return null;

            GECompanyUnitTypesInfo comUnitType=new GECompanyUnitTypesController().GetObjectByID( comUnit.FK_GECompanyUnitTypeID.Value ) as GECompanyUnitTypesInfo;
            if ( comUnitType==null )
                return null;

            BusinessObjectController ctrl=BusinessControllerFactory.GetBusinessController( comUnitType.TableName );
            if ( ctrl!=null )
                return ctrl.GetObjectByNo( comUnit.No );

            return null;
        }
        public static Guid GetRealCompanyUnitID ( Guid companyUnitID )
        {
            BusinessObject obj=GetRealCompanyUnit( companyUnitID );
            if ( obj==null )
                return Guid.Empty;

            return obj.GetID();
        }


        public static bool IsInventory ( Guid companyUnitID )
        {
            GECompanyUnitsInfo comUnit=new GECompanyUnitsController().GetObjectByID( companyUnitID ) as GECompanyUnitsInfo;
            if ( comUnit==null )
                return false;

            return comUnit.IsInventory;
        }
        public static bool IsInventory ( String strRealTableName , Guid realID )
        {
            GECompanyUnitsInfo comUnit=GetCompanyUnit( strRealTableName , realID );
            if ( comUnit==null )
                return false;

            return comUnit.IsInventory;
        }
        public static bool IsManageValuation ( Guid companyUnitID )
        {
            GECompanyUnitsInfo comUnit=new GECompanyUnitsController().GetObjectByID( companyUnitID ) as GECompanyUnitsInfo;
            if ( comUnit==null )
                return false;

            return comUnit.IsManageValuation;
        }
        public static bool IsManageValuation ( String strRealTableName , Guid realID )
        {
            GECompanyUnitsInfo comUnit=GetCompanyUnit( strRealTableName , realID );
            if ( comUnit==null )
                return false;

            return comUnit.IsManageValuation;
        }
    }
}
