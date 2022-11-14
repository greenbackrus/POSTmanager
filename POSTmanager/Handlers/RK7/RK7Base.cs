using POSTmanager.Helpers;
using POSTmanager.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Documents;

namespace POSTmanager.handlers
{
    internal class RK7Base : IApplicationStartable
    {
        private PmDbContext _context = new PmDbContext();
        private User _user;
        private Rest _rest;
        private string _windowTitle;
        private string _password;
        private string _basesDirectory;
        private string _restBaseDirectory;
        private string _restName;
        public RK7Base(Rest rest, User user)
        {
            _rest = rest;
            _user = user;
            _restName = _rest.Name;
            _password = _context.UserRests
                .Where(ur => ur.UserId == _user.Id && ur.RestId == _rest.Id)
                .Select(mc => mc.Password)
                .Single()
                .ToString();

            // параметры окна для его поиска: класс и наименование 
            _windowTitle = Properties.Settings.Default.RK7WindowTitleRus;

            // расположение эталонной и базы Рк7 конкретного ресторана
            _basesDirectory = Properties.Settings.Default.Rk7BasesDir;
            _restBaseDirectory = $@"{_basesDirectory}\{_restName}";
        }

        private void CopyBaseDir()
        {
            GeneralHelper.CopyDirectory($@"{_basesDirectory}\Rk7Manager", _restBaseDirectory, true);
        }

        private void CreateIniFile()
        {
            //TODO: pass server data from db 
            using (StreamWriter stream = new StreamWriter($@"{_restBaseDirectory}\rk7man.ini", false))
            {
                stream.Write($@"[REFEDIT]
                UserName={_user.Login}
                Client={_user.Login}-%RANDOM%
                Server={_rest.ServerName}
                LOCKONEDIT=0
                LongTimeout=200000
                [NETKERN]
                PROTOCOLS=tcpsoc.dll
                [TCPSOC]
                LISTEN=0
                PORT={_rest.Port}
                [TCPDNS]
                {_rest.ServerName}={_rest.Ip}:{_rest.Port}");
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
            CreateIniFile();
        }

        private void LoginRk7(string restName, string password) 
        {
            string rk7Version = FileVersionInfo.GetVersionInfo($@"{_restBaseDirectory}\rk7man.exe").FileVersion;
            // учитывается возможность, что форма может иметь наименование на русском, или английском
            // перечисляем варианты
            var Rk7WindowTitleVariants = new List<string>()
            {
                $@"{Properties.Settings.Default.RK7WindowTitleRus} + {rk7Version}",
                $@"{Properties.Settings.Default.RK7WindowTitleEng} + {rk7Version}"
            };
            // учитывается возможность, что кнопка ок может иметь разное написание
            // перечисляем варианты
            var okButtonName = new List<string>()
            {
                "Ok", "ОК"
            };
            // получаем главное окно аутентификации
            IntPtr hWindow = User32Helper.GetWindowWithTitleVariants(Rk7WindowTitleVariants);
            // TODO: log, exception?
            if (hWindow == IntPtr.Zero)
            {
                return;
            }
            // дальше работаем с элементами внутри окна
            IntPtr _panelContainer;
            IntPtr _maskEdit;
            IntPtr _passwordField;
            IntPtr _okButton;

            _panelContainer = User32Helper.GetElementByName(hWindow, "TPabel", null);
            _maskEdit = User32Helper.GetElementByName(_panelContainer, "TcxMaskEdit", null);
            _passwordField = User32Helper.GetElementByName(_panelContainer, "TcxCustomInnerTextEdit.UnicodeClass", null);
            // TODO: log, exception?
            if (_passwordField == IntPtr.Zero)
            {
                return;
            }
            User32Helper.SetElementText((IntPtr)_passwordField, password);
            _okButton = User32Helper.GetElementWithNameVariants(_panelContainer, "TcxButton", okButtonName);
            // TODO: log, exception?
            if (_okButton == IntPtr.Zero)
            {
                return;
            }
            User32Helper.ClickElement(_okButton);
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