using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corex.IO.Tools.WindowsService
{
    public class ScTool
    {

        public ToolArgsInfo<ScTool> Info { get; set; }
        public Tool Tool { get; set; }

        public string ServerName { get; set; }
        public string ServiceName { get; set; }
        public string Command { get; set; }
        public string Name { get; set; }

        //        Servername
        //Optional. Specifies the name of the server when you want to run the commands on a remote computer. The name must start with two backslash (\) characters (for example, \\myserver). To run Sc.exe on the local computer, do not supply this parameter.
        //Command
        //Specifies the sc command. Note that many of the sc commands require administrative privileges on the specified computer. Sc.exe supports the following commands:
        //Config
        //Changes the configuration of a service (persistent). 

        //Continue
        //Sends a Continue control request to a service. 

        //Control
        //Sends a control to a service. 

        //Create
        //Creates a service (adds it to the registry). 

        //Delete
        //Deletes a service (from the registry). 

        //EnumDepend
        //Enumerates service dependencies. 

        //GetDisplayName
        //Obtains the DisplayName for a service. 

        //GetKeyName
        //Obtains the ServiceKeyName for a service. 

        //Interrogate
        //Sends an Interrogate control request to a service. 

        //Pause
        //Sends a Pause control request to a service. 

        //qc
        //Queries configuration for the service. For detailed information, see the reference section, "SC QC." 

        //Query
        //Queries the status for a service, or enumerates the status for types of services. For detailed information, see the reference section, "SC QUERY." 

        //Start
        //Starts a service 

        //Stop
        //Sends a Stop request to a service.
        //Servicename
        //Specifies the name given to the Service key in the registry. Note that this is different from the display name (which is what you see with net start command and the Services tool in Control Panel. Sc.exe uses the service key name as the primary identifier for the service.
        //Optionname
        //The Optionname and Optionvalue parameters allow you to specify the names and values of optional command parameters. Note that there is no space between the Optionname and the equal sign. You can supply none, one, or more optional parameters name and value pairs.
        //Optionvalue
        //Specifies the value for the parameter named by Optionname. The range of valid values is often restricted for each Optionname. For a list of available values, request help for each command.
    }
}
