using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.File;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BettrDiet.Core.Common
{
    public class ObjectStorageHelper<T>
    {
        private XmlSerializer serializer;
        private string _filename = null;
        private IMvxFileStore fs;

        private string FileName(T Obj)
        {
            string ret;
            if (string.IsNullOrEmpty(_filename))
            {
                ret = String.Format("{0}.xml", Obj.GetType().Name);
            }
            else
                ret = _filename + ".xml";
            Debug.WriteLine("Filename:" + ret);
            return ret;
        }
        public ObjectStorageHelper(string filename = null)
        {
            _filename = filename;
            serializer = new XmlSerializer(typeof(T));
            fs = Mvx.Resolve<IMvxFileStore>();
        }

        public async void DeleteAsync()
        {
            try
            {
                fs.DeleteFile(_filename);
            }
            catch (Exception)
            {
                //throw;
            }
        }

        public async Task<int> SaveAsync(T Obj)
        {
            var stop = new Stopwatch();
            stop.Start();

            try
            {
                if (Obj != null)
                {
                    
                    MemoryStream s = new MemoryStream();
                    serializer.Serialize(s, Obj);
                    s.Flush();
                    byte[] a=s.ToArray();
                    var st = Encoding.UTF8.GetString(a, 0, a.Length);
                    fs.WriteFile(_filename, st);

                    s.Dispose();                    
                    
                    Debug.WriteLine("==>" + st.Length);
                }
            }
            catch (Exception)
            {
                throw;
            }
            Debug.WriteLine("SAVEFILE:" + _filename + " " + stop.ElapsedMilliseconds);

            return 0;
        }

        public async Task<int> DeleteAsync(T Obj)
        {
            var stop = new Stopwatch();
            stop.Start();

            try
            {
                if (Obj != null)
                {
                    fs.DeleteFile(_filename);
                }
            }
            catch (Exception)
            {
                throw;
            }
            Debug.WriteLine("DELETEFILE:" + _filename + " " + stop.ElapsedMilliseconds);

            return 0;
        }


        public async Task<T> LoadAsync()
        {
            var stop = new Stopwatch();
            stop.Start();
            try
            {

                
                string content;
                if (fs.TryReadTextFile(_filename, out content))
                {
                    Debug.WriteLine("LOADFILE:" + _filename);
                    Debug.WriteLine("<==" + content.Length);
                    using (MemoryStream s = new MemoryStream(Encoding.UTF8.GetBytes(content)))
                    {
                        return (T)serializer.Deserialize(s);
                    }
                }
                throw new Exception("File not found");
            }
            catch (FileNotFoundException)
            {
                //file not existing is perfectly valid so simply return the default 
                return default(T);
                //Interesting thread here: How to detect if a file exists (http://social.msdn.microsoft.com/Forums/en-US/winappswithcsharp/thread/1eb71a80-c59c-4146-aeb6-fefd69f4b4bb)
                //throw;
            }
            catch (Exception)
            {
                //Unable to load contents of file
                throw;
            }
            Debug.WriteLine("LOADFILE:" + _filename + " " + stop.ElapsedMilliseconds);

        }

    }
}
