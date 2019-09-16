using System;
using System.Globalization;
using System.IO;
using CrypKey;

namespace StorageQuest.VideoSurveillanceExporterCommon.Utility
{
    internal class SamCrypKey
    {
        private CrypKeyNET2sn ck;
        private bool isInitialized;
        private const int maxChecktime = 10;
        internal Authorization Authorization { private set; get; }

        internal SamCrypKey()
        {
            this.Authorization = new Authorization();
            try
            {
                this.ck = new CrypKey.CrypKeyNET2sn();
            }
            catch (Exception ex)
            {
                if (ProxySingleton.LogCommon != null)
                    ProxySingleton.LogCommon.Error(null, ex);
            }
            this.isInitialized = false;
        }

        internal Authorization GetAuthorization()
        {
            if (this.ck == null)
                return null;

            string  fileName;
            string MK = "d0b6377e78bd6cb85bff7bb8c0d49817e0b8eee1760edd88547f18b1a24a6bc37c95e80d1d802c132985fa8f357929e7cb059b2174316212f3b781ed3b3b5aa53a06af309763dc1bcc75df46bc398ff2f23ede0c734eed169a7e1a600d89dc23f52e6f93b97bc89834545949f15f414ebaf22d3d1e3f18226f842be725d32035";
            string UK = "D2A398F8023F5646F7";

            fileName = Path.Combine(System.Environment.CurrentDirectory, "OAM.LIC");
                //@"C:\Program Files (x86)\CrypKey SDK\Build 7300\Examples\Sample Code\Visual Studio\CSharp_Project\C# Sample\bin\x86\Debug" +
                //"\\example.exe";
            

            // Step 1, ckerr = 0 (successful), -ve (unsuccessful), 
            int ckerr = (int)this.ck.InitCrypKey(fileName, MK, UK, 0, maxChecktime);
            double ckver = this.ck.CrypKeyVersion();
            this.isInitialized = true;
            string CrypKeyVersion = "CrypKey Version: " + ((int)ckver / 10).ToString(CultureInfo.InvariantCulture) + "." + ((ckver % 10).ToString(CultureInfo.InvariantCulture));
            //Checking Authorization...
            if (ckerr == 0) //InitCrypKey OK
            {
                // Step 2
                ckerr = (int)this.ck.GetAuthorization2(1);
                if (ckerr == 0) // Program authorized.
                {
                    ckerr = (int)this.ck.GetAuthorization2(1); //make duplicate call to free any locked network licenses.
                    //GetAuthorization2 OK
                    GetRestrictionInformation();
                    GetMoreRestInfo();
                    //initNetTimer();
                    //if using level for expiry date, then verify it hasn't expired.
                    string strDate;
                    if (CheckExpiryDate(out strDate))
                    {
                        this.Authorization.IsAuthorized = false ;
                        this.Authorization.Message = "License Expired!";
                    }
                    else
                    {
                        this.Authorization.IsAuthorized = true;
                        if (strDate.Length != 0)
                            this.Authorization.Message = "License will expire on: " + strDate;
                    }
                }
                else if (ckerr < 0)// unsuccessful (program not authorized)
                {
                    this.Authorization.IsAuthorized = false;
                    this.Authorization.Message = this.ck.ExplainErr(CrypKey.CrypKeyNET2sn.CKExplainEnum.CKInitializeError, ckerr);
                    //if (checkBoxAutoRecover.Checked)//Try to restore backup of license
                    //{
                    //    EZRestore();//Restore and Try license again
                    //    ckerr = (int)this.ck.GetAuthorization2(1);
                    //    if (ckerr == 0)
                    //    {
                    //        // GetAuthorization2 OK
                    //        SetAuthButtonStates();
                    //        initNetTimer();
                    //    }
                    //    else //restore unsuccessful.
                    //    {
                    //        // GetAuthorization2 Error
                    //        string err = ckerr.ToString();
                    //        SetNoAuthButtonState();
                    //    }
                    //}
                    //else
                    //{
                    //    // GetAuthorization2 Error
                    //    string err = ckerr.ToString();
                    CreateTrialLicense();
                    //}
                }
                else // unsuccessful (program has a multi-user license and more users are requesting use of the
                     // program than the license allows).
                {
                    this.Authorization.IsAuthorized = false;
                    this.Authorization.Message = this.ck.ExplainErr(CrypKey.CrypKeyNET2sn.CKExplainEnum.CKInitializeError, ckerr);
                }
                //sitecode.Text = this.ck.GetSiteCode2();
                //buttonCheckAuthorization.Enabled = false;//don't want to call initcrypkey again until EndCrypkey has been called.
            }
            else
            {
                this.Authorization.IsAuthorized = false;
                if (ckerr == -23)
                    this.Authorization.Message = "InitCrypKey Error: crp32002.ngn not found";
                else
                    this.Authorization.Message = this.ck.ExplainErr(CrypKey.CrypKeyNET2sn.CKExplainEnum.CKInitializeError, ckerr);
            }
            
            return this.Authorization;
        }

