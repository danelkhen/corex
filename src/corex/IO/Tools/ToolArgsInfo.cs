using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Corex.IO.Tools
{
    public class ToolArgsInfo<T> where T : class, new()
    {
        public T Parse(string[] args)
        {
            return new ToolArgsSerializer<T>(this).Read(args);
        }
        public string Generate(T obj)
        {
            return new ToolArgsSerializer<T>(this).Write(obj);
        }
        List<PropertyInfo> _SwitchProperties;
        public List<PropertyInfo> SwitchProperties
        {
            get
            {
                if (_SwitchProperties == null)
                {
                    _SwitchProperties = new List<PropertyInfo>();
                    foreach (var pe in typeof(T).GetProperties())
                    {
                        if (pe == CommandProperty)
                            continue;
                        _SwitchProperties.Add(pe);
                    }
                }
                return _SwitchProperties;
            }
        }
        public PropertyInfo GetPropBySwitchName(string sw)
        {
            foreach (var pe in SwitchProperties)
            {
                var names = GetSwitchNames(pe);
                if (names.ContainsIgnoreCase(sw))
                    return pe;
            }
            HandleError("Property not found for switch: " + sw);
            return null;
        }

        public Action<string> Error { get; set; }
        void HandleError(string err)
        {
            if (Error != null)
                Error(err);
            else
                throw new Exception(err);
        }
        public string[] GetSwitchNames(PropertyInfo pi)
        {
            var att = pi.CustomAttribute<ToolArgSwitchAttribute>();
            if (att != null && att.Names.IsNotNullOrEmpty())
                return att.Names;
            return new string[] { pi.Name };
        }
        public ToolArgsInfo()
        {
            _CommandProperty = new Lazy<PropertyInfo>(GetCommandProperty);
        }
        Lazy<PropertyInfo> _CommandProperty;
        PropertyInfo GetCommandProperty()
        {
            return typeof(T).GetProperties().Where(t => t.CustomAttribute<ToolArgCommandAttribute>() != null).FirstOrDefault();
        }
        public PropertyInfo CommandProperty
        {
            get
            {
                return _CommandProperty.Value;
            }
        }


        ToolArgsSerializer<T> _Serializer;
        public ToolArgsSerializer<T> Serializer
        {
            get
            {
                if (_Serializer == null)
                    _Serializer = new ToolArgsSerializer<T>(this);
                return _Serializer;
            }
        }
        ToolArgsHelpGenerator<T> _HelpGenerator;
        public ToolArgsHelpGenerator<T> HelpGenerator
        {
            get
            {
                if (_HelpGenerator == null)
                    _HelpGenerator = new ToolArgsHelpGenerator<T>(this);
                return _HelpGenerator;
            }
        }
        public string GenerateHelp()
        {
            return new ToolArgsHelpGenerator<T>(this).Generate();
        }
    }
}
