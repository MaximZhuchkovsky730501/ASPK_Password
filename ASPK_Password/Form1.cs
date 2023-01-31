using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ASPK_Password
{
    public partial class Form1 : Form
    {
        ///////////////////////// ПЕРЕМЕННЫЕ //////////////////////////
        struct Argument
        {
            internal String name, service_name, protocol, port, login, password, newPassword, ip;
            internal List<string> host;
        }

        struct Abonent_info
        {
            public String name, comment;
        }

        static public Object locker = new Object();
        static List<Abonent_info> abonent_info_list = new List<Abonent_info>();
        static internal String second_login = "null";
        static internal String second_password = "null";
        static public bool is_password_right = false;
        static public List<Abonent> abonent_list = new List<Abonent>();

        ///////////////////////// ЛОГИЧЕСКАЯ ЧАСТЬ //////////////////////////

        static public List<String> init_script_organ_id()
        {
            List<String> command = new List<String>(); //создаём список комманд
            command.Add("SELECT VALUE FROM option WHERE parameter_id = 1");

            return command;
        }

        static public List<String> init_script_server(String user, String newPassword, String organ_id)
        {
            List<String> command = new List<String>(); //создаём список комманд
            command.Add("ALTER USER " + user + " IDENTIFIED BY " + newPassword);
            //command.Add("BEGIN");
            if (user == "CUSTOM_USER" || user == "custom_user")
            {
                command.Add("BEGIN custom_user.users_utl.INSERT_USER_REMOTE(" + organ_id + ",'" + user + "','" + newPassword + "'); " +
                    "update custom_user.users_info set DATE_EXPIRED=SYSDATE + 90 where id_user=" + organ_id + "; END;");
            }
            //command.Add("update custom_user.USERS set DATE_EXPIRED=SYSDATE + 90 where id_user=" + organ_id);
            //command.Add("END");
            command.Add("commit");

            return command;
        }

        static public List<String> init_script_local(String user, String newPassword)
        {
            List<String> command = new List<String>(); //создаём список комманд
            command.Add("ALTER USER " + user + " IDENTIFIED BY " + newPassword);
            command.Add("commit");

            return command;
        }

        static public List<String> init_script_get_all_db_abonents()
        {
            List<String> command = new List<String>(); //создаём список комманд
            command.Add("select adress from custom_user.db_abonents");
            return command;
        }

        static public List<String> init_script_get_local_db_abonents(String organ_id, String abonent_id)
        {
            List<String> command = new List<String>(); //создаём список комманд
            command.Add("select adress from custom_user.db_abonents where organ_id = " + organ_id +
                " and abonent_id not in  (" + abonent_id + ") and link not in ('L')");
            return command;
        }

        static public List<String> init_script_get_abonent_id()
        {
            List<String> command = new List<String>(); //создаём список комманд
            command.Add("select abonent_id from custom_user.db_abonents where link = 'L'");
            return command;
        }

        static public List<String> init_script_type()
        {
            List<String> command = new List<String>(); //создаём список комманд
            command.Add("SELECT value FROM custom_user.option WHERE parameter_id = 14");
            return command;
        }

        static public List<String> init_script_name()
        {
            List<String> command = new List<String>(); //создаём список комманд
            command.Add("SELECT abonent_name FROM custom_user.db_abonents WHERE link = 'L'");
            return command;
        }

        static public List<String> init_custom_script(string text)//скрипт из файла
        {
            List<String> command = new List<String>(); //создаём список комманд
            int ind1, ind2;
            try
            {
                text = text.Trim();
                bool deleted = true;
                while (deleted)
                {
                    deleted = false;
                    text = text.Trim();
                    ind1 = text.IndexOf("/*");
                    if (ind1 != -1)
                    {
                        ind2 = text.IndexOf("*/", ind1) + 2;
                        text = text.Remove(ind1, ind2 - ind1);
                        deleted = true;
                    }
                }
                deleted = true;
                while (deleted)
                {
                    deleted = false;
                    if (text.IndexOf("--") != -1)
                    {
                        ind1 = text.IndexOf("--");
                        text = text.Remove(ind1, text.IndexOf("\n", ind1) + 1 - ind1);
                        deleted = true;
                    }
                }
                text = text.Trim();
                bool begin_end = false;
                foreach (string line in text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (begin_end)
                    {
                        string newline = command.Last() + " " + line + ";";
                        if (newline.EndsWith("END;") || newline.EndsWith("end;"))
                        {
                            begin_end = false;
                        }
                        command.RemoveAt(command.Count - 1);
                        command.Add(newline);
                        continue;
                    }
                    if(line.Trim().StartsWith("BEGIN") || line.Trim().StartsWith("begin"))
                    {
                        begin_end = true;
                    }
                    command.Add(line.Trim());
                }
            }
            catch (Exception ex)
            {
                command.Add("ERROR " + ex.Message);
                return command;
            }
            return command;
        }

        //static public String init_connection_string_ping(Abonent abonent, string ip)
        //{
        //    return "Data Source = (DESCRIPTION = " +
        //                                                  "(ADDRESS = (PROTOCOL = " + abonent.protocol + ")(HOST =  " + ip + ")(PORT = " + abonent.port + ")) " +
        //                                                  "(CONNECT_DATA = " +
        //                                                    "(SERVICE_NAME =  " + abonent.service_name + ") " +
        //                                                  ") " +
        //                                                  ")";
        //}

        static public String init_connection_string_ping(Abonent abonent)
        {
            return "Data Source = (DESCRIPTION = " +
                                                          "(ADDRESS = (PROTOCOL = " + abonent.protocol + ")(HOST =  " + abonent.correct_ip + ")(PORT = " + abonent.port + ")) " +
                                                          "(CONNECT_DATA = " +
                                                            "(SERVICE_NAME =  " + abonent.service_name + ") " +
                                                          ") " +
                                                          ")";
        }

        static public String init_connection_string(Abonent abonent, string login, string password)
        {
            return "Data Source = (DESCRIPTION = " +
                                                              "(ADDRESS = (PROTOCOL = " + abonent.protocol + ")(HOST =  " + abonent.correct_ip + ")(PORT = " + abonent.port + ")) " +
                                                              "(CONNECT_DATA = " +
                                                                "(SERVICE_NAME =  " + abonent.service_name + ") " +
                                                              ") " +
                                                              ");User Id = " + login + ";password=" + password;
        }

        static public List<String> run_script(String connectionString, List<String> script)
        {
            List<String> results = new List<String>();
            try
            {
                OracleConnection con = new OracleConnection();  //текущее подключение                       \
                OracleCommand cmd = new OracleCommand();        //исполняемая команда                        > переменные необходимые для подключения к БД
                con.ConnectionString = connectionString;
                cmd.Connection = con;
                con.Open(); //подключется к БД с использованием ранее заданных свойств
                foreach (String str in script) //для каждой команды из списка
                {
                    cmd.CommandText = str; //определяем команду для выполнения на сервере 
                    OracleDataReader dr = cmd.ExecuteReader(0); //выполняем команду
                    results.Add("success");
                    while (dr.Read()) //получаем результат выполнения
                    {
                        object[] res = new object[1]; //создаём новый массив ролей из 1-го элемнта, т.к. скрипт считывает только один столбец
                        dr.GetValues(res); //помещаем считанные данные в переменную
                        results.Add(res[0].ToString());
                    }
                }

                con.Close(); //закрываем соединение
                return results;
            }
            catch (OracleException ex)
            {
                results.Add(ex.Message);
                //MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return results;
            }
            catch (Exception ex)
            {
                results.Add(ex.Message);
                //MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return results;
            }
        }

        static public void get_correct_ip(Abonent abonent)
        {
            List<Thread> threads = new List<Thread>();
            foreach (string ip in abonent.host)
            {
                Abonent temp_abonent = new Abonent();
                temp_abonent.name = abonent.name;
                temp_abonent.protocol = abonent.protocol;
                temp_abonent.port = abonent.port;
                temp_abonent.service_name = abonent.service_name;
                temp_abonent.correct_ip = ip;
                threads.Add(new Thread(new ParameterizedThreadStart(thread_ping_correct_ip)));
                threads.Last().Start(temp_abonent);
                //if (ping(init_connection_string_ping(abonent, ip)) != -1)
                //{
                //    return ip;
                //}
            }
            while (threads.Count > 0)
            {
                if (threads.Count == 1)
                {
                    threads[0].Join();
                }
                if (abonent_list[abonent_list.IndexOf(abonent_list.Find(item => item.name == abonent.name))].correct_ip.Count() > 0)
                {
                    foreach (Thread thread in threads)
                    {
                        thread.Abort();
                    }
                }
                List<Thread> remove_threads = new List<Thread>();
                foreach (Thread thread in threads)
                {
                    if (!thread.IsAlive)
                    {
                        remove_threads.Add(thread);
                    }
                }
                foreach (Thread thread in remove_threads)
                {
                    threads.Remove(thread);
                }
            }
        }

        static public void thread_get_correct_ip(Object obj)
        {
            Abonent abonent = (Abonent)obj;
            get_correct_ip(abonent);
        }

        static public void thread_ping_correct_ip(Object obj)
        {
            Abonent abonent = (Abonent)obj;
            int time = -1;
            time = ping(init_connection_string_ping(abonent));
            if (time >= 0)
            {
                lock (locker)
                {
                    abonent_list[abonent_list.IndexOf(abonent_list.Find(item => item.name == abonent.name))].correct_ip = abonent.correct_ip;
                    abonent_list[abonent_list.IndexOf(abonent_list.Find(item => item.name == abonent.name))].ping_time = time;
                }
            }
        }

        public List<Abonent> get_subnet(String ip)
        {
            List<Abonent> subnet = new List<Abonent>();
            String first_part = ip + '.';
            for (int i = 1; i < 255; i++)
            {
                Abonent abonent = new Abonent();
                abonent.host.Add(first_part + i.ToString());
                abonent.protocol = cb_protocol.SelectedItem.ToString();
                abonent.port = tb_port.Text;
                abonent.service_name = tb_service_name.Text;
                abonent.name = abonent.service_name + "\t" + abonent.host.First();
                subnet.Add(abonent);
            }
            return subnet;
        }

        public void algoritm(String login, String password, String newPassword)
        {
            List<Thread> threads = new List<Thread>();
            bool is_second_user = false;
            foreach (Abonent abonent in abonent_list) // выбираем нужный ip из нескольких
            {
                threads.Add(new Thread(new ParameterizedThreadStart(thread_get_correct_ip)));
                threads.Last().Start(abonent);
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            List<Abonent> locals_abonent = new List<Abonent>(); // список локалов
            for (int i = 0; i < abonent_list.Count; i++) // получаем инфу об абоненте
            {
                Abonent abonent = abonent_list[i];
                if (abonent.correct_ip.Count() == 0) // если ip не выбран (не подключается к БД) пропускаем этот абонент
                    continue;
                if (abonent.login == null)
                {
                    abonent.login = login;
                    abonent.password = password;
                    abonent.new_password = newPassword;
                    abonent.name = get_DB_name(abonent);
                    abonent.type = get_DB_type(abonent);
                    abonent.organ_id = get_organ_id(abonent);
                    abonent.abonent_id = get_abonent_id(abonent);
                }
                if (abonent.type == "server") // получаем все локалы с сервера
                {
                    threads = new List<Thread>();
                    locals_abonent = get_DB_local_db_abonents(abonent); // получаем список локалов с табл. db_abonents, находит их в tnsnames сервера
                    abonent_list.AddRange(locals_abonent);
                    foreach (Abonent local_abonent in locals_abonent)
                    {
                        threads.Add(new Thread(new ParameterizedThreadStart(thread_get_correct_ip))); // выбираем нужный ip из нескольких
                        threads.Last().Start(local_abonent);
                    }
                    foreach (Thread thread in threads)
                    {
                        thread.Join();
                    }
                    foreach (Abonent local_abonent in locals_abonent) // получаем инфу об абонентах сервера
                    {
                        if (local_abonent.correct_ip.Count() == 0)
                            continue;
                        int ind = abonent_list.IndexOf(local_abonent);
                        abonent_list[ind].login = login;
                        abonent_list[ind].password = password;
                        abonent_list[ind].new_password = newPassword;
                        abonent_list[ind].name = get_DB_name(abonent_list[ind]);
                        abonent_list[ind].type = get_DB_type(abonent_list[ind]);
                    }
                }
            }
            foreach (Abonent abonent in abonent_list) // меняем пароль
            {
                if (abonent.correct_ip.Count() == 0)
                    continue;
                if (abonent.type == "server")
                {
                    threads.Add(new Thread(new ParameterizedThreadStart(thread_password_change_server)));
                    threads.Last().Start(abonent);
                }
                if (abonent.type == "local")
                {
                    threads.Add(new Thread(new ParameterizedThreadStart(thread_password_change_local)));
                    threads.Last().Start(abonent);
                }
            }
            foreach (Thread thread in threads) // ждём завершения потоков
            {
                while (thread.IsAlive && !is_second_user) // если меняем пароль от SYS, вводим дополнительно пароль от другово пользователя
                {
                    if (is_password_right)
                    {
                        second_user();
                        is_password_right = false;
                        is_second_user = true;
                    }
                }
                thread.Join();
            }
            second_password = "null";
            second_login = "null";
        }

        public void algoritm_local_only(String login, String password, String newPassword)
        {
            List<Thread> threads = new List<Thread>();
            bool is_second_user = false;
            foreach (Abonent abonent in abonent_list) // выбираем нужный ip из нескольких
            {
                threads.Add(new Thread(new ParameterizedThreadStart(thread_get_correct_ip)));
                threads.Last().Start(abonent);
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            threads = new List<Thread>();
            List<Abonent> locals_abonent = new List<Abonent>(); // список локалов
            foreach (Abonent abonent in abonent_list) // получаем инфу об абоненте
            {
                if (abonent.correct_ip.Count() == 0) // если ip не выбран (не подключается к БД) пропускаем этот абонент
                    continue;
                abonent.login = login;
                abonent.password = password;
                abonent.new_password = newPassword;
                abonent.name = get_DB_name(abonent);
                abonent.type = get_DB_type(abonent);
            }
            bool flag = false;
            foreach (Abonent abonent in abonent_list) // меняем пароль
            {
                if (abonent.correct_ip.Count() == 0)
                    continue;
                if (abonent.type == "local")
                {
                    flag = true;
                    threads.Add(new Thread(new ParameterizedThreadStart(thread_password_change_local)));
                    threads.Last().Start(abonent);
                }
            }
            if (!flag)
            {
                MessageBox.Show("Локалов не найдено", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (Thread thread in threads) // ждём завершения потоков
            {
                while (thread.IsAlive && !is_second_user) // если меняем пароль от SYS, вводим дополнительно пароль от другово пользователя
                {
                    if (is_password_right)
                    {
                        second_user();
                        is_password_right = false;
                        is_second_user = true;
                    }
                }
                thread.Join();
            }
            second_password = "null";
            second_login = "null";
        }

        public void algoritm_server_only(String login, String password, String newPassword)
        {
            List<Thread> threads = new List<Thread>();
            bool is_second_user = false;
            foreach (Abonent abonent in abonent_list) // выбираем нужный ip из нескольких
            {
                threads.Add(new Thread(new ParameterizedThreadStart(thread_get_correct_ip)));
                threads.Last().Start(abonent);
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            threads = new List<Thread>();
            List<Abonent> locals_abonent = new List<Abonent>(); // список локалов
            foreach (Abonent abonent in abonent_list) // получаем инфу об абоненте
            {
                if (abonent.correct_ip.Count() == 0) // если ip не выбран (не подключается к БД) пропускаем этот абонент
                    continue;
                abonent.login = login;
                abonent.password = password;
                abonent.new_password = newPassword;
                abonent.name = get_DB_name(abonent);
                abonent.type = get_DB_type(abonent);
            }
            bool flag = false;
            foreach (Abonent abonent in abonent_list) // меняем пароль
            {
                if (abonent.correct_ip.Count() == 0)
                    continue;
                if (abonent.type == "server")
                {
                    flag = true;
                    threads.Add(new Thread(new ParameterizedThreadStart(thread_password_change_server)));
                    threads.Last().Start(abonent);
                }
            }
            if (!flag)
            {
                MessageBox.Show("Серверов не найдено", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (Thread thread in threads) // ждём завершения потоков
            {
                while (thread.IsAlive && !is_second_user) // если меняем пароль от SYS, вводим дополнительно пароль от другово пользователя
                {
                    if (is_password_right)
                    {
                        second_user();
                        is_password_right = false;
                        is_second_user = true;
                    }
                }
                thread.Join();
            }
            second_password = "null";
            second_login = "null";
        }

        public void second_user()
        {
            PasswordForm passwordForm = new PasswordForm();
            passwordForm.Owner = this;
            passwordForm.ShowDialog();
            second_login = passwordForm.cb_user.Text;
            second_password = passwordForm.tb_password.Text;
        }

        public void thread_password_change_server(Object obj)
        {
            Abonent abonent = (Abonent)obj;
            List<String> script = new List<String>();
            String connectionString;
            String organ_id;
            Abonent_info abonent_info = new Abonent_info();
            abonent_info.name = abonent.name;
            connectionString = init_connection_string(abonent, abonent.login, abonent.password);
            abonent_info.comment = get_organ_id(abonent);
            if (abonent_info.comment.StartsWith("ORA-28009"))
            {
                PasswordForm passwordForm = new PasswordForm();
                passwordForm.Owner = this;
                passwordForm.ShowDialog();
                second_login = passwordForm.cb_user.Text;
                second_password = passwordForm.tb_password.Text;
                connectionString = init_connection_string(abonent, second_login, second_password);
                abonent_info.comment = run_script(connectionString, script).Last();
            }
            if (!abonent_info.comment.StartsWith("ORA"))
            {
                organ_id = abonent_info.comment;
                script = init_script_server(abonent.login, abonent.new_password, organ_id);
                abonent_info.comment = run_script(connectionString, script).Last();
            }
            abonent_info_list.Add(abonent_info);
        }

        public void thread_password_change_local(Object obj)
        {
            Abonent_info abonent_info = new Abonent_info();
            Abonent abonent = (Abonent)obj;
            String connectionString = init_connection_string_ping(abonent);
            if (abonent.correct_ip.Count() > 0)
            {
                abonent_info.name = abonent.name;
                try
                {
                    connectionString = init_connection_string(abonent, abonent.login, abonent.password);
                    abonent_info.comment = run_script(connectionString, abonent.script).Last();
                    abonent_info_list.Add(abonent_info);
                }
                catch (Exception ex)
                {
                    abonent_info.comment = ex.Message;
                    abonent_info_list.Add(abonent_info);
                    return;
                }
            }

        }

        public void thread_custom_script(Object obj)
        {
            List<String> script = new List<String>();
            Abonent_info abonent_info = new Abonent_info();
            Abonent abonent = (Abonent)obj;
            if (abonent.ping_time >= 0)
            {
                abonent_info.name = abonent.name;
                try
                {
                    script = init_script_local(abonent.login, abonent.new_password); //получаем список команд
                    string connectionString = init_connection_string(abonent, abonent.login, abonent.password);
                    abonent_info.comment = run_script(connectionString, script).Last();
                    if (abonent_info.comment.StartsWith("ORA-28009"))
                    {
                        lock (locker)
                        {
                            while (second_password == "null" || second_login == "null")
                            {
                                is_password_right = true;
                            }
                        }
                        connectionString = init_connection_string(abonent, second_login, second_password);
                        abonent_info.comment = run_script(connectionString, script).Last();
                    }
                    abonent_info_list.Add(abonent_info);
                }
                catch (Exception ex)
                {
                    abonent_info.comment = ex.Message;
                    abonent_info_list.Add(abonent_info);
                    return;
                }
            }
        }

        static public int ping(String connectionString)
        {
            Stopwatch stopwatch = new Stopwatch();
            try
            {
                OracleConnection con = new OracleConnection();  //текущее подключение                       \
                OracleCommand cmd = new OracleCommand();        //исполняемая команда                        > переменные необходимые для подключения к БД
                con.ConnectionString = connectionString;
                cmd.Connection = con;
                stopwatch.Start();
                con.Open(); //подключется к БД с использованием ранее заданных свойств
                stopwatch.Stop();
                con.Close();
                return (int)stopwatch.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                if (ex.Message.StartsWith("ORA-01017"))
                    return (int)stopwatch.ElapsedMilliseconds;
                else
                    return -1;
            }
        }

        static public void ping_subnet()
        {
            List<Thread> threads = new List<Thread>(); ;
            Argument arg = new Argument();
            foreach (Abonent abonent in abonent_list) //для каждого сервера из списка
            {
                threads.Add(new Thread(new ParameterizedThreadStart(thread_ping_subnet)));
                threads.Last().Start(abonent);
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }

        static public void thread_ping_subnet(Object obj)
        {
            List<String> script = new List<String>();
            Abonent_info abonent_info = new Abonent_info();
            Abonent abonent = (Abonent)obj;
            get_correct_ip(abonent);
            if (abonent.ping_time >= 0)
            {

                String temp_name = get_DB_name(abonent);
                if (!temp_name.StartsWith("ORA"))
                    abonent.name = temp_name;
                String temp_type = get_DB_type(abonent);
                if (!temp_type.StartsWith("ORA"))
                    abonent.type = temp_type;
                string connectionString = init_connection_string_ping(abonent);
                int sum = abonent.ping_time + ping(connectionString) + ping(connectionString) + ping(connectionString);
                abonent.ping_time = (int)(sum / 4);
                abonent_info.name = abonent.name;
                abonent_info.comment = abonent.ping_time.ToString() + " ms\t" + abonent.type;
                abonent_info_list.Add(abonent_info);
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

        static public List<Abonent> read_tnsnames(String directory)
        {
            List<Abonent> tns_abonent_list = new List<Abonent>();
            try
            {
                FileStream file = File.OpenRead(directory); // открываем файл "option.txt" в корневой папке приложения
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
                    tns_abonent_list.Add(new_abonent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ошибка чтения tnsnames.ora", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return tns_abonent_list;
            }
            return tns_abonent_list;
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

        static public String get_DB_type(Abonent abonent)
        {
            List<String> script = init_script_type();
            String result;
            String connectionString = init_connection_string(abonent, "user_login", "user_password");
            String value = run_script(connectionString, script).Last();
            if (value == "0")
                result = "local";
            else
                if (value == "1")
                result = "server";
            else
                result = "no data";
            return result;
        }

        static public String get_DB_name(Abonent abonent)
        {
            List<String> script = init_script_name();
            String connectionString = init_connection_string(abonent, "user_login", "user_password");

            return run_script(connectionString, script).Last();
        }

        static public String get_abonent_id(Abonent abonent)
        {
            List<String> script = init_script_get_abonent_id();
            String connectionString = init_connection_string(abonent, abonent.login, abonent.password);

            return run_script(connectionString, script).Last();
        }

        static public String get_organ_id(Abonent abonent)
        {
            List<String> script = init_script_organ_id();
            String connectionString = init_connection_string(abonent, abonent.login, abonent.password);

            return run_script(connectionString, script).Last();
        }

        static public List<String> get_DB_all_db_abonents(Abonent abonent)
        {
            List<String> script = init_script_get_all_db_abonents();
            String connectionString = init_connection_string(abonent, abonent.login, abonent.password);

            return run_script(connectionString, script);
        }

        static public List<Abonent> get_DB_local_db_abonents(Abonent abonent)
        {
            List<String> script = init_script_get_local_db_abonents(abonent.organ_id, abonent.abonent_id);
            String connectionString = init_connection_string(abonent, abonent.login, abonent.password);
            List<String> locals_names = run_script(connectionString, script);
            locals_names.Remove("success");
            for (int i = 0; i < locals_names.Count; i++)
            {
                locals_names[i] = locals_names[i].Substring(locals_names[i].IndexOf("@") + 1);
            }
            String directory = "\\\\" + abonent.correct_ip + "\\admin\\tnsnames.ora";
            if (!File.Exists(directory))
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = "Укажите файл tnsnames.ora для " + abonent.name;
                dialog.Filter = "ORACLE files (*.ora)|*.ora| text files (*.txt)|*.txt| ALL files(*.*)|*.*";
                dialog.ShowDialog();
                directory = dialog.FileName;
            }
            List<Abonent> tns_abonent_list = read_tnsnames(directory);
            List<Abonent> locals = new List<Abonent>();
            foreach (Abonent tns_abonent in tns_abonent_list)
            {
                for (int i = 0; i < locals_names.Count; i++)
                {
                    if (tns_abonent.name == locals_names[i])
                    {
                        locals.Add(tns_abonent);
                    }
                }
            }
            return locals;
        }

        public void run_custom_script(String login, String password)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = "Укажите файл содержащий скрипт";
                dialog.Filter = "SQL files (*.sql)|*.sql| text files (*.txt)|*.txt| ALL files(*.*)|*.*";
                dialog.ShowDialog();
                string directory = dialog.FileName;
                FileStream file = File.OpenRead(directory);
                byte[] array = new byte[file.Length];
                int count = file.Read(array, 0, array.Length);
                string file_text = Encoding.UTF8.GetString(array);
                file.Close();
                List<String> script = init_custom_script(file_text);
                string message_string = "Выполнить следующие команды?\n";
                if (script.Last().StartsWith("ERROR"))
                {
                    MessageBox.Show("не удалось расспознать комманды", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (script.Count < 5)
                {
                    foreach (String commad in script)
                    {
                        message_string = message_string + (script.IndexOf(commad) + 1).ToString() + ":\n" + commad + "\n";
                    }
                }
                else
                {
                    message_string = message_string + 1 + ":\n" + script[0] + ":\n...\n" + script.Count + ":\n" + script.Last();
                }
                DialogResult dialogResult = MessageBox.Show(message_string, "Распознанные команды", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    List<Thread> threads = new List<Thread>();
                    foreach (Abonent abonent in abonent_list) // выбираем нужный ip из нескольких
                    {
                        threads.Add(new Thread(new ParameterizedThreadStart(thread_get_correct_ip)));
                        threads.Last().Start(abonent);
                    }
                    foreach (Thread thread in threads)
                    {
                        thread.Join();
                    }
                    foreach (Abonent abonent in abonent_list)
                    {
                        if (abonent.correct_ip.Count() == 0) // если ip не выбран (не подключается к БД) пропускаем этот абонент
                            continue;
                        abonent.login = login;
                        abonent.password = password;
                        abonent.script = script;
                    }
                    foreach (Abonent abonent in abonent_list) // меняем пароль
                    {
                        if (abonent.correct_ip.Count() == 0)
                            continue;
                        threads.Add(new Thread(new ParameterizedThreadStart(thread_custom_script)));
                        threads.Last().Start(abonent);
                    }
                    foreach (Thread thread in threads)
                    {
                        thread.Join();
                    }
                }

            }
            catch
            {

            }
        }
        ///////////////////////// ВИЗУАЛЬНАЯ ЧАСТЬ //////////////////////////

        public Form1()
        {
            InitializeComponent();
            btn_open_old_password.MouseDown += new MouseEventHandler(btn_open_old_password_MouseDown);
            btn_open_old_password.MouseUp += new MouseEventHandler(btn_open_old_password_MouseUp);
            btn_open_new_password.MouseDown += new MouseEventHandler(btn_open_new_password_MouseDown);
            btn_open_new_password.MouseUp += new MouseEventHandler(btn_open_new_password_MouseUp);
            btn_open_confirm_password.MouseDown += new MouseEventHandler (btn_open_confirm_password_MouseDown);
            btn_open_confirm_password.MouseUp += new MouseEventHandler(btn_open_confirm_password_MouseUp);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cb_user.SelectedIndex = 0;
            cb_protocol.SelectedIndex = 0;
            tb_old_password.UseSystemPasswordChar = true;
            tb_new_password.UseSystemPasswordChar = true;
            tb_confirm_password.UseSystemPasswordChar = true;
            /*
            Abonent abonent = new Abonent();
            abonent.name = "ASPK-TRANSIT-01";
            abonent.port = "1521";
            abonent.protocol = "TCP";
            abonent.correct_ip = "10.34.79.3";
            abonent.service_name = "ASPK";
            abonent.login = "custom_user";
            abonent.password = "A7982161";
            abonent.organ_id = "37";
            abonent.abonent_id = get_abonent_id(abonent);
            List<String> locals = get_DB_local_db_abonents(abonent);
            locals.Remove("success");
            for (int i = 0; i < locals.Count; i++)
            {
                locals[i] = locals[i].Substring(locals[i].IndexOf("@") + 1);
            }
            */
            //string[] allfiles = Directory.GetFiles("\\\\10.34.79.3\\admin");
            //correct();
        }
        /*
        private void correct()
        {
            FileStream file = File.OpenRead("dictionary.txt"); // открываем файл "option.txt" в корневой папке приложения
            byte[] array = new byte[file.Length];
            int count = file.Read(array, 0, array.Length); // считываем байты из файла в массив "array", count - колличество успешно считанных байт
            String file_text = Encoding.UTF8.GetString(array); //декодируем байты в текст
            file.Close(); // закрываем файл
            String[] words = file_text.Split(new char[] { '\r', '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries); // разбиваем текст по строкам
            FileStream file1 = File.OpenWrite("dictionary2.txt");
            String full_text = "";
            foreach (String word in words)
            {
                full_text = full_text + word + ' ';
            }
            byte[] array2 = new byte[full_text.Length];
            array2 = Encoding.UTF8.GetBytes(full_text.ToLower());
            file1.Write(array2, 0, array2.Length);
            file1.Close();
        }
        */
        private void btn_open_old_password_MouseDown(object sender, MouseEventArgs e)
        {
            tb_old_password.UseSystemPasswordChar = false;
        }

        private void btn_open_old_password_MouseUp(object sender, MouseEventArgs e)
        {
            tb_old_password.UseSystemPasswordChar = true;
        }

        private void btn_open_new_password_MouseDown(object sender, MouseEventArgs e)
        {
            tb_new_password.UseSystemPasswordChar = false;
        }

        private void btn_open_new_password_MouseUp(object sender, MouseEventArgs e)
        {
            tb_new_password.UseSystemPasswordChar = true;
        }

        private void btn_open_confirm_password_MouseDown(object sender, MouseEventArgs e)
        {
            tb_confirm_password.UseSystemPasswordChar = false;
        }

        private void btn_open_confirm_password_MouseUp(object sender, MouseEventArgs e)
        {
            tb_confirm_password.UseSystemPasswordChar = true;
        }

        private void btp_ping_Click(object sender, EventArgs e)
        {
            abonent_info_list.Clear();
            if (rb_ip.Checked)
            {
                Abonent abonent = new Abonent();
                abonent.host.Add(tb_server.Text);
                abonent.protocol = cb_protocol.SelectedItem.ToString();
                abonent.port = tb_port.Text;
                abonent.service_name = tb_service_name.Text;
                abonent_list.Add(abonent);
            }
            if (rb_subnet.Checked)
            {
                abonent_list = get_subnet(tb_subnet.Text);
            }
            if (!rb_ip.Checked && !rb_subnet.Checked && !rb_choose.Checked)
            {
                MessageBox.Show("Выберите абонентов", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (abonent_list.Equals(new List<Abonent>()))
            {
                MessageBox.Show("Выберите абонентов", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!rb_ip.Checked && !rb_subnet.Checked && !rb_choose.Checked)
            {
                MessageBox.Show("Выберите абонентов", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ping_subnet();
            String text = "";
            foreach (Abonent_info local in abonent_info_list)
            {
                text = text + local.name + "\t" + local.comment + "\n";
            }
            if (text == "")
            {
                text = "Доступных БД не обнаружено";
            }
            MessageBox.Show(text, "Список доступных БД", MessageBoxButtons.OK, MessageBoxIcon.Information);
            abonent_info_list.Clear();
        }

        private void btn_change_Click(object sender, EventArgs e)
        {
            if (rb_ip.Checked)
            {
                abonent_list.Clear();
                Abonent abonent = new Abonent();
                abonent.host.Add(tb_server.Text);
                abonent.protocol = cb_protocol.SelectedItem.ToString();
                abonent.port = tb_port.Text;
                abonent.service_name = tb_service_name.Text;
                abonent_list.Add(abonent);
            }
            if (rb_subnet.Checked)
            {
                abonent_list = get_subnet(tb_subnet.Text);
            }
            
            if (!rb_ip.Checked && !rb_subnet.Checked && !rb_choose.Checked)
            {
                MessageBox.Show("Выберите абонентов", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (abonent_list.Equals(new List<Abonent>()))
            {
                MessageBox.Show("Выберите абонентов", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (rb_ip.Checked || rb_subnet.Checked || rb_choose.Checked)
            {
                if (tb_old_password.Text != "")
                {
                    if (tb_new_password.Text != "")
                    {
                        if (tb_new_password.Text == tb_confirm_password.Text)
                        {
                            algoritm(cb_user.Text, tb_old_password.Text, tb_new_password.Text);
                            String text = "";
                            if (abonent_info_list.Count > 0)
                            {
                                foreach (Abonent_info abonent_info in abonent_info_list)
                                {
                                    text = text + abonent_info.name + "\t" + abonent_info.comment + "\n";
                                }
                                MessageBox.Show(text, "Список доступных БД", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            abonent_info_list.Clear();
                        }
                        else
                        {
                            MessageBox.Show("пароли не совпадают", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите новый пароль", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Введите пароль", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //abonent = new Abonent();
            //abonent_list = new List<Abonent>();
            //rb_choose.Text = "Выбрать из списка (выбрано: 0)";
        }

        private void btn_create_password_Click(object sender, EventArgs e)
        {
            FileStream file = File.OpenRead("dictionary.txt"); 
            byte[] array = new byte[file.Length];
            int count = file.Read(array, 0, array.Length); // считываем байты из файла в массив "array", count - колличество успешно считанных байт
            String file_text = System.Text.Encoding.UTF8.GetString(array); //декодируем байты в текст
            file.Close(); // закрываем файл
            String[] words = file_text.Split(' ');
            Random random = new Random();
            lbl_generate_password.Text = words[random.Next(words.Length) - 1];
        }

        private void btn_change_server_Click(object sender, EventArgs e)
        {
            if (rb_ip.Checked)
            {
                abonent_list.Clear();
                Abonent abonent = new Abonent();
                abonent.host.Add(tb_server.Text);
                abonent.protocol = cb_protocol.SelectedItem.ToString();
                abonent.port = tb_port.Text;
                abonent.service_name = tb_service_name.Text;
                abonent_list.Add(abonent);
            }
            if (rb_subnet.Checked)
            {
                abonent_list = get_subnet(tb_subnet.Text);
            }

            if (!rb_ip.Checked && !rb_subnet.Checked && !rb_choose.Checked)
            {
                MessageBox.Show("Выберите абонентов", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (abonent_list.Equals(new List<Abonent>()))
            {
                MessageBox.Show("Выберите абонентов", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (rb_ip.Checked || rb_subnet.Checked || rb_choose.Checked)
            {
                if (tb_old_password.Text != "")
                {
                    if (tb_new_password.Text != "")
                    {
                        if (tb_new_password.Text == tb_confirm_password.Text)
                        {
                            algoritm_server_only(cb_user.Text, tb_old_password.Text, tb_new_password.Text);
                            String text = "";
                            if (abonent_info_list.Count > 0)
                            {
                                foreach (Abonent_info abonent_info in abonent_info_list)
                                {
                                    text = text + abonent_info.name + "\t" + abonent_info.comment + "\n";
                                }
                                MessageBox.Show(text, "Список доступных БД", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            abonent_info_list.Clear();
                        }
                        else
                        {
                            MessageBox.Show("пароли не совпадают", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите новый пароль", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Введите пароль", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_custom_script_Click(object sender, EventArgs e)
        {
            if (rb_ip.Checked)//указан ip-адрес
            {
                abonent_list.Clear();
                Abonent abonent = new Abonent();
                abonent.host.Add(tb_server.Text);
                abonent.protocol = cb_protocol.SelectedItem.ToString();
                abonent.port = tb_port.Text;
                abonent.service_name = tb_service_name.Text;
                abonent_list.Add(abonent);
            }
            if (rb_subnet.Checked)//указана подсеть
            {
                abonent_list = get_subnet(tb_subnet.Text);
            }

            if (!rb_ip.Checked && !rb_subnet.Checked && !rb_choose.Checked)//ничего не указано
            {
                MessageBox.Show("Выберите абонентов", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (abonent_list.Equals(new List<Abonent>()))//указан абонент из tns, но не подтянулся
            {
                MessageBox.Show("Выберите абонентов", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (rb_ip.Checked || rb_subnet.Checked || rb_choose.Checked)
            {
                if (tb_old_password.Text != "")
                {
                    run_custom_script(cb_user.Text.ToString(), tb_old_password.Text.ToString());
                    abonent_info_list.Clear();
                }
                else
                {
                    MessageBox.Show("Введите пароль", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void rb_ip_CheckedChanged(object sender, EventArgs e)
        {
            tb_server.Enabled = rb_ip.Checked;
            if (!rb_subnet.Checked)
            {
                tb_port.Enabled = rb_ip.Checked;
                tb_service_name.Enabled = rb_ip.Checked;
                cb_protocol.Enabled = rb_ip.Checked;
            }
        }

        private void rb_subnet_CheckedChanged(object sender, EventArgs e)
        {
            tb_subnet.Enabled = rb_subnet.Checked;
            if (!rb_ip.Checked)
            {
                tb_port.Enabled = rb_subnet.Checked;
                tb_service_name.Enabled = rb_subnet.Checked;
                cb_protocol.Enabled = rb_subnet.Checked;
            }
        }

        private void rb_choose_Click(object sender, EventArgs e)
        {
            if (rb_choose.Checked)
            {
                abonent_list.Clear();
                AbonentChooseForm abonentChooseForm = new AbonentChooseForm();
                abonentChooseForm.Owner = this;
                abonentChooseForm.ShowDialog();
                foreach (DataGridViewRow row in abonentChooseForm.dgv_tns.SelectedRows)
                {
                    Abonent abonent = new Abonent();
                    abonent.name = row.Cells[0].Value.ToString();
                    abonent.protocol = row.Cells[1].Value.ToString();
                    abonent.host = row.Cells[2].Value.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList<String>();
                    abonent.port = row.Cells[3].Value.ToString();
                    abonent.service_name = row.Cells[4].Value.ToString();
                    abonent_list.Add(abonent);
                }
                rb_choose.Text = "Выбрать из списка (выбрано: " + abonent_list.Count + ")";
            }
        }

        private void rb_local_choose_Click(object sender, EventArgs e)
        {
            if (rb_choose.Checked)
            {
                abonent_list.Clear();
                AbonentChooseForm abonentChooseForm = new AbonentChooseForm();
                abonentChooseForm.Owner = this;
                abonentChooseForm.ShowDialog();
                foreach (DataGridViewRow row in abonentChooseForm.dgv_tns.SelectedRows)
                {
                    Abonent abonent = new Abonent();
                    abonent.name = row.Cells[0].Value.ToString();
                    abonent.protocol = row.Cells[1].Value.ToString();
                    abonent.host = row.Cells[2].Value.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList<String>();
                    abonent.port = row.Cells[3].Value.ToString();
                    abonent.service_name = row.Cells[4].Value.ToString();
                    abonent_list.Add(abonent);
                }
                rb_choose.Text = "Выбрать из списка (выбрано: " + abonent_list.Count + ")";
            }
        }

        private void btn_change_local_Click(object sender, EventArgs e)
        {
            if (rb_ip.Checked)
            {
                abonent_list.Clear();
                Abonent abonent = new Abonent();
                abonent.host.Add(tb_server.Text);
                abonent.protocol = cb_protocol.SelectedItem.ToString();
                abonent.port = tb_port.Text;
                abonent.service_name = tb_service_name.Text;
                abonent_list.Add(abonent);
            }
            if (rb_subnet.Checked)
            {
                abonent_list = get_subnet(tb_subnet.Text);
            }

            if (!rb_ip.Checked && !rb_subnet.Checked && !rb_choose.Checked)
            {
                MessageBox.Show("Выберите абонентов", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (abonent_list.Equals(new List<Abonent>()))
            {
                MessageBox.Show("Выберите абонентов", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (rb_ip.Checked || rb_subnet.Checked || rb_choose.Checked)
            {
                if (tb_old_password.Text != "")
                {
                    if (tb_new_password.Text != "")
                    {
                        if (tb_new_password.Text == tb_confirm_password.Text)
                        {
                            algoritm_local_only(cb_user.Text, tb_old_password.Text, tb_new_password.Text);
                            String text = "";
                            if (abonent_info_list.Count > 0)
                            {
                                foreach (Abonent_info abonent_info in abonent_info_list)
                                {
                                    text = text + abonent_info.name + "\t" + abonent_info.comment + "\n";
                                }
                                MessageBox.Show(text, "Список доступных БД", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            abonent_info_list.Clear();
                        }
                        else
                        {
                            MessageBox.Show("пароли не совпадают", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите новый пароль", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Введите пароль", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void rb_choose_CheckedChanged(object sender, EventArgs e)
        {
            if (!rb_choose.Checked)
            {
                abonent_list.Clear();
                rb_choose.Text = "Выбрать из списка (выбрано: 0)";
            }
        }

    }

    public class Abonent
    {
        public String name, port, service_name, protocol, type, login, password, new_password, organ_id, abonent_id, correct_ip = "";
        public int ping_time = -1;
        public List<String> host = new List<String>();
        public List<String> script = new List<String>();

        //public string time_to_string()
        //{
        //    if (this.ping_time == 0)
        //    {
        //        return "not
        //    }
        //}
    }
}
