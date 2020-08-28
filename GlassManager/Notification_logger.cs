//OLD

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Android.Service.Notification;
using Android.App;
using Android.Content;
using Java.Lang.Reflect;
using System.Xml.Serialization;

namespace GlassManager
{
    public class Notification_logger
    {

        Notification_Database this_database = new Notification_Database();
        const string presistent_data_address = "/data.dat";

        public static Notification_logger Attempt_load_in(){
            Notification_logger the_Database_Wrapper = new Notification_logger();
            string presistent_data_address = AppDomain.CurrentDomain.BaseDirectory + "/data.dat";

            string enviroment_dir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            enviroment_dir += presistent_data_address;
            try
            {
                if (System.IO.File.Exists(enviroment_dir))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = System.IO.File.Open(enviroment_dir, FileMode.Open);
                    try
                    {
                        Notification_Database database = (Notification_Database)bf.Deserialize(file);
                    } catch(Exception e){
                        int a = 0;
                    }
                    file.Dispose();
                    file.Close();
                }
            } catch(Exception e){
                int a = 0;
            }
            /*
            XmlSerializer formatter = new XmlSerializer(the_Database_Wrapper.this_database.GetType());
            FileStream stream = new FileStream(enviroment_dir, FileMode.Open);
            byte[] buffer = new byte[(int)stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            MemoryStream data = new MemoryStream(buffer);
            Notification_Database new_database = (Notification_Database)formatter.Deserialize(data);
            stream.Dispose();
            the_Database_Wrapper.this_database = new_database;
            */

   
            return the_Database_Wrapper;
        }





        public Notification_logger()
        {
            this.this_database = new Notification_Database();
        }

        public void save_data()
        {
            //try{

            //string newFilePath = Path.Combine( Path.GetDirectoryName(filePath).Replace("\\", "/").Replace(sourceDirName, destDirName), Path.GetFileName(filePath));

            string enviroment_dir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            enviroment_dir += presistent_data_address;
            BinaryFormatter formatter = new BinaryFormatter(); //a  XmlSerializer
            FileStream stream = System.IO.File.Create(enviroment_dir);
            formatter.Serialize(stream, this.this_database);
            stream.Dispose();

            return;
        }

        public void save_notification(StatusBarNotification n)
        {
            //atempt to get correct colour
            this_database.Save(n);
            this.save_data();
            return;
        }

        public string get_str_of_all(){
            return this.this_database.get_detials();
        }
    }

    [Serializable]
    public class Notification_Database{

        public List<Notification_local> the_list;
        
        public Notification_Database(){
            this.the_list = new List<Notification_local>();
        }

        public void Save(StatusBarNotification notif)
        {
            Notification_local local = new Notification_local();
            if (notif.Notification.TickerText != null) {
                string deets = notif.Notification.TickerText.ToString() + "(" + notif.PackageName + ")";
                local.set_DETAILS(deets);
                this.the_list.Add(local);
            }
        }

        public String get_detials(){
            string s = " ";
            for (int i = 0; i < this.the_list.Count; i++){
                string v = this.the_list[i].get_DETAILS();
                if(v.Equals("") == false)
                    s += i + ") " + this.the_list[i].get_DETAILS() + "\n";
            }

            return s;
        }


    }

    [Serializable]
    public class Notification_local
    {

        public string NOTIFICATION_ = "";

        public Notification_local()
        {
        }
        public void set_DETAILS(string a)
        {
            this.NOTIFICATION_ = a;
        }
        public string get_DETAILS()
        {
            return this.NOTIFICATION_;
        }

        public override bool Equals(object obj)
        {

            return base.Equals(obj);
        }

    }
}
