using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASPK_Password
{
    public partial class AbonentChooseForm : Form
    {
        public AbonentChooseForm()
        {
            InitializeComponent();
        }

        static public List<Abonent> read_tnsnames()
        {
            List<Abonent> abonent_list = new List<Abonent>();
            try
            {
                FileStream file = File.OpenRead("C:\\...\\product\\11.2.0\\client_1\\network\\admin\\tnsnames.ora"); // открываем файл "settings.txt" в корневой папке приложения
                byte[] array = new byte[file.Length];
                int count = file.Read(array, 0, array.Length); // считываем байты из файла в массив "array", count - колличество успешно считанных байт
                String file_text = Encoding.UTF8.GetString(array); //декодируем байты в текст
                file.Close(); // закрываем файл

                file_text = end_of_note(file_text);
                file_text = file_text + "\nEOF";
                while (!file_text.Trim().StartsWith("EOF"))
                {
                    Abonent new_abonent = new Abonent();
                    file_text = file_text.Trim();
                    if (file_text.StartsWith("#"))
                    {
                        file_text = file_text.Remove(0, file_text.IndexOf("\n") + 1);
                        continue;
                    }
                    new_abonent.name = file_text.Substring(0, file_text.IndexOf("=")).Trim();
                    new_abonent.protocol = find_value(file_text, "PROTOCOL");
                    for (int i = 0, ind = 0; i < file_text.Substring(0, file_text.IndexOf("EON")).Trim().Split(new string[] { "HOST" }, StringSplitOptions.None).Count() - 1; i++)
                    {
                        new_abonent.host.Add(find_value(file_text, "HOST", ind));
                        ind = file_text.IndexOf(new_abonent.host.Last());
                    }
                    new_abonent.port = find_value(file_text, "PORT");
                    new_abonent.service_name = find_value(file_text, "SERVICE_NAME");
                    file_text = file_text.Substring(file_text.IndexOf("EON") + 3);
                    abonent_list.Add(new_abonent);
                }
                return abonent_list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ошибка чтения tnsnames.ora", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return abonent_list;
            }
        }

        static public String find_value(String text, String key)
        {
            int point_one = text.IndexOf("=", text.IndexOf(key)) + 1;
            int point_two = text.IndexOf(")", text.IndexOf(key)) - point_one;
            return text.Substring(point_one, point_two).Trim();
        }

        static public String find_value(String text, String key, int start_index)
        {
            int point_one = text.IndexOf("=", text.IndexOf(key, start_index)) + 1;
            int point_two = text.IndexOf(")", text.IndexOf(key, start_index)) - point_one;
            return text.Substring(point_one, point_two).Trim();
        }

        static public String end_of_note(String text)
        {
            int n = 0;
            bool is_EON = false;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '(') n++;
                if (text[i] == ')') n--;
                if (n > 0) is_EON = true;
                if (n == 0 && is_EON)
                {
                    text = text.Insert(i + 1, "EON");
                    is_EON = false;
                }
            }
            return text;
        }

        public void init_dgv_tns()
        {
            List<Abonent> abonent_list = read_tnsnames();
            dgv_tns.RowCount = abonent_list.Count;
            foreach (Abonent ab in abonent_list)
            {
                String host = "";
                foreach (String ip in ab.host)
                {
                    host = host + ip + "  ";
                }
                String[] row = { ab.name, ab.protocol, host, ab.port, ab.service_name};
                dgv_tns.Rows[abonent_list.IndexOf(ab)].SetValues(row);
            }
        }

        private void btn_choose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AbonentChooseForm_Load(object sender, EventArgs e)
        {
            init_dgv_tns();
        }
    }
}
