using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using SolidWorks.Interop.swconst;

namespace ValueDesign
{
    public class ValueBodyPMPage
    {
        //Local Objects
        IPropertyManagerPage2 swPropertyPage = null;
        public ValueBodyPMPHandler handler = null;
        ISldWorks iSwApp = null;
        SwAddin userAddin = null;
        
        #region Property Manager Page Controls
        //Groups
        IPropertyManagerPageGroup group1;
        IPropertyManagerPageGroup group2;

        //Controls
        IPropertyManagerPageLabel lbLength;
        IPropertyManagerPageLabel lbWidth;
        IPropertyManagerPageLabel lbHeight;
        IPropertyManagerPageTextbox tbLength;
        IPropertyManagerPageTextbox tbWidth;
        IPropertyManagerPageTextbox tbHeight;


        IPropertyManagerPageTextbox textbox1;
        IPropertyManagerPageCheckbox checkbox1;
        IPropertyManagerPageOption option1;
        IPropertyManagerPageOption option2;
        IPropertyManagerPageOption option3;
        IPropertyManagerPageListbox list1;

        IPropertyManagerPageSelectionbox selection1;
        IPropertyManagerPageNumberbox num1;
        IPropertyManagerPageCombobox combo1;

        //Control IDs
        public const int group1ID = 0;
        public const int group2ID = 1;

        public const int textbox1ID = 2;
        public const int checkbox1ID = 3;
        public const int option1ID = 4;
        public const int option2ID = 5;
        public const int option3ID = 6;
        public const int list1ID = 7;

        public const int selection1ID = 8;
        public const int num1ID = 9;
        public const int combo1ID = 10;
        public const int lbLengthID = 11;
        public const int lbWidthID = 12;
        public const int lbHeightID = 13;
        public const int tbLengthID = 14;
        public const int tbWidthID = 15;
        public const int tbHeightID = 16;
        #endregion
        public ValueBodyPMPage(SwAddin addin)
        {
            userAddin = addin;
            if (userAddin != null)
            {
                iSwApp = (ISldWorks)userAddin.SwApp;
                CreatePropertyManagerPage();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("SwAddin not set.");
            }            
        }

        public int L{get;set;}
        public int W { get; set; }
        public int H { get; set; }
        private void CreatePropertyManagerPage()
        {
            int errors = -1;
            int options = (int)swPropertyManagerPageOptions_e.swPropertyManagerOptions_OkayButton |
                (int)swPropertyManagerPageOptions_e.swPropertyManagerOptions_CancelButton;

            handler = new ValueBodyPMPHandler(userAddin, this);
            swPropertyPage = (IPropertyManagerPage2)iSwApp.CreatePropertyManagerPage("阀块基体", options, handler, ref errors);
            if (swPropertyPage != null && errors == (int)swPropertyManagerPageStatus_e.swPropertyManagerPage_Okay)
            {
                try
                {
                    AddControls();
                }
                catch (Exception e)
                {
                    iSwApp.SendMsgToUser2(e.Message, 0, 0);
                }
            }
        }

        //Controls are displayed on the page top to bottom in the order 
        //in which they are added to the object.
        protected void AddControls()
        {
            short controlType = -1;
            short textctlType = -1;
            short align = -1;
            int options = -1;


            //Add the groups
            options = (int)swAddGroupBoxOptions_e.swGroupBoxOptions_Expanded |
                      (int)swAddGroupBoxOptions_e.swGroupBoxOptions_Visible;

            group1 = (IPropertyManagerPageGroup)swPropertyPage.AddGroupBox(group1ID, "尺寸选择", options);


            //textbox1
            controlType = (int)swPropertyManagerPageControlType_e.swControlType_Label;
            align = (int)swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge;
            options = (int)swAddControlOptions_e.swControlOptions_Enabled |
                      (int)swAddControlOptions_e.swControlOptions_Visible;

            textctlType = (int)swPropertyManagerPageControlType_e.swControlType_Textbox;
            
            lbLength = (IPropertyManagerPageLabel)group1.AddControl(lbLengthID, controlType, "长：", align, options, "阀块基体长度");
            tbLength = (IPropertyManagerPageTextbox)group1.AddControl(tbLengthID, textctlType, "", align, options, "阀块长度");
            lbWidth = (IPropertyManagerPageLabel)group1.AddControl(lbWidthID, controlType, "宽：", align, options, "阀块基体宽度");
            tbWidth = (IPropertyManagerPageTextbox)group1.AddControl(tbWidthID, textctlType, "", align, options, "阀块宽度");
            lbHeight = (IPropertyManagerPageLabel)group1.AddControl(lbHeightID, controlType, "高：", align, options, "阀块基体高度");
            tbHeight = (IPropertyManagerPageTextbox)group1.AddControl(tbHeightID, textctlType, "", align, options, "阀块高度");
            tbLength.Text = ValueConst.VALUE_DEFAULT_LENGTH.ToString();
            tbWidth.Text = ValueConst.VALUE_DEFAULT_WIDTH.ToString();
            tbHeight.Text = ValueConst.VALUE_DEFAULT_HEIGHT.ToString();            
        }

        public void Show()
        {
            if (swPropertyPage != null)
            {
                swPropertyPage.Show2(0);
            }
        }

        public int CubeLength
        {
            get
            {
                int ret;
                if (Int32.TryParse(this.tbLength.Text, out ret))
                {
                    return ret;
                }

                return ValueConst.VALUE_DEFAULT_LENGTH; 
            }
            
        }

        public int CubeWidth
        {
           get
           {
               int ret;
               if (Int32.TryParse(this.tbWidth.Text, out ret))
                   return ret;
               return ValueConst.VALUE_DEFAULT_WIDTH;
           }          
        }

        public int CubeHeight
        {
            get
            {
                int ret;
                if (Int32.TryParse(this.tbHeight.Text, out ret))
                    return ret;
                return ValueConst.VALUE_DEFAULT_HEIGHT;
            }          
        }
    }
}
