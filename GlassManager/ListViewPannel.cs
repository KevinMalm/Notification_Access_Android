using System;
using System.Collections.Generic;
using Android.Widget;
using Android.App;

using Android.Content;
using Android.Views;
using Java.Lang;
using Android.Content.PM;

namespace GlassManager
{
    public class ListViewPannel:BaseAdapter
    {
        private Context context;
        private List<string> data;

        public ListViewPannel(Context context)
        {
            this.context = context;
            data = new List<string>();
            PackageManager pm = Application.Context.PackageManager;
            IList<ApplicationInfo> infos = pm.GetInstalledApplications(PackageInfoFlags.MetaData);

            foreach (ApplicationInfo item in infos)
            {
                string name = item.TaskAffinity;
                this.data.Add(name);


            }
        }

        public override int Count => this.data.Count;

        public override Java.Lang.Object GetItem(int position)
        {
            return this.data[position];
        }

        public override long GetItemId(int position)
        {
            return (long)position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if(convertView == null){
                LayoutInflater inflat = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
                view = inflat.Inflate(Resource.Layout.Application_List_Page, null);
            }
            TextView txtview = view.FindViewById<TextView>(Resource.Id.AppListArea);
            var itemData = data[position];
            txtview.Text = itemData;

            return view;
        }
    }
}
