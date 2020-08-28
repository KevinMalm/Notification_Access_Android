using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace GlassManager
{


    public class Color_Database_Access{

        Color_Database this_database;
        private string presistent_data_address = "Assets/data.dat";
       
        public Color_Database_Access(){
            this.load_in_data();
        }

        public void load_in_data(){
            if(File.Exists(this.presistent_data_address)){
                FileStream s = File.Open(this.presistent_data_address, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                this.this_database = (Color_Database)bf.Deserialize(s);
                s.Close();
            }
            else{
                this.this_database = new Color_Database();
            }
        }
    
        public void save_data(){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(this.presistent_data_address);

            bf.Serialize(file, this.this_database);
            file.Close();
            return;
        }



    }
    class Color_Database
    {

        private List<Notification_data> notification_summary = new List<Notification_data>();
        int default_color = 0;

        public void add_gerenal(string Name)
        {
            //first ensure not making a duplicate 
            int i = 0;
            while (i < this.notification_summary.Count){
                if (this.notification_summary[i].get_NAME().Equals(Name)){
                    return;
                }

                i++;
            }

            //if we got here its a new boi
            Notification_data new_notification_set = new Notification_data(Name, this.default_color);
            this.notification_summary.Add(new_notification_set);
            return;
        }
       

    }
    class Notification_data
    {

        private string NOTIFICATION_NAME = "";
        private int NOTIFICATION_STATE = -1;
        private bool customized = false;

        public Notification_data(string name, int color_status)
        {
            this.NOTIFICATION_NAME = name;
            this.NOTIFICATION_STATE = color_status;
            this.customized = false;
        }

        public string get_NAME(){
            return this.NOTIFICATION_NAME;
        }
        public int get_STATE()
        {
            return this.NOTIFICATION_STATE;
        }
        public void set_NAME(string new_name){
            this.NOTIFICATION_NAME = new_name;
            return;
        }
        public void set_COLOUR(int new_colour)
        {
            this.NOTIFICATION_STATE = new_colour;
            return;
        }
    }
}