        /// <summary>
        /// Checks to see if exipry date has pass
        /// </summary>
        /// <param name="ExpiryDate">string ExpiryDate</param>
        /// <returns>True if date has passed</returns>
        private bool CheckExpiryDate(out string ExpiryDate)
        {
            bool bStatus = false;
            DateTime dt;
            string YY = string.Empty ;
            string MM = string.Empty;
            string DD = string.Empty;
            string SDate = string.Empty;
            ExpiryDate = SDate;

            //if (checkBoxExpOverride.Checked) //use test value
            //{
            //    SDate = textBoxExpTestDate.Text.Substring(0, 2) +
            //            textBoxExpTestDate.Text.Substring(3, 2) +
            //            textBoxExpTestDate.Text.Substring(6, 2);
            //}
            //else //use level
            {
                SDate = this.ck.GetLevel(13).ToString(CultureInfo.InvariantCulture); //note: using MMDDYY leaves 13 bits for options
                if (SDate == "0")
                {
                    //this.Authorization.Message.Add("Level does not indicate a valid expiry date");
                    return bStatus;
                }
            }
            if (SDate.Length == 5) SDate = "0" + SDate;//pad it for processing.
            MM = SDate.Substring(0, 2); //Get the month
            DD = SDate.Substring(2, 2); //Get the Day
            YY = SDate.Substring(4, 2); //Get the Year
            ExpiryDate = SDate;
            try
            {
                dt = Convert.ToDateTime(MM + "/" + DD + "/" + YY,CultureInfo.InvariantCulture);
                //Make a nice string
                ExpiryDate = dt.ToString();
            }
            catch { }

            //compare to current date
            try
            {
                if (Convert.ToInt16("20" + YY, CultureInfo .InvariantCulture ) < DateTime.Now.Year)
                    bStatus = true;
            }
            catch { }

            try
            {
                if (Convert.ToInt16(MM, CultureInfo.InvariantCulture) < DateTime.Now.Month)
                {
                    if (Convert.ToInt16(DD, CultureInfo.InvariantCulture) < DateTime.Now.Day)
                        bStatus = true;
                }
            }
            catch { }

            if (bStatus)
            {
                //this.ck.KillLicense();
                //DeleteBackup();
                this.Authorization.Message = "Backup License deleted!";
            }
            return bStatus;
        }

        //private void EZRestore()
        //{
            //try
            //{
            //    if (IsBackupFound())
            //    {
            //        listBox1.Items.Add("Restoring EasyLicense");
            //        File.Copy("rst.bak", "example.rst", true);
            //        File.Copy("key.bak", "example.key", true);
            //        //File.SetAttributes("key.bak", FileAttributes.Hidden);
            //        //File.SetAttributes("key.rst", FileAttributes.Hidden);
            //        listBox1.Items.Add("Done!");
            //    }
            //    else
            //        listBox1.Items.Add("Cannot restore - no backup exists!");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("License Restore Failed!" + ex.Message);
            //}
        //}

        //private void DeleteBackup()
        //{
        //    try
        //    {
        //        File.Delete("rst.bak");
        //        File.Delete("key.bak");

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error in DeleteBackup()" + ex.Message);
        //    }
        //}

