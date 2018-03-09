using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace providerReplace
{
    class Program
    {
        static void Main(string[] args)
        {
            string fullTemplate = File.OpenText(@"E:\Git\SmartCityExplorer\sources\code\smart_city_explorer_sdk_test\sce_online_services_template.xml").ReadToEnd();

            string onlineServicesFilePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Siradel\SmartCityExplorer\Config\sce_online_services.xml";

            var providerArray = new[] {
            new { name = "bing-pro", driver = "BingFree", apikey = "" },
            new { name = "google-pro", driver = "GoogleMapsPro", apikey = "" },
            new { name ="bing", driver= "BingFree" , apikey = "Alv3zD1MiqUOcPUdhPNFtXcOYC27ZU8hsXGCoHEnkYsew11V0PWrBqUUrAn2McLL"},
            new { name = "google", driver = "GoogleMapsFree", apikey = "AIzaSyC6SvRyJHOSmHyXLiKxRAs4x_KjviLViqM" }};

            //The first Provider will be the current goecoding provider
            fullTemplate = fullTemplate.Replace("##CURRENT_PROVIDER##", providerArray[0].name);
            fullTemplate = fullTemplate.Replace("##PROVIDERS##", string.Join(",", providerArray.Select(p => p.name)));

            //retrieve block between
            string providerTemplate = fullTemplate.Substring(fullTemplate.IndexOf("##PROVIDER_NODE##") + "##PROVIDER_NODE##".Length, fullTemplate.LastIndexOf("##PROVIDER_NODE##") - fullTemplate.IndexOf("##PROVIDER_NODE##") - "##PROVIDER_NODE##".Length);

            //remove the block from the template
            fullTemplate = fullTemplate.Replace(providerTemplate, "");


            List<string> providerNodes = new List<string>();
            foreach (var item in providerArray)
            {
                string xmlNode = providerTemplate;
                xmlNode = xmlNode.Replace("##PROVIDER_NAME##", item.name);
                xmlNode = xmlNode.Replace("##PROVIDER_DRIVER##", item.driver);
                if (string.IsNullOrEmpty(item.apikey))
                {
                    //Remove the line
                    xmlNode = xmlNode.Replace("<ConfigEntry name=\"ApiKey\">##API_KEY##</ConfigEntry>", "");
                }
                else
                {
                    xmlNode = xmlNode.Replace("##API_KEY##", item.apikey);
                }
                providerNodes.Add(xmlNode);
            }
            fullTemplate = fullTemplate.Replace("##PROVIDER_NODE####PROVIDER_NODE##", string.Join("\r\n", providerNodes.ToArray()));

            File.WriteAllText(onlineServicesFilePath, fullTemplate);
        }
    }
}
