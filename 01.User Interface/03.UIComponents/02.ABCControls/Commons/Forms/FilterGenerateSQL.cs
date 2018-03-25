using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using DevExpress.Data.Filtering;

using DevExpress.Data.Filtering.Helpers;

using DevExpress.XtraEditors.Filtering;

 
namespace DACLib
{

    public struct SQLDATA
    {
        public string Filters;
        public Dictionary<string, string> Parameters;
    }

    public class FilterControlHelper
    {

        private CriteriaOperator _criteria;
        private Node _head;
        private Dictionary<string, string> _params = null;
        private bool _useParams = false;
        private Stack<string> _groupClauseStack = null;

        public FilterControlHelper(DevExpress.XtraEditors.FilterControl filterControl)
        {
            _criteria = filterControl.FilterCriteria;
            _head = (Node)CriteriaToTreeProcessor.GetTree(new FilterControlNodesFactory(), _criteria, null);
        }

        public bool ErrorOnMissingFilters { get; set; }
      
        /// <summary>
        /// Set to true to use a null check on the Is Blank operation
        /// Set to false (default) to check both empty string and null on the Is Blank operation.
        /// </summary>
        public bool BlankAsNullOnly { get; set; }

        public string GetSQLFilter()
        {
            SQLDATA ret = GetSQLFilter(false);
            return ret.Filters;
        }

        /// <summary>
        /// Gets the SQL filter compatible with the SQL database engine.
        /// </summary>
        /// <param name="UseParameters">Replaces strings entered with parameters (Use GetParameters() to get the filled parameters)</param>
        /// <returns>The SQL filter string</returns>
        public SQLDATA GetSQLFilter(bool UseParameters)
        {
            _useParams = UseParameters;
            _params = new Dictionary<string, string>();
            _groupClauseStack = new Stack<string>();

            StringBuilder sqlQuery = new StringBuilder();
            BuildSQL(sqlQuery, _head);

            SQLDATA ret = new SQLDATA();
            ret.Filters = sqlQuery.ToString();

            // Make a copy so the internal reference isnt tainted
            if (_useParams)
                ret.Parameters = new Dictionary<string, string>(_params);
        
            return ret;
        }

        private void BuildSQL(StringBuilder SQLQuery, Node Node)
        {
            if (Node is GroupNode)
                BuildGroupSQL(SQLQuery, (GroupNode)Node);
          
            else if (Node is ClauseNode)
                BuildClauseSQL(SQLQuery, (ClauseNode)Node);
        }

