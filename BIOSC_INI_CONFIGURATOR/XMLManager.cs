using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BIOSC_INI_CONFIGURATOR
{
    class XMLManager
    {
        private XDocument Parameters;
        private String m_ConfigurationFile;
        public event EventHandler eConfigSaved;

        public void Open(String p_ConfigurationFile)
        {
            m_ConfigurationFile = p_ConfigurationFile;
            Parameters = new XDocument(XDocument.Load(p_ConfigurationFile));
            //m_ConfigSaved = false;
        }
        public IEnumerable<XElement> Get_MainNode_Nodes(string MainNodeName)
        {
            return Parameters.Root.Elements(MainNodeName).Elements();
            //Parameters.Root.Elements(NodeName).Where(x => x.Attribute(Attribute_Name).Value == AttributeToSearch).FirstOrDefault().ToString()     
            //return Parameters.Root.Elements("Method").Where(j => j.Attribute(Attribute_Name).Value == AttributeToNodeSearch).Elements().Where(x => x.Attribute(Attribute_Name).Value == AttributeToSubNodeSearch).ToString();
        }
        public void Remove_Node(String NodeName)
        {
            Parameters.Element(NodeName).Remove();
        }
        public void Add_Node(String NodeName, String Attribute_Name="name", String Attribute_Value="",Boolean IsRoot=false)
        { 
            var Node = new XElement(NodeName, new XAttribute(Attribute_Name, Attribute_Value));
            if(!IsRoot){ Parameters.Root.Add(Node); }
            else { Parameters.Add(Node); } 
        }
        public string Get_Node_Value(String NodeName, String AttributeToNodeSearch, String AttributeToSubNodeSearch,String Attribute_Name = "name")
        {
            //Parameters.Root.Elements(NodeName).Where(x => x.Attribute(Attribute_Name).Value == AttributeToSearch).FirstOrDefault().ToString()     
            return Parameters.Root.Elements("Method").Where(j => j.Attribute(Attribute_Name).Value == AttributeToNodeSearch).Elements().Where(x => x.Attribute(Attribute_Name).Value == AttributeToSubNodeSearch).ToString() ;
        }
        public void Add_SubNode(String NodeName, String AttributeToSearch, String SubNodeName, String Attribute_Name = "name", String Attribute_Value = "", String Value = "")
        {
            //Parameters.Root.Descendants(NodeName).Where(x=>x.Attribute())
            var NodeSelected = Parameters.Root.Elements(NodeName).Where(x=> x.Attribute(Attribute_Name).Value == AttributeToSearch);
            var NodeChild = new XElement(SubNodeName, new XAttribute(Attribute_Name, Attribute_Value),Value);
            foreach(var c in NodeSelected)
            {
                c.Add(NodeChild);
            }
            
        }
        public void Save()
        {
            Parameters.Save(m_ConfigurationFile);
            //m_ConfigSaved = true;
        }

        //Properties
        public Boolean m_ConfigSaved
        {
            get { return m_ConfigSaved; }
            set { eConfigSaved(this, EventArgs.Empty); }
        }
    }
}
