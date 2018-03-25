using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ABCBusinessEntities;
using NCalc;

namespace ABCProvider
{
    public class FormulaProvider
    {
        public static Dictionary<String , SortedDictionary<int , GEFormulaItemsInfo>> FormulasList;

        public static bool Calculate ( BaseVoucher voucher , BusinessObject obj , bool isSave )
        {
            return Calculate( voucher , obj , String.Empty , isSave );
        }
        public static bool Calculate ( BaseVoucher voucher , BusinessObject obj , String strAferValidateFieldName , bool isSave )
        {
            if ( FormulasList==null )
                InitFormulas();

            if ( !FormulasList.ContainsKey( obj.AATableName ) )
                return false;

            bool isCalculated=false;

            Dictionary<String , double> lstVariables=new Dictionary<string , double>();
            bool isContinue=false;

        #region Calculate current
		    foreach ( GEFormulaItemsInfo formula in FormulasList[obj.AATableName].Values )
            {
                if ( String.IsNullOrWhiteSpace( formula.FormulaName ) )
                    continue;

                try
                {
                    #region isNeedCalc
                    bool isNeedCalc=isContinue;
                    if ( !isNeedCalc )
                    {
                        if ( formula.IsVariable )
                        {
                            isNeedCalc=true;
                        }
                        else
                        {
                            if ( String.IsNullOrWhiteSpace( strAferValidateFieldName ) )
                            {
                                isNeedCalc=true;
                            }
                            else
                            {
                                if ( FormulasList[obj.AATableName].Values.Count( t => t.FormulaName==strAferValidateFieldName )>0 )
                                {
                                    if ( formula.FormulaName==strAferValidateFieldName )
                                    {
                                        isContinue=true;
                                        continue;
                                    }
                                }
                                else
                                {
                                    if ( DataConfigProvider.GetFieldSortOrder( obj.AATableName , strAferValidateFieldName )<=DataConfigProvider.GetFieldSortOrder( obj.AATableName , formula.FormulaName ) )
                                    {
                                        isContinue=true;
                                        isNeedCalc=true;
                                    }
                                }
                            }
                        }
                    }

                    if ( !isNeedCalc )
                        continue;
                    #endregion

                    object objAmt=null;
                    if ( formula.IsUseQuery&&!String.IsNullOrWhiteSpace( formula.QueryString ) )
                    {
                        #region Query
                        String strQuery=formula.QueryString.Replace( "{TableName}" , obj.AATableName );
                        if ( obj.GetID()!=Guid.Empty )
                            strQuery=strQuery.Replace( "{ID}" , obj.GetID().ToString() );

                        foreach ( String strProperty in DataStructureProvider.DataTablesList[obj.AATableName].ColumnsList.Keys )
                        {
                            if ( strQuery.Contains( "{"+strProperty+"}" ) )
                            {
                                object objValue=ABCDynamicInvoker.GetValue( obj , strProperty );
                                if ( objValue==null||objValue==DBNull.Value )
                                    strQuery=strQuery.Replace( "{"+strProperty+"}" , "NULL" );
                                else
                                    strQuery=strQuery.Replace( "{"+strProperty+"}" , objValue.ToString() );

                            }
                        }

                        foreach ( String strVariableName in lstVariables.Keys )
                        {
                            if ( strQuery.Contains( "{"+strVariableName+"}" ) )
                                strQuery=strQuery.Replace( "{"+strVariableName+"}" , lstVariables[strVariableName].ToString() );
                        }
                        #endregion

                        objAmt=BusinessObjectController.GetData( strQuery );
                    }
                    else if ( formula.IsUseFormula&&!String.IsNullOrWhiteSpace( formula.Formula ) )
                    {
                        String strExpression=formula.Formula;

                        #region Formula
                        foreach ( String strProperty in DataStructureProvider.DataTablesList[obj.AATableName].ColumnsList.Keys )
                            strExpression=strExpression.Replace( "{"+strProperty+"}" , "["+strProperty+"]" );

                        foreach ( String strVariableName in lstVariables.Keys )
                            strExpression=strExpression.Replace( "{"+strVariableName+"}" , "["+strVariableName+"]" );


                        Expression e=new Expression( strExpression );
                        foreach ( String strProperty in DataStructureProvider.DataTablesList[obj.AATableName].ColumnsList.Keys )
                        {
                            if ( strExpression.Contains( "["+strProperty+"]" ) )
                            {
                                object objValue=ABCDynamicInvoker.GetValue( obj , strProperty );
                                if ( objValue==null )
                                    continue;

                                double dbValue=0;
                                Double.TryParse( objValue.ToString() , out dbValue );
                                e.Parameters[strProperty]=dbValue;
                            }
                        }

                        foreach ( String strVariableName in lstVariables.Keys )
                        {
                            if ( strExpression.Contains( "["+strVariableName+"]" ) )
                                e.Parameters[strVariableName]=lstVariables[strVariableName];
                        }

                        #endregion

                        objAmt=e.Evaluate();
                    }

                    if ( formula.IsVariable&&lstVariables.ContainsKey( formula.FormulaName )==false )
                        lstVariables.Add( formula.FormulaName , Math.Round( Convert.ToDouble( objAmt ) , 3 ) );

                    bool isCalculatedWithCurrentFormula=false;
                    if ( !formula.IsVariable&&DataStructureProvider.IsTableColumn( obj.AATableName , formula.FormulaName ) )
                    {
                        if ( objAmt!=null )
                        {
                            if ( objAmt is double )
                                objAmt=Math.Round( Convert.ToDouble( objAmt ) , 3 );

                            if ( ABCDynamicInvoker.GetValue( obj , formula.FormulaName ).ToString()!=objAmt.ToString() )
                            {
                                ABCDynamicInvoker.SetValue( obj , formula.FormulaName , objAmt );
                                isCalculatedWithCurrentFormula=true;
                            }
                        }
                    }


                    if ( voucher!=null&&formula.IsCustomByCode )
                        isCalculatedWithCurrentFormula=voucher.CustomFormulaCalc( obj , null , formula );

                    isCalculated=isCalculated|isCalculatedWithCurrentFormula;

                    if ( isCalculatedWithCurrentFormula&&!string.IsNullOrWhiteSpace( formula.FieldRelations ) )
                    {
                        foreach ( String strRelation in formula.FieldRelations.Split( ';' ).ToList() )
                        {
                            if ( FormulasList[obj.AATableName].Values.Count( t => t.FormulaName==strRelation )>0 )
                            {
                                int? iIndex=FormulasList[obj.AATableName].Values.Where( t => t.FormulaName==strRelation ).Select( t => t.CalcIndex ).First();
                                if ( !iIndex.HasValue )
                                    continue;

                                if ( FormulasList[obj.AATableName].Values.Count( t => t.CalcIndex==iIndex.Value-1 )>0 )
                                {
                                    iIndex=FormulasList[obj.AATableName].Values.Where( t => t.CalcIndex==iIndex.Value-1 ).Select( t => t.CalcIndex ).First();
                                    if ( !iIndex.HasValue )
                                        continue;

                                    Calculate( obj , strRelation , isSave );
                                }
                            }
                        }
                    }

                }
                catch ( Exception ex )
                {
                }
            }
 
	#endregion
         
            if ( isCalculated&&obj.GetID()!=null&&isSave )
            {
                BusinessControllerFactory.GetBusinessController( obj.AATableName ).UpdateObject( obj );

                if ( !CalculateQueue.ContainsKey( obj.AATableName ) )
                    CalculateQueue.Add( obj.AATableName , new List<Guid>() );

                if ( !CalculateQueue[obj.AATableName].Contains( obj.GetID() ) )
                {

                    CalculateQueue[obj.AATableName].Add( obj.GetID() );

                    if ( DataStructureProvider.DataTablesList.ContainsKey( obj.AATableName ) )
                    {
                        foreach ( String strFkCol in DataStructureProvider.DataTablesList[obj.AATableName].ForeignColumnsList.Keys )
                        {
                            Guid fkID=ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( obj , strFkCol ) );
                            if ( fkID==Guid.Empty )
                                continue;

                            BusinessObjectController FKCtrl=BusinessControllerFactory.GetBusinessController( DataStructureProvider.GetTableNameOfForeignKey( obj.AATableName , strFkCol ) );
                            if ( FKCtrl!=null )
                            {
                                BusinessObject fkObj=FKCtrl.GetObjectByID( fkID );
                                if ( fkObj!=null )
                                    Calculate( fkObj , true );
                            }

                        }
                    }

                    if ( !CalculateQueue.ContainsKey( obj.AATableName ) )
                        CalculateQueue.Add( obj.AATableName , new List<Guid>() );

                    if ( !CalculateQueue[obj.AATableName].Contains( obj.GetID() ) )
                        CalculateQueue[obj.AATableName].Remove( obj.GetID() );
                }
            }

