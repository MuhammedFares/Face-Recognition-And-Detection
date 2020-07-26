using SMARTY.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SMARTY.Trainer
{
   public class Variables
    {
        /// <summary>
        /// a Class To Store The Result Of Predication Operation
        /// </summary>
        public class RecognitionResult
        {
            //Recognised Face Name
            public string Label;
            //Simillarity Value
            public int Int;

            //Error
            public bool HasError;
            public string ErrorMessage;
        }

        /// <summary>
        /// DataSet Folder : Contains EgienFaces Images and Labels File
        /// </summary>
        /// <returns>Path to Data Folder</returns>
        public static string DataSetFolder { get { return App+Resources.TrainingDataSet; } }
        /// <summary>
        /// Contains Names For Every Egien Face Stored In Data Set
        /// </summary>
        /// <returns>Path to Labels File</returns>
        public static string LabelsFile   { get  { return App+Resources.TrainingDataLabels; } }
        /// <summary>
        /// Haar Like Cascade Classifier File
        /// </summary>
        /// <returns>Path to Cascade File File</returns>
        public static string CascadeFile { get { return (App+Resources.CacadeFile); } }
       
      
        private static string App { get { return System.IO.Directory.GetCurrentDirectory(); }
            
        }
    }
}
