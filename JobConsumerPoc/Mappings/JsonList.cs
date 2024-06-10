using Newtonsoft.Json;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System.Data.Common;

namespace JobConsumerPoc.Mappings
{
    public class JsonList<T> : IUserType
    {
        public SqlType[] SqlTypes => [new StringSqlType()];

        public Type ReturnedType => typeof(List<T>);

        public bool IsMutable => true;

        public object Assemble(object cached, object owner)
        {
            return DeepCopy(cached);
        }

        public object DeepCopy(object value)
        {
            if (value == null) return null;

            var result = new List<T>();
            var incoming = (List<T>)value;
            foreach (var item in incoming)
            {
                result.Add(item);
            }
            return result;
        }

        public object Disassemble(object value)
        {
            return DeepCopy(value);
        }

        public new bool Equals(object x, object y)
        {
            return JsonConvert.SerializeObject(x).Equals(JsonConvert.SerializeObject(y));
        }

        public int GetHashCode(object x)
        {
            return JsonConvert.SerializeObject(x).GetHashCode();
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            object r = rs[names[0]];
            if (r == DBNull.Value)
            {
                return null;
            }

            var json = (string)r;
            var result = JsonConvert.DeserializeObject<List<T>>(json);
            return result;
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            object paramValue = DBNull.Value;
            if (value != null)
            {
                paramValue = JsonConvert.SerializeObject(value);
            }
            var param = cmd.Parameters[index];
            param.Value = paramValue;
        }

        public object Replace(object original, object target, object owner)
        {
            return DeepCopy(target);
        }
    }
}