            return isCalculated;
        }
        public static bool Calculate ( BusinessObject obj , bool isSave )
        {
            return Calculate( null , obj , String.Empty , isSave );
        }
        public static bool Calculate ( BusinessObject obj , String strAferValidateFieldName , bool isSave )
        {
            return Calculate( null , obj , strAferValidateFieldName , isSave );
        }


        public static bool CalculateMainOnly ( BaseVoucher voucher , BusinessObject obj , Dictionary<String , IEnumerable<BusinessObject>> lstObjecItems , bool isSave )
        {
            return CalculateMainOnly( voucher , obj , lstObjecItems , String.Empty , isSave );
        }
        public static bool CalculateMainOnly ( BaseVoucher voucher , BusinessObject obj , Dictionary<String , IEnumerable<BusinessObject>> lstObjecItems , String strAferValidateFieldName , bool isSave )
        {
            if ( FormulasList==null )
                InitFormulas();

            if ( !FormulasList.ContainsKey( obj.AATableName ) )
                return false;

            bool isCalculated=false;

            Dictionary<String , double> lstVariables=new Dictionary<string , double>();
            bool isContinue=false;

            foreach ( GEFormulaItemsInfo formula in FormulasList[obj.AATableName].Values )
            {
                if ( String.IsNullOrWhiteSpace( formula.FormulaName ) )
                    continue;

                #region isNeedCalc
                bool isNeedCalc=isContinue;
                if ( !isNeedCalc )
                {
                    if ( formula.IsVariable )
                    {
                        isNeedCalc=true;
                    }
                    else
                    {
                        if ( String.IsNullOrWhiteSpace( strAferValidateFieldName ) )
                        {
                            isNeedCalc=true;
                        }
                        else
                        {
                            if ( FormulasList[obj.AATableName].Values.Count( t => t.FormulaName==strAferValidateFieldName )>0 )
                            {
                                if ( formula.FormulaName==strAferValidateFieldName )
                                {
                                    isContinue=true;
                                    continue;
                                }
                            }
                            else
                            {
                                if ( DataConfigProvider.GetFieldSortOrder( obj.AATableName , strAferValidateFieldName )<=DataConfigProvider.GetFieldSortOrder( obj.AATableName , formula.FormulaName ) )
                                {
                                    isContinue=true;
                                    isNeedCalc=true;
                                }
                            }
                        }
                    }
                }

                if ( !isNeedCalc )
                    continue;
                #endregion

                object objAmt=null;
                if ( formula.IsUseQuery&&!String.IsNullOrWhiteSpace( formula.QueryString ) )
                {
                    #region Query
                    String strQuery=formula.QueryString.Replace( "{TableName}" , obj.AATableName );
                    if ( obj.GetID()!=Guid.Empty )
                        strQuery=strQuery.Replace( "{ID}" , obj.GetID().ToString() );

                    foreach ( String strProperty in DataStructureProvider.DataTablesList[obj.AATableName].ColumnsList.Keys )
                    {
                        if ( strQuery.Contains( "{"+strProperty+"}" ) )
                        {
                            object objValue=ABCDynamicInvoker.GetValue( obj , strProperty );
                            if ( objValue==null||objValue==DBNull.Value )
                                strQuery=strQuery.Replace( "{"+strProperty+"}" , "NULL" );
                            else
                                strQuery=strQuery.Replace( "{"+strProperty+"}" , objValue.ToString() );

                        }
                    }

                    foreach ( String strVariableName in lstVariables.Keys )
                    {
                        if ( strQuery.Contains( "{"+strVariableName+"}" ) )
                            strQuery=strQuery.Replace( "{"+strVariableName+"}" , lstVariables[strVariableName].ToString() );
                    }
                    #endregion

                    objAmt=BusinessObjectController.GetData( strQuery );
                }
                else if ( formula.IsUseFormula&&!String.IsNullOrWhiteSpace( formula.Formula ) )
                {
                    String strExpression=formula.Formula;

                    #region Formula
                    foreach ( String strProperty in DataStructureProvider.DataTablesList[obj.AATableName].ColumnsList.Keys )
                        strExpression=strExpression.Replace( "{"+strProperty+"}" , "["+strProperty+"]" );

                    foreach ( String strVariableName in lstVariables.Keys )
                        strExpression=strExpression.Replace( "{"+strVariableName+"}" , "["+strVariableName+"]" );


                    Expression e=new Expression( strExpression );
                    foreach ( String strProperty in DataStructureProvider.DataTablesList[obj.AATableName].ColumnsList.Keys )
                    {
                        if ( strExpression.Contains( "["+strProperty+"]" ) )
                        {
                            object objValue=ABCDynamicInvoker.GetValue( obj , strProperty );
                            if ( objValue==null )
                                continue;

                            double dbValue=0;
                            Double.TryParse( objValue.ToString() , out dbValue );
                            e.Parameters[strProperty]=dbValue;
                        }
                    }

                    foreach ( String strVariableName in lstVariables.Keys )
                    {
                        if ( strExpression.Contains( "["+strVariableName+"]" ) )
                            e.Parameters[strVariableName]=lstVariables[strVariableName];
                    }

                    #endregion

                    objAmt=e.Evaluate();
                }
                else if ( formula.IsUseSumFromChild )
                {
                    objAmt=0;
                    if ( lstObjecItems.ContainsKey( formula.SumChildTableName )&&DataStructureProvider.IsTableColumn( formula.SumChildTableName , formula.SumChildFieldName ) )
                        objAmt=lstObjecItems[formula.SumChildTableName].Sum( t => Convert.ToDouble( ABCDynamicInvoker.GetValue( (BusinessObject)t , formula.SumChildFieldName ) ) );
                }

                if ( formula.IsVariable&&lstVariables.ContainsKey( formula.FormulaName )==false )
                    lstVariables.Add( formula.FormulaName , Math.Round( Convert.ToDouble( objAmt ) , 3 ) );

                bool isCalculatedWithCurrentFormula=false;
                if ( !formula.IsVariable&&DataStructureProvider.IsTableColumn( obj.AATableName , formula.FormulaName ) )
                {
                    if ( objAmt!=null )
                    {
                        if ( objAmt is double )
                            objAmt=Math.Round( Convert.ToDouble( objAmt ) , 3 );

                        if ( ABCDynamicInvoker.GetValue( obj , formula.FormulaName ).ToString()!=objAmt.ToString() )
                        {
                            ABCDynamicInvoker.SetValue( obj , formula.FormulaName , objAmt );
                            isCalculatedWithCurrentFormula=true;
                        }
                    }

                }


                if ( voucher!=null&&formula.IsCustomByCode )
                    isCalculatedWithCurrentFormula=voucher.CustomFormulaCalc( obj , lstObjecItems , formula );

                isCalculated=isCalculated|isCalculatedWithCurrentFormula;

                if ( isCalculatedWithCurrentFormula&&!string.IsNullOrWhiteSpace( formula.FieldRelations ) )
                {
                    foreach ( String strRelation in formula.FieldRelations.Split( ';' ).ToList() )
                    {
                        if ( FormulasList[obj.AATableName].Values.Count( t => t.FormulaName==strRelation )>0 )
                        {
                            int? iIndex=FormulasList[obj.AATableName].Values.Where( t => t.FormulaName==strRelation ).Select( t => t.CalcIndex ).First();
                            if ( !iIndex.HasValue )
                                continue;

                            if ( FormulasList[obj.AATableName].Values.Count( t => t.CalcIndex==iIndex.Value-1 )>0 )
                            {
                                iIndex=FormulasList[obj.AATableName].Values.Where( t => t.CalcIndex==iIndex.Value-1 ).Select( t => t.CalcIndex ).First();
                                if ( !iIndex.HasValue )
                                    continue;

                                CalculateMainOnly( voucher , obj , lstObjecItems , strRelation , isSave );
                            }
                        }
                    }
                }
            }

            if ( isCalculated&&obj.GetID()!=null&&isSave )
            {
                BusinessControllerFactory.GetBusinessController( obj.AATableName ).UpdateObject( obj );

                if ( !CalculateQueue.ContainsKey( obj.AATableName ) )
                    CalculateQueue.Add( obj.AATableName , new List<Guid>() );

                if ( !CalculateQueue[obj.AATableName].Contains( obj.GetID() ) )
                {

                    CalculateQueue[obj.AATableName].Add( obj.GetID() );

                    if ( DataStructureProvider.DataTablesList.ContainsKey( obj.AATableName ) )
                    {
                        foreach ( String strFkCol in DataStructureProvider.DataTablesList[obj.AATableName].ForeignColumnsList.Keys )
                        {
                            Guid fkID=ABCHelper.DataConverter.ConvertToGuid( ABCDynamicInvoker.GetValue( obj , strFkCol ) );
                            if ( fkID==Guid.Empty )
                                continue;

                            BusinessObjectController FKCtrl=BusinessControllerFactory.GetBusinessController( DataStructureProvider.GetTableNameOfForeignKey( obj.AATableName , strFkCol ) );
                            if ( FKCtrl!=null )
                            {
                                BusinessObject fkObj=FKCtrl.GetObjectByID( fkID );
                                if ( fkObj!=null )
                                    Calculate( fkObj , true );
                            }

                        }
                    }

                    if ( !CalculateQueue.ContainsKey( obj.AATableName ) )
                        CalculateQueue.Add( obj.AATableName , new List<Guid>() );

                    if ( !CalculateQueue[obj.AATableName].Contains( obj.GetID() ) )
                        CalculateQueue[obj.AATableName].Remove( obj.GetID() );
                }
            }

            return isCalculated;
        }

