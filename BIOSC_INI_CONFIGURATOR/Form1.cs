using Microsoft.Extensions.Configuration;
using PeanutButter.INI;
using System.Xml.Linq;

namespace BIOSC_INI_CONFIGURATOR
{
    public partial class Form1 : Form
    {
        INIFile IniMng = new INIFile(@"C:\test_ini\Test.ini");
        Dictionary<string, List<string>> dict_GroupeSections = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> dict_conf = new Dictionary<string, List<string>>();
        clsSystem syst = new clsSystem();
        XMLManager xmlManager = new XMLManager();
        string folderPath;
        string CompType;
        string Condition;
        string ContentPossib;
        public Form1()
        {

            InitializeComponent();

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    folderPath = Path.GetDirectoryName(openFileDialog.FileName);
                  
                    label1.Text = "Path folder of ini file and xml file :  " +folderPath;

                }
            }

            xmlManager.Open(folderPath+ @"\Config.xml");


            //Rechercher dans config pour construire le dictionnaire
            ConfigurationBuilder configuration = new ConfigurationBuilder();
            configuration.AddIniFile(@"C:\test_ini\Conf.ini", optional: false, reloadOnChange: true);

            foreach ((string key, string value) in configuration.Build().AsEnumerable().Where(t => t.Value is not null).OrderBy(t => t.Key))
            {
                if (!dict_conf.ContainsKey(key.Split(":")[0]))
                {
                    dict_conf.Add(key.Split(":")[0], new List<string>());
                    dict_conf[key.Split(":")[0]].Add(key.Split(":")[1]);
                }
                else
                {
                    dict_conf[key.Split(":")[0]].Add(key.Split(":")[1]);
                }
            }

            //Construction des tab et leurs contenus => solution list et groupe


            foreach (String k in dict_conf.Keys) //dict_GroupeSections.Keys
            {
                TabPage tabPageGrp = new TabPage();
                tabPageGrp.Name = k;
                tabPageGrp.Text = k;
                tabPageGrp.Font = new Font("Arial", 11);

                TabControl TabCtrlSub = new TabControl();
                TabCtrlSub.Dock = DockStyle.Fill;
                TableLayoutPanel tlpTabCtrlSub = new TableLayoutPanel();
                tlpTabCtrlSub.AutoSize = true;
                tlpTabCtrlSub.Dock = System.Windows.Forms.DockStyle.Fill;

                //Construction des tab groupe general


                foreach (String SectionInList in dict_conf[k]) //dict_GroupeSections[k]
                {
                    TabPage tabPageSub = new TabPage();
                    tabPageSub.Name = SectionInList;
                    tabPageSub.Text = SectionInList;
                    tabPageSub.Font = new Font("Arial", 10);
                    tabPageSub.BackColor = Color.LightGray;

                    int rownum = 0;

                    TableLayoutPanel tableLayoutPanel1 = new TableLayoutPanel();
                    tableLayoutPanel1.AutoSize = true;
                    tableLayoutPanel1.ColumnCount = 2;
                    tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
          
                    //Construction des tab sous groupe issus des sections
                    foreach ((string key, string value) in IniMng.GetSection(SectionInList))
                    {

                        CompType = "";
                        foreach (XElement nd in xmlManager.Get_MainNode_Nodes(SectionInList))
                        {
                            CompType = "";
                            String FirstNodeName = nd.Name.ToString();
                            

                            CompType = nd.Element("ComponentType").Value;
                            Condition = nd.Element("Condition").Value;
                            ContentPossib = nd.Element("ContentPossibilities").Value;
                            if (FirstNodeName == key)
                            {
                                break;
                            }
                            else
                            {
                                CompType = "";
                            }

                        }

                        Label Keytoadd = new Label();
                        Keytoadd.Dock = System.Windows.Forms.DockStyle.Fill;
                        Keytoadd.AutoSize = true;
                        Keytoadd.Name = Keytoadd.Text = key;
                        tableLayoutPanel1.Controls.Add(Keytoadd, 0, rownum);
                        tableLayoutPanel1.Padding = new Padding(15,20,15,0);

                        if (CompType == "")//textbox
                        {
                            TextBox Valtoadd = new TextBox();
                            Valtoadd.Dock = System.Windows.Forms.DockStyle.Fill;
                            Valtoadd.AutoSize = true;
                            Valtoadd.Name = SectionInList + ":" + key;
                            Valtoadd.Text = value;
                            Valtoadd.Leave += new System.EventHandler(TxtOnLeave);
                            tableLayoutPanel1.Controls.Add(Valtoadd, 1, rownum);
                        }
                        if (CompType == "1")//combobox
                        {
                            ComboBox Valtoadd = new ComboBox();
                            Valtoadd.Dock = System.Windows.Forms.DockStyle.Fill;
                            Valtoadd.AutoSize = true;
                            Valtoadd.Name = SectionInList + ":" + key;
                            
                            foreach (string cnt in ContentPossib.Split(";"))
                            { 
                                Valtoadd.Items.Add(cnt); 
                            }
                            if(value!="") Valtoadd.Items.Add(value);

                            //Valtoadd.Leave += new System.EventHandler(TxtOnLeave);
                            tableLayoutPanel1.Controls.Add(Valtoadd, 1, rownum);
                        }
                        if (CompType == "2")
                        {
                            CheckBox Valtoadd = new CheckBox();
                            Valtoadd.Dock = System.Windows.Forms.DockStyle.Fill;
                            Valtoadd.Name = SectionInList + ":" + key;
                            bool.TryParse( value, out bool g);
                            Valtoadd.Checked = g;
                            tableLayoutPanel1.Controls.Add(Valtoadd, 1, rownum);
                            Valtoadd.CheckedChanged += new System.EventHandler(CheckOnChange);
                        }


                        tabPageSub.Controls.Add(tableLayoutPanel1);

                        rownum++;
                    }

                    TabCtrlSub.Controls.Add(tabPageSub);

                    tlpTabCtrlSub.Controls.Add(TabCtrlSub);
                }

                tabPageGrp.Controls.Add(tlpTabCtrlSub);
                tabCtrlGroup.Controls.Add(tabPageGrp);

            }


        }
        public void TxtOnLeave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            IniMng.SetValue(textBox.Name.Split(":")[0].ToString(), textBox.Name.Split(":")[1].ToString(), textBox.Text);
            IniMng.Persist();
        }
        public void CheckOnChange(object sender, EventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            IniMng.SetValue(chkbox.Name.Split(":")[0].ToString(), chkbox.Name.Split(":")[1].ToString(), chkbox.Checked.ToString());
            IniMng.Persist();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string filePath = openFileDialog.FileName;
                    label1.Text = filePath;
                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                }
            }
        }
    }
}