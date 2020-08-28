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

        public void throw_notification(String str, int p)
        {
            //atempt to get correct colour
            int color_value = this_database.determine_color(str, p);
            this.push_alert(color_value);
            return;
        }


        private void push_alert(int color_space){

            //serialize color to be commuted over bluetooth protocol 


            return;
        }


    }
    class Color_Database
    {

        private Notification_data[] notification_summary;
        float notification_fill = 0;
        int default_color = 0;

        public Color_Database(){
            notification_summary = new Notification_data[100];
        }

        //returns int of corresponding color space 
        //input: str ( notification title) p ( notifiication prioirty)
        public int determine_color(String str, int p){

            return 0;
        }
        private void reallocate_array(){
            return;
        }
        //adds raw Notification Data Class to array - assumes default values 
        public void add_gerenal(string Name, int priority)
        {
            if(notification_fill > 0.7f){
                this.reallocate_array();
            }

            int hash = Name.GetHashCode() % this.notification_summary.Length;


            
            

            /*
            while (i < this.notification_summary.Length){ //replace with hash
                if (this.notification_summary[i].get_NAME().Equals(Name)){
                    return;
                }

                i++;
            }
            */

            //if we got here its a new boi
            Notification_data new_notification_set = new Notification_data(Name, this.default_color);
            ///this.notification_summary.Add(new_notification_set);
            return;
        }
       

    }
    class Notification_data
    {

        private string NOTIFICATION_NAME = "";
        private string GENERAL_NAME = "";
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
