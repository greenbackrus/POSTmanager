using POSTmanager.Helpers;
using POSTmanager.Models;
using System;
using System.Diagnostics;
using System.IO;

namespace POSTmanager.handlers
{
    internal class RK7Base : IApplicationStartable
    {
        private string _windowTitle;
        private string _windowClass;
        private string _username;
        private string _password;
        private string _basesDirectory;
        private string _restBaseDirectory;
        private string _restName;
        public RK7Base(string restName, string username, string password)
        {
            _restName = restName;
            _username = username;
            _password = password;

            // параметры окна для его поиска: класс и наименование 
            _windowTitle = Properties.Settings.Default.RK7WindowTitle;
            _windowClass = Properties.Settings.Default.RK7WindowClass;

            // расположение эталонной и базы Рк7 конкретного ресторана
            _basesDirectory = Properties.Settings.Default.Rk7BasesDir;
            _restBaseDirectory = $@"{_basesDirectory}/{restName}";
        }

        private void CopyBaseDir()
        {
            GeneralHelper.CopyDirectory($@"{_basesDirectory}\Rk7Manager", _restBaseDirectory, true);
        }

        private void CreateIniFile(User currentUser, Rest currentRest)
        {
            //TODO: pass server data from db 
            using (StreamWriter stream = new StreamWriter($@"{_restBaseDirectory}\rk7man.ini", false))
            {
                stream.Write($@"[REFEDIT]
                UserName={currentUser.Login}
                Client={currentUser.Login}-%RANDOM%
                Server={currentRest.ServerName}
                LOCKONEDIT=0
                LongTimeout=200000
                [NETKERN]
                PROTOCOLS=tcpsoc.dll
                [TCPSOC]
                LISTEN=0
                PORT={currentRest.Port}
                [TCPDNS]
                {currentRest.ServerName}={currentRest.Ip}:{currentRest.Port}");
                stream.Close();
            }
        }

        private void CreateBatFile()
        {
            using (StreamWriter stream = new StreamWriter($@"{_restBaseDirectory}\rk7man.bat", false, System.Text.Encoding.GetEncoding(866)))
            {
                stream.Write($@"REM RK7Man update and start
                cd {_restBaseDirectory}
                SET PRELOADPATH=.\PRELOAD
                if /%1 == / goto defini
                SET CASHINIPATH=%1
                goto now_run
                :defini
                SET CASHINIPATH=.\rk7man.ini

                :now_run
                preload.exe %CASHINIPATH%
                for %%c in (%PRELOADPATH%\*.dll) do del /F %%~nc.bak
                for %%c in (%PRELOADPATH%\*.dll) do ren %%~nc.dll *.bak
                
                if not EXIST rk7copy.exe goto nork7copy
                
                rk7copy %PRELOADPATH% .\ /S /C
                goto continue
                
                :nork7copy
                echo rk7copy.exe not found
                xcopy %PRELOADPATH% .\ /S /C /R /Y
                
                :continue
                rmdir %PRELOADPATH% /S /Q
                start FSupdate.exe /console sch
                start "" rk7man.exe %CASHINIPATH%");
                stream.Close();
            }
        }

        private Boolean RestDirExist()
        {
            if (!Directory.Exists(_restBaseDirectory))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetupNewRest()
        {
            CopyBaseDir();
            CreateBatFile();
            //CreateIniFile();
        }

        public void AfterStart()
        {
            throw new NotImplementedException();
        }

        public void BeforeStart()
        {
            // если базы нет, копируем эталонную и настраиваем
            if (!RestDirExist())
            {
                SetupNewRest();
            }
        }

        public void Start()
        {
            Process.Start($@"rk7man.bat");
        }
    }
}