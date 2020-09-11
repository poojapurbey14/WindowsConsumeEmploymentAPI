using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsConsumeEmploymentAPI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }        

        private void button1_Click(object sender, EventArgs e)
        {
            var employee = new Employee();
            employee.FirstName = textBox1.Text;
            employee.LastName = textBox2.Text;
            employee.Gender = textBox5.Text;
            employee.Age = Convert.ToInt32(textBox3.Text);
            employee.Race = textBox4.Text;
            employee.DateOfBirth = dateTimePicker1.Value;
            CreateEmployee(Form1.GetHttpClient(), employee);

        }

        public static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8080/");
            client.Timeout = new TimeSpan(0, 2, 0);
            var defaultRequestHeaders = client.DefaultRequestHeaders;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private void CreateEmployee(HttpClient client, Employee emp)
        {
            HttpResponseMessage response = HttpClientExtensions.SendAsJsonAsync<Employee>(client, HttpMethod.Post, "emp/v1/emp/details/new", emp).Result;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 fm = new Form2();
            fm.Show();
            this.Visible = false;
        }
    }

    public static class HttpClientExtensions
    {
        /// <summary>
        /// Sends an HTTP message containing a JSON payload to the target URL.  
        /// </summary>
        /// <typeparam name="T">The type of the data to send in the message content (payload).</typeparam>
        /// <param name="client">A preconfigured HTTP client.</param>
        /// <param name="method">The HTTP method to invoke.</param>
        /// <param name="requestUri">The relative URL of the message request.</param>
        /// <param name="value">The data to send in the payload. The data will be converted to a serialized JSON payload.</param>
        /// <returns>An HTTP response message.</returns>
        public static Task<HttpResponseMessage> SendAsJsonAsync<T>(this HttpClient client, HttpMethod method, string requestUri, T value)
        {
            string content = String.Empty;
            if (value.GetType().Name.Equals("JObject"))
                content = value.ToString();
            else
                content = JsonConvert.SerializeObject(value, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore });

            HttpRequestMessage request = new HttpRequestMessage(method, requestUri);
            request.Content = new StringContent(content);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var response = client.SendAsync(request);
            return response;
        }
    }
}