        private void GetRestrictionInformation()
        {
            int authOpt = 0;
            ulong startDate = 0;
            int numAllowed = 0;
            int numUsed = 0;
            int err;
            int restrictionInfo;

            //"Restriction Information...")
            try
            {
                restrictionInfo = (int)CrypKey.CrypKeyNET2sn.CKRestrictionTypeEnum.CKRestrictionTypeNone;
                err = (int)this.ck.GetRestrictionInfo(ref authOpt, ref startDate, ref numAllowed, ref numUsed);//(ref restrictionInfo, ref startDate, ref numAllowed, ref numUsed);
                if (err == 0)
                {
                    switch (restrictionInfo)
                    {
                        case (int)CrypKey.CrypKeyNET2sn.CKRestrictionTypeEnum.CKRestrictionTypeNone:
                            this.Authorization.IsAuthorized = true;
                            this.Authorization.RestrictionInformation = "UNLIMITED LICENSE";
                            break;

                        case (int)CrypKey.CrypKeyNET2sn.CKRestrictionTypeEnum.CKRestrictionTypeRuns:
                            if (numUsed >= numAllowed)
                            {
                                this.Authorization.IsAuthorized = false;
                                this.Authorization.RestrictionInformation = "NO MORE RUNS LEFT - LICENSE EXPIRED";
                            }
                            else
                            {
                                this.Authorization.IsAuthorized = true;
                                int numLeft = numAllowed - numUsed;
                                this.Authorization.RestrictionInformation = numLeft.ToString(CultureInfo.InvariantCulture) + " runs remaining";
                            }
                            break;

                        case (int)CrypKey.CrypKeyNET2sn.CKRestrictionTypeEnum.CKRestrictionTypeTime:
                            if (numUsed >= numAllowed)
                            {
                                this.Authorization.IsAuthorized = false;
                                this.Authorization.RestrictionInformation = "NO MORE DAYS LEFT - LICENSE EXPIRED";
                            }
                            else
                            {
                                this.Authorization.IsAuthorized = true;
                                int numLeft = numAllowed - numUsed;
                                this.Authorization.RestrictionInformation = numLeft.ToString(CultureInfo.InvariantCulture) + " days remaining";
                            }
                            break;
                    }
                }
                else
                {
                    this.Authorization.IsAuthorized = false;
                    err = (int)CrypKey.CrypKeyNET2sn.CKRestrictionErrorEnum.CKRestrictionInvalid;
                }
            }
            catch (Exception ex)
            {
                if (ProxySingleton.LogCommon != null)
                    ProxySingleton.LogCommon.Error(null, ex);

                this.Authorization.IsAuthorized = false;
            }
        }

