using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corex.CodingTools.CSharp
{
    public class Attribute
    {
        public Attribute()
        {
            Parameters = new List<string>();
            NamedParamters = new Dictionary<string, string>();
        }
        public Attribute Clone()
        {
            return new Attribute { Name = Name, Parameters = new List<string>(Parameters), NamedParamters = new Dictionary<string, string>(NamedParamters) };
        }
        public string Name { get; set; }
        public List<string> Parameters { get; set; }
        public Dictionary<string, string> NamedParamters { get; set; }
    }
    public class Assembly
    {
        public Assembly()
        {
            Classes = new List<Class>();
            Usings = new List<string>();
        }
        public List<Class> Classes { get; set; }
        public string Namespace { get; set; }
        public List<string> Usings { get; set; }
    }

    public class Member
    {
        public Member()
        {
            Attributes = new List<Attribute>();
        }
        public List<Attribute> Attributes { get; set; }
        public string Summary { get; set; }
        public string Remarks { get; set; }

        public string Name { get; set; }
        public Class Type { get; set; }
        public override string ToString()
        {
            return String.Format("{0} {1} {2}", GetType().Name, Type, Name); ;
        }
        public virtual Member Clone2()
        {
            var me = Activator.CreateInstance(GetType()) as Member;
            me.Name = Name;
            me.Type = Type;
            return me;
        }
    }
    public class Class : Member
    {
        public Class()
        {
            Methods = new List<Method>();
            Properties = new List<Property>();
            GenericArguments = new List<Class>();
            GenericClasses = new List<Class>();
        }
        public Class BaseClass { get; set; }

        public Class MakeGenericClass(Class[] args)
        {
            foreach (var ce in GenericClasses)
            {
                for (var i = 0; i < args.Length; i++)
                {
                    var arg = args[i];
                    var arg2 = ce.GenericArguments[i];
                    if (arg != arg2)
                        break;
                }
                return ce;
            }
            var ce2 = new Class { Name = Name };
            ce2.GenericArguments.AddRange(args);
            GenericClasses.Add(ce2);

            return ce2;
        }
        public List<Class> GenericClasses { get; set; }
        public List<Class> GenericArguments { get; set; }
        public List<Method> Methods { get; set; }
        public List<Property> Properties { get; set; }
        public IEnumerable<Member> Members
        {
            get
            {
                return Methods.Concat<Member>(Properties);
            }
        }

        public void AddMember(Member me)
        {
            if (me is Property)
                Properties.Add((Property)me);
            else if (me is Method)
                Methods.Add((Method)me);
        }
        public void RemoveMember(Member me)
        {
            if (me is Property)
                Properties.Remove((Property)me);
            else if (me is Method)
                Methods.Remove((Method)me);
        }

        public override Member Clone2()
        {
            var x = base.Clone2() as Class;
            x.Properties.AddRange(Properties.Select(t => t.Clone()));
            x.Methods.AddRange(Methods.Select(t => t.Clone()));
            return x;
        }
    }
    public class Method : Member
    {
        public Method()
        {
            Parameters = new List<Parameter>();
        }
        public List<Parameter> Parameters { get; set; }
        public override Member Clone2()
        {
            var x = base.Clone2() as Method;
            x.IsStatic = IsStatic;
            x.Attributes.AddRange(Attributes.Select(t => t.Clone()));
            x.Parameters.AddRange(Parameters.Select(t => t.Clone()));
            x.Summary = Summary;
            x.Remarks = Remarks;
            x.ReturnsSummary = ReturnsSummary;
            return x;
        }
        public string ReturnsSummary { get; set; }

        public bool IsStatic { get; set; }

    }
    public class Parameter : Member
    {
        public bool IsOptional { get; set; }
        public override Member Clone2()
        {
            var x = base.Clone2() as Parameter;
            x.IsOptional = IsOptional;
            return x;
        }
    }
    public class Property : Member
    {
        public bool IsStatic { get; set; }
    }



}
