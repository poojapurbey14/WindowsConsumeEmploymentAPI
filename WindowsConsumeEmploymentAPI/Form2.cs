using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsConsumeEmploymentAPI
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            HttpClient client = Form1.GetHttpClient();
            var response = client.GetAsync(client.BaseAddress + "emp/v1/emp/details" + "?empId=" + textBox1.Text).Result;
            var entity = JsonConvert.DeserializeObject<Employee>(response.Content.ReadAsStringAsync().Result);
            StringBuilder sb = new StringBuilder();
            sb.Append(entity.FirstName);
            sb.Append(Environment.NewLine);
            sb.Append(entity.LastName);
            textBox2.Text = sb.ToString();
            
        }
    }
}