        public static Dictionary<String , List<Guid>> CalculateQueue=new Dictionary<string , List<Guid>>();
        public static bool CalculateMainOnly ( BusinessObject obj , Dictionary<String , IEnumerable<BusinessObject>> lstObjecItems , bool isSave )
        {
            return CalculateMainOnly( null , obj , lstObjecItems , String.Empty , isSave );
        }
        public static bool CalculateMainOnly ( BusinessObject obj , Dictionary<String , IEnumerable<BusinessObject>> lstObjecItems , String strAferValidateFieldName , bool isSave )
        {
            return CalculateMainOnly( null , obj , lstObjecItems , strAferValidateFieldName , isSave );
        }

        [ABCRefreshTable( "GEFormulas" )]
        public static void InitFormulas ( )
        {
            FormulasList=new Dictionary<String , SortedDictionary<int , GEFormulaItemsInfo>>();
            foreach ( GEFormulasInfo config in new GEFormulasController().GetListAllObjects().Cast<GEFormulasInfo>().ToList() )
            {
                if ( !FormulasList.ContainsKey( config.TableName ) )
                    FormulasList.Add( config.TableName , new SortedDictionary<int , GEFormulaItemsInfo>() );

                foreach ( GEFormulaItemsInfo formula in new GEFormulaItemsController().GetListByForeignKey( "FK_GEFormulaID" , config.GetID() ) )
                {

                    int iIndex=0;
                    if ( formula.CalcIndex.HasValue )
                        iIndex=formula.CalcIndex.Value;

                    if ( !FormulasList[config.TableName].ContainsKey( iIndex ) )
                        FormulasList[config.TableName].Add( iIndex , formula );
                }
            }
        }
    }
}