        private void GetMoreRestInfo()
        {
            int type = 0 , allowed = 0, used = 0;
            this.Authorization.Type = (CKWhichLicenseType ) this.ck.Get1RestInfo(CrypKey.CrypKeyNET2sn.CKWhichEnum.CKWhichLicenseType);
            switch (this.Authorization.Type)
            {
                case CKWhichLicenseType.UnLimited:
                    if (ProxySingleton.LogCommon != null)
                        ProxySingleton.LogCommon.Info("CKWhichLicenseType = " + type.ToString(CultureInfo.InvariantCulture) + " :unLimited");
                    break;

                case CKWhichLicenseType.DayLimited:
                    if (ProxySingleton.LogCommon != null)
                        ProxySingleton.LogCommon.Info("CKWhichLicenseType = " + type.ToString(CultureInfo.InvariantCulture) + " :  Day Limited");

                    this.Authorization.CKWhichNumAllowed = this.ck.Get1RestInfo(CrypKey.CrypKeyNET2sn.CKWhichEnum.CKWhichNumAllowed);

                    if (ProxySingleton.LogCommon != null)
                        ProxySingleton.LogCommon.Info("CKWhichNumAllowed = " + allowed.ToString(CultureInfo.InvariantCulture) + " Days allowed");

                    this.Authorization.CKWhichNumUsed = this.ck.Get1RestInfo(CrypKey.CrypKeyNET2sn.CKWhichEnum.CKWhichNumUsed);

                    if (ProxySingleton.LogCommon != null)
                    {
                        ProxySingleton.LogCommon.Info("CKWhichNumUsed = " + used.ToString(CultureInfo.InvariantCulture) + " Days Used");
                        ProxySingleton.LogCommon.Info("Remaining = " + (allowed - used).ToString(CultureInfo.InvariantCulture) + " Days Remaining");
                    }
                    break;

                case CKWhichLicenseType.RunLimited:
                    if (ProxySingleton.LogCommon != null)
                        ProxySingleton.LogCommon.Info("CKWhichLicenseType = " + type.ToString(CultureInfo.InvariantCulture) + " :  Run Limited");

                    this.Authorization.CKWhichNumAllowed = this.ck.Get1RestInfo(CrypKey.CrypKeyNET2sn.CKWhichEnum.CKWhichNumAllowed);

                    if (ProxySingleton.LogCommon != null)
                        ProxySingleton.LogCommon.Info("CKWhichNumAllowed = " + allowed.ToString(CultureInfo.InvariantCulture) + " Runs allowed");

                    this.Authorization.CKWhichNumUsed = this.ck.Get1RestInfo(CrypKey.CrypKeyNET2sn.CKWhichEnum.CKWhichNumUsed);

                    if (ProxySingleton.LogCommon != null)
                    {
                        ProxySingleton.LogCommon.Info("CKWhichNumUsed = " + used.ToString(CultureInfo.InvariantCulture) + " Runs used ");
                        ProxySingleton.LogCommon.Info("Remaining = " + (allowed - used).ToString(CultureInfo.InvariantCulture) + " Runs remaining");
                    }
                    break;

                default:
                    break;
            }

            if (ProxySingleton.LogCommon != null)
            {
                ProxySingleton.LogCommon.Info("Multi-User Count: " + this.ck.GetNumMultiUsers().ToString(CultureInfo.InvariantCulture));
                ProxySingleton.LogCommon.Info("Copies: " + this.ck.GetNumCopies().ToString(CultureInfo.InvariantCulture));
            }
        }

        internal string GetSiteCode()
        {
            if (!this.isInitialized)
                this.GetAuthorization();
            return this.ck.GetSiteCode2();
        }

