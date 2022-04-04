using Microsoft.Extensions.Configuration;
using PeanutButter.INI;

namespace BIOSC_INI_CONFIGURATOR
{
    public partial class Form1 : Form
    {
        INIFile IniMng = new INIFile(@"C:\test_ini\Test.ini");
        Dictionary<string, List<string>> dict_GroupeSections = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> dict_conf = new Dictionary<string, List<string>>();

        public Form1()
        {

            InitializeComponent();


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

                    //Construction des tab sous groupe issus des sections
                    foreach ((string key, string value) in IniMng.GetSection(SectionInList))
                    {
                        Label Keytoadd = new Label();
                        TextBox Valtoadd = new TextBox();
                        Keytoadd.Name = Keytoadd.Text = key;

                        Valtoadd.Name = SectionInList + ":" + key;
                        Valtoadd.Text = value;
                        Valtoadd.Leave += new System.EventHandler(TxtOnLeave);

                        tableLayoutPanel1.Controls.Add(Keytoadd, 0, rownum);
                        tableLayoutPanel1.Controls.Add(Valtoadd, 1, rownum);

                        tabPageSub.Controls.Add(tableLayoutPanel1);

                        rownum++;
                    }

                    TabCtrlSub.Controls.Add(tabPageSub);

                    tlpTabCtrlSub.Controls.Add(TabCtrlSub);
                }

                tabPageGrp.Controls.Add(tlpTabCtrlSub);
                tabCtrlGroup.Controls.Add(tabPageGrp);

            }
            //IniMng.SetValue("", "", "");
            /*foreach (string key in IniManager.AllSections)
            {

            }*/

        }
        public void TxtOnLeave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            //textBox.Text = "blaaa";

            IniMng.SetValue(textBox.Name.Split(":")[0].ToString(), textBox.Name.Split(":")[1].ToString(), textBox.Text);
            IniMng.Persist();
        }


    }
}