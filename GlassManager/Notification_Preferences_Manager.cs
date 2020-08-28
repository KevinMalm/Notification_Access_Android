using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Android.App;
using Android.Service.Notification;
using Android.Widget;


namespace GlassManager
{
    /*
    * This Class is Responsible for saving all Notification Preferences for the system
    * Duties: Load in Serialzied Data. Serialize new Data. Instruct bluetooth manager when needed
    */
    public class Notification_Preferences_Manager
    {

        //String: Preference save to file address 
        const string presistent_address = "/notification_preferences_data.dat";
        //Point to constrcuted User Database where all detialis will be stored
        Notification_Preferences_Database user_database = new Notification_Preferences_Database();

        //Constructor 
        private Notification_Preferences_Manager()
        {
            this.user_database = new Notification_Preferences_Database();
        }



        /*
         * * * * * * * * * * * STATIC METHODS * * * * * * * * * * *
         */
        //Only public constructor of this class. Tries to load in a file, if fails: return new preferene object
        public static Notification_Preferences_Manager attempt_load_in(){

            Notification_Preferences_Manager new_manager = new Notification_Preferences_Manager();
            string enviroment_dir = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + presistent_address;

            try{ //Attempt retrieving file
                if(File.Exists(enviroment_dir)){
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream fileStream = File.Open(enviroment_dir, FileMode.Open);
                    try{ //try to deserialize 
                        Notification_Preferences_Database retrieved_database = (Notification_Preferences_Database)bf.Deserialize(fileStream);
                        new_manager.user_database = retrieved_database;
                    } catch(Exception e){
                        //File was not in the proper format and casting failed
                        Toast.MakeText(Application.Context, "Preference File became Currupted :(", ToastLength.Long).Show();

                    }
                    //Close Stream
                    fileStream.Dispose();
                    fileStream.Close();
                }
            } catch (Exception e){
                // no database could be read in. Returns new Database instead
                Toast.MakeText(Application.Context, "No File was Found to Read From.", ToastLength.Long).Show();
            }

            return new_manager;
        }

        /*
         * * * * * * * * * * * CLASS METHODS * * * * * * * * * * *
         */
        public void save_data(){
            try {
                string enviroment_dir = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + presistent_address;
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fileStream = File.Create(enviroment_dir);
                bf.Serialize(fileStream, this.user_database);
                fileStream.Dispose();
                fileStream.Close();
            } catch(Exception e){
                //Unalbe to Save Data
                Toast.MakeText(Application.Context, "Unable to Update Preferences", ToastLength.Long).Show();
            }
        }


    public void throw_notification(StatusBarNotification status){
            //Check if what should be done
            if (status == null)
                return;
            //get ID
            
            string app_name = status.PackageName;
            if (app_name.Equals(""))
                return;

            Single_Notification_Preference prefereces_ = this.user_database.FIND(app_name);
            if (prefereces_ == null)
                return;

            // SEND DATA

        }

    }

    [Serializable]
    public class Notification_Preferences_Database{

        static Single_Notification_Preference DELETED = Single_Notification_Preference.get_DELETED();
        
        private Single_Notification_Preference[] database = new Single_Notification_Preference[100]; //Hash map interaction 
        int size = 100;
        int cnt = 0;
        float q = 0;
        const float threshold = 0.7f;
        public Notification_Preferences_Database(){}

        
        public void reallocate(){
            int new_size = this.size * 2;
            Single_Notification_Preference[] next_database = new Single_Notification_Preference[new_size];
            //rehash everyone

            for (int i = 0; i < this.size; i++){
                Single_Notification_Preference local = this.database[i];
                if (local != null || local != DELETED){
                    int og_index = local.get_APP_COM().GetHashCode();
                    og_index %= new_size;

                    //Loop through till open spot found
                    while (next_database[og_index] != null || next_database[og_index] != DELETED){
                        og_index += 1;
                        og_index %= size;
                    }
                    //add to spot
                    next_database[og_index] = local;
                }
            }

            this.size = new_size;
            this.database = next_database;
            this.q = this.cnt / this.size;
        }

        public void Add(Single_Notification_Preference a) {
            if (a == null)
                return;
            if(this.q > threshold) 
                this.reallocate();

            int og_index = a.get_APP_COM().GetHashCode();
            og_index %= this.size;

            //Loop through till open spot found
            while(this.database[og_index] != null || this.database[og_index] != DELETED){
                og_index += 1;
                og_index %= size;
            }
            //add to spot
            this.database[og_index] = a;
            this.cnt += 1;
            this.q = this.cnt / this.size;

            return;
        }

        public Single_Notification_Preference FIND(string id){
            if(id == null)
                return null;
            int hash = id.GetHashCode();
            hash %= this.size;
            while(this.database[hash] != null || this.database[hash]!= DELETED || this.database[hash].CompareToString(id) == false){
                hash += 1;
                hash %= this.size;
            }
            if(this.database[hash].CompareToString(id)){
                return this.database[hash]; //WE FOUND IT :D
            }

            return null; //BAD INDEX :(
        }
    }

    [Serializable]
    public class Single_Notification_Preference{
        //class variables
        private string APP_COM = "nan"; //Application identification

        private short COLOR_ID = -1; //Color of Notification
        private short LOCATION = -1; //Physical Location Case
        private short WAVE = -1; //-1: Off, else specified dir
        private short DURATION = 500; //Time in mS: less than 1 means stay on
        private short HOLD_DURATION = 500; //Time in mS between blinks
        private short REPEAT = 1; //time to repeat 

        //Constructor
        public Single_Notification_Preference(){
            this.COLOR_ID = -1;
            this.LOCATION = -1;
            this.WAVE = -1;
            this.DURATION = 500;
            this.HOLD_DURATION = 500;
            this.REPEAT = 1;
        }
        /*
         * * * * * * * * * * * STATIC METHODS * * * * * * * * * * *
         */
        public static Single_Notification_Preference get_DELETED(){
            Single_Notification_Preference deleted_copy = new Single_Notification_Preference();
            deleted_copy.APP_COM = "DELETED";
            return deleted_copy;
        }

        /*
         * * * * * * * * * * * CLASS METHODS * * * * * * * * * * *
         */
        public string get_APP_COM() { return this.APP_COM; }

        public bool CompareToString(string a){
            if (this.APP_COM.Equals(a))
                return true;
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            try
            {
                Single_Notification_Preference converted = (Single_Notification_Preference)obj;
                if (this.APP_COM.Equals(converted.APP_COM))
                    return true;
            } catch(Exception e){

            }
            return false;
        }


    }
}
