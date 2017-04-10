using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.XPath;
using System.Xml;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication2
{
    public struct man
    {
        public string name;
        public string soname;
        public string email;
        public man(string _name, string _soname, string _email)
        {
            name = _name;
            soname = _soname;
            email = _email;
        }
        public override string ToString()
        {
            return String.Format(name + "  " + soname);
        }
    }
    public partial class Form1 : Form
    {

        string path;
        List<man> mans = new List<man>();
        public Form1()
        {
            InitializeComponent();
            button2.Enabled = false;
            button5.Enabled = false;
            
            //listBox1.Leave += DisableButton;
        }

        private void DisableButton(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button5.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != String.Empty && richTextBox2.Text != String.Empty && richTextBox3.Text != String.Empty)
            {
                man tmp = new man(richTextBox1.Text, richTextBox2.Text, richTextBox3.Text);
                mans.Add(tmp);
                listBox1.Items.Add(tmp);
                richTextBox1.Text = String.Empty;
                richTextBox2.Text = String.Empty;
                richTextBox3.Text = String.Empty;
            }
            else MessageBox.Show("Недостаточно данных", String.Empty, MessageBoxButtons.OK);
            if (button1.Text == "Изменить")
            {
                button1.Text = "Добавить";
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button5.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                man tmp = (man)listBox1.SelectedItem;
                listBox1.Items.Remove(listBox1.SelectedItem);
                richTextBox1.Text = tmp.name;
                richTextBox2.Text = tmp.soname;
                richTextBox3.Text = tmp.email;
                button1.Text = "Изменить";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TextBox temp = new TextBox();
            temp.Location = new Point(button3.Location.X + 100, button3.Location.Y);
            this.Controls.Add(temp);
            temp.Focus();
            Button but = new Button();
            but.Location = new Point(temp.Location.X + 100, temp.Location.Y);
            this.Controls.Add(but);
            but.Text = "Подтвердить";
            but.AutoSize = false;
            but.Size = new Size(but.Size.Width + 10, but.Size.Height);
            but.Click += setpath;
            temp.TextChanged += temp_TextChanged;
        }

        void temp_TextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            path = t.Text;
        }

        private void setpath(object sender, EventArgs e)
        {
            
          
            if (path != String.Empty)
            {
               
                
                int index = path.LastIndexOf('/');
                char[] dir = new char[index+1];
                path.CopyTo(0, dir, 0, index+1);
                string dirpath = String.Empty;
                foreach (char c in dir)
                {
                    dirpath += c.ToString();
                }
                if (Directory.Exists(dirpath))
                {
                    string lastthree = String.Format(path[path.Length - 3].ToString() + path[path.Length - 2] + path[path.Length-1]);
                    if (lastthree == "txt")
                    {
                        StreamWriter sw = new StreamWriter(path, true);
                        foreach (man m in mans)
                        {
                            sw.WriteLine(m.name + " " + m.soname + " " + m.email);
                        }
                        sw.Close();
                    }
                    if (lastthree == "xml")
                    {
                        
                        XmlTextWriter xt = new XmlTextWriter(path, Encoding.Unicode);
                        xt.Formatting = Formatting.Indented;
                        xt.WriteStartDocument();
                        xt.WriteStartElement("DataBase");
                        xt.WriteStartElement("Mans");
                        foreach (man m in mans)
                        {
                            xt.WriteStartElement("man");
                            xt.WriteElementString("Name", m.name);
                            xt.WriteElementString("Soname", m.soname);
                            xt.WriteElementString("Email", m.email);
                            xt.WriteEndElement();
                        }
                        xt.WriteEndElement();
                        xt.WriteEndElement();
                        xt.WriteEndDocument();
                        xt.Close();
                    }
                }

                else
                {
                    MessageBox.Show("Папка не существует"); 
                }
               
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != String.Empty)
            {
                path=textBox1.Text;
                if (File.Exists(path))
                {
                    string lastthree = String.Format(path[path.Length - 3].ToString() + path[path.Length - 2] + path[path.Length-1]);

                    if (lastthree == "txt")
                    {
                        StreamReader sr = new StreamReader(path);
                        string tmp = sr.ReadToEnd();
                        Regex reg = new Regex(@"\w*");
                        MatchCollection mc = reg.Matches(tmp);
                        for (int j = 0; j < mc.Count; j = j + 3)
                        {
                            mans.Add(new man(mc[j].ToString(), mc[j + 1].ToString(), mc[j + 2].ToString()));
                            listBox1.Items.Add(new man(mc[j].ToString(), mc[j + 1].ToString(), mc[j + 2].ToString()));
                        }
                        sr.Close();
                    }
                    if (lastthree == "xml")
                    {
                        XPathDocument xp = new XPathDocument(path);
                        XPathNavigator xn = xp.CreateNavigator();
                        XPathNodeIterator iter = xn.Select("//man");
                        XPathNodeIterator nameIter = xn.Select("//Name");
                        XPathNodeIterator sonameIter = xn.Select("//Soname");
                        XPathNodeIterator emailIter = xn.Select("//Email");
                        while (iter.MoveNext()&&nameIter.MoveNext()&&sonameIter.MoveNext()&&emailIter.MoveNext())
                        {
                            mans.Add(new man(nameIter.Current.Value.ToString(), sonameIter.Current.Value.ToString(), emailIter.Current.Value.ToString()));
                            listBox1.Items.Add(new man(nameIter.Current.Value.ToString(), sonameIter.Current.Value.ToString(), emailIter.Current.Value.ToString()));

                        }
                    }
                }
            }
        }
    }
}
