using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corex.Reflection;
using System.Reflection;
using System.Collections;

namespace Corex.IO.Tools
{
    public class ToolArgsSerializer<T> where T : class, new()
    {
        public ToolArgsSerializer()
        {
            ArgsInfo = new ToolArgsInfo<T>();
        }
        public ToolArgsSerializer(ToolArgsInfo<T> info)
        {
            ArgsInfo = info;
        }
        ToolArgsInfo<T> ArgsInfo;
        void SetValue(ToolArgNode node, InstanceProperty pe)
        {
            if (node.Value != null)
            {
                if (pe.Property.PropertyType.Implements<IList>())
                    ((IList)pe.Value).Add(node.Value);
                else
                    pe.Value = node.Value;
            }
            else
            {
                pe.Value = true;
            }
        }
        public T Read(string args)
        {
            var args2 = new ToolArgsTokenizer().Tokenize(args);
            return Read(args2);
        }
        public T Read(string[] args)
        {
            var obj = new T();
            ReadInto(obj, args);
            return obj;
        }
        public void ReadInto(T obj, string[] args)
        {
            ReadInto(obj, new ToolArgsParser().Parse(args));
        }
        public void ReadInto(T obj, IEnumerable<ToolArgNode> nodes)
        {
            var props = obj.InstanceProperties().ToList();
            foreach (var node in nodes)
            {
                if (node.Switch != null)
                {
                    var pe = ArgsInfo.GetPropBySwitchName(node.Name);
                    SetValue(node, pe.ForInstance(obj));
                }
                else
                {
                    var pe = ArgsInfo.CommandProperty;
                    SetValue(node, pe.ForInstance(obj));
                }
            }
        }


        public string Write(T target)
        {
            var sb = new StringBuilder();
            var props = target.InstanceProperties().ToList();
            if (ArgsInfo.CommandProperty != null)
            {
                var value = ArgsInfo.CommandProperty.GetValue(target, null);
                sb.AppendFormat("\"{0}\" ", value);
            }
            foreach (var pe in ArgsInfo.SwitchProperties.ForInstance(target))
            {
                var value = pe.Value;
                if (value != null)
                {
                    var name = ArgsInfo.GetSwitchNames(pe.Property).First();
                    sb.Append("/");
                    sb.Append(name);
                    sb.Append("=");
                    sb.Append(value);
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }

    }
}