        internal bool Validate(string siteKey)
        {
            int ckerr;
            bool result = false;
            this.Authorization.ExplainErr = string.Empty;

            if (!string.IsNullOrEmpty(siteKey))
            {
                ckerr = (int)this.ck.SaveSiteKey(siteKey);
                if (ProxySingleton.LogCommon != null)
                    ProxySingleton.LogCommon.Info("Saving sitekey...");
                if (ckerr == 0)
                {
                    result = true;
                    if (ProxySingleton.LogCommon != null)
                        ProxySingleton.LogCommon.Info("SaveSiteKey OK:");
                    ////this.EZBackup();//backup EasyLicense
                    ckerr = (int)this.ck.GetAuthorization2(1);
                    if (ckerr != 0)
                    {
                        result = false;
                        if (ProxySingleton.LogCommon != null)
                            ProxySingleton.LogCommon.Info("GetAuthorization2 Error: " + ckerr.ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        if (ProxySingleton.LogCommon != null)
                            ProxySingleton.LogCommon.Info("GetAuthorization2 OK");
                        this.GetRestrictionInformation();
                    }
                }
                else
                {
                    if (ProxySingleton.LogCommon != null)
                        ProxySingleton.LogCommon.Error("SaveSiteKey Error: " + ckerr.ToString(CultureInfo.InvariantCulture));
                }
            }
            else
            {
                //Logging.LogGUI.Info("Paste the sitekey and then validate!");
            }
            return result;
        }

        //private void EZBackup()
        //{
        //    try
        //    {
        //        if (CheckEasyLicense())
        //        {
        //            Logging.LogGUI.Info("Backing up EasyLicense");
        //            string fileName = Path.Combine(System.Environment.CurrentDirectory, "OAM.rst");
        //            if (File.Exists(fileName))
        //                File.Copy(fileName, Path.Combine(System.Environment.CurrentDirectory,"rst.bak"), true);

        //            //fileName = Path.Combine(System.Environment.CurrentDirectory, "OAM.rst");
        //            File.Copy(Path.Combine(System.Environment.CurrentDirectory,"OAM.key"), Path.Combine(System.Environment.CurrentDirectory,"key.bak"), true);
        //            //File.SetAttributes("key.bak", FileAttributes.Hidden);
        //            //File.SetAttributes("key.rst", FileAttributes.Hidden);
        //            Logging.LogGUI.Info("Done!");
        //        }
        //        else
        //        {
        //            Logging.LogGUI.Info("Cannot backup - This is NOT an EasyLicense!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.LogGUI.Error(null, ex);
        //    }
        //}

        //private bool CheckEasyLicense()
        //{
        //    //CrypKey.CrypKeyNET2sn ck = new CrypKey.CrypKeyNET2sn();
        //    CrypKey.CrypKeyNET2sn.CKEasyLicenseErrorEnum eErr;
        //    eErr = (CrypKey.CrypKeyNET2sn.CKEasyLicenseErrorEnum)this.ck.IsEasyLicense();
        //    switch (eErr)
        //    {
        //        case CrypKeyNET2sn.CKEasyLicenseErrorEnum.CKIsNotEasy:
        //            {
        //                //groupBox1.Enabled = false;//transfers not allowed with EasyLicense                     
        //                return true;
        //            }
        //        case CrypKeyNET2sn.CKEasyLicenseErrorEnum.CKIsEasy:
        //            {
        //                //groupBox1.Enabled = true;
        //                return false;
        //            }
        //        default:
        //            {
        //                return false;
        //            }
        //    }
        //}

        private void CreateTrialLicense()
        {
            int ckerr;
            int iCopiesOrUsers = 1;
            int iTrialVersion = 1;
            int iDaysOrRuns = 28;
            int OplevelValue = 0;
            ckerr = (int)this.ck.ReadyToTryDays(OplevelValue, iDaysOrRuns, iTrialVersion, iCopiesOrUsers);

            if (ckerr == 0)
            {
                this.Authorization.IsAuthorized = true;
                if (ProxySingleton.LogCommon != null)
                    ProxySingleton.LogCommon.Info("Trial license activated");
                GetRestrictionInformation();
            }
            else
            {
                this.Authorization.IsAuthorized = false;
                if (ProxySingleton.LogCommon != null)
                    ProxySingleton.LogCommon.Info("Unable to activate Trial: " + ckerr.ToString(CultureInfo.InvariantCulture));
                if (ckerr == -19)
                {
                    if (ProxySingleton.LogCommon != null)
                        ProxySingleton.LogCommon.Info("Trial has already been used.");
                }
            }
        }

        internal bool RegisterTransfer(string newPath, string appDir)
        {
            this.Authorization.ExplainErr = string.Empty;
            bool result = false;
            try
            {
                CleanupFiles(newPath, appDir);//remove leftover files from incomplete previous transfer
                int ckerr = (int)ck.RegisterTransfer(newPath);

                //listBox1.Items.Add("ckerr = " + ckerr.ToString(CultureInfo.InvariantCulture));
                if (ckerr < 0)
                    this.Authorization.ExplainErr = ck.ExplainErr(CrypKey.CrypKeyNET2sn.CKExplainEnum.CKRegisterTransferError, ckerr);
                else
                {
                    result = true;
                    this.Authorization.Message = "RegtisterTransfer successful.";
                }
            }
            catch (Exception ex)
            {
                if (ProxySingleton.LogCommon != null)
                    ProxySingleton.LogCommon.Error(null, ex);
            }
            return result;
        }

        private static void CleanupFiles(string xferdir, string appDir)
        {
            try
            {
                string fileName = xferdir + "\\example._eg";
                if(File.Exists (fileName))
                    File.Delete(fileName);
                //listBox1.Items.Add("._eg file deleted");
                fileName = xferdir + "\\example._ey";
                if (File.Exists(fileName))
                    File.Delete(fileName);
                //listBox1.Items.Add("._ey file deleted");
                fileName = xferdir + "\\example._st";
                if (File.Exists(fileName))
                    File.Delete(fileName);
                //listBox1.Items.Add("._st file deleted");
                fileName = appDir + "\\example.reg";
                if (File.Exists(fileName))
                    File.Delete(fileName);
                //listBox1.Items.Add(".reg file deleted");
            }
            catch (Exception ex)
            {
                if (ProxySingleton.LogCommon != null)
                    ProxySingleton.LogCommon.Error(null, ex);
            }
        }

        internal bool TransferIn(string newPath)//, string appDir)
        {
            this.Authorization.ExplainErr = string.Empty ;
            bool result = false;
            try
            {
                //listBox1.Items.Add("Transferring License in from: " + newPath.ToString());
                int ckerr = (int)ck.TransferIn(newPath);
                if (ckerr < 0)
                {
                    //listBox1.Items.Add("License Error: " + ck.ExplainErr(CrypKey.CrypKeyNET2sn.CKExplainEnum.CKTransferInError, ckerr));
                    this.Authorization.ExplainErr = ck.ExplainErr(CrypKey.CrypKeyNET2sn.CKExplainEnum.CKTransferInError, ckerr);
                }
                else
                {
                    result = true;
                    //listBox1.Items.Add("License Transfer Complete.");
                    ckerr = (int)ck.GetAuthorization2(1);
                    if (ckerr != 0)
                    {
                        //listBox1.Items.Add("GetAuthorization2 Error: " + ckerr.ToString());
                    }
                    else
                    {
                        //listBox1.Items.Add("GetAuthorization2 OK");
                        GetRestrictionInformation();
                       // sitecode.Text = ck.GetSiteCode2();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ProxySingleton.LogCommon != null)
                    ProxySingleton.LogCommon.Error(null, ex);
                result = false;
                //listBox1.Items.Add(ex.Message);
            }
            return result;
        }

        internal bool TransferOut(string newPath)//, string appDir)
        {
            this.Authorization.ExplainErr = string.Empty;
            bool result = false;

            try
            {
                //listBox1.Items.Add("Transferring License out to: " + xferdir.ToString());
                int ckerr = (int)ck.TransferOut(newPath);
                if (ckerr < 0)
                {
                    //listBox1.Items.Add("License Error: " + ck.ExplainErr(CrypKey.CrypKeyNET2sn.CKExplainEnum.CKTransferOutError, ckerr));
                    this.Authorization.ExplainErr = ck.ExplainErr(CrypKey.CrypKeyNET2sn.CKExplainEnum.CKTransferOutError, ckerr);
                }
                else
                {
                    result = true;
                    //listBox1.Items.Add("TransferOut Successful.");
                    ckerr = (int)ck.GetAuthorization2(1);
                    if (ckerr != 0)
                    {
                        //listBox1.Items.Add("GetAuthorization2 Error: " + ckerr.ToString());
                    }
                    else
                    {
                        //listBox1.Items.Add("GetAuthorization2 OK");
                        GetRestrictionInformation();
                        //sitecode.Text = ck.GetSiteCode2();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ProxySingleton.LogCommon != null)
                    ProxySingleton.LogCommon.Error(null, ex);
            }
            return result;
        }

        internal string KillLicense()
        {
            string confirmCode = string.Empty;

            try
            {
                int ret = this.ck.KillLicense(ref confirmCode);
                if (ProxySingleton.LogCommon != null)
                    ProxySingleton.LogCommon.Info(string.Format(CultureInfo.InvariantCulture, "Return value = {0}, Confirmed Code = {1}", ret.ToString(CultureInfo.InvariantCulture), confirmCode));
            }
            catch (Exception ex)
            {
                if (ProxySingleton.LogCommon != null)
                    ProxySingleton.LogCommon.Error(null, ex);
            }

            return confirmCode;
        }

        internal void Close()
        {
            if (this.ck != null)
                this.ck.EndCrypKey();
        }
     
    }

    internal class Authorization
    {
        internal bool IsAuthorized { set; get; }
        //internal DateTime ExpiryDate { set; get; }
        internal string ExplainErr { set; get; }
        internal string RestrictionInformation { set; get; }

        internal CKWhichLicenseType Type { set; get; }
        internal int CKWhichNumAllowed { set; get; }
        internal int CKWhichNumUsed { set; get; }

        internal string Message { set; get; }
        
        internal Authorization()
        {
        }
    }

    internal enum CKWhichLicenseType : int
    {
        UnLimited = 0,
        DayLimited = 1,
        RunLimited = 2
    }
}