        private void BuildClauseSQL(StringBuilder SQLQuery, ClauseNode ClauseNode)
        {
            // Non-group node
            var operation = ClauseNode.Operation;

            // Check for values that arent filled in.
            if (ClauseNode.AdditionalOperands.Count > 0
                && ClauseNode.AdditionalOperands[0] is DevExpress.Data.Filtering.OperandValue
                && ((DevExpress.Data.Filtering.OperandValue)ClauseNode.AdditionalOperands[0]).Value == null)
            {
                if (ErrorOnMissingFilters)
                    throw new ArgumentException("Not all filters are properly filled in. If you need to search for blanks or nulls please use the is blank operator.");
                else
                    operation = ClauseType.IsNull;
            }

            string[ addlOper = Array.ConvertAll<object, string>(ClauseNode.AdditionalOperands.ToArray(), delegate(object o) { return o.ToString(); });
            string oper = "";

            // Hook into parameter system
            if (_useParams)
                ConvertDataToParams(addlOper, operation);

            if (addlOper.Length > 0)
                oper = addlOper[0];

            string stmt = "";
            string field = ClauseNode.FirstOperand.PropertyName;

            switch (operation)
            {
                case ClauseType.AnyOf:
                    stmt = GetAnyOf(field, addlOper);
                    break;

                case ClauseType.BeginsWith:
                    stmt = GetBeginWith(field, oper);
                    break;

                case ClauseType.Between:
                    stmt = GetBetween(field, addlOper[0], addlOper[1]);
                    break;

                case ClauseType.Contains:
                    stmt = GetContain(field, oper);
                    break;

                case ClauseType.DoesNotContain:
                    stmt = GetNotContain(field, oper);
                    break;

                case ClauseType.DoesNotEqual:
                    stmt = GetNotEqual(field, oper);
                    break;

                case ClauseType.EndsWith:
                    stmt = GetEndsWith(field, oper);
                    break;

                case ClauseType.Equals:
                    stmt = GetEqual(field, oper);
                    break;

                case ClauseType.Greater:
                    stmt = GetGreaterThan(field, oper);
                    break;

                case ClauseType.GreaterOrEqual:
                    stmt = GetGreaterThanOrEqual(field, oper);
                    break;

                case ClauseType.IsNotNull:
                    stmt = GetNotBlank(field);
                    break;

                case ClauseType.IsNull:
                    stmt = GetBlank(field);
                    break;

                case ClauseType.Less:
                    stmt = GetLessThan(field, oper);
                    break;

                case ClauseType.LessOrEqual:
                    stmt = GetLessThanOrEqual(field, oper);
                    break;

                case ClauseType.Like:
                    stmt = GetLike(field, oper);
                    break;

                case ClauseType.NoneOf:
                    stmt = GetNotAnyOf(field, oper);
                    break;

                case ClauseType.NotBetween:
                    stmt = GetNotBetween(field, addlOper[0], addlOper[1]);
                    break;

                case ClauseType.NotLike:
                    stmt = GetNotLike(field, oper);
                    break;

                default:
                    throw new NotImplementedException("Unknown enum for the node's operation type.");
            }

            if (SQLQuery.Length > 0 && !SQLQuery.ToString().EndsWith(GetStartGroup()))
                SQLQuery.AppendFormat(" {0} ", _groupClauseStack.Peek());

            SQLQuery.Append(stmt);
        }

        private void BuildGroupSQL(StringBuilder SQLQuery, GroupNode GroupNode)
        {
            if (_groupClauseStack.Count > 0 && SQLQuery.Length > 0 && !SQLQuery.ToString().EndsWith(GetStartGroup()))
                SQLQuery.AppendFormat(" {0} ", _groupClauseStack.Peek());

            _groupClauseStack.Push(GetGroupOperator(GroupNode.NodeType));

            // Start the group.
            SQLQuery.Append(GetStartGroup());

            // Build sub-node information
            foreach (object subNode in GroupNode.SubNodes)
                BuildSQL(SQLQuery, (Node)subNode);

            _groupClauseStack.Pop();

            SQLQuery.Append(GetEndGroup());
        }

        /// <summary>
        /// Converts data into parameters and adds to the dictionary collection
        /// </summary>
        /// <param name="Data"></param>
        private void ConvertDataToParams(string[] Data, ClauseType operation)
        {

            for (int i = 0; i < Data.Length; i++)
            {
                string s = Data[i];
                string paramName = string.Format("@PM{0}", _params.Count + 1);

                // Strip the single quotes when adding parameters.
                if (s.StartsWith("'") || s.EndsWith("'"))
                    _params.Add(paramName, s.Substring(1, s.Length - 2));
                else
                    _params.Add(paramName, s);

                switch (operation)
                {                   
                    case ClauseType.BeginsWith:
                        _params[paramName] = string.Format("{0}%", _params[paramName]);
                        break;

                    case ClauseType.EndsWith:
                        _params[paramName] = string.Format("%{0}", _params[paramName]);
                        break;

                    case ClauseType.Contains:
                    case ClauseType.DoesNotContain:
                    case ClauseType.Like:
                    case ClauseType.NotLike:
                        _params[paramName] = string.Format("%{0}%", _params[paramName]);

                        break;
                }

                Data[i] = paramName;
            }
        }

        // Override these virtual methods if you need to adjust how the system formats SQL.
        // This is intended for use with other databases that have different syntax.
        #region Virtual Methods
        public virtual string GetStartGroup()
        {
            return "(";
        }
        public virtual string GetEndGroup()
        {
            return ")";
        }

        public virtual string GetGroupOperator(GroupType GroupOperator)

        {

            switch (GroupOperator)

            {

                case GroupType.And:

                    return "AND";

                case GroupType.NotAnd:

                    return "NOT AND";

                case GroupType.NotOr:

                    return "NOT OR";

                case GroupType.Or:

                    return "OR";

                default:

                    throw new ArgumentException("Group operator not supported");

            }

        }

 

 

        public virtual string FormatField(string Field)

        {

            // Zayeem -- To support field names qualified with respective table names

            // i.e. field names in format TableName.FieldName

            // rbarone -- Added as virtual method to allow for override

            return string.Format("[{0}]", Field.Replace(".", "].["));

        }

 

        public virtual string GetEqual(string Field, string Data)

        {

            return string.Format("{0} = {1}", FormatField(Field), Data);

        }

 

        public virtual string GetNotEqual(string Field, string Data)

        {

            return string.Format("{0} <> {1}", FormatField(Field), Data);

        }

 

        public virtual string GetGreaterThan(string Field, string Data)

        {

            return string.Format("{0} > {1}", FormatField(Field), Data);

        }

 

        public virtual string GetGreaterThanOrEqual(string Field, string Data)

        {

            return string.Format("{0} >= {1}", FormatField(Field), Data);

        }

 

        public virtual string GetLessThan(string Field, string Data)

        {

            return string.Format("{0} < {1}", FormatField(Field), Data);

        }

 

        public virtual string GetLessThanOrEqual(string Field, string Data)

        {

            return string.Format("{0} <= {1}", FormatField(Field), Data);

        }

 

        public virtual string GetBetween(string Field, string Data1, string Data2)

        {

            return string.Format("{0} BETWEEN {1} AND {2}", FormatField(Field), Data1, Data2);

        }

 

        public virtual string GetNotBetween(string Field, string Data1, string Data2)

        {

            return string.Format("{0} NOT BETWEEN {1} AND {2}", FormatField(Field), Data1, Data2);

        }

 

        public virtual string GetContain(string Field, string Data)

        {

            return GetLike(Field, Data);

        }

 

        public virtual string GetNotContain(string Field, string Data)

        {

            return GetNotLike(Field, Data);

        }

 

        public virtual string GetBeginWith(string Field, string Data)

        {

            return GetLike(Field, Data);

        }

 

        public virtual string GetEndsWith(string Field, string Data)

        {

            return GetLike(Field, Data);

        }

 

        public virtual string GetLike(string Field, string Data)

        {

            return string.Format("{0} LIKE {1}", FormatField(Field), Data);

        }

 

        public virtual string GetNotLike(string Field, string Data)

        {

            return string.Format("{0} NOT LIKE {1}", FormatField(Field), Data);

        }

 

        public virtual string GetBlank(string Field)

        {

            if (BlankAsNullOnly)

                return string.Format("{0} IS NULL", FormatField(Field));

            else

                return string.Format("({0} IS NULL OR {0} = '')", FormatField(Field));

        }

 

        public virtual string GetNotBlank(string Field)

        {

            if (BlankAsNullOnly)

                return string.Format("{0} IS NOT NULL", FormatField(Field));

            else

                return string.Format("{1}{0} IS NOT NULL AND {0} <> ''{2}", FormatField(Field), GetStartGroup(), GetEndGroup());

        }

 

        public virtual string GetAnyOf(string Field, params string[ Data)

        {

            return string.Format("{0} IN {2}{1}{3}", FormatField(Field), string.Join(", ", Data), GetStartGroup(), GetEndGroup());

        }

 

        public virtual string GetNotAnyOf(string Field, params string[ Data)

        {

            return string.Format("{0} NOT IN {2}{1}{3}", FormatField(Field), string.Join(", ", Data), GetStartGroup(), GetEndGroup());

        }

 

        #endregion

 

 

 

    }

 

 

}

 